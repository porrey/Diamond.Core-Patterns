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
using Microsoft.Extensions.Configuration;

namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// Provides a configuration provider that loads configuration key-value pairs from JSON files located in a specified
	/// directory. Supports loading multiple files and merging their contents into a single configuration source.
	/// </summary>
	/// <remarks>This provider reads all JSON files in the specified directory and its subdirectories, combining
	/// their contents into a single configuration dictionary. Keys from multiple files are merged, and array indices are
	/// adjusted to ensure continuity across files. If the directory does not exist and the source is marked as
	/// non-optional, an exception is thrown.</remarks>
	public class ServicesConfigurationFolderProvider : FileConfigurationProvider
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ServicesConfigurationFolderProvider"/> class using the specified <see
		/// cref="ServicesConfigurationFolderSource"/>.
		/// </summary>
		/// <remarks>The <see cref="ServicesConfigurationFolderProvider"/> retrieves configuration data from the specified
		/// <see cref="ServicesConfigurationFolderSource"/> and makes it available to the application. Ensure that the <paramref
		/// name="source"/> is properly initialized before passing it to this constructor.</remarks>
		/// <param name="source">The configuration source that provides the service-based configuration data. This parameter cannot be <see
		/// langword="null"/>.</param>
		public ServicesConfigurationFolderProvider(ServicesConfigurationFolderSource source)
			: base(source)
		{
		}

		/// <summary>
		/// Gets the typed configuration source for this provider.
		/// </summary>
		protected new ServicesConfigurationFolderSource Source => (ServicesConfigurationFolderSource)base.Source;

		/// <summary>
		/// Loads configuration data from JSON files located in the specified directory and its subdirectories.
		/// </summary>
		/// <remarks>This method reads all JSON files in the directory (or directories) specified by the <see cref="FileConfigurationSource.Path"/>
		/// and <see cref="ServicesConfigurationFolderSource.AdditionalPaths"/> properties, parses their contents into
		/// key-value pairs, and adds them to the services collection. Array indices are kept contiguous across all
		/// folders so that entries from different folders do not collide in the merged configuration. If a directory
		/// does not exist and the <see cref="FileConfigurationSource.Optional"/> property is set to <see langword="false"/>, a <see
		/// cref="DirectoryNotFoundException"/> is thrown.</remarks>
		/// <exception cref="DirectoryNotFoundException">Thrown if any directory does not exist and <see cref="FileConfigurationSource.Optional"/> is
		/// <see langword="false"/>.</exception>
		public override void Load()
		{
			//
			// Build the ordered list of folder paths to process: primary path first, then any additional paths.
			//
			IEnumerable<string> allPaths = new[] { this.Source.Path }
				.Concat(this.Source.AdditionalPaths ?? [])
				.Where(p => !string.IsNullOrWhiteSpace(p));

			//
			// When reading arrays from multiple files/folders the index needs to be
			// contiguous across all of them to avoid key collisions in the merged
			// configuration. This index tracks the running offset.
			//
			int baseIndex = 0;

			foreach (string folderPath in allPaths)
			{
				//
				// Use AppContext.BaseDirectory so that relative paths resolve correctly
				// in all deployment models (including single-file executables).
				//
				string fullPath = Path.Combine(AppContext.BaseDirectory, folderPath);
				DirectoryInfo dir = new(fullPath);

				if (dir.Exists)
				{
					//
					// Get all of the files.
					//
					FileInfo[] files = dir.GetFiles("*.json", SearchOption.AllDirectories);

					//
					// Load each file, keeping the base index contiguous across files and folders.
					//
					foreach (FileInfo file in files)
					{
						//
						// Read the JSON.
						//
						string json = File.ReadAllText(file.FullName);

						//
						// Parse the data into a flattened dictionary.
						//
						IDictionary<string, string> result = ServicesConfigurationFileParser.Parse(baseIndex, json);

						//
						// Add the results to the current list. This list collects all values
						// across the multiple files and folders.
						//
						foreach (KeyValuePair<string, string> item in result)
						{
							if (item.Value != null)
							{
								this.Data.Add(item);
							}
						}

						//
						// Update the base index for the next file.
						//
						baseIndex = this.Data.Count;
					}
				}
				else
				{
					if (!this.Source.Optional)
					{
						//
						// The folder was not optional so throw an exception.
						//
						throw new DirectoryNotFoundException($"The configuration services path '{dir.FullName}' was not found.");
					}
				}
			}
		}

		/// <summary>
		/// Loads data from the specified stream into the current instance.
		/// </summary>
		/// <param name="stream">The input stream containing the data to load. Must be readable and not null.</param>
		/// <exception cref="NotImplementedException">Thrown if the method is not implemented.</exception>
		public override void Load(Stream stream)
		{
			throw new NotImplementedException();
		}
	}
}
