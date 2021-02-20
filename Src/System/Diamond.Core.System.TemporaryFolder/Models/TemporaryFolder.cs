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
using System.IO;

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
		/// Internally creates a default instance.
		/// </summary>
		internal TemporaryFolder()
		{
			this.AssertWhenNotDisposed = false;
			this.Create();
		}

		/// <summary>
		/// Internally creates an instance of ITemporaryFolder using
		/// the given name format.
		/// </summary>
		/// <param name="namingFormat">Specifies the naming format to
		/// use with this new instance</param>
		internal TemporaryFolder(string namingFormat)
			: this()
		{
			this.NamingFormat = namingFormat;
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual void Create()
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
		public string FullPath { get; set; }

		/// <summary>
		/// Gets/sets a string format with two variables, {0} and {1}, where
		/// the first place holder will be replaced with the temporary folder
		/// path and the second place holder will be replaced with the temporary
		/// folder name. The default is "{0}{1}".
		/// </summary>
		public string NamingFormat { get; set; } = "{0}{1}";

		/// <summary>
		/// 
		/// </summary>
		protected override void OnDisposeManagedObjects()
		{
			try
			{
				if (Directory.Exists(this.FullPath))
				{
					//
					// Delete the folder and everything in it
					//
					Directory.Delete(this.FullPath, true);
				}
			}
			catch
			{
				//
				// Not a big deal if this fails...
				//
			}
		}

		/// <summary>
		/// Gets a factory used for creating instance of this TemporaryFolder.
		/// </summary>
		public static ITemporaryFolderFactory Factory { get; } = new TemporaryFolderFactory();
	}
}
