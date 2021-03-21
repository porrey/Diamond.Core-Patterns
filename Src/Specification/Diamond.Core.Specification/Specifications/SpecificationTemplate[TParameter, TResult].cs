using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Specification
{
	/// <summary>
	/// Provides the template for an object that implements <see cref="ISpecification{TParameter, TResult}"/>.
	/// </summary>
	/// <typeparam name="TParameter">The type of input(s) required for the selection.</typeparam>
	/// <typeparam name="TResult">The type of the result when the selection executes.</typeparam>
	public abstract class SpecificationTemplate<TParameter, TResult> : SpecificationTemplate, ISpecification<TParameter, TResult>
	{
		/// <summary>
		/// Creates an instance of <see cref="SpecificationTemplate{TParameter, TResult}"/> with the specified logger.
		/// </summary>
		/// <param name="logger"></param>
		public SpecificationTemplate(ILogger<SpecificationTemplate<TParameter, TResult>> logger)
			: base(logger)
		{
		}

		/// <summary>
		/// Creates a default instance of <see cref="SpecificationTemplate{TParameter, TResult}"/>.
		/// </summary>
		public SpecificationTemplate()
			: base()
		{
		}

		/// <summary>
		/// Executes the selection by the specification design. The default implementation
		/// calls OnExecuteSelectionAsync().
		/// </summary>
		/// <param name="inputs">Specifies the inputs used as the selection criteria. To specify more
		/// than one value, use a Tuple for TParameter.</param>
		/// <returns>Returns the result of the selection as type TResult.</returns>
		public virtual Task<TResult> ExecuteSelectionAsync(TParameter inputs)
		{
			return this.OnExecuteSelectionAsync(inputs);
		}

		/// <summary>
		/// Override in the concrete class to perform the selection.
		/// </summary>
		/// <param name="input"></param>
		/// <returns>Returns the result of the selection as type TResult.</returns>
		protected virtual Task<TResult> OnExecuteSelectionAsync(TParameter input)
		{
			throw new NotImplementedException();
		}
	}
}
