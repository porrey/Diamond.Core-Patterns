//
// Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
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
using System.Threading.Tasks;
using Diamond.Core.Workflow.State;

namespace Diamond.Core.Workflow
{
	/// <summary>
	/// Defines a generic context.
	/// </summary>
	public interface IContext
	{
		/// <summary>
		/// Gets the name of the context.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets/sets properties to be contained  within the context.
		/// </summary>
		IStateDictionary Properties { get; }

		/// <summary>
		/// Resets the context.
		/// </summary>
		Task ResetAsync();

		/// <summary>
		/// The optional arguments supplied to the application.
		/// </summary>
		string[] Arguments { get; }
	}
}
