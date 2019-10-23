using System;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;
using Unity;
using Unity.Lifetime;

namespace Diamond.Patterns.UnityConfiguration
{
	public class ConsoleUnityBootstrapper : DisposableObject, IApplicationContext
	{
		public static class WellKnown
		{
			public static class Container
			{
				public const string ObjectFactory = "ObjectFactory";
			}
		}

		protected virtual IUnityContainer Container { get; set; }
		public virtual string[] Arguments { get; set; }
		public virtual IObjectFactory ObjectFactory => this.Container.Resolve<IObjectFactory>(WellKnown.Container.ObjectFactory);
		public virtual string Name { get; }

		public ConsoleUnityBootstrapper()
		{
		}

		public virtual Task<bool> Initialize(string[] args)
		{
			bool returnValue = false;

			try
			{
				this.Arguments = args;

				// ***
				// *** Create a new container.
				// ***
				this.Container = new UnityContainer();

				// ***
				// *** Add a named registration for the container.
				// ***
				_ = this.Container.RegisterInstance(typeof(IUnityContainer).Name, this.Container, new ContainerControlledLifetimeManager());

				// ***
				// *** Register the application context.
				// ***
				_ = this.Container.RegisterInstance<IApplicationContext>("ApplicationContext", this, new ContainerControlledLifetimeManager());

				if (this.OnInitializeContainer().Result)
				{
					returnValue = true;
				}
			}
			catch
			{
				returnValue = false;
			}

			return Task.FromResult(returnValue);
		}

		public virtual async Task<int> Run()
		{
			return await this.OnExecute();
		}

		protected virtual Task<bool> OnInitializeContainer()
		{
			return Task.FromResult(true);
		}

		protected virtual Task<int> OnExecute()
		{
			throw new NotImplementedException();
		}

		protected override void OnDisposeManagedObjects()
		{
			if (this.Container != null)
			{
				this.Container.Dispose();
				this.Container = null;
			}
		}
	}
}