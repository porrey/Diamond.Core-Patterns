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

namespace Diamond.Core.AspNet.DoAction {
	/// <summary>
	/// Defines a factory to create/retrieve decorator instances.
	/// </summary>
	public interface IDoActionFactory {
		/// <summary>
		/// Gets the specific action by type and action key.
		/// </summary>
		/// <typeparam name="TInputs">The inputs required for the action handler.</typeparam>
		/// <typeparam name="TResult">The type of the result returned by the action.</typeparam>
		/// <param name="actionKey">The unique name of the action.</param>
		/// <returns>The result of the action.</returns>
		Task<IDoAction<TInputs, TResult>> GetAsync<TInputs, TResult>(string actionKey);
	}
}
