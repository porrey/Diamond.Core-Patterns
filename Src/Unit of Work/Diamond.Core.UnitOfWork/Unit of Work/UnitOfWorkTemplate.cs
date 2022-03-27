using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.UnitOfWork
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class UnitOfWorkTemplate : DisposableObject, IUnitOfWork
	{
		/// <summary>
		/// Creates an instance of <see cref="UnitOfWorkTemplate"/> with the specified logger.
		/// </summary>
		/// <param name="logger">In instance of <see cref="ILogger"/> used for logging.</param>
		public UnitOfWorkTemplate(ILogger<UnitOfWorkTemplate> logger)
			: this()
		{
			this.Logger = logger;
		}

		/// <summary>
		/// Creates a default instance of <see cref="UnitOfWorkTemplate{TResult, TSourceItem}"/>.
		/// </summary>
		public UnitOfWorkTemplate()
		{
			this.Name = this.GetType().Name.Replace("UnitOfWork", "");
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual ILogger<UnitOfWorkTemplate> Logger { get; set; } = new NullLogger<UnitOfWorkTemplate>();

		/// <summary>
		/// 
		/// </summary>
		public string Name { get; set; }
	}
}
