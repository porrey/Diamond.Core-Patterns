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
using System.Threading.Tasks;

namespace Diamond.Patterns.Command
{
	/// <summary>
	/// Defines a command to be used in the command pattern.
	/// </summary>
	public interface ICommand
	{
		/// <summary>
		/// 
		/// </summary>
		string Key { get; set; }

		/// <summary>
		/// Executes the command.
		/// </summary>
		/// <returns>Returns a code indicating the result. The code is specific to the command.</returns>
		Task<int> ExecuteAsync();
	}
}
