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
using Swashbuckle.AspNetCore.Filters;

namespace Diamond.Core.AspNetCore.Swagger
{
	/// <summary>
	/// Provides a default patch example for Swagger documentation.
	/// </summary>
	public class JsonPatchDefaultExample : IExamplesProvider<Operation[]>
	{
		/// <summary>
		/// Gets a list of operations.
		/// </summary>
		/// <returns>An array of <see cref="Operation"/> objects to be used as examples.</returns>
		public virtual Operation[] GetExamples()
		{
			return this.OnGetExamples();
		}

		/// <summary>
		/// Provides a default implementation that can be overridden.
		/// </summary>
		/// <returns>An array of <see cref="Operation"/> objects to be used as examples.</returns>
		protected Operation[] OnGetExamples()
		{
			return (Operation[])new[]
			{
				new Operation
				{
					Op = "replace",
					Path = "/name",
					Value = "John Doe"
				},
				new Operation
				{
					Op = "replace",
					Path = "/total",
					Value = "45.50"
				}
			};
		}
	}
}
