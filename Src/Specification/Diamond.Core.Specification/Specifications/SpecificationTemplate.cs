using System;
using System.Threading.Tasks;

namespace Diamond.Core.Specification
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
		public virtual Task<TResult> ExecuteSelectionAsync()
		{
			return this.OnExecuteSelectionAsync();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected virtual Task<TResult> OnExecuteSelectionAsync()
		{
			throw new NotImplementedException();
		}
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
		public virtual Task<TResult> ExecuteSelectionAsync(TParameter inputs)
		{
			return this.OnExecuteSelectionAsync(inputs);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected virtual Task<TResult> OnExecuteSelectionAsync(TParameter input)
		{
			throw new NotImplementedException();
		}
	}
}
