using System;

namespace Diamond.Patterns.Abstractions
{
	/// <summary>
	/// Provides a wrapper for creating and managing temporary folders. Concrete
	/// classes should implement IDisposable (not required) to remove temporary
	/// files and folders when the instance is no longer in use.
	/// </summary>
	public interface ITemporaryFolder : IDisposable
	{
		/// <summary>
		/// Gets/sets a string format with two variables, {0} and {1}, where
		/// the first place holder will be replaced with the temporary folder
		/// path and the second place holder will be replaced with the temporary
		/// folder name.
		/// </summary>
		string NamingFormat { get; set; }

		/// <summary>
		/// Gets the full path to the temporary folder that is created by this instance.
		/// </summary>
		string FullPath { get; }
	}

	/// <summary>
	/// Factory for creating instances of ITemporaryFolder.
	/// </summary>
	public interface ITemporaryFolderFactory
	{
		/// <summary>
		/// Creates a default instance of ITemporaryFolder.
		/// </summary>
		/// <returns>An instance of ITemporaryFolder.</returns>
		ITemporaryFolder Create();

		/// <summary>
		/// Creates a default instance of ITemporaryFolder using
		/// the given name format.
		/// </summary>
		/// <param name="namingFormat">Specifies the naming format to
		/// use with this new instance</param>
		/// <returns>An instance of ITemporaryFolder.</returns>
		ITemporaryFolder Create(string namingFormat);
	}
}
