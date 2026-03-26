// Copyright(C) 2019-2026, Daniel M. Porrey. All rights reserved.
// Licensed under the LGPL-3.0-or-later license.
using System;
using System.Collections.Generic;
using Diamond.Core.Workflow.State;
using Xunit;

namespace Diamond.Core.Tests
{
	/// <summary>
	/// Tests for <see cref="StateDictionary"/> and its built-in type converters.
	/// </summary>
	public class StateDictionaryTests
	{
		// ─── Add / ContainsKey / indexer ──────────────────────────────────────

		[Fact]
		public void Add_ThenContainsKey_ReturnsTrue()
		{
			var dict = new StateDictionary();
			dict.Add("myKey", "value");
			Assert.True(dict.ContainsKey("myKey"));
		}

		[Fact]
		public void ContainsKey_CaseInsensitive_ReturnsTrue()
		{
			var dict = new StateDictionary();
			dict.Add("MyKey", "value");
			Assert.True(dict.ContainsKey("mykey"));
			Assert.True(dict.ContainsKey("MYKEY"));
		}

		[Fact]
		public void Add_DuplicateKey_ThrowsAddItemToStateException()
		{
			var dict = new StateDictionary();
			dict.Add("key", "first");
			Assert.Throws<AddItemToStateException>(() => dict.Add("key", "second"));
		}

		[Fact]
		public void Indexer_Set_UpdatesExistingKey()
		{
			var dict = new StateDictionary();
			dict.Add("k", 1);
			dict["k"] = 42;
			Assert.Equal(42, dict["k"]);
		}

		// ─── Get<T>(key, defaultValue) ────────────────────────────────────────

		[Fact]
		public void Get_WithDefault_MissingKey_ReturnsDefault()
		{
			var dict = new StateDictionary();
			int result = dict.Get("missing", 99);
			Assert.Equal(99, result);
		}

		[Fact]
		public void Get_WithDefault_ExistingIntValue_ReturnsValue()
		{
			var dict = new StateDictionary();
			dict.Add("num", 7);
			int result = dict.Get("num", 0);
			Assert.Equal(7, result);
		}

		[Fact]
		public void Get_WithDefault_ExistingStringAsInt_ReturnsConvertedValue()
		{
			var dict = new StateDictionary();
			dict.Add("num", "42");
			int result = dict.Get("num", 0);
			Assert.Equal(42, result);
		}

		// ─── Get<T>(key) ──────────────────────────────────────────────────────

		[Fact]
		public void Get_NoDefault_MissingKey_ThrowsMissingContextPropertyException()
		{
			var dict = new StateDictionary();
			Assert.Throws<MissingContextPropertyException>(() => dict.Get<int>("missing"));
		}

		[Fact]
		public void Get_NoDefault_ExistingValue_ReturnsValue()
		{
			var dict = new StateDictionary();
			dict.Add("flag", true);
			bool result = dict.Get<bool>("flag");
			Assert.True(result);
		}

		// ─── TryGet<T> ────────────────────────────────────────────────────────

		[Fact]
		public void TryGet_MissingKey_InitializesAndReturnsDefault()
		{
			var dict = new StateDictionary();
			int result = dict.TryGet("x", 55);
			Assert.Equal(55, result);
			Assert.True(dict.ContainsKey("x"));
		}

		[Fact]
		public void TryGet_ExistingKey_ReturnsExistingValue()
		{
			var dict = new StateDictionary();
			dict.Add("x", 10);
			int result = dict.TryGet("x", 55);
			Assert.Equal(10, result);
		}

		// ─── Set<T> ────────────────────────────────────────────────────────────

		[Fact]
		public void Set_NewKey_AddsEntry()
		{
			var dict = new StateDictionary();
			dict.Set("newKey", "hello");
			Assert.True(dict.ContainsKey("newKey"));
		}

		[Fact]
		public void Set_ExistingKey_UpdatesValue()
		{
			var dict = new StateDictionary();
			dict.Add("key", "old");
			dict.Set("key", "new");
			Assert.Equal("new", dict["key"]);
		}

