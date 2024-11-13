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
namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// 
	/// </summary>
	public class DatabaseDescriptorConfiguration : ServiceDescriptorConfiguration
	{
		/// <summary>
		/// Get/sets the DbContext object.
		/// </summary>
		public virtual string Context
		{
			get
			{
				return this.ImplementationType;
			}
			set
			{
				this.ImplementationType = value;
				this.ServiceType = value;
			}
		}

		/// <summary>
		/// Gets/sets the connection string for the database.
		/// </summary>
		public virtual string ConnectionString { get; set; }

		/// <summary>
		/// Gets/sets the timeout in seconds for a command.
		/// </summary>
		public virtual int? CommandTimeout { get; set; }

		/// <summary>
		/// Gets/sets the factory used to configured the DbContext.
		/// </summary>
		public virtual string Factory { get; set; }
	}
}
