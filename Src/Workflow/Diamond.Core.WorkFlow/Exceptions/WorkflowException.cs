﻿//
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
using System;
using System.Runtime.Serialization;
using Diamond.Core.Abstractions;

namespace Diamond.Core.Workflow
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class WorkflowException : DiamondCoreException
	{
		/// <summary>
		/// 
		/// </summary>
		public WorkflowException()
			: base()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		public WorkflowException(string message)
				: base(message)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected WorkflowException(SerializationInfo info, StreamingContext context)
				: base(info, context)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message"></param>
		/// <param name="innerException"></param>
		public WorkflowException(string message, Exception innerException) :
				base(message, innerException)
		{
		}
	}
}
