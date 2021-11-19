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
namespace Diamond.Core.Example
{
	/// <summary>
	/// Specifies the active database configuration to use.
	/// </summary>
	public enum ActiveDatabase
	{
		/// <summary>
		/// SQL Server database is used.
		/// </summary>
		SqlServer,
		/// <summary>
		/// SQLite database is used.
		/// </summary>
		SQLite,
		/// <summary>
		/// The PostgreSQL is used.
		/// </summary>
		PostgreSQL,
		/// <summary>
		/// The in memory database is used.
		/// </summary>
		InMemory
	}

	/// <summary>
	/// Contains connection strings for the database. This model
	/// is used for binding against the application settings.
	/// </summary>
	public class DatabaseOptions
	{
		public const string Key = "DatabaseOptions";

		/// <summary>
		/// Specifies the currently active database.
		/// </summary>
		public ActiveDatabase ActiveDatabase { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string SqlServer { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string SQLite { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string PostgreSQL { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string InMemory { get; set; }
	}
}
