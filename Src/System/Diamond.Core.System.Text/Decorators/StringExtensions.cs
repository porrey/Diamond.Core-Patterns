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
using System.Security.Cryptography;
using System.Text;

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
		/// Computes the signature of a string using MD5 hash and
		/// returns it in string format.
		/// </summary>
		/// <param name="data">The string data from which the signature is computed.</param>
		/// <param name="includeDashes">Specifies whether or not the return string should included dashes.</param>
		/// <returns>The computed signature of the input string.</returns>
		public static string Signature(this string data, bool includeDashes = false)
		{
			string returnValue = string.Empty;

			using (MD5 md5 = MD5.Create())
			{
				byte[] inputBytes = Encoding.UTF8.GetBytes(data);
				byte[] hashBytes = md5.ComputeHash(inputBytes);
				returnValue = BitConverter.ToString(hashBytes);

				if (!includeDashes)
				{
					returnValue = returnValue.Replace("-", "");
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Computes the SHA512 hash of a string and returns it in string format.
		/// </summary>
		/// <param name="data">The string data from which the signature is computed.</param>
		/// <param name="includeDashes">Specifies whether or not the return string should included dashes.</param>
		/// <returns>The computed signature of the input string.</returns>
		public static string ComputeHash(this string data, bool includeDashes = false)
		{
			string returnValue = string.Empty;

			using (SHA512 hash = SHA512.Create())
			{
				byte[] inputBytes = Encoding.UTF8.GetBytes(data);
				byte[] hashBytes = hash.ComputeHash(inputBytes);
				returnValue = BitConverter.ToString(hashBytes);

				if (!includeDashes)
				{
					returnValue = returnValue.Replace("-", "");
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Computes the hash of a string and returns it in string format.
		/// </summary>
		/// <param name="data">The string data from which the signature is computed.</param>
		/// <param name="hashName">
		/// <list type="table">
		///		<listheader>
		///			<term>Possible Values</term>
		///			<description>The hash algorithm implementation to use. This table shows the list of possible values</description>
		///		</listheader>
		///		<item>
		///			<term>SHA</term>
		///			<description><see cref="SHA1"/></description>
		///		</item>
		///		<item>
		///			<term>SHA1</term>
		///			<description><see cref="SHA1"/></description>
		///		</item>
		///		<item>
		///			<term>MD5</term>
		///			<description><see cref="MD5"/></description>
		///		</item>
		///		<item>
		///			<term>SHA256 or SHA-256</term>
		///			<description><see cref="SHA256"/></description>
		///		</item>
		///		<item>
		///			<term>SHA384 or SHA-384</term>
		///			<description><see cref="SHA384"/></description>
		///		</item>
		///		<item>
		///			<term>SHA512 or SHA-512</term>
		///			<description><see cref="SHA512"/></description>
		///		</item>
		/// </list>
		/// </param>
		/// <param name="includeDashes">Specifies whether or not the return string should included dashes.</param>
		/// <returns>The computed signature of the input string.</returns>
		public static string ComputeHash(this string data, string hashName, bool includeDashes = false)
		{
			string returnValue = string.Empty;

			using (HashAlgorithm hash = HashAlgorithm.Create(hashName))
			{
				byte[] inputBytes = Encoding.UTF8.GetBytes(data);
				byte[] hashBytes = hash.ComputeHash(inputBytes);
				returnValue = BitConverter.ToString(hashBytes);

				if (!includeDashes)
				{
					returnValue = returnValue.Replace("-", "");
				}
			}

			return returnValue;
		}
	}
}
