using Diamond.Patterns.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Unity;

namespace Diamond.Patterns.Mvc.Unity
{
	public delegate void OnBeforeCreateDeleagte(ControllerContext context);
	public delegate void OnAfterCreateDeleagte(ControllerContext context, object controller);
	public delegate void OnReleaseControllerDeleagte(ControllerContext context, object controller);

	public class ControllerActivator : IControllerActivator, ILogger
	{
		public OnBeforeCreateDeleagte OnBeforeCreate { get; set; }
		public OnAfterCreateDeleagte OnAfterCreate { get; set; }
		public OnReleaseControllerDeleagte OnReleaseController { get; set; }

		public ControllerActivator(IUnityContainer container)
		{
			this.Container = container;
		}

		public ControllerActivator(IUnityContainer container, ILoggerSubscriber loggerSubscriber)
		{
			this.Container = container;
			this.LoggerSubscriber = loggerSubscriber;
		}

		protected IUnityContainer Container { get; set; }

		/// <summary>
		/// Gets/sets the instance of <see cref="ILoggerSubscriber"/> that
		/// will listen for logs events originating from this instance.
		/// </summary>
		public ILoggerSubscriber LoggerSubscriber { get; set; }

		public object Create(ControllerContext context)
		{
			object returnValue = null;

			this.LoggerSubscriber.Verbose("Method Create() was called on ControllerActivator.");
			this.OnBeforeCreate?.Invoke(context);
			returnValue = this.Container.Resolve(context.ActionDescriptor.ControllerTypeInfo.AsType());
			this.OnAfterCreate?.Invoke(context, returnValue);

			return returnValue;
		}

		public void Release(ControllerContext context, object controller)
		{
			this.LoggerSubscriber.Verbose("Method Release() was called on ControllerActivator.");
			this.OnReleaseController?.Invoke(context, controller);
		}
	}
}
