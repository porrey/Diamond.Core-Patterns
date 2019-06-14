using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IObjectFactory
	{
		TService GetInstance<TService>();
		TService GetInstance<TService>(string name);
		object GetInstance(Type objectType);
		object GetInstance(Type objectType, string name);
		IEnumerable<TService> GetAllInstances<TService>();
		IEnumerable<object> GetAllInstances(Type objectType);
		Task<IList<T>> ResolveByInterfaceAsync<T>();
		void RegisterSingletonInstance<T>(string name, T instance);
	}
}
