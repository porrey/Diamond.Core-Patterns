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
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Extensions.JsonServices
{
	/// <summary>
	/// 
	/// </summary>
	public class ServiceDescriptorConfiguration
	{
		/// <summary>
		/// 
		/// </summary>
		public string ImplementationType { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Lifetime { get; set; } = ServiceLifetime.Scoped.ToString();

		/// <summary>
		/// 
		/// </summary>
		public string ServiceType { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Condition Condition { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<Property> Properties { get; set; }
	}
}
