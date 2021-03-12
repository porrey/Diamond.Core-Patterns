using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Decorator
{
	/// <summary>
	/// Provide a template from which other decorators can be derived. This is a stateful
	/// object and should be defined as transient in containers.
	/// </summary>
	/// <typeparam name="TDecoratedItem"></typeparam>
	/// <typeparam name="TResult"></typeparam>
	public abstract class DecoratorTemplate<TDecoratedItem, TResult> : DisposableObject, IDecorator<TDecoratedItem, TResult>
		where TDecoratedItem : class
	{
		private TDecoratedItem _item = default;

		/// <summary>
		/// Creates a default instance of <see cref="DecoratorTemplate{TDecoratedItem, TResult}"/>.
		/// </summary>
		public DecoratorTemplate()
		{
		}

		/// <summary>
		/// Creates an instance of <see cref="DecoratorTemplate{TDecoratedItem, TResult}"/> with
		/// the given logger instance.
		/// </summary>
		/// <param name="logger">An instance of the logger.</param>
		public DecoratorTemplate(ILogger<DecoratorTemplate<TDecoratedItem, TResult>> logger)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// An instance if a logger specific to this instance.
		/// </summary>
		protected ILogger<DecoratorTemplate<TDecoratedItem, TResult>> Logger { get; set; }

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
				if (_item == null)
				{
					_item = value;
				}
				else
				{
					if (value != null)
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
			if (this.Item == null)
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
			if (this.Item == null)
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
			this.Item = null;
			GC.SuppressFinalize(this);
		}
	}
}
