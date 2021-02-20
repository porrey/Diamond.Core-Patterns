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
using System;

namespace Diamond.Core.System.TemporaryFolder
{
	/// <summary>
	/// Provides a wrapper for creating and managing temporary folders. Concrete
	/// classes should implement IDisposable (not required) to remove temporary
	/// files and folders when the instance is no longer in use.
	/// </summary>
	public interface ITemporaryFolder : IDisposable
	{
		/// <summary>
		/// Creates the temporary folder.
		/// </summary>
		void Create();

		/// <summary>
		/// Gets/sets a string format with two variables, {0} and {1}, where
		/// the first place holder will be replaced with the temporary folder
		/// path and the second place holder will be replaced with the temporary
		/// folder name.
		/// </summary>
		string NamingFormat { get; set; }

		/// <summary>
		/// Gets the full path to the temporary folder that is created by this instance.
		/// </summary>
		string FullPath { get; }
	}
}
