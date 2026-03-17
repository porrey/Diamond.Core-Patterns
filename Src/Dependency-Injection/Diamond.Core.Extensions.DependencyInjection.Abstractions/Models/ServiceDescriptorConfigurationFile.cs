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
namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// Represents the configuration for service descriptors, including aliases, services, 
	/// and databases, as defined in a configuration file.
	/// </summary>
	public class ServiceDescriptorConfigurationFile
	{
		/// <summary>
		/// Gets or sets the collection of alias configurations associated with this object.
		/// </summary>
		public AliasDescriptorConfiguration[] Aliases { get; set; }

		/// <summary>
		/// Gets or sets the collection of service descriptor configurations associated with this object.
		/// </summary>
		public ServiceDescriptorConfiguration[] Services { get; set; }

		/// <summary>
		/// Gets or sets the collection of database configurations used by the application.
		/// </summary>
		public DatabaseDescriptorConfiguration[] Databases { get; set; }
	}
}
