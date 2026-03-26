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
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.AutoMapperExtensions
{
	/// <summary>
	/// Provides a base class for profile implementations that require logging functionality.
	/// </summary>
	/// <remarks>Inherit from this class to create custom profiles with integrated logging support. The logger can
	/// be used to record diagnostic or operational information relevant to the profile's behavior.</remarks>
	public abstract class ProfileTemplate : Profile
	{
		/// <summary>
		/// Initializes a new instance of the ProfileTemplate class with the specified logger.
		/// </summary>
		/// <param name="logger">The logger instance used to record diagnostic and operational messages for the ProfileTemplate.</param>
		public ProfileTemplate(ILogger<ProfileTemplate> logger)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// Gets or sets the logger used to record diagnostic and operational messages for the profile template.
		/// </summary>
		/// <remarks>Assign a custom logger to integrate with application-wide logging infrastructure. By default, a
		/// no-op logger is used, and no log output is produced unless a different logger is provided.</remarks>
		public ILogger<ProfileTemplate> Logger { get; set; } = new NullLogger<ProfileTemplate>();
	}
}
