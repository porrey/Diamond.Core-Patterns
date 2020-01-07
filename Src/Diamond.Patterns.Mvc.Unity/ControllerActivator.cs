using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Unity;

namespace Diamond.Patterns.Mvc.Unity
{
	public class ControllerActivator : IControllerActivator
	{
		public ControllerActivator(IUnityContainer container)
		{
			this.Container = container;
		}

		protected IUnityContainer Container { get; set; }

		public object Create(ControllerContext context)
		{
			return this.Container.Resolve(context.ActionDescriptor.ControllerTypeInfo.AsType());
		}

		public void Release(ControllerContext context, object controller)
		{

		}
	}
}
