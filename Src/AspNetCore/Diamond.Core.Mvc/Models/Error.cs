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
	/// Holds an error message from a bad request.
	/// </summary>
	public class Error
	{
		/// <summary>
		/// A code to identify this error.
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// Description of an error that occurred.
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// The error that is the cause of the current error, or a null reference
		/// if no inner error is specified.
		/// </summary>
		public Error InnerError { get; set; }
	}
}
