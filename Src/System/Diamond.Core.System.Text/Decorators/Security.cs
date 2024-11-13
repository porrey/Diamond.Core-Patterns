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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace System
{
	/// <summary>
	/// Extensions for the <see cref="String"/> type.
	/// </summary>
	public static class SecureData
	{
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

			byte[] inputBytes = Encoding.UTF8.GetBytes(data);
			byte[] hashBytes = MD5.HashData(inputBytes);
			returnValue = BitConverter.ToString(hashBytes);

			if (!includeDashes)
			{
				returnValue = returnValue.Replace("-", "");
			}

			return returnValue;
		}

		/// <summary>
		/// Computes the SHA512 hash of a string and returns it in string format.
		/// </summary>
		/// <param name="data">The string data from which the hash is computed.</param>
		/// <param name="includeDashes">Specifies whether or not the return string should included dashes.</param>
		/// <returns>The computed hash of the input string.</returns>
		public static string ComputeHash(this string data, bool includeDashes = false)
		{
			string returnValue = string.Empty;

			byte[] inputBytes = Encoding.UTF8.GetBytes(data);
			byte[] hashBytes = SHA512.HashData(inputBytes);
			returnValue = BitConverter.ToString(hashBytes);

			if (!includeDashes)
			{
				returnValue = returnValue.Replace("-", "");
			}

			return returnValue;
		}

		/// <summary>
		/// Computes the hash of a string and returns it in string format.
		/// </summary>
		/// <param name="data">The string data from which the hash is computed.</param>
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
		/// <returns>The computed hash of the input string.</returns>
		public static string ComputeHash(this string data, string hashName, bool includeDashes = false)
		{
			string returnValue = string.Empty;
			HashAlgorithm hashAlgorithm = null;

			try
			{
				switch (hashName)
				{
					case "SHA":
					case "SHA1":
						hashAlgorithm = SHA1.Create();
						break;
					case "MD5":
						hashAlgorithm = MD5.Create();
						break;
					case "SHA256":
						hashAlgorithm = SHA256.Create();
						break;
					case "SHA384":
						hashAlgorithm = SHA384.Create();
						break;
					case "SHA512":
						hashAlgorithm = SHA512.Create();
						break;
					default:
						throw new ArgumentException($"Invalid hash name '{hashName}'.");
				}

				if (hashAlgorithm != null)
				{
					byte[] inputBytes = Encoding.UTF8.GetBytes(data);
					byte[] hashBytes = hashAlgorithm.ComputeHash(inputBytes);
					returnValue = BitConverter.ToString(hashBytes);

					if (!includeDashes)
					{
						returnValue = returnValue.Replace("-", "");
					}
				}
			}
			finally
			{
				hashAlgorithm?.Dispose();
			}

			return returnValue;
		}

		/// <summary>
		/// Computes a has for a list of items.
		/// </summary>
		/// <typeparam name="TItem">The list from which the hash is computed.</typeparam>
		/// <param name="items"></param>
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
		/// <returns>The computed hash of the input string.</returns>
		public static string ComputeHash<TItem>(this IEnumerable<TItem> items, string hashName = "SHA512", bool includeDashes = false)
		{
			string returnValue = string.Empty;
			HashAlgorithm hashAlgorithm = null;

			try
			{
				switch (hashName)
				{
					case "SHA":
					case "SHA1":
						hashAlgorithm = SHA1.Create();
						break;
					case "MD5":
						hashAlgorithm = MD5.Create();
						break;
					case "SHA256":
						hashAlgorithm = SHA256.Create();
						break;
					case "SHA384":
						hashAlgorithm = SHA384.Create();
						break;
					case "SHA512":
						hashAlgorithm = SHA512.Create();
						break;
					default:
						throw new ArgumentException($"Invalid hash name '{hashName}'.");
				}

				if (hashAlgorithm != null)
				{
					//
					// Concatenate the items with a '.'
					//
					string itemsString = String.Join(".", items.Select(t => Convert.ToString(t)));

					//
					// Convert the string to a byte array.
					//
					byte[] data = Encoding.UTF8.GetBytes(itemsString);

					//
					// Compute the hash.
					//
					byte[] hashBytes = hashAlgorithm.ComputeHash(data);

					//
					// Format the hash to a string.
					//
					returnValue = BitConverter.ToString(hashBytes);

					if (!includeDashes)
					{
						returnValue = returnValue.Replace("-", "");
					}
				}
			}
			finally
			{
				hashAlgorithm?.Dispose();
			}

			return returnValue;
		}
	}
}
