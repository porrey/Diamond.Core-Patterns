using System;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;
using Unity;
using Unity.Lifetime;

namespace Diamond.Patterns.UnityConfiguration
{
	public class ConsoleUnityBootstrapper : DisposableObject, IApplicationContext, IExceptionContext
	{
		protected virtual IUnityContainer Container { get; set; }
		public virtual string[] Arguments { get; set; }
		public virtual IObjectFactory ObjectFactory => this.Container.Resolve<IObjectFactory>("ObjectFactory");
		public virtual string Name { get; }
		public Exception Exception { get; protected set; }

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

		public void SetException(Exception ex)
		{
			this.Exception = ex;
		}

		public void SetException(string message)
		{
			this.Exception = new Exception(message);
		}

		public void SetException(string format, params object[] args)
		{
			this.Exception = new Exception(String.Format(format, args));
		}
	}
}