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

		protected IUnityContainer Unity { get; set; }

		public IEnumerable<object> GetAllInstances(Type objectType)
		{
			return this.Unity.ResolveAll(objectType);
		}

		public IEnumerable<TService> GetAllInstances<TService>()
		{
			return this.Unity.ResolveAll<TService>();
		}

		public object GetInstance(Type objectType)
		{
			return this.Unity.Resolve(objectType);
		}

		public object GetInstance(Type objectType, string name)
		{
			return this.Unity.Resolve(objectType, name);
		}

		public TService GetInstance<TService>()
		{
			return this.Unity.Resolve<TService>();
		}

		public TService GetInstance<TService>(string name)
		{
			return this.Unity.Resolve<TService>(name);
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
			this.Unity.RegisterInstance(typeof(T), name, instance, new ContainerControlledLifetimeManager());
		}
	}
}
