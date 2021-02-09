using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Diamond.Core.AspNet.DoAction {
	/// <summary>
	/// 
	/// </summary>
	public static class DoActionResult {
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static ProblemDetails Ok() {
			return new ProblemDetails {
				Status = StatusCodes.Status200OK
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static ProblemDetails Created() {
			return new ProblemDetails {
				Status = StatusCodes.Status201Created
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static ProblemDetails NoContent() {
			return new ProblemDetails {
				Status = StatusCodes.Status204NoContent
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="detail"></param>
		/// <param name="instance"></param>
		/// <param name="title"></param>
		/// <param name="extensions"></param>
		/// <returns></returns>
		public static ProblemDetails BadRequest(string detail, string instance = null, string title = null, IDictionary<string, object> extensions = null) {
			ProblemDetails returnValue = new ProblemDetails {
				Status = StatusCodes.Status400BadRequest,
				Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
				Title = !String.IsNullOrWhiteSpace(title) ? title : "Bad Request",
				Detail = detail,
				Instance = instance
			};

			if (extensions != null && extensions.Any()) {
				foreach (var item in extensions) {
					returnValue.Extensions.Add(item);
				}
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="detail"></param>
		/// <param name="instance"></param>
		/// <param name="title"></param>
		/// <param name="extensions"></param>
		/// <returns></returns>
		public static ProblemDetails NotFound(string detail, string instance = null, string title = null, IDictionary<string, object> extensions = null) {
			ProblemDetails returnValue = new ProblemDetails {
				Status = StatusCodes.Status400BadRequest,
				Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
				Title = !String.IsNullOrWhiteSpace(title) ? title : "Not Found",
				Detail = detail,
				Instance = instance
			};

			if (extensions != null && extensions.Any()) {
				foreach (var item in extensions) {
					returnValue.Extensions.Add(item);
				}
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="detail"></param>
		/// <param name="instance"></param>
		/// <param name="title"></param>
		/// <param name="extensions"></param>
		/// <returns></returns>
		public static ProblemDetails NotImplemented(string detail, string instance = null, string title = null, IDictionary<string, object> extensions = null) {
			ProblemDetails returnValue = new ProblemDetails {
				Status = StatusCodes.Status501NotImplemented,
				Type = "https://tools.ietf.org/html/rfc7231#section-6.6.2",
				Title = !String.IsNullOrWhiteSpace(title) ? title : "Not Implemented",
				Detail = detail,
				Instance = instance
			};

			if (extensions != null && extensions.Any()) {
				foreach (var item in extensions) {
					returnValue.Extensions.Add(item);
				}
			}

			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="detail"></param>
		/// <param name="instance"></param>
		/// <param name="title"></param>
		/// <param name="extensions"></param>
		/// <returns></returns>
		public static ProblemDetails InternalServerError(string detail, string instance = null, string title = null, IDictionary<string, object> extensions = null) {
			ProblemDetails returnValue = new ProblemDetails {
				Status = StatusCodes.Status500InternalServerError,
				Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
				Title = !String.IsNullOrWhiteSpace(title) ? title : "Internal Server Error",
				Detail = detail,
				Instance = instance
			};

			if (extensions != null && extensions.Any()) {
				foreach (var item in extensions) {
					returnValue.Extensions.Add(item);
				}
			}

			return returnValue;
		}
	}
}
