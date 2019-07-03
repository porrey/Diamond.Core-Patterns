using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;
using Diamond.Patterns.WorkFlow;

namespace Lsc.Logistics.DynaMail.WorkFlow.Core
{
	/// <summary>
	///  This work-flow manager executes ALL steps in a work flow. If the
	///  current step indicates it should not be executed it is skipped
	///  and the work flow moves on to the next step.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ConditionalWorkFlowManager<T> : IWorkFlowManager<T> where T : IContext
	{
		public ConditionalWorkFlowManager(IWorkFlowItem<T>[] steps)
		{
			this.Group = null;
			this.Steps = steps;
		}

		public ConditionalWorkFlowManager(IWorkFlowItem<T>[] steps, string group)
		{
			this.Group = group;
			this.Steps = steps;
		}

		public ConditionalWorkFlowManager(IWorkFlowItemFactory workFlowItemFactory, string group)
		{
			this.Group = group;
			this.WorkFlowItemFactory = workFlowItemFactory;
		}

		protected IWorkFlowItemFactory WorkFlowItemFactory { get; set; }

		private IWorkFlowItem<T>[] _steps = null;
		public IWorkFlowItem<T>[] Steps
		{
			get
			{
				return _steps;
			}
			set
			{
				// ***
				// *** Ensure that the states are numbered contiguously.
				// ***
				if (value.OrderBy(t => t.Ordinal).First().Ordinal == 1)
				{
					bool isContiguous = !value.OrderBy(s => s.Ordinal).Select(t => t.Ordinal).Select((i, j) => i - j).Distinct().Skip(1).Any();

					if (isContiguous)
					{
						// ***
						// *** Store the steps ordered by the ordinal property value.
						// ***
						_steps = value.OrderBy(s => s.Ordinal).ToArray();
					}
					else
					{
						throw new ArgumentOutOfRangeException($"The state items for group {this.Group} are not numbered consecutively.");
					}
				}
				else
				{
					throw new ArgumentOutOfRangeException($"The state items for group {this.Group} must be numbered starting with 1.");
				}
			}
		}

		public string Group { get; set; }

		public async Task<bool> ExecuteWorkflowAsync(IContextDecorator<T> context)
		{
			bool returnValue = true;

			// ***
			// *** Initialize the work flow
			// ***
			await this.LoadAsync();

			// ***
			// *** Create a stop watch to time the work-flow steps.
			// ***
			Stopwatch stopWatch = new Stopwatch();

			// ***
			// *** Initialize this flag to True.
			// ***
			context.Properties.Set(WellKnown.Context.LastStepSuccess, true);
			context.Properties.Set(WellKnown.Context.WorkFlowFailed, false);

			// ***
			// *** Loop through each work-flow step executing them one at a time.
			// ***
			for (int i = 0; i < this.Steps.Count(); i++)
			{
				if (this.Steps[i].ShouldExecute(context))
				{
					// ***
					// *** Publish a progress update.
					// ***
					Trace.TraceInformation($"Starting work-flow step '{this.Steps[i].Name}' [{i + 1} of {this.Steps.Count()}].");

					// ***
					// *** Start the stop watch.
					// ***
					stopWatch.Start();

					// ***
					// ***
					// ***
					bool result = false;

					try
					{
						result = await this.ExecuteStepAsync(this.Steps[i], context);
					}
					finally
					{
						// ***
						// *** Stop the stop watch.
						// ***
						stopWatch.Stop();

						// ***
						// *** Check the result.
						// ***
						if (result)
						{
							context.Properties.Set(WellKnown.Context.LastStepSuccess, true);
							string time = stopWatch.Elapsed.TotalSeconds < 1.0 ? "< 1 second" : $"{stopWatch.Elapsed.TotalSeconds:#,##0.0}";
							Trace.TraceInformation($"The work-flow step '{this.Steps[i].Name}' completed successfully [Execution time = {time} second(s)].");
						}
						else
						{
							context.Properties.Set(WellKnown.Context.WorkFlowFailed, true);
							context.Properties.Set(WellKnown.Context.LastStepSuccess, false);
							Trace.TraceError($"The work-flow step '{this.Steps[i].Name}' failed.");
						}

						// ***
						// *** Reset the stop watch.
						// ***
						stopWatch.Reset();
					}
				}
				else
				{
					Trace.TraceError($"Skipping work-flow step '{this.Steps[i].Name}' [{i + 1} of {this.Steps.Count()}].");
				}
			}


			return returnValue;
		}

		protected async Task LoadAsync()
		{
			if (this.Steps == null || this.Steps.Count() == 0)
			{
				this.Steps = (await this.WorkFlowItemFactory.GetItemsAsync<T>(this.Group)).ToArray();

				if (this.Steps.Count() == 0)
				{
					throw new MissingStepsException(this.Group);
				}
			}
		}

		protected async Task<bool> ExecuteStepAsync(IWorkFlowItem<T> step, IContextDecorator<T> context)
		{
			bool returnValue = false;

			try
			{
				if (await step.ExecuteStepAsync(context))
				{
					returnValue = true;
				}
				else
				{
					if (context.Item is IExceptionContext exceptionContext && exceptionContext.Exception != null)
					{
						throw exceptionContext.Exception;
					}
					else
					{
						throw new Exception($"The step '{step.Name}' failed for an unknown reason.");
					}
				}
			}
			catch (Exception ex)
			{
				if (context.Item is IExceptionContext exceptionContext)
				{
					exceptionContext.SetException(ex);
				}

				returnValue = false;
			}

			return returnValue;
		}
	}
}
