// ***
// *** Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
// *** 
// *** This program is free software: you can redistribute it and/or modify
// *** it under the terms of the GNU Lesser General Public License as published
// *** by the Free Software Foundation, either version 3 of the License, or
// *** (at your option) any later version.
// *** 
// *** This program is distributed in the hope that it will be useful,
// *** but WITHOUT ANY WARRANTY; without even the implied warranty of
// *** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// *** GNU Lesser General Public License for more details.
// *** 
// *** You should have received a copy of the GNU Lesser General Public License
// *** along with this program. If not, see http://www.gnu.org/licenses/.
// *** 
namespace Diamond.Core.AspNet.DoAction
{
	/// <summary>
	/// Provides detail for a failed request.
	/// </summary>
	public class FailedRequest
	{
		/// <summary>
		/// Creates a default instance of BadRequest.
		/// </summary>
		public FailedRequest()
		{
		}

		/// <summary>
		/// Creates an instance of BadRequest.
		/// </summary>
		/// <param name="code">The error code.</param>
		/// <param name="message">The error message.</param>
		/// <param name="innerError">The inner error.</param>
		public FailedRequest(string code, string message, Error innerError = null)
		{
			this.Error = new Error()
			{
				Code = code,
				Message = message,
				InnerError = innerError
			};
		}

		/// <summary>
		/// The resulting error from the bad request.
		/// </summary>
		public Error Error { get; set; }
	}
}
