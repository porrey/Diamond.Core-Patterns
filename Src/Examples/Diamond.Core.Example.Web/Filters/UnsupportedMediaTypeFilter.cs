using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Diamond.Core.Example
{
	public class UnsupportedMediaTypeFilter : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			// ***
			// *** Get all custom attributes that are POST or PUT.
			// ***
			IEnumerable<CustomAttributeData> attrs = from tbl in context.MethodInfo.CustomAttributes
													 where tbl.AttributeType == typeof(HttpPostAttribute) ||
													 tbl.AttributeType == typeof(HttpPutAttribute)
													 select tbl;

			// ***
			// *** Check if any of those attributes were found.
			// ***
			if (attrs.Any())
			{
				// ***
				// *** If found, add a response of 415 to the documentation.
				// ***
				operation.Responses.Add($"{StatusCodes.Status415UnsupportedMediaType}", new OpenApiResponse { Description = "The requested body media type is not supported." });
			}
		}
	}
}
