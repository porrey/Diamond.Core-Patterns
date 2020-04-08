// ***
// *** Copyright(C) 2019-2020, Daniel M. Porrey. All rights reserved.
// *** 
// *** This program is free software: you can redistribute it and/or modify
// *** it under the terms of the GNU Lesser General Public License as published
// *** by the Free Software Foundation, either version 3 of the License, or
// *** (at your option) any later version.
// *** 
// *** This program is distributed in the hope that it will be useful,
// *** but WITHOUT ANY WARRANTY; without even the implied warranty of
// *** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// *** GNU Lesser General Public License for more details.
// *** 
// *** You should have received a copy of the GNU Lesser General Public License
// *** along with this program. If not, see http://www.gnu.org/licenses/.
// *** 
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
