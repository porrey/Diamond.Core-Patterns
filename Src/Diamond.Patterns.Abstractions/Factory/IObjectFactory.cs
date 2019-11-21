using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IObjectFactory
	{
		TService GetInstance<TService>(bool skipInitialization = false);
		TService GetInstance<TService>(string name, bool skipInitialization = false);
		object GetInstance(Type objectType, bool skipInitialization = false);
		object GetInstance(Type objectType, string name, bool skipInitialization = false);
		IEnumerable<TService> GetAllInstances<TService>();
		IEnumerable<object> GetAllInstances(Type objectType);
		Task<IList<T>> ResolveByInterfaceAsync<T>();
		void RegisterSingletonInstance<T>(string name, T instance);
		Task<bool> InitializeIfRequiredAsync(object item);
	}
}
