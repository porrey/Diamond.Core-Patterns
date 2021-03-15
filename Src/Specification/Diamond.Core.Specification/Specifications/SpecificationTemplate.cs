using System.Threading.Tasks;

namespace Diamond.Core.Specification.Specifications
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	public abstract class SpecificationTemplate<TResult> : ISpecification<TResult>
	{
		/// <summary>
		/// 
		/// </summary>
		public virtual string Name => this.GetType().Name.Replace("Specification", "");

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public abstract Task<TResult> ExecuteSelectionAsync();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TParameter"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	public abstract class SpecificationTemplate<TParameter, TResult> : ISpecification<TParameter, TResult>
	{
		/// <summary>
		/// 
		/// </summary>
		public virtual string Name => this.GetType().Name.Replace("Specification", "");

		/// <summary>
		/// 
		/// </summary>
		/// <param name="inputs"></param>
		/// <returns></returns>
		public abstract Task<TResult> ExecuteSelectionAsync(TParameter inputs);
	}
}
