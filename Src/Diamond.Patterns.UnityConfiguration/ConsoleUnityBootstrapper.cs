using System;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;
using Unity;
using Unity.Lifetime;

#pragma warning disable DF0000

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

		public static ILoggerSubscriber LoggerSubscriber { get; set; }

		public virtual IUnityContainer Container { get; set; }
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
				ConsoleUnityBootstrapper.LoggerSubscriber?.Verbose("Initializing application.");
				this.Arguments = args;

				// ***
				// *** Create a new container.
				// ***
				this.Container = new UnityContainer();
				ConsoleUnityBootstrapper.LoggerSubscriber?.Verbose("Created Unity container.");

				// ***
				// *** Add a named registration for the container.
				// ***
				_ = this.Container.RegisterInstance(typeof(IUnityContainer).Name, this.Container, new ContainerControlledLifetimeManager());
				ConsoleUnityBootstrapper.LoggerSubscriber?.Verbose($"Added Unity Container to the container with name '{typeof(IUnityContainer).Name}'.");

				// ***
				// *** Register the application context.
				// ***
				_ = this.Container.RegisterInstance<IApplicationContext>("ApplicationContext", this, new ContainerControlledLifetimeManager());
				ConsoleUnityBootstrapper.LoggerSubscriber?.Verbose("Added IApplicationContext to the container with name 'ApplicationContext'.");

				ConsoleUnityBootstrapper.LoggerSubscriber?.Verbose("Calling initialize container.");
				if (this.OnInitializeContainer().Result)
				{
					ConsoleUnityBootstrapper.LoggerSubscriber?.Verbose("Initialize container completed.");
					returnValue = true;
				}
			}
			catch (Exception ex)
			{
				ConsoleUnityBootstrapper.LoggerSubscriber?.Exception(ex);
				returnValue = false;
			}

			return Task.FromResult(returnValue);
		}

		public virtual async Task<int> Run()
		{
			ConsoleUnityBootstrapper.LoggerSubscriber?.Verbose("Starting Application.Run().");
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
			lock (this.Container)
			{
				if (this.Container != null)
				{
					ConsoleUnityBootstrapper.LoggerSubscriber?.Verbose("Disposing Unity Container.");
					this.Container.Dispose();
					this.Container = null;
				}
			}
		}
	}
}