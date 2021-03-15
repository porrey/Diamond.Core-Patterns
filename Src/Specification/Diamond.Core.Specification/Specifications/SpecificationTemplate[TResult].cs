using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Specification
{
	/// <summary>
	/// Provides the template for an object that implements <see cref="ISpecification{TResult}"/>.
	/// </summary>
	/// <typeparam name="TResult">The type of the result when the selection executes.</typeparam>
	public abstract class SpecificationTemplate<TResult> : SpecificationTemplate, ISpecification<TResult>
	{
		/// <summary>
		/// Creates an instance of <see cref="SpecificationTemplate{TResult}"/> with the specified logger.
		/// </summary>
		/// <param name="logger">In instance of <see cref="ILogger{SpecificationTemplate}"/> used for logging.</param>
		public SpecificationTemplate(ILogger<SpecificationTemplate<TResult>> logger)
			:base(logger)
		{
		}

		/// <summary>
		/// Creates a default instance of <see cref="SpecificationTemplate{TResult}"/>.
		/// </summary>
		public SpecificationTemplate()
		{
		}

		/// <summary>
		/// Executes the selection by the specification design. The default implementation
		/// calls OnExecuteSelectionAsync().
		/// </summary>
		/// <returns>Returns the result of the selection as type TResult.</returns>
		public virtual Task<TResult> ExecuteSelectionAsync()
		{
			return this.OnExecuteSelectionAsync();
		}

		/// <summary>
		/// Override in the concrete class to perform the selection.
		/// </summary>
		/// <returns>Returns the result of the selection as type TResult.</returns>
		protected virtual Task<TResult> OnExecuteSelectionAsync()
		{
			throw new NotImplementedException();
		}
	}
}
