﻿//
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

namespace System
{
    /// <summary>
    /// Extensions for the <see cref="String"/> type.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Limits a string to the given length by truncating characters. If
        /// the string is less than the given length, the string is unmodified.
        /// </summary>
        /// <param name="text">The string to be truncated.</param>
        /// <param name="maxLength">The maximum length in characters of the string.</param>
        /// <returns>A new string that is no longer than maxLength in characters.</returns>
        public static string Limit(this string text, int maxLength)
        {
            string returnValue = string.Empty;

            if (text.Length > maxLength)
            {
                returnValue = text[..maxLength];
            }
            else
            {
                returnValue = text;
            }

            return returnValue;
        }

        /// <summary>
        /// Indicates whether a specified string is null, empty.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>True if the value parameter is null or System.String.Empty; false otherwise.</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Indicates whether a specified string is null, empty, or consists only of white-space
        /// characters.
        /// </summary>
        /// <param name="value">The string to test.</param>
        /// <returns>True if the value parameter is null or System.String.Empty, or if value consists
        /// exclusively of white-space characters; false otherwise.</returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
            
        }
    }
}
