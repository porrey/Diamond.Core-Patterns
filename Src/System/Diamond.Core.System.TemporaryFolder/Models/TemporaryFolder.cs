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
using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.System.TemporaryFolder
{
	/// <summary>
	/// Provides a wrapper for creating and managing temporary folders. Disposing this
	/// object will cleanup all temporary files and the folder that were created from
	/// this instance (as long as no files are locked when the object is disposed).
	/// </summary>
	public class TemporaryFolder : DisposableObject, ITemporaryFolder
	{
		/// <summary>
		/// Creates a default instance.
		/// </summary>
		public TemporaryFolder()
		{
			this.AssertWhenNotDisposed = false;
			this.Create();
		}

		/// <summary>
		/// Creates a default instance with the given <see cref="ILogger"/>.
		/// </summary>
		public TemporaryFolder(ILogger<TemporaryFolder> logger)
			: this()
		{
			this.Logger = logger;
			this.AssertWhenNotDisposed = false;
			this.Create();
		}

		/// <summary>
		/// Creates an instance of ITemporaryFolder using
		/// the given name format.
		/// </summary>
		/// <param name="namingFormat">Specifies the naming format to
		/// use with this new instance</param>
		public TemporaryFolder(string namingFormat)
			: this()
		{
			this.NamingFormat = namingFormat;
		}

		/// <summary>
		/// Internally creates an instance of ITemporaryFolder using
		/// the given name format and <see cref="ILogger"/>.
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="namingFormat"></param>
		public TemporaryFolder(ILogger<TemporaryFolder> logger, string namingFormat)
			: this(logger)
		{
			this.NamingFormat = namingFormat;
		}

		/// <summary>
		/// The <see cref="ILogger"/> instance used for logging.
		/// </summary>
		protected virtual ILogger<TemporaryFolder> Logger { get; set; } = new NullLogger<TemporaryFolder>();

		/// <summary>
		/// Creates the temporary folder.
		/// </summary>
		public virtual void Create()
		{
			this.FullPath = String.Format(this.NamingFormat, Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName()));

			if (!Directory.Exists(this.FullPath))
			{
				Directory.CreateDirectory(this.FullPath);
			}
		}

		/// <summary>
		/// Gets the full path to the temporary folder that is created by this instance.
		/// </summary>
		public virtual string FullPath { get; set; }

		/// <summary>
		/// Gets/sets a string format with two variables, {0} and {1}, where
		/// the first place holder will be replaced with the temporary folder
		/// path and the second place holder will be replaced with the temporary
		/// folder name. The default is "{0}{1}".
		/// </summary>
		public virtual string NamingFormat { get; set; } = "{0}{1}";

		/// <summary>
		/// Disposes managed objects.
		/// </summary>
		protected override void OnDisposeManagedObjects()
		{
			try
			{
				this.Logger.LogDebug("Attempting to delete temporary folder '{path}'.", this.FullPath);

				if (Directory.Exists(this.FullPath))
				{
					//
					// Delete the folder and everything in it
					//
					Directory.Delete(this.FullPath, true);
					this.Logger.LogDebug("The temporary folder {path}' and it's contents ' were successfully deleted.", this.FullPath);
				}
			}
			catch (Exception ex)
			{
				//
				// Not a big deal if this fails...
				//
				this.Logger.LogError(ex, "OnDisposeManagedObjects() failed to remove the temporary folder '{path}',", this.FullPath);
			}
		}
	}
}
