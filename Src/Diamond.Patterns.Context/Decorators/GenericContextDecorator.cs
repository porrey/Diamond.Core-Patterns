using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Context
{
	public class GenericContextDecorator<TContext> : IContextDecorator<TContext>
		where TContext : IContext
	{
		public GenericContextDecorator(TContext item)
		{
			this.Item = item;
		}

		/// <summary>
		/// Provides access to the state properties.
		/// </summary>
		public virtual IStateDictionary Properties { get; set; }

		/// <summary>
		/// Gets the underlying context item.
		/// </summary>
		public virtual TContext Item { get; set; }

		/// <summary>
		/// Resets the context.
		/// </summary>
		public virtual Task ResetAsync()
		{
			this.Properties.Clear();
			return Task.FromResult(0);
		}
	}
}
