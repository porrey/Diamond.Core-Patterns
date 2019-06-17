using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;
using Diamond.Patterns.Context;

namespace Diamond.Patterns.Context
{
	public class GenericContextDecorator<TContext> : IContextDecorator<TContext> where TContext : IContext
	{
		public GenericContextDecorator(TContext item)
		{
			this.Item = item;
		}

		/// <summary>
		/// Provides access to the state properties.
		/// </summary>
		public IStateDictionary Properties { get; } = new StateDictionary();

		/// <summary>
		/// Gets the underlying context item.
		/// </summary>
		public TContext Item { get; set; }

		/// <summary>
		/// Resets the context.
		/// </summary>
		public Task ResetAsync()
		{
			this.Properties.Clear();
			return Task.FromResult(0);
		}
	}
}
