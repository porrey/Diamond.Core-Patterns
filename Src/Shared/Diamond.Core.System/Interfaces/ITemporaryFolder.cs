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

namespace Diamond.Core.System
{
	/// <summary>
	/// Provides a wrapper for creating and managing temporary folders. Concrete
	/// classes should implement IDisposable (not required) to remove temporary
	/// files and folders when the instance is no longer in use.
	/// </summary>
	public interface ITemporaryFolder : IDisposable
	{
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

	/// <summary>
	/// Factory for creating instances of ITemporaryFolder.
	/// </summary>
	public interface ITemporaryFolderFactory
	{
		/// <summary>
		/// Creates a default instance of ITemporaryFolder.
		/// </summary>
		/// <returns>An instance of ITemporaryFolder.</returns>
		ITemporaryFolder Create();

		/// <summary>
		/// Creates a default instance of ITemporaryFolder using
		/// the given name format.
		/// </summary>
		/// <param name="namingFormat">Specifies the naming format to
		/// use with this new instance</param>
		/// <returns>An instance of ITemporaryFolder.</returns>
		ITemporaryFolder Create(string namingFormat);
	}
}
