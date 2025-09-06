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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace System
{
	/// <summary>
	/// Represents a base-36 numeric value, which uses the digits 0–9 and the letters A–Z to encode integers.
	/// </summary>
	/// <remarks>The <see cref="Base36"/> struct provides functionality for working with base-36 encoded numbers, 
	/// including parsing, formatting, and arithmetic operations. It supports implicit and explicit conversions  to and
	/// from integers and strings, and implements various interfaces for comparison, formatting, and parsing. <para>
	/// Base-36 encoding is case-insensitive, but this implementation always uses uppercase letters for formatting. </para>
	/// <para> The range of values supported by <see cref="Base36"/> corresponds to the range of a 32-bit signed integer 
	/// (<see cref="int.MinValue"/> to <see cref="int.MaxValue"/>). </para></remarks>
	public readonly struct Base36 :
		IEquatable<Base36>,
		IComparable<Base36>,
		IComparable,
		IFormattable,
		ISpanFormattable,
		IParsable<Base36>,
		ISpanParsable<Base36>
	{
		/// <summary>
		/// The characters used in base-36 representation, where '0' = 0, '1' = 1, ..., '9' = 9, 'A' = 10, ..., 'Z' = 35.
		/// </summary>
		private const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

		/// <summary>
		/// Represents the number zero (0) in base-36.
		/// </summary>
		public static readonly Base36 Zero = new(0);

		/// <summary>
		/// Represents the number one (1) in base-36.
		/// </summary>
		public static readonly Base36 One = new(1);

		/// <summary>
		/// Represents the smallest possible value of <see cref="Base36"/>, which is equivalent to <see cref="int.MinValue"/>.
		/// </summary>
		public static readonly Base36 MinValue = new(int.MinValue);

		/// <summary>
		/// Represents the largest possible value of <see cref="Base36"/>, which is equivalent to <see cref="int.MaxValue"/>.
		/// </summary>
		public static readonly Base36 MaxValue = new(int.MaxValue);

		/// <summary>
		/// The underlying integer value.
		/// </summary>
		public int Value { get; }

		/// <summary>
		/// Creates a Base36 from an integer value.
		/// </summary>
		/// <param name="value">The integer value.</param>
		public Base36(int value)
		{
			this.Value = value;
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="value"></param>
		public static implicit operator Base36(int value)
		{
			return new Base36(value);
		}

		/// <summary>
		/// Converts the specified <see cref="Base36"/> instance to its equivalent integer representation.
		/// </summary>
		/// <param name="value">The <see cref="Base36"/> instance to convert.</param>
		public static explicit operator int(Base36 value)
		{
			return value.Value;
		}

		/// <summary>
		/// Implicitly converts a base-36 string (e.g., "9IX", "ZZ") to Base36.
		/// Throws FormatException on invalid text and ArgumentNullException on null.
		/// </summary>
		public static implicit operator Base36(string text)
		{
			Base36 returnValue;

			if (text is null)
			{
				throw new ArgumentNullException(nameof(text));
			}
			else
			{
				if (TryParse(text, provider: null, out Base36 parsed))
				{
					returnValue = parsed;
				}
				else
				{
					throw new FormatException("Input is not a valid base-36 number or is out of Int32 range.");
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Implicitly formats Base36 to its base-36 string (uppercase A–Z).
		/// </summary>
		public static implicit operator string(Base36 value)
		{
			return value.ToString(); // base-36 by default
		}

		/// <summary>
		/// Compares the current instance with another <see cref="Base36"/> object and returns an integer  that indicates
		/// whether the current instance precedes, follows, or occurs in the same position  in the sort order as the other
		/// object.
		/// </summary>
		/// <param name="other">The <see cref="Base36"/> object to compare to the current instance.</param>
		/// <returns>A signed integer that indicates the relative order of the objects being compared: <list type="bullet"> <item>
		/// <description>Less than zero: The current instance precedes <paramref name="other"/> in the sort
		/// order.</description> </item> <item> <description>Zero: The current instance occurs in the same position in the
		/// sort order as <paramref name="other"/>.</description> </item> <item> <description>Greater than zero: The current
		/// instance follows <paramref name="other"/> in the sort order.</description> </item> </list></returns>
		public int CompareTo(Base36 other)
		{
			return this.Value.CompareTo(other.Value);
		}

		/// <summary>
		/// Compares the current instance with another object and returns an integer that indicates whether the current
		/// instance  precedes, follows, or occurs in the same position in the sort order as the other object.
		/// </summary>
		/// <param name="obj">The object to compare with the current instance. Must be of type <see cref="Base36"/>.</param>
		/// <returns>A signed integer that indicates the relative order of the objects being compared: <list type="bullet"> <item>
		/// <description>Less than zero: The current instance precedes <paramref name="obj"/> in the sort order.</description>
		/// </item> <item> <description>Zero: The current instance occurs in the same position in the sort order as <paramref
		/// name="obj"/>.</description> </item> <item> <description>Greater than zero: The current instance follows <paramref
		/// name="obj"/> in the sort order.</description> </item> </list></returns>
		/// <exception cref="ArgumentException">Thrown if <paramref name="obj"/> is not of type <see cref="Base36"/>.</exception>
		int IComparable.CompareTo(object obj)
		{
			int returnValue;

			if (obj is Base36 other)
			{
				returnValue = this.CompareTo(other);
			}
			else
			{
				throw new ArgumentException("Object must be of type Base36.");
			}

			return returnValue;
		}

		/// <summary>
		/// Determines whether the current instance is equal to the specified <see cref="Base36"/> instance.
		/// </summary>
		/// <param name="other">The <see cref="Base36"/> instance to compare with the current instance.</param>
		/// <returns><see langword="true"/> if the current instance and the specified <see cref="Base36"/> instance have the same
		/// value; otherwise, <see langword="false"/>.</returns>
		public bool Equals(Base36 other)
		{
			return this.Value == other.Value;
		}

		/// <summary>
		/// Determines whether the specified object is equal to the current instance.
		/// </summary>
		/// <param name="obj">The object to compare with the current instance.</param>
		/// <returns><see langword="true"/> if the specified object is a <see cref="Base36"/> instance  and is equal to the current
		/// instance; otherwise, <see langword="false"/>.</returns>
		public override bool Equals(object obj)
		{
			bool returnValue;

			if (obj is Base36 other)
			{
				returnValue = this.Equals(other);
			}
			else
			{
				returnValue = false;
			}

			return returnValue;
		}

		/// <summary>
		/// Returns the hash code for the current object.
		/// </summary>
		/// <remarks>The hash code is derived from the <see cref="Value"/> property.  This method is suitable for use
		/// in hashing algorithms and data structures such as a hash table.</remarks>
		/// <returns>A 32-bit signed integer that serves as the hash code for the current object.</returns>
		public override int GetHashCode()
		{
			return this.Value.GetHashCode();
		}

		/// <summary>
		/// Determines whether two <see cref="Base36"/> instances are equal.
		/// </summary>
		/// <param name="left">The first <see cref="Base36"/> instance to compare.</param>
		/// <param name="right">The second <see cref="Base36"/> instance to compare.</param>
		/// <returns><see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are equal; 
		/// otherwise, <see langword="false"/>.</returns>
		public static bool operator ==(Base36 left, Base36 right) => left.Value == right.Value;

		/// <summary>
		/// Determines whether two <see cref="Base36"/> instances are not equal.
		/// </summary>
		/// <param name="left">The first <see cref="Base36"/> instance to compare.</param>
		/// <param name="right">The second <see cref="Base36"/> instance to compare.</param>
		/// <returns><see langword="true"/> if the values of <paramref name="left"/> and <paramref name="right"/> are not equal; 
		/// otherwise, <see langword="false"/>.</returns>
		public static bool operator !=(Base36 left, Base36 right) => left.Value != right.Value;

		/// <summary>
		/// Determines whether one <see cref="Base36"/> instance is less than another.
		/// </summary>
		/// <param name="left">The first <see cref="Base36"/> instance to compare.</param>
		/// <param name="right">The second <see cref="Base36"/> instance to compare.</param>
		/// <returns><see langword="true"/> if the value of <paramref name="left"/> is less than the value of  <paramref
		/// name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator <(Base36 left, Base36 right) => left.Value < right.Value;

		/// <summary>
		/// Determines whether the value of the left <see cref="Base36"/> operand is greater than the value of the right <see
		/// cref="Base36"/> operand.
		/// </summary>
		/// <param name="left">The first <see cref="Base36"/> operand to compare.</param>
		/// <param name="right">The second <see cref="Base36"/> operand to compare.</param>
		/// <returns><see langword="true"/> if the value of <paramref name="left"/> is greater than the value of <paramref
		/// name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator >(Base36 left, Base36 right) => left.Value > right.Value;

		/// <summary>
		/// Determines whether the value of the left <see cref="Base36"/> instance is less than or equal to the value of the
		/// right <see cref="Base36"/> instance.
		/// </summary>
		/// <param name="left">The first <see cref="Base36"/> instance to compare.</param>
		/// <param name="right">The second <see cref="Base36"/> instance to compare.</param>
		/// <returns><see langword="true"/> if the value of <paramref name="left"/> is less than or equal to the value of <paramref
		/// name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator <=(Base36 left, Base36 right) => left.Value <= right.Value;

		/// <summary>
		/// Determines whether the value of the left <see cref="Base36"/> operand is greater than or equal to the value of the
		/// right <see cref="Base36"/> operand.
		/// </summary>
		/// <param name="left">The first <see cref="Base36"/> operand to compare.</param>
		/// <param name="right">The second <see cref="Base36"/> operand to compare.</param>
		/// <returns><see langword="true"/> if the value of <paramref name="left"/> is greater than or equal to the value of <paramref
		/// name="right"/>; otherwise, <see langword="false"/>.</returns>
		public static bool operator >=(Base36 left, Base36 right) => left.Value >= right.Value;

		/// <summary>
		/// Adds two <see cref="Base36"/> values and returns the result.
		/// </summary>
		/// <param name="a">The first <see cref="Base36"/> value to add.</param>
		/// <param name="b">The second <see cref="Base36"/> value to add.</param>
		/// <returns>A new <see cref="Base36"/> instance representing the sum of <paramref name="a"/> and <paramref name="b"/>.</returns>
		public static Base36 operator +(Base36 a, Base36 b)
		{
			Base36 returnValue;
			checked
			{
				returnValue = new Base36(a.Value + b.Value);
			}
			return returnValue;
		}

		/// <summary>
		/// Subtracts one <see cref="Base36"/> value from another and returns the result.
		/// </summary>
		/// <param name="a">The minuend, represented as a <see cref="Base36"/> value.</param>
		/// <param name="b">The subtrahend, represented as a <see cref="Base36"/> value.</param>
		/// <returns>A new <see cref="Base36"/> instance representing the result of the subtraction.</returns>
		public static Base36 operator -(Base36 a, Base36 b)
		{
			Base36 returnValue;
			checked
			{
				returnValue = new Base36(a.Value - b.Value);
			}
			return returnValue;
		}

		/// <summary>
		/// Multiplies two <see cref="Base36"/> values and returns the result.
		/// </summary>
		/// <param name="a">The first <see cref="Base36"/> value to multiply.</param>
		/// <param name="b">The second <see cref="Base36"/> value to multiply.</param>
		/// <returns>A new <see cref="Base36"/> instance representing the product of <paramref name="a"/> and <paramref name="b"/>.</returns>
		public static Base36 operator *(Base36 a, Base36 b)
		{
			Base36 returnValue;
			checked
			{
				returnValue = new Base36(a.Value * b.Value);
			}
			return returnValue;
		}

		/// <summary>
		/// Divides one <see cref="Base36"/> value by another and returns the result as a new <see cref="Base36"/> instance.
		/// </summary>
		/// <remarks>The division is performed on the underlying numeric values of the <see cref="Base36"/>
		/// instances.</remarks>
		/// <param name="a">The dividend, represented as a <see cref="Base36"/> instance.</param>
		/// <param name="b">The divisor, represented as a <see cref="Base36"/> instance.</param>
		/// <returns>A new <see cref="Base36"/> instance representing the result of the division.</returns>
		public static Base36 operator /(Base36 a, Base36 b)
		{
			return new Base36(a.Value / b.Value);
		}

		/// <summary>
		/// Computes the remainder of the division of one <see cref="Base36"/> value by another.
		/// </summary>
		/// <param name="a">The dividend, represented as a <see cref="Base36"/> value.</param>
		/// <param name="b">The divisor, represented as a <see cref="Base36"/> value.</param>
		/// <returns>A new <see cref="Base36"/> instance representing the remainder of the division of <paramref name="a"/> by
		/// <paramref name="b"/>.</returns>
		public static Base36 operator %(Base36 a, Base36 b)
		{
			return new Base36(a.Value % b.Value);
		}

		/// <summary>
		/// Returns the specified <see cref="Base36"/> instance unchanged.
		/// </summary>
		/// <param name="a">The <see cref="Base36"/> instance to return.</param>
		/// <returns>The same <see cref="Base36"/> instance as the input parameter <paramref name="a"/>.</returns>
		public static Base36 operator +(Base36 a) => a;

		/// <summary>
		/// Negates the value of the specified <see cref="Base36"/> instance.
		/// </summary>
		/// <param name="a">The <see cref="Base36"/> instance to negate.</param>
		/// <returns>A new <see cref="Base36"/> instance representing the negated value of <paramref name="a"/>.</returns>
		public static Base36 operator -(Base36 a)
		{
			Base36 returnValue;
			checked
			{
				returnValue = new Base36(-a.Value);
			}
			return returnValue;
		}

		/// <summary>
		/// Increments the value of the specified <see cref="Base36"/> instance by one.
		/// </summary>
		/// <param name="a">The <see cref="Base36"/> instance to increment.</param>
		/// <returns>A new <see cref="Base36"/> instance representing the incremented value.</returns>
		public static Base36 operator ++(Base36 a)
		{
			Base36 returnValue;
			checked
			{
				returnValue = new Base36(a.Value + 1);
			}
			return returnValue;
		}

		/// <summary>
		/// Decrements the value of the specified <see cref="Base36"/> instance by one.
		/// </summary>
		/// <param name="a">The <see cref="Base36"/> instance to decrement.</param>
		/// <returns>A new <see cref="Base36"/> instance representing the decremented value.</returns>
		public static Base36 operator --(Base36 a)
		{
			Base36 returnValue;
			checked
			{
				returnValue = new Base36(a.Value - 1);
			}
			return returnValue;
		}

		/// <summary>
		/// Adds an integer value to a <see cref="Base36"/> instance and returns the resulting <see cref="Base36"/> value.
		/// </summary>
		/// <param name="a">The <see cref="Base36"/> instance to add to.</param>
		/// <param name="b">The integer value to add to the <see cref="Base36"/> instance.</param>
		/// <returns>A new <see cref="Base36"/> instance representing the sum of the original value and the integer.</returns>
		public static Base36 operator +(Base36 a, int b) => new(checked(a.Value + b));

		/// <summary>
		/// Subtracts an integer value from a <see cref="Base36"/> instance and returns the resulting <see cref="Base36"/>
		/// value.
		/// </summary>
		/// <param name="a">The <see cref="Base36"/> instance to subtract from.</param>
		/// <param name="b">The integer value to subtract.</param>
		/// <returns>A new <see cref="Base36"/> instance representing the result of the subtraction.</returns>
		public static Base36 operator -(Base36 a, int b) => new(checked(a.Value - b));

		/// <summary>
		/// Multiplies the numeric value of a <see cref="Base36"/> instance by an integer.
		/// </summary>
		/// <param name="a">The <see cref="Base36"/> instance to be multiplied.</param>
		/// <param name="b">The integer by which to multiply the value of <paramref name="a"/>.</param>
		/// <returns>A new <see cref="Base36"/> instance representing the product of the multiplication.</returns>
		public static Base36 operator *(Base36 a, int b) => new(checked(a.Value * b));

		/// <summary>
		/// Divides the value of the specified <see cref="Base36"/> instance by an integer and returns the result as a new
		/// <see cref="Base36"/> instance.
		/// </summary>
		/// <remarks>This operator performs integer division, truncating any fractional part of the result.</remarks>
		/// <param name="a">The <see cref="Base36"/> instance to be divided.</param>
		/// <param name="b">The integer divisor. Must not be zero.</param>
		/// <returns>A new <see cref="Base36"/> instance representing the result of the division.</returns>
		public static Base36 operator /(Base36 a, int b) => new(a.Value / b);

		/// <summary>
		/// Computes the remainder of the division of the value of the specified <see cref="Base36"/> instance by an integer.
		/// </summary>
		/// <param name="a">The <see cref="Base36"/> instance whose value is to be divided.</param>
		/// <param name="b">The integer divisor. Must be a non-zero value.</param>
		/// <returns>A new <see cref="Base36"/> instance representing the remainder of the division.</returns>
		public static Base36 operator %(Base36 a, int b) => new(a.Value % b);

		/// <summary>
		/// Returns a string representation of the current object.
		/// </summary>
		/// <remarks>This method uses the invariant culture for formatting. To customize the formatting, use the  <see
		/// cref="ToString(IFormatProvider)"/> overload.</remarks>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			return this.ToString(null, CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Formats as base-36 by default. If format == "D" or "d", formats decimal (base-10).
		/// Any other format is treated as base-36.
		/// </summary>
		public string ToString(string format, IFormatProvider formatProvider)
		{
			string returnValue;

			if (string.Equals(format, "D", StringComparison.OrdinalIgnoreCase))
			{
				returnValue = this.Value.ToString(formatProvider);
			}
			else
			{
				Span<char> buffer = stackalloc char[33]; // enough for Int32 in base-36 with sign
				if (this.TryFormat(buffer, out int charsWritten, format, formatProvider))
				{
					returnValue = new string(buffer.Slice(0, charsWritten));
				}
				else
				{
					// Fallback via heap if ever needed
					returnValue = FormatBase36(this.Value);
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Attempts to format the current value into the provided character span.
		/// </summary>
		/// <remarks>If the specified format is 'D' or 'd', the value is formatted as a decimal number. For other
		/// formats, the method may use an alternative representation, such as Base36. Ensure that the <paramref
		/// name="destination"/> span is large enough to accommodate the formatted value; otherwise, the method will return
		/// <see langword="false"/>.</remarks>
		/// <param name="destination">The span of characters to which the formatted value will be written.</param>
		/// <param name="charsWritten">When this method returns, contains the number of characters written to <paramref name="destination"/>.</param>
		/// <param name="format">A read-only span of characters that specifies the format to use. Use 'D' or 'd' for decimal formatting; other
		/// formats may be used for alternative representations.</param>
		/// <param name="provider">An object that provides culture-specific formatting information.</param>
		/// <returns><see langword="true"/> if the value was successfully formatted and written to <paramref name="destination"/>;
		/// otherwise, <see langword="false"/>.</returns>
		public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider)
		{
			bool writeDecimal = (format.Length == 1 && (format[0] == 'D' || format[0] == 'd'));
			bool ok;

			if (writeDecimal)
			{
				ok = this.Value.TryFormat(destination, out charsWritten, format, provider);
			}
			else
			{
				ok = TryFormatBase36(this.Value, destination, out charsWritten);
			}

			return ok;
		}

		/// <summary>
		/// Converts the specified integer value to its equivalent string representation in base 36.
		/// </summary>
		/// <remarks>Base 36 is a numeral system that uses 36 distinct characters: the digits '0'-'9' and the letters
		/// 'A'-'Z'. This method handles both positive and negative integers, as well as zero.</remarks>
		/// <param name="value">The integer value to convert. Can be positive, negative, or zero.</param>
		/// <returns>A string representing the base 36 equivalent of the specified integer.  The string will use characters '0'-'9' and
		/// 'A'-'Z' for the digits, and a leading '-' for negative values.</returns>
		private static string FormatBase36(int value)
		{
			string returnValue;

			if (value == 0)
			{
				returnValue = "0";
			}
			else
			{
				bool negative = (value < 0);
				long n = value;
				if (negative)
				{
					n = -(long)value;
				}

				char[] tmp = new char[33];
				int pos = tmp.Length;

				while (n > 0)
				{
					long rem = n % 36;
					n /= 36;
					tmp[--pos] = Digits[(int)rem];
				}

				if (negative)
				{
					tmp[--pos] = '-';
				}

				returnValue = new string(tmp, pos, tmp.Length - pos);
			}

			return returnValue;
		}

		/// <summary>
		/// Attempts to format the specified integer value as a Base-36 string and write it to the provided destination
		/// buffer.
		/// </summary>
		/// <remarks>Base-36 encoding uses the characters '0'-'9' and 'A'-'Z' to represent values. Negative numbers
		/// are prefixed with a '-' character. If the <paramref name="destination"/> buffer is too small to contain the
		/// formatted result, the method returns <see langword="false"/> and does not modify the buffer.</remarks>
		/// <param name="value">The integer value to format. Can be positive, negative, or zero.</param>
		/// <param name="destination">The buffer to which the formatted Base-36 string will be written. The buffer must be large enough to hold the
		/// result.</param>
		/// <param name="charsWritten">When this method returns, contains the number of characters written to the <paramref name="destination"/> buffer,
		/// if the operation succeeds; otherwise, 0.</param>
		/// <returns><see langword="true"/> if the value was successfully formatted and written to the <paramref name="destination"/>
		/// buffer; otherwise, <see langword="false"/>.</returns>
		private static bool TryFormatBase36(int value, Span<char> destination, out int charsWritten)
		{
			bool returnValue;

			if (value == 0)
			{
				if (destination.Length >= 1)
				{
					destination[0] = '0';
					charsWritten = 1;
					returnValue = true;
				}
				else
				{
					charsWritten = 0;
					returnValue = false;
				}
			}
			else
			{
				bool negative = (value < 0);
				long n = value;
				if (negative)
				{
					n = -(long)value;
				}

				int pos = destination.Length;
				long work = n;

				while (work > 0 && pos > 0)
				{
					long rem = work % 36;
					work /= 36;
					destination[--pos] = Digits[(int)rem];
				}

				if (work != 0)
				{
					charsWritten = 0;
					returnValue = false;
				}
				else
				{
					if (negative)
					{
						if (pos == 0)
						{
							charsWritten = 0;
							returnValue = false;
							return returnValue;
						}

						destination[--pos] = '-';
					}

					int len = destination.Length - pos;
					if (pos > 0)
					{
						destination.Slice(pos, len).CopyTo(destination);
					}

					charsWritten = len;
					returnValue = true;
				}
			}

			return returnValue;
		}

		/// <summary>
		/// Converts the specified string representation of a base-36 number to its <see cref="Base36"/> equivalent.
		/// </summary>
		/// <param name="s">The string representation of a base-36 number to convert.</param>
		/// <param name="provider">An object that provides culture-specific formatting information. This parameter is currently not used.</param>
		/// <returns>A <see cref="Base36"/> instance that represents the number specified by <paramref name="s"/>.</returns>
		/// <exception cref="FormatException">Thrown if <paramref name="s"/> is not a valid base-36 number or if the value is outside the range of a 32-bit
		/// signed integer.</exception>
		public static Base36 Parse(string s, IFormatProvider provider)
		{
			Base36 returnValue;

			if (TryParse(s, provider, out Base36 value))
			{
				returnValue = value;
			}
			else
			{
				throw new FormatException("Input is not a valid base-36 number or is out of Int32 range.");
			}

			return returnValue;
		}

		/// <summary>
		/// Attempts to parse the specified string representation of a Base36 number into its equivalent <see cref="Base36"/>
		/// value.
		/// </summary>
		/// <remarks>This method does not throw an exception if the conversion fails. Instead, it returns <see
		/// langword="false"/> and sets <paramref name="result"/> to <see cref="Base36.Zero"/>.</remarks>
		/// <param name="s">The string representation of the Base36 number to parse. Cannot be <see langword="null"/>.</param>
		/// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information, or <see langword="null"/>
		/// to use the current culture.</param>
		/// <param name="result">When this method returns, contains the <see cref="Base36"/> value equivalent to the number contained in <paramref
		/// name="s"/>,  if the conversion succeeded; otherwise, <see cref="Base36.Zero"/>. This parameter is passed
		/// uninitialized.</param>
		/// <returns><see langword="true"/> if <paramref name="s"/> was successfully parsed; otherwise, <see langword="false"/>.</returns>
		public static bool TryParse([NotNullWhen(true)] string s, IFormatProvider provider, out Base36 result)
		{
			Base36 outValue;
			bool ok;

			if (s is null)
			{
				result = Zero;
				ok = false;
			}
			else
			{
				ok = TryParse((ReadOnlySpan<char>)s.AsSpan(), provider, out outValue);
				result = outValue;
			}

			return ok;
		}

		/// <summary>
		/// Converts the specified read-only span of characters representing a base-36 number  into its <see cref="Base36"/>
		/// equivalent.
		/// </summary>
		/// <param name="s">The span of characters containing the base-36 number to convert.</param>
		/// <param name="provider">An object that provides culture-specific formatting information. This parameter is currently ignored.</param>
		/// <returns>A <see cref="Base36"/> instance that represents the number specified by <paramref name="s"/>.</returns>
		/// <exception cref="FormatException">Thrown if <paramref name="s"/> does not represent a valid base-36 number  or if the value is outside the range of
		/// a 32-bit signed integer.</exception>
		public static Base36 Parse(ReadOnlySpan<char> s, IFormatProvider provider)
		{
			Base36 returnValue;

			if (TryParse(s, provider, out Base36 value))
			{
				returnValue = value;
			}
			else
			{
				throw new FormatException("Input is not a valid base-36 number or is out of Int32 range.");
			}

			return returnValue;
		}

		/// <summary>
		/// Attempts to parse the specified span of characters as a Base36 number.
		/// </summary>
		/// <remarks>The method supports optional leading '+' or '-' signs to indicate the number's sign.  If the span
		/// contains invalid characters or the resulting value exceeds the range of a Base36 number,  the method returns <see
		/// langword="false"/> and sets <paramref name="result"/> to <see cref="Base36.Zero"/>.</remarks>
		/// <param name="s">The span of characters to parse.</param>
		/// <param name="provider">An object that provides culture-specific formatting information. This parameter is currently ignored.</param>
		/// <param name="result">When this method returns, contains the parsed <see cref="Base36"/> value if the conversion succeeded,  or <see
		/// cref="Base36.Zero"/> if the conversion failed. This parameter is passed uninitialized.</param>
		/// <returns><see langword="true"/> if the span was successfully parsed as a Base36 number; otherwise, <see langword="false"/>.</returns>
		public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out Base36 result)
		{
			bool ok;

			int sign = 1;
			int idx = 0;
			int len = s.Length;

			if (len == 0)
			{
				result = Zero;
				ok = false;
			}
			else
			{
				char first = s[0];
				if (first == '+' || first == '-')
				{
					if (first == '-')
					{
						sign = -1;
					}
					idx = 1;
					if (idx >= len)
					{
						result = Zero;
						return false;
					}
				}

				int acc = 0;
				bool valid = true;

				for (int i = idx; valid && i < len; i++)
				{
					char c = s[i];
					int d = CharToDigit(c);
					if (d < 0)
					{
						valid = false;
					}
					else
					{
						try
						{
							checked
							{
								acc = acc * 36 + d;
							}
						}
						catch (OverflowException)
						{
							valid = false;
						}
					}
				}

				if (valid)
				{
					try
					{
						checked
						{
							acc = acc * sign;
						}
						result = new Base36(acc);
						ok = true;
					}
					catch (OverflowException)
					{
						result = Zero;
						ok = false;
					}
				}
				else
				{
					result = Zero;
					ok = false;
				}
			}

			return ok;
		}

		/// <summary>
		/// Converts a character to its corresponding numeric value.
		/// </summary>
		/// <remarks>The conversion is case-insensitive. Non-alphanumeric characters will result in a return value of
		/// -1.</remarks>
		/// <param name="c">The character to convert. Can be a digit ('0'-'9') or an uppercase or lowercase letter ('A'-'Z' or 'a'-'z').</param>
		/// <returns>The numeric value of the character: 0-9 for digits, 10-35 for letters ('A' or 'a' = 10, 'B' or 'b' = 11, ..., 'Z'
		/// or 'z' = 35). Returns -1 if the character is not a valid digit or letter.</returns>
		private static int CharToDigit(char c)
		{
			int returnValue;
			char u = char.ToUpperInvariant(c);

			if (u >= '0' && u <= '9')
			{
				returnValue = (u - '0');
			}
			else if (u >= 'A' && u <= 'Z')
			{
				returnValue = 10 + (u - 'A');
			}
			else
			{
				returnValue = -1;
			}

			return returnValue;
		}
	}
}