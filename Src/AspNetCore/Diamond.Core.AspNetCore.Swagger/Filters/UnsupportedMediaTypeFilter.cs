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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Diamond.Core.AspNetCore.Swagger
{
	/// <summary>
	/// 
	/// </summary>
	public class UnsupportedMediaTypeFilter : IOperationFilter
	{
		/// <summary>
		/// Adds a Swagger documentation description for HTTP status 415 to all POST and PUT methods.
		/// </summary>
		/// <param name="operation">A reference to the <see cref="OpenApiOperation"/>.</param>
		/// <param name="context">A reference to the <see cref="OperationFilterContext"/>.</param>
		public virtual void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			//
			// Get all custom attributes that are POST or PUT.
			//
			IEnumerable<CustomAttributeData> attrs = from tbl in context.MethodInfo.CustomAttributes
													 where tbl.AttributeType == typeof(HttpPostAttribute) ||
													 tbl.AttributeType == typeof(HttpPutAttribute)
													 select tbl;

			//
			// Check if any of those attributes were found.
			//
			if (attrs.Any())
			{
				//
				// If found, add a response of 415 to the documentation.
				//
				operation.Responses.Add($"{StatusCodes.Status415UnsupportedMediaType}", new OpenApiResponse { Description = "The requested body media type is not supported." });
			}
		}
	}
}
