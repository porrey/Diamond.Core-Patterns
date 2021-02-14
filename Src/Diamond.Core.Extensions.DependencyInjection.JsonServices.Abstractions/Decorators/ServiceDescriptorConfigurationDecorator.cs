using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Extensions.DependencyInjection.JsonServices
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
			AliasList = aliasList.ToDictionary(p => $"<{p.Key}>");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="typeDefinition"></param>
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
		/// <param name="typeDefinition"></param>
		/// <returns></returns>
		public static string TransformAlias(this ServiceDescriptorConfiguration item, TypeSource typeSource)
		{
			string returnValue = item.ExtractTypeDefinition(typeSource);

			//
			// Use regex to replace aliases.
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
		public static ServiceDescriptor CreateServiceDescriptor(this ServiceDescriptorConfiguration item)
		{
			ServiceDescriptor returnValue = null;

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
					returnValue = ServiceDescriptor.Scoped(serviceType, implementationType);
					break;
				case "Singleton":
					returnValue = ServiceDescriptor.Singleton(serviceType, implementationType);
					break;
				case "Transient":
					returnValue = ServiceDescriptor.Transient(serviceType, implementationType);
					break;
			}

			return returnValue;
		}
	}
}
