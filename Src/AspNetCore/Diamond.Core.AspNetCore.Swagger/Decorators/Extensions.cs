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
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Diamond.Core.AspNetCore.Swagger
{
	/// <summary>
	/// Provides extensions to support Swagger documentation.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Load any XML comment files found in the folder ./XmlDocs this
		/// are to be used for Swagger documentation.
		/// </summary>
		/// <param name="config"></param>
		/// <param name="dir"></param>
		public static void LoadXmlCommentFiles(this SwaggerGenOptions config, DirectoryInfo dir)
		{
			if (dir.Exists)
			{
				FileInfo[] files = dir.GetFiles("*.xml");

				foreach (FileInfo file in files)
				{
					config.IncludeXmlComments(file.FullName);
				}
			}
		}
	}
}
