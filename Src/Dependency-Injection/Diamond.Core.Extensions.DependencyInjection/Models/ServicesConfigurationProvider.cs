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
using System.Reflection;
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
	public class ServicesConfigurationProvider : FileConfigurationProvider
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ServicesConfigurationProvider"/> class using the specified <see
		/// cref="ServicesConfigurationSource"/>.
		/// </summary>
		/// <remarks>The <see cref="ServicesConfigurationProvider"/> retrieves configuration data from the specified
		/// <see cref="ServicesConfigurationSource"/> and makes it available to the application. Ensure that the <paramref
		/// name="source"/> is properly initialized before passing it to this constructor.</remarks>
		/// <param name="source">The configuration source that provides the service-based configuration data. This parameter cannot be <see
		/// langword="null"/>.</param>
		public ServicesConfigurationProvider(ServicesConfigurationSource source)
			: base(source)
		{
		}

		/// <summary>
		/// Loads configuration data from JSON files located in the specified directory and its subdirectories.
		/// </summary>
		/// <remarks>This method reads all JSON files in the directory specified by the <see cref="FileConfigurationSource.Path"/>
		/// property, parses their contents into key-value pairs, and adds them to the services collection. If the
		/// directory does not exist and the <see cref="FileConfigurationSource.Optional"/> property is set to <see langword="false"/>, a <see
		/// cref="DirectoryNotFoundException"/> is thrown. The method ensures that indices across multiple files are
		/// contiguous when parsing arrays.</remarks>
		/// <exception cref="DirectoryNotFoundException">Thrown if the directory specified by <see cref="FileConfigurationSource.Path"/> does not exist and <see cref="FileConfigurationSource.Optional"/> is
		/// <see langword="false"/>.</exception>
		public override void Load()
		{
			//
			// Get a DirectoryInfo object to convert relative paths to full paths.
			//
			string fullPath = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/{this.Source.Path}";
			DirectoryInfo dir = new(fullPath);

			//
			// Ensure the directory exists.
			//
			if (dir.Exists)
			{
				//
				// Get all of the files.
				//
				FileInfo[] files = dir.GetFiles("*.json", SearchOption.AllDirectories);

				//
				// When reading the arrays from multiple files, the index needs to
				// to be contiguous across the multiple files. This index will keep track.
				//
				int baseIndex = 0;

				//
				// Load each file.
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
					// across the multiple files.
					//
					foreach (KeyValuePair<string, string> item in result)
					{
						this.Data.Add(item);
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
