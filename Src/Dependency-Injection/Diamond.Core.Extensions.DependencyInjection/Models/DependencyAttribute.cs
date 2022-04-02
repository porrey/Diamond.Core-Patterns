using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// 
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class DependencyAttribute : Attribute
	{
		/// <summary>
		/// 
		/// </summary>
		public DependencyAttribute()
			: base()
		{
		}

		/// <summary>
		/// Throw an exception if the property type is not in the container.
		/// </summary>
		public virtual bool Required { get; set; } = false;

		/// <summary>
		/// If the property is already set, override the
		/// value with the container value.
		/// </summary>
		public virtual bool OverrideValue { get; set; } = false;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static IEnumerable<DependencyInfo> GetDependencyProperties(Type type)
		{
			//
			// Get all properties on the object that have the Dependency attribute.
			//
			return (from tbl in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
					let x = tbl.GetCustomAttribute(typeof(DependencyAttribute))
					where x != null
					select new DependencyInfo()
					{
						PropertyInfo = tbl,
						DependencyAttribute = (DependencyAttribute)x,

					}).ToArray();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <param name="dependencyProperties"></param>
		/// <param name="instance"></param>
		public static void SetDependencyProperties(IServiceProvider serviceProvider, IEnumerable<DependencyInfo> dependencyProperties, object instance)
		{
			foreach (DependencyInfo dependencyProperty in dependencyProperties)
			{
				//
				// Get the dependency instance from the service provider.
				//
				object propertyInstance = serviceProvider.GetService(dependencyProperty.PropertyInfo.PropertyType);

				//
				// Check if the dependency existed or not.
				//
				if (propertyInstance != null)
				{
					//
					// Ensure the property is writable.
					//
					if (dependencyProperty.PropertyInfo.CanWrite)
					{
						//
						// Get the current property value.
						//
						object currentPropertyValue = dependencyProperty.PropertyInfo.GetValue(instance);

						//
						// Set the value if it is null or the override option has been specified.
						//
						if (currentPropertyValue == null || dependencyProperty.DependencyAttribute.OverrideValue)
						{
							//
							// Set the value.
							//
							dependencyProperty.PropertyInfo.SetValue(instance, propertyInstance);
						}
					}
					else
					{
						//
						// The property is read-only. Throw an exception.
						//
						throw new PropertyIsReadOnlyException(instance.GetType(), dependencyProperty.PropertyInfo.Name);
					}
				}
				else
				{
					//
					// Check if this dependency is required.
					//
					if (dependencyProperty.DependencyAttribute.Required)
					{
						//
						// Throw an exception.
						//
						throw new DependencyNotFoundException(instance.GetType(), dependencyProperty.PropertyInfo.Name, dependencyProperty.PropertyInfo.PropertyType);
					}
				}
			}
		}
	}
}
