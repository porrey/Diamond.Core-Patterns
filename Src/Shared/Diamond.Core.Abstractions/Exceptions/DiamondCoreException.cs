﻿//
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
using System.Runtime.Serialization;

namespace Diamond.Core.Abstractions
{
	/// <summary>
	/// This the base or all exceptions in the Diamond.Core library. This allows exceptions
	/// specific to this library to be caught separately than other exceptions.
	/// </summary>
	public class DiamondCoreException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DiamondCoreException"/> class.
		/// </summary>
		public DiamondCoreException()
			: base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DiamondCoreException"/> class with a specified error message.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public DiamondCoreException(string message)
				: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DiamondCoreException"/> class with a specified error
		/// message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference
		/// if no inner exception is specified.</param>
		public DiamondCoreException(string message, Exception innerException) :
				base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DiamondCoreException"/> class with serialized data.
		/// </summary>
		/// <param name="info">The System.Runtime.Serialization.SerializationInfo that holds the serialized
		/// object data about the exception being thrown.</param>
		/// <param name="context">The System.Runtime.Serialization.StreamingContext that contains contextual information
		/// about the source or destination.</param>
		protected DiamondCoreException(SerializationInfo info, StreamingContext context)
		: base(info, context)
		{
		}
	}
}
