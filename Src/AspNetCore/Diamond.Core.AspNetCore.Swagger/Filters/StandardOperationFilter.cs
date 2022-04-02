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
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Diamond.Core.AspNetCore.Swagger
{
	/// <summary>
	/// Adds Swagger documentation response descriptions for standard 
	/// HTTP status codes such as 406, 500 and 50q to all methods.
	/// </summary>
	public class StandardOperationFilter : IOperationFilter
	{
		/// <summary>
		/// Applies the response description to a given method.
		/// </summary>
		/// <param name="operation">A reference to the <see cref="OpenApiOperation"/>.</param>
		/// <param name="context">A reference to the <see cref="OperationFilterContext"/>.</param>
		public virtual void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			if (operation.Responses.ContainsKey($"{StatusCodes.Status406NotAcceptable}"))
			{
				operation.Responses[$"{StatusCodes.Status406NotAcceptable}"].Description = "The requested return media type is not supported.";
			}

			if (operation.Responses.ContainsKey($"{StatusCodes.Status500InternalServerError}"))
			{
				operation.Responses[$"{StatusCodes.Status500InternalServerError}"].Description = "Internal server or application error.";
			}

			if (operation.Responses.ContainsKey($"{StatusCodes.Status501NotImplemented}"))
			{
				operation.Responses[$"{StatusCodes.Status501NotImplemented}"].Description = "Method is not implemented.";
			}
		}
	}
}
