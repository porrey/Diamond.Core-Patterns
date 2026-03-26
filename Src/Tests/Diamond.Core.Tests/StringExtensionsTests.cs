// Copyright(C) 2019-2026, Daniel M. Porrey. All rights reserved.
// Licensed under the LGPL-3.0-or-later license.
using Xunit;

namespace Diamond.Core.Tests
{
	/// <summary>
	/// Tests for <see cref="StringExtensions"/>.
	/// </summary>
	public class StringExtensionsTests
	{
		// ─── Limit ────────────────────────────────────────────────────────────

		[Fact]
		public void Limit_WhenStringLongerThanMax_ReturnsTruncated()
		{
			string result = "HelloWorld".Limit(5);
			Assert.Equal("Hello", result);
		}

		[Fact]
		public void Limit_WhenStringEqualToMax_ReturnsOriginal()
		{
			string result = "Hello".Limit(5);
			Assert.Equal("Hello", result);
		}

		[Fact]
		public void Limit_WhenStringShorterThanMax_ReturnsOriginal()
		{
			string result = "Hi".Limit(10);
			Assert.Equal("Hi", result);
		}

		[Fact]
		public void Limit_EmptyString_ReturnsEmpty()
		{
			string result = "".Limit(5);
			Assert.Equal("", result);
		}

		[Fact]
		public void Limit_MaxLengthZero_ReturnsEmpty()
		{
			string result = "Hello".Limit(0);
			Assert.Equal("", result);
		}

		// ─── IsNullOrEmpty ────────────────────────────────────────────────────

		[Fact]
		public void IsNullOrEmpty_NullString_ReturnsTrue()
		{
			string s = null;
			Assert.True(s.IsNullOrEmpty());
		}

		[Fact]
		public void IsNullOrEmpty_EmptyString_ReturnsTrue()
		{
			Assert.True("".IsNullOrEmpty());
		}

		[Fact]
		public void IsNullOrEmpty_NonEmptyString_ReturnsFalse()
		{
			Assert.False("hello".IsNullOrEmpty());
		}

		[Fact]
		public void IsNullOrEmpty_WhitespaceString_ReturnsFalse()
		{
			Assert.False("  ".IsNullOrEmpty());
		}

		// ─── IsNullOrWhiteSpace ───────────────────────────────────────────────

		[Fact]
		public void IsNullOrWhiteSpace_NullString_ReturnsTrue()
		{
			string s = null;
			Assert.True(s.IsNullOrWhiteSpace());
		}

		[Fact]
		public void IsNullOrWhiteSpace_EmptyString_ReturnsTrue()
		{
			Assert.True("".IsNullOrWhiteSpace());
		}

		[Fact]
		public void IsNullOrWhiteSpace_WhitespaceOnly_ReturnsTrue()
		{
			Assert.True("   ".IsNullOrWhiteSpace());
		}

		[Fact]
		public void IsNullOrWhiteSpace_NonEmptyString_ReturnsFalse()
		{
			Assert.False("hello".IsNullOrWhiteSpace());
		}
	}
}
