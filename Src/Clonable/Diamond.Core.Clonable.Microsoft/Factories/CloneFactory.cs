using System;
using System.Text.Json;

namespace Diamond.Core.Clonable.NetCore
{
	/// <summary>
	/// Implements IObjectCloneFactory using Newtonsoft JSON.Net.
	/// </summary>
	public class ObjectCloneFactory : IObjectCloneFactory
	{
		/// <summary>
		/// Creates a deep clone of an object.
		/// </summary>
		/// <param name="instance">The object to clone.</param>
		/// <returns>The new object that s a deep clone of instance.</returns>
		public object CloneInstance(ICloneable instance)
		{
			object returnValue = null;

			//
			// Tell the converter to store the type names.
			//
			JsonSerializerOptions settings = new JsonSerializerOptions()
			{				
			};

			//
			// Serialize this instance
			//
			string json = JsonSerializer.Serialize(instance, settings);

			//
			// De-serialize into a new instance
			//
			returnValue = JsonSerializer.Deserialize<object>(json, settings);

			return returnValue;
		}

		/// <summary>
		/// Creates a deep clone of an object.
		/// </summary>
		/// <typeparam name="T">The type of the instance to clone.</typeparam>
		/// <param name="instance">The object to clone.</param>
		/// <returns>The new object that s a deep clone of instance.</returns>
		public  T CloneInstance<T>(T instance)
			where T : ICloneable
		{
			T returnValue = default;

			//
			// Tell the converter to store the type names.
			//
			JsonSerializerOptions settings = new JsonSerializerOptions()
			{
			};

			//
			// Serialize this instance
			//
			string json = JsonSerializer.Serialize(instance, settings);

			//
			// De-serialize into a new instance
			//
			returnValue = JsonSerializer.Deserialize<T>(json, settings);

			return returnValue;
		}
	}
}
