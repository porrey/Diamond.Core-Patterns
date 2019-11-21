using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;
using Unity;
using Unity.Lifetime;

namespace Diamond.Patterns.ObjectFactory.Unity
{
	public class ObjectFactory : IObjectFactory
	{
		public ObjectFactory(IUnityContainer unity)
		{
			this.Unity = unity;
		}

		internal IUnityContainer Unity { get; set; }

		public IEnumerable<object> GetAllInstances(Type objectType)
		{
			return this.Unity.ResolveAll(objectType);
		}

		public IEnumerable<TService> GetAllInstances<TService>()
		{
			return this.Unity.ResolveAll<TService>();
		}

		public object GetInstance(Type objectType, bool skipInitialization = false)
		{
			object returnValue = default;

			returnValue = this.Unity.Resolve(objectType);
			if (!skipInitialization) { this.InitializeIfRequiredAsync(returnValue).Wait(); }

			return returnValue;
		}

		public object GetInstance(Type objectType, string name, bool skipInitialization = false)
		{
			object returnValue = default;

			returnValue = this.Unity.Resolve(objectType, name);
			this.InitializeIfRequiredAsync(returnValue).Wait();

			return returnValue;
		}

		public TService GetInstance<TService>(bool skipInitialization = false)
		{
			TService returnValue = default;

			returnValue = this.Unity.Resolve<TService>();
			if (!skipInitialization) { this.InitializeIfRequiredAsync(returnValue).Wait(); }

			return returnValue;
		}

		public TService GetInstance<TService>(string name, bool skipInitialization = false)
		{
			TService returnValue = default;

			returnValue = this.Unity.Resolve<TService>(name);
			if (!skipInitialization) { this.InitializeIfRequiredAsync(returnValue).Wait(); }

			return returnValue;
		}

		public Task<IList<T>> ResolveByInterfaceAsync<T>()
		{
			// ***
			// *** Create a list of types T
			// ***
			IList<T> items = new List<T>();

			// ***
			// *** Check every registration to find objects that
			// *** implement interface T.
			// ***
			foreach (IContainerRegistration registration in this.Unity.Registrations)
			{
				Type source = registration.MappedToType;
				Type target = typeof(T);

				// ***
				// *** Check if the source type implements T.
				// ***
				if (target.IsAssignableFrom(source))
				{
					T instance = (T)this.Unity.Resolve(registration.RegisteredType, registration.Name);
					items.Add(instance);
				}
			}

			return Task.FromResult(items);
		}

		public void RegisterSingletonInstance<T>(string name, T instance)
		{
			_ = this.Unity.RegisterInstance(typeof(T), name, instance, new ContainerControlledLifetimeManager());
		}

		public async Task<bool> InitializeIfRequiredAsync(object item)
		{
			bool returnValue = false;

			if (item != null && item is IRequiresInitialization initializeObject)
			{
				if (initializeObject.CanInitialize && !initializeObject.IsInitialized)
				{
					returnValue = await initializeObject.InitializeAsync();
				}
				else
				{
					returnValue = true;
				}
			}
			else
			{
				returnValue = true;
			}

			return returnValue;
		}
	}
}
