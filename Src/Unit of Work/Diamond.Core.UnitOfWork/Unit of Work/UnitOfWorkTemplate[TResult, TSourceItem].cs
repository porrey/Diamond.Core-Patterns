using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.UnitOfWork
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	/// <typeparam name="TSourceItem"></typeparam>
	public abstract class UnitOfWorkTemplate<TResult, TSourceItem> : UnitOfWorkTemplate, IUnitOfWork<TResult, TSourceItem>
	{
		/// <summary>
		/// Creates an instance of <see cref="UnitOfWorkTemplate{TResult, TSourceItem}"/> with the specified logger.
		/// </summary>
		/// <param name="logger">In instance of <see cref="ILogger{UnitOfWorkTemplate}"/> used for logging.</param>
		public UnitOfWorkTemplate(ILogger<UnitOfWorkTemplate<TResult, TSourceItem>> logger)
			: base(logger)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// Creates a default instance of <see cref="UnitOfWorkTemplate{TResult, TSourceItem}"/>.
		/// </summary>
		public UnitOfWorkTemplate()
			: base()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual Task<TResult> CommitAsync(TSourceItem item)
		{
			return this.OnCommitAsync(item);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected virtual Task<TResult> OnCommitAsync(TSourceItem item)
		{
			throw new NotImplementedException();
		}
	}
}
