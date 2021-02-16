//
// Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
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
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// 
	/// </summary>
	public static class ServiceDescriptorConfigurationDecorator
	{
		/// <summary>
		/// 
		/// </summary>
		public enum TypeSource
		{
			/// <summary>
			/// 
			/// </summary>
			Service,
			/// <summary>
			/// 
			/// </summary>
			Implemenation
		}

		/// <summary>
		/// 
		/// </summary>
		private static IDictionary<string, Alias> AliasList = new Dictionary<string, Alias>();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="aliasList"></param>
		public static void Set(this IEnumerable<Alias> aliasList)
		{
			try
			{
				AliasList = aliasList.ToDictionary(p => $"<{p.Key}>");
			}
			catch (ArgumentException ex)
			{
				string message = ex.Message.Replace("An item with the same key has already been added. Key: ", "");
				throw new DuplicateAliasException(message);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public static IConfiguration Configuration { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <param name="typeSource"></param>
		/// <returns></returns>
		public static Type FindType(this ServiceDescriptorConfiguration item, TypeSource typeSource)
		{
			Type returnValue = null;

			//
			// Get the actual type definition. The type definition
			// passed here can contain one or more aliases.
			//
			string actualTypeDefinition = item.TransformAlias(typeSource);

			//
			// Get the type.
			//
			returnValue = Type.GetType(actualTypeDefinition, true);

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <param name="typeSource"></param>
		/// <returns></returns>
		public static string ExtractTypeDefinition(this ServiceDescriptorConfiguration item, TypeSource typeSource)
		{
			string returnValue = null;

			if (typeSource == TypeSource.Implemenation)
			{
				returnValue = item.ImplementationType;
			}
			else
			{
				returnValue = item.ServiceType;
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <param name="typeSource"></param>
		/// <returns></returns>
		public static string TransformAlias(this ServiceDescriptorConfiguration item, TypeSource typeSource)
		{
			string returnValue = item.ExtractTypeDefinition(typeSource);

			//
			// Use regular expressions to replace aliases.
			//
			Regex regex = new Regex("<(\"[^\"]*\"|'[^']*'|[^'\">])*>");
			returnValue = regex.Replace(returnValue, m =>
			{
				string returnValue = null;

				if (AliasList.ContainsKey(m.Value))
				{
					returnValue = AliasList[m.Value].TypeDefinition;
				}
				else
				{
					//
					// This alias was not found; throw an exception.
					//
					throw new AliasNotFoundException(m.Value);
				}

				return returnValue;
			});

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool ShouldCreate(this ServiceDescriptorConfiguration item)
		{
			bool returnValue = false;

			if (item.Condition != null)
			{
				returnValue = Configuration[item.Condition.Key] == item.Condition.Value;
			}
			else
			{
				returnValue = true;
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static ServiceDescriptor CreateServiceDescriptor(this ServiceDescriptorConfiguration item)
		{
			ServiceDescriptor returnValue = null;

			if (item.ShouldCreate())
			{
				//
				// Get the service type.
				//
				Type serviceType = item.FindType(TypeSource.Service);

				//
				// Get the implementation type.
				//
				Type implementationType = item.FindType(TypeSource.Implemenation);

				//
				// Select the lifetime and create the descriptor.
				//
				switch (item.Lifetime)
				{
					case "Scoped":
						if (item.Properties == null)
						{
							//
							// Standard definition.
							//
							returnValue = ServiceDescriptor.Scoped(serviceType, implementationType);
						}
						else
						{
							//
							// Factory based definition.
							//
							returnValue = ServiceDescriptor.Scoped(serviceType, sp => (new DependencyFactory(implementationType, item)).GetInstance(sp));
						}
						break;
					case "Singleton":
						if (item.Properties == null)
						{
							//
							// Standard definition.
							//
							returnValue = ServiceDescriptor.Singleton(serviceType, implementationType);
						}
						else
						{
							//
							// Factory based definition.
							//
							returnValue = ServiceDescriptor.Singleton(serviceType, sp => (new DependencyFactory(implementationType, item)).GetInstance(sp));
						}
						break;
					case "Transient":
						if (item.Properties == null)
						{
							//
							// Standard definition.
							//
							returnValue = ServiceDescriptor.Transient(serviceType, implementationType);
						}
						else
						{
							//
							// Factory based definition.
							//
							returnValue = ServiceDescriptor.Transient(serviceType, sp => (new DependencyFactory(implementationType, item)).GetInstance(sp));
						}
						break;
				}
			}

			return returnValue;
		}
	}
}
