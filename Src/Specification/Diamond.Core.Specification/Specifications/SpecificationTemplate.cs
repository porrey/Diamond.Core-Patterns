using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

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
		/// <param name="logger"></param>
		public SpecificationTemplate(ILogger<SpecificationTemplate<TResult>> logger)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// 
		/// </summary>
		public SpecificationTemplate()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		protected ILogger<SpecificationTemplate<TResult>> Logger { get; set; } = new NullLogger<SpecificationTemplate<TResult>>();

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
		/// <param name="logger"></param>
		public SpecificationTemplate(ILogger<SpecificationTemplate<TParameter, TResult>> logger)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// 
		/// </summary>
		public SpecificationTemplate()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		protected ILogger<SpecificationTemplate<TParameter, TResult>> Logger { get; set; } = new NullLogger<SpecificationTemplate<TParameter, TResult>>();

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
