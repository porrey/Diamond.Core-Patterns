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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diamond.Core.AspNetCore.DoAction
{
	/// <summary>
	/// 
	/// </summary>
	public static class DoActionResult
	{
		/// <summary>
		/// Creates a response with an HTTP Status Code of 200.
		/// </summary>
		/// <returns></returns>
		public static ProblemDetails Ok()
		{
			return new ProblemDetails
			{
				Status = StatusCodes.Status200OK
			};
		}

		/// <summary>
		/// Creates a response with an HTTP Status Code of 201.
		/// </summary>
		/// <returns></returns>
		public static ProblemDetails Created()
		{
			return new ProblemDetails
			{
				Status = StatusCodes.Status201Created
			};
		}

		/// <summary>
		/// Creates a response with an HTTP Status Code of 204.
		/// </summary>
		/// <returns></returns>
		public static ProblemDetails NoContent()
		{
			return new ProblemDetails
			{
				Status = StatusCodes.Status204NoContent
			};
		}

		/// <summary>
		/// Creates a response with an HTTP Status Code of 400 (Bad Request).
		/// </summary>
		/// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
		/// <param name="instance">A URI reference that identifies the specific occurrence of the problem.
		/// It may or may not yield further information if dereferenced.</param>
		/// <param name="title">A short, human-readable summary of the problem type. It SHOULD NOT change from
		/// occurrence to occurrence of the problem, except for purposes of localization(e.g.,
		/// using proactive content negotiation; see[RFC7231], Section 3.4).</param>
		/// <param name="extensions">Gets the System.Collections.Generic.IDictionary`2 for extension members.
		/// Problem type definitions MAY extend the problem details object with additional
		/// members. Extension members appear in the same namespace as other members of a
		/// problem type.</param>
		/// <returns>Returns the newly created <see cref="ProblemDetails"/> instance with the provided details.</returns>
		public static ProblemDetails BadRequest(string detail, string instance = null, string title = null, IDictionary<string, object> extensions = null)
		{
			ProblemDetails returnValue = new()
			{
				Status = StatusCodes.Status400BadRequest,
				Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
				Title = !String.IsNullOrWhiteSpace(title) ? title : "Bad Request",
				Detail = detail,
				Instance = instance
			};

			if (extensions != null && extensions.Any())
			{
				foreach (var item in extensions)
				{
					returnValue.Extensions.Add(item);
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Creates a response with an HTTP Status Code of 404 (Not Found).
		/// </summary>
		/// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
		/// <param name="instance">A URI reference that identifies the specific occurrence of the problem.
		/// It may or may not yield further information if dereferenced.</param>
		/// <param name="title">A short, human-readable summary of the problem type. It SHOULD NOT change from
		/// occurrence to occurrence of the problem, except for purposes of localization(e.g.,
		/// using proactive content negotiation; see[RFC7231], Section 3.4).</param>
		/// <param name="extensions">Gets the System.Collections.Generic.IDictionary`2 for extension members.
		/// Problem type definitions MAY extend the problem details object with additional
		/// members. Extension members appear in the same namespace as other members of a
		/// problem type.</param>
		/// <returns>Returns the newly created <see cref="ProblemDetails"/> instance with the provided details.</returns>
		public static ProblemDetails NotFound(string detail, string instance = null, string title = null, IDictionary<string, object> extensions = null)
		{
			ProblemDetails returnValue = new()
			{
				Status = StatusCodes.Status404NotFound,
				Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
				Title = !String.IsNullOrWhiteSpace(title) ? title : "Not Found",
				Detail = detail,
				Instance = instance
			};

			if (extensions != null && extensions.Any())
			{
				foreach (var item in extensions)
				{
					returnValue.Extensions.Add(item);
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Creates a response with an HTTP Status Code of 501 (Not Implemented).
		/// </summary>
		/// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
		/// <param name="instance">A URI reference that identifies the specific occurrence of the problem.
		/// It may or may not yield further information if dereferenced.</param>
		/// <param name="title">A short, human-readable summary of the problem type. It SHOULD NOT change from
		/// occurrence to occurrence of the problem, except for purposes of localization(e.g.,
		/// using proactive content negotiation; see[RFC7231], Section 3.4).</param>
		/// <param name="extensions">Gets the System.Collections.Generic.IDictionary`2 for extension members.
		/// Problem type definitions MAY extend the problem details object with additional
		/// members. Extension members appear in the same namespace as other members of a
		/// problem type.</param>
		/// <returns>Returns the newly created <see cref="ProblemDetails"/> instance with the provided details.</returns>
		public static ProblemDetails NotImplemented(string detail, string instance = null, string title = null, IDictionary<string, object> extensions = null)
		{
			ProblemDetails returnValue = new()
			{
				Status = StatusCodes.Status501NotImplemented,
				Type = "https://tools.ietf.org/html/rfc7231#section-6.6.2",
				Title = !String.IsNullOrWhiteSpace(title) ? title : "Not Implemented",
				Detail = detail,
				Instance = instance
			};

			if (extensions != null && extensions.Any())
			{
				foreach (var item in extensions)
				{
					returnValue.Extensions.Add(item);
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Creates a response with an HTTP Status Code of 413 (Payload Too Large).
		/// </summary>
		/// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
		/// <param name="instance">A URI reference that identifies the specific occurrence of the problem.
		/// It may or may not yield further information if dereferenced.</param>
		/// <param name="title">A short, human-readable summary of the problem type. It SHOULD NOT change from
		/// occurrence to occurrence of the problem, except for purposes of localization(e.g.,
		/// using proactive content negotiation; see[RFC7231], Section 3.4).</param>
		/// <param name="extensions">Gets the System.Collections.Generic.IDictionary`2 for extension members.
		/// Problem type definitions MAY extend the problem details object with additional
		/// members. Extension members appear in the same namespace as other members of a
		/// problem type.</param>
		/// <returns>Returns the newly created <see cref="ProblemDetails"/> instance with the provided details.</returns>
		public static ProblemDetails PayloadTooLarge(string detail, string instance = null, string title = null, IDictionary<string, object> extensions = null)
		{
			ProblemDetails returnValue = new()
			{
				Status = StatusCodes.Status413PayloadTooLarge,
				Type = "https://tools.ietf.org/html/rfc7231#section-6.5.11",
				Title = !String.IsNullOrWhiteSpace(title) ? title : "Payload Too Large",
				Detail = detail,
				Instance = instance
			};

			if (extensions != null && extensions.Any())
			{
				foreach (var item in extensions)
				{
					returnValue.Extensions.Add(item);
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Creates a response with an HTTP Status Code of 500 (Internal Server Error).
		/// </summary>
		/// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
		/// <param name="instance">A URI reference that identifies the specific occurrence of the problem.
		/// It may or may not yield further information if dereferenced.</param>
		/// <param name="title">A short, human-readable summary of the problem type. It SHOULD NOT change from
		/// occurrence to occurrence of the problem, except for purposes of localization(e.g.,
		/// using proactive content negotiation; see[RFC7231], Section 3.4).</param>
		/// <param name="extensions">Gets the System.Collections.Generic.IDictionary`2 for extension members.
		/// Problem type definitions MAY extend the problem details object with additional
		/// members. Extension members appear in the same namespace as other members of a
		/// problem type.</param>
		/// <returns>Returns the newly created <see cref="ProblemDetails"/> instance with the provided details.</returns>
		public static ProblemDetails InternalServerError(string detail, string instance = null, string title = null, IDictionary<string, object> extensions = null)
		{
			ProblemDetails returnValue = new()
			{
				Status = StatusCodes.Status500InternalServerError,
				Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
				Title = !String.IsNullOrWhiteSpace(title) ? title : "Internal Server Error",
				Detail = detail,
				Instance = instance
			};

			if (extensions != null && extensions.Any())
			{
				foreach (var item in extensions)
				{
					returnValue.Extensions.Add(item);
				}
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="statusCode"></param>
		/// <param name="detail"></param>
		/// <param name="instance"></param>
		/// <param name="title"></param>
		/// <param name="type"></param>
		/// <param name="extensions"></param>
		/// <returns></returns>
		public static ProblemDetails FromStatusCode(int statusCode, string detail, string instance = null, string title = null, string type = null, IDictionary<string, object> extensions = null)
		{
			ProblemDetails returnValue = null;

			switch (statusCode)
			{
				case StatusCodes.Status200OK:
					returnValue = DoActionResult.Ok();
					break;
				case StatusCodes.Status201Created:
					returnValue = DoActionResult.Created();
					break;
				case StatusCodes.Status204NoContent:
					returnValue = DoActionResult.NoContent();
					break;
				case StatusCodes.Status400BadRequest:
					returnValue = DoActionResult.BadRequest(detail, instance, title, extensions);
					break;
				case StatusCodes.Status404NotFound:
					returnValue = DoActionResult.NotFound(detail, instance, title, extensions);
					break;
				case StatusCodes.Status413PayloadTooLarge:
					returnValue = DoActionResult.PayloadTooLarge(detail, instance, title, extensions);
					break;
				case StatusCodes.Status500InternalServerError:
					returnValue = DoActionResult.InternalServerError(detail, instance, title, extensions);
					break;
				case StatusCodes.Status501NotImplemented:
					returnValue = DoActionResult.NotImplemented(detail, instance, title, extensions);
					break;
				default:
					returnValue = new ProblemDetails()
					{
						Status = statusCode,
						Type = type,
						Title = title,
						Detail = detail,
						Instance = instance
					};
					break;
			}

			return returnValue;
		}
	}
}
