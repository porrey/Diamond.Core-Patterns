using System.Threading.Tasks;

namespace Diamond.Core.Abstractions
{
	/// <summary>
	/// Defines a factory that can create new instances of the specified
	/// database context.
	/// </summary>
	/// <typeparam name="TContext">The type of the database context
	/// supported by this factory instance.</typeparam>
	public interface IContextFactory<TContext>
	{
		/// <summary>
		/// Creates an instance of TContext.
		/// </summary>
		/// <returns>The new instance of TContext.</returns>
		Task<TContext> CreateContextAsync();

		/// <summary>
		/// Creates an instance of TContext.
		/// </summary>
		/// <returns>The new instance of TContext.</returns>
		TContext CreateContext();
	}
}
