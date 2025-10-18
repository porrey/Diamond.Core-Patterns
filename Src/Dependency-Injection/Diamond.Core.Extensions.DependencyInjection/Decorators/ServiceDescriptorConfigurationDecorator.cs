//
// Copyright(C) 2019-2025, Daniel M. Porrey. All rights reserved.
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
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// Provides extension methods and utilities for configuring and creating service descriptors in dependency injection
	/// scenarios. This class includes methods for alias transformation, type resolution, and service descriptor creation.
	/// </summary>
	/// <remarks>The <see cref="ServiceDescriptorConfigurationDecorator"/> class is designed to simplify the process
	/// of configuring and managing service descriptors, including support for alias resolution and dependency injection.
	/// It provides methods to handle service and implementation types, create service descriptors, and manage
	/// configuration-based conditions.</remarks>
	public static class ServiceDescriptorConfigurationDecorator
	{
		/// <summary>
		/// Specifies the source type for a given object or service in the application.
		/// </summary>
		public enum TypeSource
		{
			/// <summary>
			/// Represents a service that provides functionality or operations within the application.
			/// </summary>
			/// <remarks>This class serves as a base or core component for implementing specific service-related logic.
			/// It can be extended or instantiated to perform various tasks depending on the application's
			/// requirements.</remarks>
			Service,
			/// <summary>
			/// Represents the implementation of a specific functionality or behavior.
			/// </summary>
			/// <remarks>This class or method serves as a placeholder for the actual implementation.  Ensure that the
			/// specific functionality is defined and documented appropriately  when extending or using this
			/// implementation.</remarks>
			Implemenation
		}

		/// <summary>
		/// Represents a collection of aliases mapped by their unique string identifiers.
		/// </summary>
		/// <remarks>This dictionary is used to store and retrieve <see cref="Alias"/> objects based on their
		/// associated string keys. It is initialized as an empty dictionary and is intended for internal use only.</remarks>
		private static IDictionary<string, Alias> AliasList = new Dictionary<string, Alias>();

		/// <summary>
		/// Updates the internal alias dictionary with the specified collection of aliases.
		/// </summary>
		/// <remarks>This method converts the provided collection of aliases into a dictionary, using each alias's key
		/// as the dictionary key. If duplicate keys are detected, a <see cref="DuplicateAliasException"/> is thrown,
		/// providing details about the conflicting keys.</remarks>
		/// <param name="aliasList">A collection of <see cref="Alias"/> objects to be added to the alias dictionary. Each alias must have a unique
		/// key.</param>
		/// <exception cref="DuplicateAliasException">Thrown when the collection contains duplicate keys, indicating that multiple aliases share the same key.</exception>
		public static void Set(this IEnumerable<Alias> aliasList)
		{
			try
			{
				AliasList = aliasList.ToDictionary(p => $"<{p.Key}>");
			}
			catch (ArgumentException ex)
			{
				string duplicateKeys = string.Join("; ", AliasList.Keys.GroupBy(t => t).Where(t => t.Count() > 1).Select(t => t.Key).ToList());
				string message = ex.Message.Replace("An item with the same key has already been added. Key: ", "");
				throw new DuplicateAliasException(message);
			}
		}

		/// <summary>
		/// Gets or sets the application's configuration settings.
		/// </summary>
		/// <remarks>This property is typically used to retrieve or modify configuration settings for the application,
		/// such as connection strings, application settings, or environment-specific values.</remarks>
		public static IConfiguration Configuration { get; set; }

		/// <summary>
		/// Retrieves the <see cref="Type"/> corresponding to the specified type source, resolving any aliases defined in the
		/// service descriptor configuration.
		/// </summary>
		/// <remarks>This method resolves the actual type definition by transforming any aliases present in the
		/// provided type source. It then attempts to load the corresponding <see cref="Type"/> using the resolved type
		/// definition.</remarks>
		/// <param name="item">The <see cref="ServiceDescriptorConfiguration"/> instance containing the alias definitions and transformation
		/// logic.</param>
		/// <param name="typeSource">The source of the type to resolve, which may include aliases.</param>
		/// <returns>The resolved <see cref="Type"/> based on the provided type source.</returns>
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
		/// Retrieves the <see cref="Type"/> object associated with the specified type name.
		/// </summary>
		/// <remarks>The method resolves any aliases in the provided <paramref name="typeName"/> before attempting to
		/// locate the type.</remarks>
		/// <param name="typeName">The name of the type to locate. This can include aliases that will be resolved to the actual type definition.</param>
		/// <returns>The <see cref="Type"/> object representing the specified type.</returns>
		private static Type FindType(string typeName)
		{
			Type returnValue = null;

			//
			// Get the actual type definition. The type definition
			// passed here can contain one or more aliases.
			//
			string actualTypeDefinition = TransformAlias(typeName);

			//
			// Get the type.
			//
			returnValue = Type.GetType(actualTypeDefinition, true);

			return returnValue;
		}

		/// <summary>
		/// Extracts the type definition from the specified <see cref="ServiceDescriptorConfiguration"/> based on the provided
		/// <see cref="TypeSource"/>.
		/// </summary>
		/// <param name="item">The <see cref="ServiceDescriptorConfiguration"/> instance from which to extract the type definition.</param>
		/// <param name="typeSource">Specifies the source of the type definition to extract. Use <see cref="TypeSource.Implemenation"/> to extract the
		/// implementation type, or <see cref="TypeSource.Service"/> to extract the service type.</param>
		/// <returns>A <see cref="string"/> representing the extracted type definition. Returns the implementation type if <paramref
		/// name="typeSource"/> is <see cref="TypeSource.Implemenation"/>, or the service type if <paramref
		/// name="typeSource"/> is <see cref="TypeSource.Service"/>.</returns>
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
		/// Transforms the alias of the specified <see cref="ServiceDescriptorConfiguration"/> based on the provided <see
		/// cref="TypeSource"/>.
		/// </summary>
		/// <remarks>This method uses the type definition extracted from the <paramref name="item"/> and the provided
		/// <paramref name="typeSource"/> to perform the alias transformation.</remarks>
		/// <param name="item">The <see cref="ServiceDescriptorConfiguration"/> instance whose alias is to be transformed.</param>
		/// <param name="typeSource">The <see cref="TypeSource"/> used to extract the type definition for the transformation.</param>
		/// <returns>A string representing the transformed alias.</returns>
		public static string TransformAlias(this ServiceDescriptorConfiguration item, TypeSource typeSource)
		{
			return TransformAlias(item.ExtractTypeDefinition(typeSource));
		}

		/// <summary>
		/// Replaces alias placeholders in the specified string with their corresponding type definitions.
		/// </summary>
		/// <remarks>This method uses regular expressions to identify alias placeholders in the input string. Each
		/// placeholder is replaced with its corresponding type definition from the alias list. If a placeholder does not have
		/// a matching alias, an <see cref="AliasNotFoundException"/> is thrown.</remarks>
		/// <param name="value">The input string containing alias placeholders to be replaced.</param>
		/// <returns>A string where all alias placeholders have been replaced with their corresponding type definitions.</returns>
		/// <exception cref="AliasNotFoundException">Thrown if an alias placeholder in the input string does not exist in the alias list.</exception>
		private static string TransformAlias(string value)
		{
			string returnValue = value;

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
		/// Determines whether a new service descriptor should be created based on the specified configuration.
		/// </summary>
		/// <remarks>If the <paramref name="item"/> contains a condition, the method evaluates whether the
		/// configuration value associated with the condition's key matches the expected value. If no condition is specified,
		/// the method returns <see langword="true"/> by default.</remarks>
		/// <param name="item">The <see cref="ServiceDescriptorConfiguration"/> instance containing the condition to evaluate.</param>
		/// <returns><see langword="true"/> if the condition specified in <paramref name="item"/> is met or if no condition is defined;
		/// otherwise, <see langword="false"/>.</returns>
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
		/// Creates a <see cref="ServiceDescriptor"/> based on the specified <see cref="ServiceDescriptorConfiguration"/>.
		/// </summary>
		/// <remarks>This method evaluates the provided <see cref="ServiceDescriptorConfiguration"/> to determine
		/// whether a service descriptor should be created. If the configuration specifies valid service and implementation
		/// types, the method creates a descriptor with the appropriate lifetime and dependencies. If additional properties or
		/// dependencies are defined, a factory-based descriptor is created instead of a standard descriptor.</remarks>
		/// <param name="item">The <see cref="ServiceDescriptorConfiguration"/> that defines the service type, implementation type, lifetime, and
		/// other configuration details required to create the service descriptor.</param>
		/// <returns>A <see cref="ServiceDescriptor"/> representing the service registration. Returns <see langword="null"/> if the
		/// configuration does not meet the criteria for creating a service descriptor.</returns>
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
				// Check if the implementation type has any dependency properties.
				//
				IEnumerable<DependencyInfo> dependencyProperties = DependencyAttribute.GetDependencyProperties(implementationType);

				//
				// Select the lifetime.
				//
				ServiceLifetime lifetime = item.Lifetime == "Scoped" ? ServiceLifetime.Scoped : item.Lifetime == "Singleton" ? ServiceLifetime.Singleton : ServiceLifetime.Transient;

				//
				// Create the descriptor.
				//
				if (item.Properties == null && !dependencyProperties.Any())
				{
					//
					// Standard definition.
					//
					returnValue = ServiceDescriptor.Describe(serviceType, implementationType, lifetime);
				}
				else
				{
					//
					// Factory based definition.
					//
					returnValue = ServiceDescriptor.Describe(serviceType, sp => (new DependencyFactory(implementationType, item, dependencyProperties)).GetInstance(sp), lifetime);
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Creates a <see cref="ServiceDescriptor"/> for a database context based on the specified configuration.
		/// </summary>
		/// <remarks>This method uses the provided configuration to resolve the service type, implementation type, and
		/// factory type. It creates a service descriptor that registers the database context with the specified lifetime
		/// (Scoped, Singleton, or Transient). The method also handles dependency injection for the database context,
		/// including resolving connection strings, command timeouts, and other dependencies.</remarks>
		/// <param name="item">The <see cref="DatabaseDescriptorConfiguration"/> containing the configuration details for creating the database
		/// descriptor.</param>
		/// <returns>A <see cref="ServiceDescriptor"/> that describes the database context service, or <see langword="null"/> if the
		/// configuration indicates that the service should not be created.</returns>
		/// <exception cref="DbContextNotFoundException">Thrown if the service type or implementation type cannot be resolved from the configuration.</exception>
		/// <exception cref="DbContextDependencyFactoryNotFoundException">Thrown if the factory type specified in the configuration cannot be resolved.</exception>
		public static ServiceDescriptor CreateDatabaseDescriptor(this DatabaseDescriptorConfiguration item)
		{
			ServiceDescriptor returnValue = null;

			if (item.ShouldCreate())
			{
				//
				// Get the service type.
				//
				Type serviceType = null;
				Type implementationType = null;
				IEnumerable<DependencyInfo> dependenctProperties = Array.Empty<DependencyInfo>();

				try
				{
					serviceType = item.FindType(TypeSource.Service);
					implementationType = item.FindType(TypeSource.Implemenation);
					dependenctProperties = DependencyAttribute.GetDependencyProperties(implementationType);
				}
				catch
				{
					throw new DbContextNotFoundException(item.Context);
				}

				//
				// Get the factory type.
				//
				Type factoryType = null;

				try
				{
					factoryType = FindType(item.Factory);
				}
				catch
				{
					throw new DbContextDependencyFactoryNotFoundException(item.Factory);
				}

				//
				// Create the service descriptor.
				//
				returnValue = ServiceDescriptor.Describe(serviceType, (sp) =>
				{
					//
					// Get the connection string.
					//
					IConfiguration configuration = sp.GetService<IConfiguration>();
					string connectionString = configuration[item.ConnectionString];

					//
					// Get the command timeout
					//
					int? commandTimeout = item.CommandTimeout;

					//
					// Get the dependency factory and assign the properties.
					//
					IDependencyFactory dependencyFactory = (IDependencyFactory)ActivatorUtilities.CreateInstance(sp, factoryType, implementationType, item);
					DependencyFactory.AssignProperties(item.Properties, implementationType, dependencyFactory);

					//
					// Create the context and set the dependencies.
					//
					object instance = null;

					if (commandTimeout.HasValue)
					{
						instance = dependencyFactory.GetInstance(sp, connectionString, commandTimeout.Value);
					}
					else
					{
						instance = dependencyFactory.GetInstance(sp, connectionString);
					}

					DependencyAttribute.SetDependencyProperties(sp, dependenctProperties, instance);

					//
					// Return the context instance.
					//
					return instance;
				}, item.Lifetime == "Scoped" ? ServiceLifetime.Scoped : item.Lifetime == "Singleton" ? ServiceLifetime.Singleton : ServiceLifetime.Transient);
			}

			return returnValue;
		}
	}
}
