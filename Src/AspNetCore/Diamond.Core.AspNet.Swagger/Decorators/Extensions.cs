using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Diamond.Core.AspNet.Swagger {
	/// <summary>
	/// 
	/// </summary>
	public static class Extensions {
		/// <summary>
		/// Load any XML comment files found in the folder ./XmlDocs this
		/// are to be used for Swagger documentation.
		/// </summary>
		/// <param name="config"></param>
		/// <param name="dir"></param>
		public static void LoadXmlCommentFiles(this SwaggerGenOptions config, DirectoryInfo dir) {
			if (dir.Exists) {
				FileInfo[] files = dir.GetFiles("*.xml");

				foreach (FileInfo file in files) {
					config.IncludeXmlComments(file.FullName);
				}
			}
		}
	}
}
