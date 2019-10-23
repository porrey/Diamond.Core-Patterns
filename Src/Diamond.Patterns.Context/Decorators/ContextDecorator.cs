using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Context
{
	public class ContextDecorator<T> : IContextDecorator<T>
		where T : IContext
	{
		public ContextDecorator(T item)
		{
			this.Item = item;
		}

		public virtual IStateDictionary Properties { get; set; }

		public virtual T Item { get; set; }

		public virtual Task ResetAsync()
		{
			this.Properties.Clear();
			return Task.FromResult(0);
		}
	}
}
