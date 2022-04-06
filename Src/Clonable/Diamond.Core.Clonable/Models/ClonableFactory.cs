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

namespace Diamond.Core.Clonable
{
	/// <summary>
	/// Holds the current factory for cloning objects. 
	/// </summary>
public static class ClonableFactory
	{
		private static IObjectCloneFactory Factory { get; set; }

		/// <summary>
		/// Gets the current factory for cloning objects.
		/// </summary>
		/// <returns></returns>
		public static IObjectCloneFactory GetFactory()
		{
			if (ClonableFactory.Factory == null)
			{
				throw new NoClonableFactorySetException();
			}

			return ClonableFactory.Factory;
		}

		/// <summary>
		/// Sets the current factory for cloning objects. This value can be set directly
		/// or by referencing a NuGet package that implements the IObjectCloneFactory interface.
		/// </summary>
		/// <param name="factory"></param>
		public static void SetFactory(IObjectCloneFactory factory)
		{
			ClonableFactory.Factory = factory;
		}
	}
}
