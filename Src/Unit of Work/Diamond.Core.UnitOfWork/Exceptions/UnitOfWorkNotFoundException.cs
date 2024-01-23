//
// Copyright(C) 2019-2024, Daniel M. Porrey. All rights reserved.
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
using Diamond.Core.Abstractions;

namespace Diamond.Core.UnitOfWork
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TResult"></typeparam>
	/// <typeparam name="TSourceItem"></typeparam>
	public class UnitOfWorkNotFoundException<TResult, TSourceItem> : DiamondCoreException
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public UnitOfWorkNotFoundException(string name)
			: base($"A Unit Of Work of type 'IUnitOfWork<{typeof(TResult).Name}, {typeof(TSourceItem).Name}>' with name '{name}' has not been configured.")
		{
		}
	}
}
