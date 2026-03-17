//
// Copyright(C) 2019-2026, Daniel M. Porrey. All rights reserved.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see http://www.gnu.org/licenses/.
//
using System.ComponentModel;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// Provides functionality for creating and configuring instances of a specified implementation type, including
	/// assignment of dependency properties and configuration values.
	/// </summary>
	/// <remarks>The class supports dependency injection scenarios by allowing property assignment and configuration
	/// of implementation types at runtime. It is typically used to instantiate services with custom property values and
	/// dependencies resolved from a service provider. Thread safety is not guaranteed; concurrent access should be managed
	/// externally if required.</remarks>
	public class DependencyFactory : IDependencyFactory
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DependencyFactory"/> class.
		/// </summary>
		/// <param name="implementationType"></param>
		/// <param name="configuration"></param>
		/// <param name="dependencyProperties"></param>
		public DependencyFactory(Type implementationType, ServiceDescriptorConfiguration configuration, IEnumerable<DependencyInfo> dependencyProperties)
		{
			this.ImplementationType = implementationType;
			this.Configuration = configuration;
			this.DependencyProperties = dependencyProperties;
		}

		/// <summary>
		/// Initializes a new instance of the DependencyFactory class with the specified service key, implementation type,
		/// configuration, and dependency properties.
		/// </summary>
		/// <param name="serviceKey">The unique key identifying the service for which dependencies are being managed. Cannot be null or empty.</param>
		/// <param name="implementationType">The type that implements the service. Used to resolve dependencies for the specified service.</param>
		/// <param name="configuration">The configuration settings that control how the service descriptor is constructed and managed.</param>
		/// <param name="dependencyProperties">A collection of dependency information objects representing the properties required by the implementation type.
		/// Cannot be null.</param>
		public DependencyFactory(object serviceKey, Type implementationType, ServiceDescriptorConfiguration configuration, IEnumerable<DependencyInfo> dependencyProperties)
		{
			this.ServiceKey = serviceKey;
			this.ImplementationType = implementationType;
			this.Configuration = configuration;
			this.DependencyProperties = dependencyProperties;
		}

		/// <summary>
		/// Gets or sets the unique key used to identify the service instance.
		/// </summary>
		public object ServiceKey { get; set; }

		/// <summary>
		/// Gets or sets the type of the implementation.
		/// </summary>
		public virtual Type ImplementationType { get; set; }

		/// <summary>
		/// Gets or sets the service descriptor configuration.
		/// </summary>
		public virtual ServiceDescriptorConfiguration Configuration { get; set; }

		/// <summary>
		/// Gets or sets the dependency properties that are required for the implementation type.
		/// </summary>
		public virtual IEnumerable<DependencyInfo> DependencyProperties { get; set; }

		/// <summary>
		///	 Creates an instance of the implementation type using the provided service provider and parameters.
		/// </summary>
		/// <returns></returns>
		public virtual object GetInstance(IServiceProvider sp, params object[] parameters)
		{
			object instance = ActivatorUtilities.CreateInstance(sp, this.ImplementationType, parameters);
			this.AssignProperties(instance);
			DependencyAttribute.SetDependencyProperties(sp, this.DependencyProperties, instance);
			return instance;
		}

		/// <summary>
		/// Assigns the properties from the configuration to the instance of the implementation type.
		/// </summary>
		/// <param name="instance"></param>
		protected virtual void AssignProperties(object instance)
		{
			DependencyFactory.AssignProperties(this.Configuration.Properties, this.Configuration.ArrayProperties, this.ImplementationType, instance);
		}

		/// <summary>
		/// Assigns values to the properties of the specified instance using the provided property dictionaries.
		/// </summary>
		/// <remarks>Only properties that are writable and exist on the instance type will be assigned. Property names
		/// are matched using a case-insensitive comparison. Array values are converted as needed before assignment.
		/// Exceptions provide detailed context about the property and value causing the error.</remarks>
		/// <param name="properties">A dictionary containing property names and their corresponding values to assign to the instance. Property names
		/// are matched case-insensitively.</param>
		/// <param name="arrayProperties">A dictionary containing property names and their corresponding array values to assign to the instance. Property
		/// names are matched case-insensitively.</param>
		/// <param name="implementationType">The type representing the implementation for which properties are being assigned. Used for exception context.</param>
		/// <param name="instance">The object instance whose properties will be assigned values from the provided dictionaries.</param>
		/// <exception cref="PropertyConversionException">Thrown if a property value cannot be converted to the target property type during assignment.</exception>
		/// <exception cref="PropertyIsReadOnlyException">Thrown if an attempt is made to assign a value to a read-only property.</exception>
		/// <exception cref="PropertyNotFoundException">Thrown if a property specified in the dictionaries does not exist on the instance type.</exception>
		public static void AssignProperties(IDictionary<string, object> properties, IDictionary<string, object[]> arrayProperties, Type implementationType, object instance)
		{
			if (properties != null && properties.Any())
			{
				//
				// Get the instance type.
				//
				Type instanceType = instance.GetType();

				//
				// Get the properties for the instance type.
				//
				PropertyInfo[] propertyInfos = instanceType.GetProperties();

				//
				// Loop through each property in the configuration and attempt to assign
				// it to the matching property on the instance.
				//
				if (properties != null)
				{
					foreach (KeyValuePair<string, object> property in properties)
					{
						//
						// Check if this property exists on the instance.
						//
						PropertyInfo propertyInfo = propertyInfos.Where(t => t.Name.Equals(property.Key, StringComparison.CurrentCultureIgnoreCase)).SingleOrDefault();

						if (propertyInfo != null)
						{
							//
							// Check if the property can be set.
							//
							if (propertyInfo.CanWrite)
							{
								//
								// The property was found.
								//
								try
								{
									if (property.Value != null)
									{
										//
										// Attempt to convert and assign the value.
										//
										TypeConverter typeConverter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
										propertyInfo.SetValue(instance, typeConverter.ConvertFrom(property.Value));
									}
								}
								catch (Exception ex)
								{
									//
									// The property value assignment failed. Throw an exception.
									//
									throw new PropertyConversionException(implementationType, property.Key, property.Value, ex);
								}
							}
							else
							{
								//
								// The property is read-only. Throw an exception.
								//
								throw new PropertyIsReadOnlyException(implementationType, property.Key, property.Value);
							}
						}
						else
						{
							//
							// The instance type did not have the current property. Throw an exception.
							//
							throw new PropertyNotFoundException(implementationType, property.Key);
						}
					}
				}

				//
				// Loop through each property in the configuration and attempt to assign
				// it to the matching property on the instance.
				//
				if (arrayProperties != null)
				{
					foreach (KeyValuePair<string, object[]> arrayProperty in arrayProperties)
					{
						//
						// Check if this property exists on the instance.
						//
						PropertyInfo propertyInfo = propertyInfos.Where(t => t.Name.Equals(arrayProperty.Key, StringComparison.CurrentCultureIgnoreCase)).SingleOrDefault();

						if (propertyInfo != null)
						{
							//
							// Check if the property can be set.
							//
							if (propertyInfo.CanWrite)
							{
								//
								// The property was found.
								//
								try
								{
									if (arrayProperty.Value != null)
									{
										//
										// Attempt to convert and assign the value.
										//
										Array array = arrayProperty.Value.ConvertArray(instance, propertyInfo);
										propertyInfo.SetValue(instance, array);
									}
								}
								catch (Exception ex)
								{
									//
									// The property value assignment failed. Throw an exception.
									//
									throw new PropertyConversionException(implementationType, arrayProperty.Key, arrayProperty.Value, ex);
								}
							}
							else
							{
								//
								// The property is read-only. Throw an exception.
								//
								throw new PropertyIsReadOnlyException(implementationType, arrayProperty.Key, arrayProperty.Value);
							}
						}
						else
						{
							//
							// The instance type did not have the current property. Throw an exception.
							//
							throw new PropertyNotFoundException(implementationType, arrayProperty.Key);
						}
					}
				}
			}
		}
	}
}
