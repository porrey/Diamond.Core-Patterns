// Copyright(C) 2019-2026, Daniel M. Porrey. All rights reserved.
// Licensed under the LGPL-3.0-or-later license.
using System;
using Xunit;

namespace Diamond.Core.Tests
{
	/// <summary>
	/// Tests for the <see cref="Base36"/> struct.
	/// </summary>
	public class Base36Tests
	{
		// ─── Construction & static members ───────────────────────────────────

		[Fact]
		public void Constructor_StoresValue()
		{
			var b = new Base36(42);
			Assert.Equal(42, b.Value);
		}

		[Fact]
		public void Zero_HasValueZero()
		{
			Assert.Equal(0, Base36.Zero.Value);
		}

		[Fact]
		public void One_HasValueOne()
		{
			Assert.Equal(1, Base36.One.Value);
		}

		[Fact]
		public void MinValue_EqualsIntMinValue()
		{
			Assert.Equal(int.MinValue, Base36.MinValue.Value);
		}

		[Fact]
		public void MaxValue_EqualsIntMaxValue()
		{
			Assert.Equal(int.MaxValue, Base36.MaxValue.Value);
		}

		// ─── Implicit / explicit conversions ─────────────────────────────────

		[Fact]
		public void ImplicitFromInt_CreatesCorrectBase36()
		{
			Base36 b = 100;
			Assert.Equal(100, b.Value);
		}

		[Fact]
		public void ExplicitToInt_ReturnsUnderlyingValue()
		{
			var b = new Base36(999);
			Assert.Equal(999, (int)b);
		}

		[Fact]
		public void ImplicitFromString_ParsesValidBase36()
		{
			Base36 b = "Z";
			Assert.Equal(35, b.Value);
		}

