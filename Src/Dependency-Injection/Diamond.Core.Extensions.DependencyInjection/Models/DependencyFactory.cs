﻿//
// Copyright(C) 2019-2022, Daniel M. Porrey. All rights reserved.
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// 
	/// </summary>
	public class DependencyFactory : IDependencyFactory
	{
		/// <summary>
		/// 
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
		/// 
		/// </summary>
		public Type ImplementationType { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public ServiceDescriptorConfiguration Configuration { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<DependencyInfo> DependencyProperties { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public object GetInstance(IServiceProvider sp, params object[] parameters)
		{
			object instance = ActivatorUtilities.CreateInstance(sp, this.ImplementationType, parameters);
			this.AssignProperties(instance);
			DependencyAttribute.SetDependencyProperties(sp, this.DependencyProperties, instance);
			return instance;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="instance"></param>
		protected virtual void AssignProperties(object instance)
		{
			DependencyFactory.AssignProperties(this.Configuration.Properties, this.ImplementationType, instance);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="properties"></param>
		/// <param name="implementationType"></param>
		/// <param name="instance"></param>
		public static void AssignProperties(IDictionary<string, object> properties, Type implementationType, object instance)
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
				foreach (KeyValuePair<string, object> property in properties)
				{
					//
					// Check if this property exists on the instance.
					//
					PropertyInfo propertyInfo = propertyInfos.Where(t => t.Name.ToLower() == property.Key.ToLower()).SingleOrDefault();

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
								//
								// Attempt to convert and assign the value.
								//
								TypeConverter typeConverter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
								propertyInfo.SetValue(instance, typeConverter.ConvertFrom(property.Value));
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
		}
	}
}
