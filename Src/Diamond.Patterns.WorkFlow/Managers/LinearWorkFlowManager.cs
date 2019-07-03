using System;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.WorkFlow
{
	public class WorkFlowManager<TContext> : IWorkFlowManager<TContext> where TContext : IContext
	{
		public WorkFlowManager(IWorkFlowItemFactory workFlowItemFactory, string group)
		{
			this.Group = group;
			this.Steps = workFlowItemFactory.GetItemsAsync<TContext>(group).Result.ToArray();

			if (this.Steps.Count() == 0)
			{
				throw new ArgumentOutOfRangeException($"No state items with group '{group}' were found.");
			}
		}

		private IWorkFlowItem<TContext>[] _steps = null;
		public IWorkFlowItem<TContext>[] Steps
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
					throw new ArgumentOutOfRangeException($"The state items for group {this.Group} are not numbered consecutively.");
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

		public async Task<bool> ExecuteWorkflowAsync(IContextDecorator<TContext> context)
		{
			bool returnValue = true;

			// ***
			// *** Loop through each work-flow step executing them one at a time.
			// ***
			foreach (IWorkFlowItem<TContext> step in this.Steps)
			{
				if (!await step.ExecuteStepAsync(context))
				{
					if (context.Item is IExceptionContext exceptionContext &&  exceptionContext.Exception != null)
					{
						throw exceptionContext.Exception;
					}
					else
					{
						throw new Exception($"The step '{step.Name}' failed for an unknown reason.");
					}
				}
			}

			return returnValue;
		}
	}
}