		[Fact]
		public void ImplicitFromString_NullInput_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				Base36 b = (string)null;
			});
		}

		[Fact]
		public void ImplicitFromString_InvalidInput_ThrowsFormatException()
		{
			Assert.Throws<FormatException>(() =>
			{
				Base36 b = "!!!";
			});
		}

		[Fact]
		public void ImplicitToString_ReturnsBase36Representation()
		{
			var b = new Base36(35);
			string s = b;
			Assert.Equal("Z", s);
		}

		// ─── ToString overloads ───────────────────────────────────────────────

		[Fact]
		public void ToString_NoArgs_ReturnsBase36String()
		{
			Assert.Equal("Z", new Base36(35).ToString());
		}

		[Fact]
		public void ToString_FormatD_ReturnsDecimalString()
		{
			Assert.Equal("35", new Base36(35).ToString("D", null));
		}

		[Fact]
		public void ToString_LowercaseFormatD_ReturnsDecimalString()
		{
			Assert.Equal("35", new Base36(35).ToString("d", null));
		}

		[Theory]
		[InlineData(0, "0")]
		[InlineData(1, "1")]
		[InlineData(9, "9")]
		[InlineData(10, "A")]
		[InlineData(35, "Z")]
		[InlineData(36, "10")]
		[InlineData(1295, "ZZ")]
		[InlineData(1296, "100")]
		public void ToString_KnownValues_ProducesCorrectBase36String(int value, string expected)
		{
			Assert.Equal(expected, new Base36(value).ToString());
		}

		// ─── Parse / TryParse ─────────────────────────────────────────────────

		[Theory]
		[InlineData("0", 0)]
		[InlineData("1", 1)]
		[InlineData("A", 10)]
		[InlineData("Z", 35)]
		[InlineData("10", 36)]
		[InlineData("ZZ", 1295)]
		public void Parse_ValidString_ReturnsCorrectValue(string input, int expected)
		{
			Base36 b = Base36.Parse(input, null);
			Assert.Equal(expected, b.Value);
		}

		[Fact]
		public void Parse_InvalidString_ThrowsFormatException()
		{
			Assert.Throws<FormatException>(() => Base36.Parse("!!!", null));
		}

		[Fact]
		public void TryParse_ValidString_ReturnsTrueAndCorrectValue()
		{
			bool ok = Base36.TryParse("A", null, out Base36 result);
			Assert.True(ok);
			Assert.Equal(10, result.Value);
		}

		[Fact]
		public void TryParse_InvalidString_ReturnsFalse()
		{
			bool ok = Base36.TryParse("!!", null, out Base36 result);
			Assert.False(ok);
		}

		[Fact]
		public void TryParse_NullString_ReturnsFalse()
		{
			bool ok = Base36.TryParse((string)null, null, out Base36 result);
			Assert.False(ok);
		}

		// ─── Arithmetic operators ─────────────────────────────────────────────

		[Fact]
		public void Addition_TwoBase36_ReturnsCorrectSum()
		{
			Base36 a = 10;
			Base36 b = 20;
			Assert.Equal(30, (a + b).Value);
		}

		[Fact]
		public void Subtraction_TwoBase36_ReturnsCorrectDifference()
		{
			Base36 a = 30;
			Base36 b = 10;
			Assert.Equal(20, (a - b).Value);
		}

		[Fact]
		public void Multiplication_TwoBase36_ReturnsCorrectProduct()
		{
			Base36 a = 6;
			Base36 b = 7;
			Assert.Equal(42, (a * b).Value);
		}

		[Fact]
		public void Division_TwoBase36_ReturnsCorrectQuotient()
		{
			Base36 a = 42;
			Base36 b = 7;
			Assert.Equal(6, (a / b).Value);
		}

		[Fact]
		public void Modulo_TwoBase36_ReturnsCorrectRemainder()
		{
			Base36 a = 10;
			Base36 b = 3;
			Assert.Equal(1, (a % b).Value);
		}

		[Fact]
		public void Addition_Base36AndInt_ReturnsCorrectSum()
		{
			Base36 a = 10;
			Assert.Equal(15, (a + 5).Value);
		}

		[Fact]
		public void Subtraction_Base36AndInt_ReturnsCorrectDifference()
		{
			Base36 a = 10;
			Assert.Equal(5, (a - 5).Value);
		}

		[Fact]
		public void Multiplication_Base36AndInt_ReturnsCorrectProduct()
		{
			Base36 a = 10;
			Assert.Equal(30, (a * 3).Value);
		}

		[Fact]
		public void Division_Base36AndInt_ReturnsCorrectQuotient()
		{
			Base36 a = 10;
			Assert.Equal(5, (a / 2).Value);
		}

		[Fact]
		public void Modulo_Base36AndInt_ReturnsCorrectRemainder()
		{
			Base36 a = 10;
			Assert.Equal(0, (a % 2).Value);
		}

		[Fact]
		public void Increment_IncreasesValueByOne()
		{
			Base36 a = 5;
			a++;
			Assert.Equal(6, a.Value);
		}

		[Fact]
		public void Decrement_DecreasesValueByOne()
		{
			Base36 a = 5;
			a--;
			Assert.Equal(4, a.Value);
		}

		[Fact]
		public void UnaryMinus_NegatesValue()
		{
			Base36 a = 5;
			Assert.Equal(-5, (-a).Value);
		}

		[Fact]
		public void UnaryPlus_ReturnsSameValue()
		{
			Base36 a = 5;
			Assert.Equal(5, (+a).Value);
		}

		[Fact]
		public void Addition_Overflow_ThrowsOverflowException()
		{
			Assert.Throws<OverflowException>(() =>
			{
				Base36 max = Base36.MaxValue;
				_ = max + Base36.One;
			});
		}

		// ─── Comparison operators ─────────────────────────────────────────────

		[Fact]
		public void Equality_SameValues_ReturnsTrue()
		{
			Base36 a = 5;
			Base36 b = 5;
			Assert.True(a == b);
		}

		[Fact]
		public void Inequality_DifferentValues_ReturnsTrue()
		{
			Base36 a = 5;
			Base36 b = 6;
			Assert.True(a != b);
		}

		[Fact]
		public void LessThan_ReturnsCorrectResult()
		{
			Assert.True((Base36)3 < (Base36)5);
		}

		[Fact]
		public void GreaterThan_ReturnsCorrectResult()
		{
			Assert.True((Base36)5 > (Base36)3);
		}

		[Fact]
		public void LessThanOrEqual_EqualValues_ReturnsTrue()
		{
			Assert.True((Base36)5 <= (Base36)5);
		}

		[Fact]
		public void GreaterThanOrEqual_EqualValues_ReturnsTrue()
		{
			Assert.True((Base36)5 >= (Base36)5);
		}

		// ─── IComparable ──────────────────────────────────────────────────────

		[Fact]
		public void CompareTo_LesserValue_ReturnsNegative()
		{
			var a = new Base36(3);
			var b = new Base36(5);
			Assert.True(a.CompareTo(b) < 0);
		}

		[Fact]
		public void CompareTo_GreaterValue_ReturnsPositive()
		{
			var a = new Base36(5);
			var b = new Base36(3);
			Assert.True(a.CompareTo(b) > 0);
		}

		[Fact]
		public void CompareTo_EqualValue_ReturnsZero()
		{
			var a = new Base36(5);
			var b = new Base36(5);
			Assert.Equal(0, a.CompareTo(b));
		}

		// ─── IEquatable ───────────────────────────────────────────────────────

		[Fact]
		public void Equals_SameValue_ReturnsTrue()
		{
			Assert.True(new Base36(5).Equals(new Base36(5)));
		}

		[Fact]
		public void Equals_DifferentValue_ReturnsFalse()
		{
			Assert.False(new Base36(5).Equals(new Base36(6)));
		}

		[Fact]
		public void GetHashCode_SameValues_ReturnsSameHash()
		{
			Assert.Equal(new Base36(10).GetHashCode(), new Base36(10).GetHashCode());
		}

		// ─── Case-insensitive parsing ─────────────────────────────────────────

		[Theory]
		[InlineData("a", 10)]
		[InlineData("z", 35)]
		[InlineData("ff", 555)]
		public void Parse_LowercaseInput_ParsesCorrectly(string input, int expected)
		{
			Base36 b = Base36.Parse(input, null);
			Assert.Equal(expected, b.Value);
		}
	}
}
