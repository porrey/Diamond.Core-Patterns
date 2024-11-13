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
using System;
using Diamond.Core.Abstractions;

namespace Diamond.Core.Workflow.State
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TTargetType"></typeparam>
	public class ConverterBase<TTargetType> : IStateTypeConverter
	{
		/// <summary>
		/// 
		/// </summary>
		public Type TargetType => typeof(TTargetType);

		/// <summary>
		/// 
		/// </summary>
		protected string SourceStringValue { get; set; }

		/// <summary>
		/// 
		/// </summary>
		protected object SourceObjectValue { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public Type SpecificTargetType { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sourceValue"></param>
		/// <param name="specificTargetType"></param>
		/// <returns></returns>
		public virtual (bool, string, object) ConvertSource(object sourceValue, Type specificTargetType)
		{
			this.SourceObjectValue = sourceValue;
			this.SourceStringValue = Convert.ToString(sourceValue);
			this.SpecificTargetType = specificTargetType;
			return this.OnConvertSource();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected virtual (bool, string, object) OnConvertSource()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.TargetType.Name;
		}
	}
}
