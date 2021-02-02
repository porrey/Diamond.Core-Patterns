// ***
// *** Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;
using Unity;
using Unity.Lifetime;

#pragma warning disable DF0000

namespace Diamond.Patterns.ObjectFactory.Unity
{
	public class ObjectFactory : IObjectFactory, ILoggerPublisher
	{
		public ObjectFactory(IUnityContainer unity)
		{
			this.Unity = unity;
		}

		public ObjectFactory(IUnityContainer unity, ILoggerSubscriber loggerSubscriber)
		{
			this.Unity = unity;
			this.LoggerSubscriber = loggerSubscriber;
		}

		internal IUnityContainer Unity { get; set; }

		public ILoggerSubscriber LoggerSubscriber { get; set; } = new NullLoggerSubscriber();

		public IEnumerable<object> GetAllInstances(Type objectType)
		{
			IEnumerable<object> returnValue = this.Unity.ResolveAll(objectType);

			returnValue.Select(t => this.LoggerSubscriber.AddToInstance(t));
			returnValue.Select(t => this.InitializeIfRequiredAsync(t));

			return returnValue;
		}

		public IEnumerable<TService> GetAllInstances<TService>()
		{
			IEnumerable<TService> returnValue = this.Unity.ResolveAll<TService>();

			returnValue.Select(t => this.LoggerSubscriber.AddToInstance(t));
			returnValue.Select(t => this.InitializeIfRequiredAsync(t));

			return returnValue;
		}

		public object GetInstance(Type objectType, bool skipInitialization = false)
		{
			object returnValue = default;

			returnValue = this.Unity.Resolve(objectType);

			this.LoggerSubscriber.AddToInstance(returnValue);

			if (!skipInitialization)
			{
				this.InitializeIfRequiredAsync(returnValue).Wait();
			}

			return returnValue;
		}

		public object GetInstance(Type objectType, string name, bool skipInitialization = false)
		{
			object returnValue = default;

			returnValue = this.Unity.Resolve(objectType, name);
			this.LoggerSubscriber.AddToInstance(returnValue);

			if (!skipInitialization)
			{
				this.InitializeIfRequiredAsync(returnValue).Wait();
			}

			return returnValue;
		}

		public TService GetInstance<TService>(bool skipInitialization = false)
		{
			TService returnValue = default;

			returnValue = this.Unity.Resolve<TService>();
			this.LoggerSubscriber.AddToInstance(returnValue);

			if (!skipInitialization)
			{
				this.InitializeIfRequiredAsync(returnValue).Wait();
			}

			return returnValue;
		}

		public TService GetInstance<TService>(string name, bool skipInitialization = false)
		{
			TService returnValue = default;

			returnValue = this.Unity.Resolve<TService>(name);
			this.LoggerSubscriber.AddToInstance(returnValue);

			if (!skipInitialization)
			{
				this.InitializeIfRequiredAsync(returnValue).Wait();
			}

			return returnValue;
		}

		public Task<IList<T>> ResolveByInterfaceAsync<T>()
		{
			// ***
			// *** Create a list of types T
			// ***
			IList<T> returnValue = new List<T>();

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
					returnValue.Add(instance);
				}
			}

			returnValue.Select(t => this.LoggerSubscriber.AddToInstance(t));
			returnValue.Select(t => this.InitializeIfRequiredAsync(t));

			return Task.FromResult(returnValue);
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
					this.LoggerSubscriber.Verbose($"Initializing instance of '{item.GetType().Name}'.");
					returnValue = await initializeObject.InitializeAsync();
				}
				else
				{
					this.LoggerSubscriber.Verbose($"The instance of '{item.GetType().Name}' is either already initialized or cannot be initialized at this time.");
					returnValue = true;
				}
			}
			else
			{
				this.LoggerSubscriber.Verbose($"The instance of '{item.GetType().Name}' does not implement IRequiresInitialization.");
				returnValue = true;
			}

			return returnValue;
		}
	}
}
