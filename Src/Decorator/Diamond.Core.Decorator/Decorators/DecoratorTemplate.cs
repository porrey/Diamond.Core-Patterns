using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.Decorator
{
	/// <summary>
	/// Provide a template from which other decorators can be derived. This is a stateful
	/// object and should be defined as transient in containers.
	/// </summary>
	/// <typeparam name="TDecoratedItem"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	public abstract class DecoratorTemplate<TDecoratedItem, TResult> : DisposableObject, IDecorator<TDecoratedItem, TResult>
	{
		private TDecoratedItem _item = default;

		/// <summary>
		/// Creates a default instance of <see cref="DecoratorTemplate{TDecoratedItem, TResult}"/>.
		/// </summary>
		public DecoratorTemplate()
		{
			this.Name = this.GetType().Name.Replace("Decorator", "");
		}

		/// <summary>
		/// Creates an instance of <see cref="DecoratorTemplate{TDecoratedItem, TResult}"/> with
		/// the given logger instance.
		/// </summary>
		/// <param name="logger">An instance of the logger.</param>
		public DecoratorTemplate(ILogger<DecoratorTemplate<TDecoratedItem, TResult>> logger)
			: this()
		{
			this.Logger = logger;
		}

		/// <summary>
		/// An instance if a logger specific to this instance.
		/// </summary>
		protected virtual ILogger<DecoratorTemplate<TDecoratedItem, TResult>> Logger { get; set; } = new NullLogger<DecoratorTemplate<TDecoratedItem, TResult>>();

		/// <summary>
		/// 
		/// </summary>
		public virtual TDecoratedItem Item
		{
			get
			{
				return _item;
			}
			set
			{
				
				if (EqualityComparer<TDecoratedItem>.Default.Equals(_item, default(TDecoratedItem)))
				{
					_item = value;
				}
				else
				{
					if (EqualityComparer<TDecoratedItem>.Default.Equals(value, default(TDecoratedItem)))
					{
						throw new DecoratedItemInstanceAlreadySetException();
					}
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public virtual string Name { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual Task<TResult> TakeActionAsync()
		{
			if (EqualityComparer<TDecoratedItem>.Default.Equals(this.Item, default(TDecoratedItem)))
			{
				throw new DecoratedItemInstanceNotSetException();
			}

			return this.OnTakeActionAsync(this.Item);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual Task<TResult> TakeActionAsync(TDecoratedItem item)
		{
			if (EqualityComparer<TDecoratedItem>.Default.Equals(this.Item, default(TDecoratedItem)))
			{
				this.Item = item;
			}
			else
			{
				throw new DecoratedItemInstanceAlreadySetException();
			}

			return this.OnTakeActionAsync(item);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected virtual Task<TResult> OnTakeActionAsync(TDecoratedItem item)
		{
			return Task.FromResult<TResult>(default);
		}

		/// <summary>
		/// 
		/// </summary>
		protected override void OnDisposeManagedObjects()
		{
			this.Logger.LogDebug("{instance}[{item}] disposed.", this.GetType().Name, this.Item?.ToString());
			this.Item = default;
			GC.SuppressFinalize(this);
		}
	}
}
