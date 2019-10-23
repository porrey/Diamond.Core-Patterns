using System;
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
			this.Steps = workFlowItemFactory.GetItemsAsync<TContextDecorator, TContext>(group).Result.ToArray();

			if (this.Steps.Count() == 0)
			{
				throw new ArgumentOutOfRangeException($"No state items with group '{group}' were found.");
			}
		}

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

		public async Task<bool> ExecuteWorkflowAsync(TContextDecorator context)
		{
			bool returnValue = true;

			// ***
			// *** Loop through each work-flow step executing them one at a time.
			// ***
			foreach (IWorkFlowItem<TContextDecorator, TContext> step in this.Steps)
			{
				if (!await step.ExecuteStepAsync(context))
				{
					if (context.HasException())
					{
						throw new WorkFlowFailureException(context.GetException(), step.Ordinal);
					}
					else
					{
						throw new UnknownFailureException(step.Name);
					}
				}
			}

			return returnValue;
		}
	}

	public class LinearWorkFlowManager<TContext> : LinearWorkFlowManager<ContextDecorator<TContext>, TContext>
		where TContext : IContext
	{
		public LinearWorkFlowManager(IWorkFlowItemFactory workFlowItemFactory, string group)
			: base(workFlowItemFactory, group)
		{
		}
	}
}
