using System;
using Newtonsoft.Json;

namespace Diamond.Patterns.System
{
	/// <summary>
	/// Supports cloning, which creates a new instance of a class with the same value
	/// as an existing instance.
	/// </summary>
	public abstract class Cloneable : ICloneable
	{
		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			return this.OnClone();
		}

		/// <summary>
		/// A class that inherits from Cloneable can implement this
		/// member to override the default cloning mechanism.
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		protected virtual object OnClone()
		{
			object returnValue = null;

			lock (this)
			{
				// ***
				// *** Tell the converter to store the type names.
				// ***
				JsonSerializerSettings settings = new JsonSerializerSettings()
				{
					ObjectCreationHandling = ObjectCreationHandling.Replace,
					TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,
					TypeNameHandling = TypeNameHandling.All,
					ReferenceLoopHandling = ReferenceLoopHandling.Serialize
				};

				// ***
				// *** Serialize this instance
				// ***
				string json = JsonConvert.SerializeObject(this, settings);

				// ***
				// *** De-serialize into a new instance
				// ***
				returnValue = JsonConvert.DeserializeObject(json, settings);
			}

			return returnValue;
		}
	}
}
