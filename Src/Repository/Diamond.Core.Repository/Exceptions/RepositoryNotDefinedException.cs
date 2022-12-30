﻿//
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
using System;

namespace Diamond.Core.Repository
{
	/// <summary>
	/// 
	/// </summary>
	public class RepositoryNotDefinedException : DiamondRepositoryException
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="t"></param>
		public RepositoryNotDefinedException(Type t)
			: base($"A repository for Type {t.Name} has not been configured.")
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="t"></param>
		/// <param name="name"></param>
		public RepositoryNotDefinedException(Type t, string name)
			: base($"A repository for Type {t.Name} and name '{name}' has not been configured.")
		{
		}
	}
}
