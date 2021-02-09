//
// Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see http://www.gnu.org/licenses/.
// 
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Core.Extensions.InterfaceInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.WorkFlow {
	/// <summary>
	/// 
	/// </summary>
	public class LinearWorkFlowManager : IWorkFlowManager, ILoggerPublisher<LinearWorkFlowManager> {
		/// <summary>
		/// 
		/// </summary>
		public LinearWorkFlowManager() {
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="workFlowItemFactory"></param>
		/// <param name="group"></param>
		public LinearWorkFlowManager(IWorkFlowItemFactory workFlowItemFactory, string group) {
			this.Group = group;
			this.WorkFlowItemFactory = workFlowItemFactory;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="workFlowItemFactory"></param>
		public LinearWorkFlowManager(IWorkFlowItemFactory workFlowItemFactory) {
			this.WorkFlowItemFactory = workFlowItemFactory;
		}

		/// <summary>
		/// 
		/// </summary>
		private IWorkFlowItem[] _steps = null;

		/// <summary>
		/// 
		/// </summary>
		public virtual IWorkFlowItemFactory WorkFlowItemFactory { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public virtual ILogger<LinearWorkFlowManager> Logger { get; set; } = new NullLogger<LinearWorkFlowManager>();

		/// <summary>
		/// 
		/// </summary>
		public virtual IWorkFlowItem[] Steps {
			get {
				if (_steps == null) {
					_steps = this.WorkFlowItemFactory.GetItemsAsync(this.Group).Result.ToArray();

					if (_steps.Count() == 0) {
						throw new ArgumentOutOfRangeException($"No work flow items with group '{this.Group}' were found.");
					}
				}

				return _steps;
			}
			set {
				//
				// Ensure that the states are numbered contiguously.
				//
				bool isContiguous = !value.OrderBy(s => s.Ordinal).Select(t => t.Ordinal).Select((i, j) => i - j).Distinct().Skip(1).Any();

				if (!isContiguous) {
					string itemOrdinals = String.Join(",", value.Select(t => t.Ordinal));
					throw new ArgumentOutOfRangeException($"The {value.Count()} [{itemOrdinals}] state items for group {this.Group} are not numbered consecutively.");
				}
				else {
					//
					// Store the steps ordered by the ordinal property value.
					//
					_steps = value.OrderBy(s => s.Ordinal).ToArray();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual string Group { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public virtual async Task<bool> ExecuteWorkflowAsync(IContext context) {
			bool returnValue = true;

			//
			// Initialize the work flow
			//
			await this.LoadAsync();

			//
			// Initialize this flag to True.
			//
			context.Properties.Set(DiamondWorkFlow.WellKnown.Context.LastStepSuccess, true);
			context.Properties.Set(DiamondWorkFlow.WellKnown.Context.WorkFlowFailed, false);

			try {
				//
				// Create a stop watch to time the work-flow steps.
				//
				Stopwatch stopWatch = new Stopwatch();

				//
				// Loop through each work-flow step executing them one at a time.
				//
				for (int i = 0; i <= this.FinalStepOfWorkflow; i++) {
					this.Logger.LogTrace($"Starting work-flow step '{this.Steps[i].Name}' [{i + 1} of {this.Steps.Count()}].");

					//
					// Start the stop watch.
					//
					stopWatch.Start();

					//
					// Stores the result of the step.
					//
					bool result = false;

					try {
						result = await this.ExecuteStepAsync(this.Steps[i], context);
					}
					finally {
						//
						// Stop the stop watch.
						//
						stopWatch.Stop();

						//
						// Check the result.
						//
						if (result) {
							string time = stopWatch.Elapsed.TotalSeconds < 1.0 ? "< 1 second" : $"{stopWatch.Elapsed.TotalSeconds:#,##0.0}";
							this.Logger.LogTrace($"The work-flow step '{this.Steps[i].Name}' completed successfully [Execution time = {time} second(s)].");
						}
						else {
							returnValue = false;
							this.Logger.LogTrace($"The work-flow step '{this.Steps[i].Name}' failed.");
						}

						//
						// Reset the stop watch.
						//
						stopWatch.Reset();
					}

					//
					// Exit the loop if the last step failed.
					//
					if (!result) {
						break;
					}
				}
			}
			finally {
				//
				// If the last step of a work-flow is marked as final,
				// it should always run as the last step even if the
				// one of the other steps fail.
				//
				if (this.HasAlwaysExecuteStep) {
					this.Logger.LogTrace($"Starting final work-flow step '{this.Steps[this.AlwaysExecuteStepIndex].Name}' [{this.AlwaysExecuteStepIndex + 1} of {this.Steps.Count()}].");

					if (await this.ExecuteStepAsync(this.Steps[this.AlwaysExecuteStepIndex], context)) {
						this.Logger.LogTrace($"The final work-flow step '{this.Steps[this.AlwaysExecuteStepIndex].Name}' completed successfully.");
					}
					else {
						this.Logger.LogTrace($"The final work-flow step '{this.Steps[this.AlwaysExecuteStepIndex].Name}' failed.");
					}
				}
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="step"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		protected virtual async Task<bool> ExecuteStepAsync(IWorkFlowItem step, IContext context) {
			bool returnValue = false;

			try {
				if (await step.ExecuteStepAsync(context)) {
					returnValue = true;
				}
				else {
					if (context.HasException()) {
						context.SetException(new WorkFlowFailureException(context.GetException(), step.Name, step.Ordinal));
					}
					else {
						context.SetException(new UnknownFailureException(step.Name, step.Ordinal));
					}
				}
			}
			catch (Exception ex) {
				this.Logger.LogError(ex, nameof(ExecuteStepAsync));
				context.SetException(ex);
				returnValue = false;
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected virtual async Task LoadAsync() {
			if (this.Steps == null || this.Steps.Count() == 0) {
				this.Steps = (await this.WorkFlowItemFactory.GetItemsAsync(this.Group)).ToArray();

				if (this.Steps.Count() == 0) {
					throw new MissingStepsException(this.Group);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual int AlwaysExecuteStepIndex {
			get {
				int returnValue = -1;

				if (this.Steps[this.Steps.Count() - 1].AlwaysExecute) {
					returnValue = this.Steps.Count() - 1;
				}

				return returnValue;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual bool HasAlwaysExecuteStep {
			get {
				return this.Steps[this.Steps.Count() - 1].AlwaysExecute;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual int FinalStepOfWorkflow {
			get {
				//
				// The last step of the work-flow is the last step,
				// unless it is marked final. Then it is the step
				// before it.
				//
				int returnValue = this.Steps.Count() - 1;

				if (this.HasAlwaysExecuteStep) {
					returnValue -= 1;
				}

				return returnValue;
			}
		}
	}
}