		// ─── BoolConverter ────────────────────────────────────────────────────

		[Theory]
		[InlineData("true", true)]
		[InlineData("True", true)]
		[InlineData("yes", true)]
		[InlineData("1", true)]
		[InlineData("on", true)]
		[InlineData("false", false)]
		[InlineData("no", false)]
		[InlineData("0", false)]
		[InlineData("off", false)]
		public void BoolConverter_StringValues_ConvertCorrectly(string input, bool expected)
		{
			var dict = new StateDictionary();
			dict.Add("flag", input);
			bool result = dict.Get<bool>("flag");
			Assert.Equal(expected, result);
		}

		// ─── IntConverter ─────────────────────────────────────────────────────

		[Fact]
		public void IntConverter_ValidString_ReturnsInt()
		{
			var dict = new StateDictionary();
			dict.Add("n", "123");
			int result = dict.Get<int>("n");
			Assert.Equal(123, result);
		}

		[Fact]
		public void IntConverter_InvalidString_ReturnsDefault()
		{
			var dict = new StateDictionary();
			dict.Add("n", "not-a-number");
			int result = dict.Get("n", -1);
			Assert.Equal(-1, result);
		}

		// ─── DoubleConverter ──────────────────────────────────────────────────

		[Fact]
		public void DoubleConverter_ValidString_ReturnsDouble()
		{
			var dict = new StateDictionary();
			dict.Add("d", "3.14");
			double result = dict.Get<double>("d");
			Assert.Equal(3.14, result, precision: 5);
		}

		// ─── StringConverter ──────────────────────────────────────────────────

		[Fact]
		public void StringConverter_StringValue_ReturnsSameString()
		{
			var dict = new StateDictionary();
			dict.Add("s", "hello");
			string result = dict.Get<string>("s");
			Assert.Equal("hello", result);
		}

		// ─── Enum converter ───────────────────────────────────────────────────

		private enum Color { Red, Green, Blue }

		[Fact]
		public void EnumConverter_ValidStringValue_ReturnsEnum()
		{
			var dict = new StateDictionary();
			dict.Add("color", "Green");
			Color result = dict.Get<Color>("color");
			Assert.Equal(Color.Green, result);
		}

		// ─── StringArrayConverter ─────────────────────────────────────────────

		[Fact]
		public void StringArrayConverter_CommaSeparatedString_ReturnsArray()
		{
			var dict = new StateDictionary();
			dict.Add("arr", "a,b,c");
			string[] result = dict.Get<string[]>("arr");
			Assert.Equal(3, result.Length);
		}

		// ─── NullableIntConverter ─────────────────────────────────────────────

		[Fact]
		public void NullableIntConverter_ValidString_ReturnsNullableInt()
		{
			var dict = new StateDictionary();
			dict.Add("ni", "7");
			int? result = dict.Get<int?>("ni");
			Assert.Equal(7, result);
		}

		[Fact]
		public void NullableIntConverter_EmptyString_ReturnsNull()
		{
			var dict = new StateDictionary();
			dict.Add("ni", "");
			int? result = dict.Get<int?>("ni", null);
			Assert.Null(result);
		}

		// ─── IDictionaryConverter ─────────────────────────────────────────────

		[Fact]
		public void IDictionaryConverter_ExistingDictValue_ReturnsDictionary()
		{
			var dict = new StateDictionary();
			var inner = new Dictionary<string, string> { { "a", "1" } };
			dict.Add("d", inner);
			IDictionary<string, string> result = dict.Get<IDictionary<string, string>>("d");
			Assert.NotNull(result);
			Assert.Equal("1", result["a"]);
		}

		// ─── ConvertParameter ─────────────────────────────────────────────────

		[Fact]
		public void ConvertParameter_UnsupportedType_ThrowsArgumentOutOfRangeException()
		{
			var dict = new StateDictionary();
			dict.Add("val", "test");
			Assert.Throws<ArgumentOutOfRangeException>(() => dict.ConvertParameter("val", typeof(Guid)));
		}
	}
}
