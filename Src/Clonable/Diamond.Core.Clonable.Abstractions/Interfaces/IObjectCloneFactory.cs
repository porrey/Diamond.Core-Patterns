using System;

namespace Diamond.Core.Clonable
{
	/// <summary>
	/// Defines an interface for a factory that can clone objects.
	/// </summary>
	public interface IObjectCloneFactory
	{
		/// <summary>
		/// Creates a deep clone of an object.
		/// </summary>
		/// <param name="instance">The object to clone.</param>
		/// <returns>The new object that s a deep clone of instance.</returns>
		object CloneInstance(ICloneable instance);

		/// <summary>
		/// Creates a deep clone of an object.
		/// </summary>
		/// <typeparam name="T">The type of the instance to clone.</typeparam>
		/// <param name="instance">The object to clone.</param>
		/// <returns>The new object that s a deep clone of instance.</returns>
		T CloneInstance<T>(T instance) where T : ICloneable;
	}
}
