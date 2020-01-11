using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;
using Diamond.Patterns.Context;

namespace Diamond.Patterns.WorkFlow
{
	public class LinearWorkFlowManager<TContextDecorator, TContext> : IWorkFlowManager<TContextDecorator, TContext>
		where TContext : IContext
		where TContextDecorator : IContextDecorator<TContext>
	{
		public LinearWorkFlowManager(IWorkFlowItemFactory workFlowItemFactory, string group)
		{
			this.Group = group;
			this.WorkFlowItemFactory = workFlowItemFactory;
			//this.Steps = workFlowItemFactory.GetItemsAsync<TContextDecorator, TContext>(group).Result.ToArray();

			//if (this.Steps.Count() == 0)
			//{
			//	throw new ArgumentOutOfRangeException($"No work flow items with group '{group}' were found.");
			//}
		}

		public LinearWorkFlowManager(IWorkFlowItemFactory workFlowItemFactory, string group, ILoggerSubscriber loggerSubscriber)
			 : this(workFlowItemFactory, group)
		{
			this.LoggerSubscriber = loggerSubscriber;
		}

		protected IWorkFlowItemFactory WorkFlowItemFactory { get; set; }
		private IWorkFlowItem<TContextDecorator, TContext>[] _steps = null;
		public IWorkFlowItem<TContextDecorator, TContext>[] Steps
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
				bool isContiguous = !value.OrderBy(s => s.Ordinal).Select(t => t.Ordinal).Select((i, j) => i - j).Distinct().Skip(1).Any();

				if (!isContiguous)
				{
					string itemOrdinals = String.Join(",", value.Select(t => t.Ordinal));
					throw new ArgumentOutOfRangeException($"The {value.Count()} [{itemOrdinals}] state items for group {this.Group} are not numbered consecutively.");
				}
				else
				{
					// ***
					// *** Store the steps ordered by the ordinal property value.
					// ***
					_steps = value.OrderBy(s => s.Ordinal).ToArray();
				}
			}
		}

		public string Group { get; set; }

		/// <summary>
		/// Gets/sets the instance of <see cref="ILoggerSubscriber"/> that
		/// will listen for logs events originating from this instance.
		/// </summary>
		public ILoggerSubscriber LoggerSubscriber { get; set; }

		public async Task<bool> ExecuteWorkflowAsync(TContextDecorator context)
		{
			bool returnValue = true;

			// ***
			// *** Initialize the work flow
			// ***
			await this.LoadAsync();

			// ***
			// *** Initialize this flag to True.
			// ***
			context.Properties.Set(DiamondWorkFlow.WellKnown.Context.LastStepSuccess, true);
			context.Properties.Set(DiamondWorkFlow.WellKnown.Context.WorkFlowFailed, false);

			try
			{
				// ***
				// *** Create a stop watch to time the work-flow steps.
				// ***
				Stopwatch stopWatch = new Stopwatch();

				// ***
				// *** Loop through each work-flow step executing them one at a time.
				// ***
				for (int i = 0; i <= this.FinalStepOfWorkflow; i++)
				{
					this.LoggerSubscriber.Verbose($"Starting work-flow step '{this.Steps[i].Name}' [{i + 1} of {this.Steps.Count()}].");

					// ***
					// *** Start the stop watch.
					// ***
					stopWatch.Start();

					// ***
					// *** Stores the result of the step.
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
							string time = stopWatch.Elapsed.TotalSeconds < 1.0 ? "< 1 second" : $"{stopWatch.Elapsed.TotalSeconds:#,##0.0}";
							this.LoggerSubscriber.Verbose($"The work-flow step '{this.Steps[i].Name}' completed successfully [Execution time = {time} second(s)].");
						}
						else
						{
							returnValue = false;
							this.LoggerSubscriber.Verbose($"The work-flow step '{this.Steps[i].Name}' failed.");
						}

						// ***
						// *** Reset the stop watch.
						// ***
						stopWatch.Reset();
					}

					// ***
					// *** Exit the loop if the last step failed.
					// ***
					if (!result)
					{
						break;
					}
				}
			}
			finally
			{
				// ***
				// *** If the last step of a work-flow is marked as final,
				// *** it should always run as the last step even if the
				// *** one of the other steps fail.
				// ***
				if (this.HasAlwaysExecuteStep)
				{
					this.LoggerSubscriber.Verbose($"Starting final work-flow step '{this.Steps[this.AlwaysExecuteStepIndex].Name}' [{this.AlwaysExecuteStepIndex + 1} of {this.Steps.Count()}].");

					if (await this.ExecuteStepAsync(this.Steps[this.AlwaysExecuteStepIndex], context))
					{
						this.LoggerSubscriber.Verbose($"The final work-flow step '{this.Steps[this.AlwaysExecuteStepIndex].Name}' completed successfully.");
					}
					else
					{
						this.LoggerSubscriber.Verbose($"The final work-flow step '{this.Steps[this.AlwaysExecuteStepIndex].Name}' failed.");
					}
				}
			}

			return returnValue;
		}

		protected async Task<bool> ExecuteStepAsync(IWorkFlowItem<TContextDecorator, TContext> step, TContextDecorator context)
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
					if (context.HasException())
					{
						context.SetException(new WorkFlowFailureException(context.GetException(), step.Name, step.Ordinal));
					}
					else
					{
						context.SetException(new UnknownFailureException(step.Name, step.Ordinal));
					}
				}
			}
			catch (Exception ex)
			{
				context.SetException(ex);
				returnValue = false;
			}

			return returnValue;
		}

		protected async Task LoadAsync()
		{
			if (this.Steps == null || this.Steps.Count() == 0)
			{
				this.Steps = (await this.WorkFlowItemFactory.GetItemsAsync<TContextDecorator, TContext>(this.Group)).ToArray();

				if (this.Steps.Count() == 0)
				{
					throw new MissingStepsException(this.Group);
				}
			}
		}

		protected int AlwaysExecuteStepIndex
		{
			get
			{
				int returnValue = -1;

				if (this.Steps[this.Steps.Count() - 1].AlwaysExecute)
				{
					returnValue = this.Steps.Count() - 1;
				}

				return returnValue;
			}
		}

		protected bool HasAlwaysExecuteStep
		{
			get
			{
				return this.Steps[this.Steps.Count() - 1].AlwaysExecute;
			}
		}

		protected int FinalStepOfWorkflow
		{
			get
			{
				// ***
				// *** The last step of the work-flow is the last step,
				// *** unless it is marked final. Then it is the step
				// *** before it.
				// ***
				int returnValue = this.Steps.Count() - 1;

				if (this.HasAlwaysExecuteStep)
				{
					returnValue -= 1;
				}

				return returnValue;
			}
		}
	}

	public class LinearWorkFlowManager<TContext> : LinearWorkFlowManager<ContextDecorator<TContext>, TContext>
		where TContext : IContext
	{
		public LinearWorkFlowManager(IWorkFlowItemFactory workFlowItemFactory, string group)
			: base(workFlowItemFactory, group)
		{
		}

		public LinearWorkFlowManager(IWorkFlowItemFactory workFlowItemFactory, string group, ILoggerSubscriber loggerSubscriber)
			 : base(workFlowItemFactory, group, loggerSubscriber)
		{
		}
	}
}
