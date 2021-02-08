using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Diamond.Core.Example
{
	/// <summary>
	/// 
	/// </summary>
	public class StandardOperationFilter : IOperationFilter
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="operation"></param>
		/// <param name="context"></param>
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
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
				operation.Responses[$"{StatusCodes.Status501NotImplemented}"].Description = "Method is not implemented yet.";
			}
		}
	}
}
