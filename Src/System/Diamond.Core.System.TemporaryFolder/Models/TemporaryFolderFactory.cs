//
// Copyright(C) 2019-2023, Daniel M. Porrey. All rights reserved.
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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.System.TemporaryFolder
{
	/// <summary>
	/// 
	/// </summary>
	public class TemporaryFolderFactory : ITemporaryFolderFactory
	{
		/// <summary>
		/// Prevents instances of this class from being created externally.
		/// </summary>
		public TemporaryFolderFactory(ILogger<TemporaryFolderFactory> logger)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// Prevents instances of this class from being created externally.
		/// </summary>
		public TemporaryFolderFactory()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual ILogger<TemporaryFolderFactory> Logger { get; set; } = new NullLogger<TemporaryFolderFactory>();

		/// <summary>
		/// Creates a default instance of ITemporaryFolder.
		/// </summary>
		/// <returns>An instance of ITemporaryFolder.</returns>
		public virtual ITemporaryFolder Create()
		{
			ITemporaryFolder returnValue = null;

			this.Logger.LogDebug("Creating temporary folder.");
			returnValue = new TemporaryFolder();
			this.Logger.LogDebug("Created temporary folder '{folder}'.", returnValue.FullPath);

			return returnValue;
		}

		/// <summary>
		/// Creates an instance of ITemporaryFolder using
		/// the given name format.
		/// </summary>
		/// <param name="namingFormat">Specifies the naming format to
		/// use with this new instance</param>
		/// <returns>An instance of ITemporaryFolder.</returns>
		public virtual ITemporaryFolder Create(string namingFormat)
		{
			ITemporaryFolder returnValue = null;

			this.Logger.LogDebug("Creating temporary folder.");
			returnValue = new TemporaryFolder(namingFormat);
			this.Logger.LogDebug("Created temporary folder '{folder}'.", returnValue.FullPath);

			return returnValue;
		}
	}
}
