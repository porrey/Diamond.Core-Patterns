//
// Copyright(C) 2019-2025, Daniel M. Porrey. All rights reserved.
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
namespace Diamond.Core.AspNetCore.DataTables
{
	/// <summary>
	/// Represents a column with an associated sort direction for ordering operations.
	/// </summary>
	/// <remarks>The <see cref="OrderedColumn"/> class is used to specify the order in which data should be sorted.
	/// The <see cref="ColumnName"/> property indicates the name of the column to sort by, and the <see cref="Direction"/>
	/// property specifies the sort direction, typically "ASC" for ascending or "DESC" for descending.</remarks>
	public class OrderedColumn
	{
		/// <summary>
		/// Gets or sets the name of the column to be ordered.
		/// </summary>
		public string ColumnName { get; set; }

		/// <summary>
		/// Gets or sets the direction of the sort operation.
		/// </summary>
		public string Direction { get; set; }
	}
}
