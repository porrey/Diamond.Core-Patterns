using System;
using System.IO;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.System
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

		protected override void OnDisposeManagedObjects()
		{
			try
			{
				if (Directory.Exists(this.FullPath))
				{
					// ***
					// *** Delete the folder and everything in it
					// ***
					Directory.Delete(this.FullPath, true);
				}
			}
			catch
			{
				// ***
				// *** Not a big deal if this fails...
				// ***
			}
		}

		/// <summary>
		/// Gets a factory used for creating instance of this TemporaryFolder.
		/// </summary>
		public static ITemporaryFolderFactory Factory { get; } = new TemporaryFolderFactory();
	}
}
