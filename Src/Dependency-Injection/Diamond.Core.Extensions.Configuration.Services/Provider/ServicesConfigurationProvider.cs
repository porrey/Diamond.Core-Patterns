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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Diamond.Core.Extensions.Configuration.Services
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

		public override void Load()
		{
			DirectoryInfo dir = new DirectoryInfo(this.Source.Path);

			if (dir.Exists)
			{
				FileInfo[] files = dir.GetFiles("*.json", SearchOption.AllDirectories);
				int baseIndex = 0;

				foreach (FileInfo file in files)
				{
					string json = File.ReadAllText(file.FullName);
					IDictionary<string, string> result = JsonConfigurationFileParser.Parse(baseIndex, json);

					foreach (var item in result)
					{
						this.Data.Add(item);
					}

					baseIndex = this.Data.Count();
				}
			}
			else
			{
				if (!this.Source.Optional)
				{
					throw new DirectoryNotFoundException($"The configuration services path '{dir.FullName}' was not found.");
				}
			}
		}

		public override void Load(Stream stream)
		{
			throw new NotImplementedException();
		}
	}
}
