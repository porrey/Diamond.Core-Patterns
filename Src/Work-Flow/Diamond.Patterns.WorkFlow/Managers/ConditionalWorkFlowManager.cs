// ***
// *** Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
// *** 
// *** This program is free software: you can redistribute it and/or modify
// *** it under the terms of the GNU Lesser General Public License as published
// *** by the Free Software Foundation, either version 3 of the License, or
// *** (at your option) any later version.
// *** 
// *** This program is distributed in the hope that it will be useful,
// *** but WITHOUT ANY WARRANTY; without even the implied warranty of
// *** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// *** GNU Lesser General Public License for more details.
// *** 
// *** You should have received a copy of the GNU Lesser General Public License
// *** along with this program. If not, see http://www.gnu.org/licenses/.
// *** 
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Patterns.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Patterns.WorkFlow
{
	/// <summary>
	///  This work-flow manager executes ALL steps in a work flow. If the
	///  current step indicates it should not be executed it is skipped
	///  and the work flow moves on to the next step.
	/// </summary>
	public class ConditionalWorkFlowManager : IWorkFlowManager, ILoggerPublisher<ConditionalWorkFlowManager>
	{
		/// <summary>
		/// 
		/// </summary>
		public ConditionalWorkFlowManager()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="steps"></param>
		public ConditionalWorkFlowManager(IWorkFlowItem[] steps)
		{
			this.Group = null;
			this.Steps = steps;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="steps"></param>
		/// <param name="group"></param>
		public ConditionalWorkFlowManager(IWorkFlowItem[] steps, string group)
		{
			this.Group = group;
			this.Steps = steps;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="workFlowItemFactory"></param>
		/// <param name="group"></param>
		public ConditionalWorkFlowManager(IWorkFlowItemFactory workFlowItemFactory, string group)
		{
			this.Group = group;
			this.WorkFlowItemFactory = workFlowItemFactory;
		}

		/// <summary>
		/// 
		/// </summary>
		public IWorkFlowItemFactory WorkFlowItemFactory { get; set; }

		/// <summary>
		/// 
		/// </summary>
		private IWorkFlowItem[] _steps = null;

		/// <summary>
		/// 
		/// </summary>
		public IWorkFlowItem[] Steps
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

		/// <summary>
		/// 
		/// </summary>
		public ILogger<ConditionalWorkFlowManager> Logger { get; set; } = new NullLogger<ConditionalWorkFlowManager>();

		/// <summary>
		/// 
		/// </summary>
		public string Group { get; set; }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public async Task<bool> ExecuteWorkflowAsync(IContext context)
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
			context.Properties.Set(DiamondWorkFlow.WellKnown.Context.LastStepSuccess, true);
			context.Properties.Set(DiamondWorkFlow.WellKnown.Context.WorkFlowFailed, false);

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
					this.Logger.LogTrace($"Starting work-flow step '{this.Steps[i].Name}' [{i + 1} of {this.Steps.Count()}].");

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
							context.Properties.Set(DiamondWorkFlow.WellKnown.Context.LastStepSuccess, true);
							string time = stopWatch.Elapsed.TotalSeconds < 1.0 ? "< 1 second" : $"{stopWatch.Elapsed.TotalSeconds:#,##0.0}";
							this.Logger.LogTrace($"The work-flow step '{this.Steps[i].Name}' completed successfully [Execution time = {time} second(s)].");
							returnValue = true;
						}
						else
						{
							context.Properties.Set(DiamondWorkFlow.WellKnown.Context.WorkFlowFailed, true);
							context.Properties.Set(DiamondWorkFlow.WellKnown.Context.LastStepSuccess, false);
							this.Logger.LogTrace($"The work-flow step '{this.Steps[i].Name}' failed.");
							returnValue = false;
						}

						// ***
						// *** Reset the stop watch.
						// ***
						stopWatch.Reset();
					}
				}
				else
				{
					this.Logger.LogTrace($"Skipping work-flow step '{this.Steps[i].Name}' [{i + 1} of {this.Steps.Count()}].");
				}
			}

			// ***
			// *** Check if the context contains a failed flag.
			// ***
			if (context.Properties.ContainsKey(DiamondWorkFlow.WellKnown.Context.WorkFlowFailed))
			{
				returnValue = !context.Properties.Get<bool>(DiamondWorkFlow.WellKnown.Context.WorkFlowFailed);
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected async Task LoadAsync()
		{
			if (this.Steps == null || this.Steps.Count() == 0)
			{
				this.Logger.LogTrace("Loading Work-Flow steps.");
				this.Steps = (await this.WorkFlowItemFactory.GetItemsAsync(this.Group)).ToArray();
				this.Logger.LogTrace($"{this.Steps.Count()} steps were loaded.");

				if (this.Steps.Count() == 0)
				{
					this.Logger.LogTrace($"Throwing exception because no steps were found for the Work-Flow with group name '{this.Group}'.");
					throw new MissingStepsException(this.Group);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="step"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		protected async Task<bool> ExecuteStepAsync(IWorkFlowItem step, IContext context)
		{
			bool returnValue = false;

			try
			{
				this.Logger.LogTrace($"Executing Work-Flow step '{step.Name}'.");
				if (await step.ExecuteStepAsync(context))
				{
					this.Logger.LogTrace($"The Work-Flow step '{step.Name}' completed successfully.");
					returnValue = true;
				}
				else
				{
					this.Logger.LogError($"The Work-Flow step '{step.Name}' failed.");

					if (context.HasException())
					{
						this.Logger.LogTrace($"The Work-Flow step '{step.Name}' had an exception set.");
						context.SetException(new WorkFlowFailureException(context.GetException(), step.Name, step.Ordinal));
					}
					else
					{
						this.Logger.LogWarning($"The Work-Flow step '{step.Name}' did NOT have an exception set.");
						context.SetException(new UnknownFailureException(step.Name, step.Ordinal));
					}
				}
			}
			catch (Exception ex)
			{
				this.Logger.LogError(ex, nameof(ExecuteStepAsync));
				context.SetException(ex);
				returnValue = false;
			}

			return returnValue;
		}
	}
}
