using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.System
{
	public class TemporaryFolderFactory : ITemporaryFolderFactory
	{
		/// <summary>
		/// Prevents instances of this class from being created externally.
		/// </summary>
		internal TemporaryFolderFactory()
		{
		}

		/// <summary>
		/// Creates a default instance of ITemporaryFolder.
		/// </summary>
		/// <returns>An instance of ITemporaryFolder.</returns>
		public ITemporaryFolder Create()
		{
			return new TemporaryFolder();
		}

		/// <summary>
		/// Creates an instance of ITemporaryFolder using
		/// the given name format.
		/// </summary>
		/// <param name="namingFormat">Specifies the naming format to
		/// use with this new instance</param>
		/// <returns>An instance of ITemporaryFolder.</returns>
		public ITemporaryFolder Create(string namingFormat)
		{
			return new TemporaryFolder(namingFormat);
		}
	}
}
