//
// Copyright(C) 2019-2022, Daniel M. Porrey. All rights reserved.
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Diamond.Core.Extensions.DependencyInjection
{
	/// <summary>
	/// 
	/// </summary>
	public class ServicesConfigurationProvider : FileConfigurationProvider
	{
		/// <summary>
		/// Initializes a new instance with the specified source.
		/// </summary>
		/// <param name="source">The source settings.</param>
		public ServicesConfigurationProvider(ServicesConfigurationSource source)
			: base(source)
		{
		}

		/// <summary>
		/// Load the files from the source folder.
		/// </summary>
		public override void Load()
		{
			//
			// Get a DirectoryInof object to convert relative paths to full paths.
			//
			DirectoryInfo dir = new DirectoryInfo($"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}/{this.Source.Path}");

			//
			// Ensue the directory exists.
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
					baseIndex = this.Data.Count();
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
		/// Not currently implemented.
		/// </summary>
		/// <param name="stream">The stream used to load the files.</param>
		public override void Load(Stream stream)
		{
			throw new NotImplementedException();
		}
	}
}
