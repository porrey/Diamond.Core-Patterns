using Unity;
using Unity.Lifetime;

#pragma warning disable DF0001
#pragma warning disable DF0000

namespace Diamond.Patterns.Mocks.Unity
{
	public static class IUnityContainerDecorator
	{
		public static void RemoveRegistration<TInterface>(this IUnityContainer container, string name)
		{
			// ***
			// *** Replace the registration with a factory that throws the same
			// *** exception the container would if a registration was not found.
			// ***
			_ = container.RegisterFactory<TInterface>(name, x => throw new ResolutionFailedException(typeof(TInterface), name, "Error thrown to simulate not registered condition."), new SingletonLifetimeManager());
		}
	}
}
