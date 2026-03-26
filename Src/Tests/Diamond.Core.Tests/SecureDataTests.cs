// Copyright(C) 2019-2026, Daniel M. Porrey. All rights reserved.
// Licensed under the LGPL-3.0-or-later license.
using System;
using System.Collections.Generic;
using Xunit;

namespace Diamond.Core.Tests
{
	/// <summary>
	/// Tests for <see cref="SecureData"/> extension methods.
	/// </summary>
	public class SecureDataTests
	{
		private const string SampleInput = "Hello, World!";

		// ─── Signature (MD5) ──────────────────────────────────────────────────

		[Fact]
		public void Signature_ReturnsNonEmptyString()
		{
			string result = SampleInput.Signature();
			Assert.False(string.IsNullOrEmpty(result));
		}

		[Fact]
		public void Signature_NoDashes_ContainsNoHyphens()
		{
			string result = SampleInput.Signature(includeDashes: false);
			Assert.DoesNotContain("-", result);
		}

		[Fact]
		public void Signature_WithDashes_ContainsHyphens()
		{
			string result = SampleInput.Signature(includeDashes: true);
			Assert.Contains("-", result);
		}

		[Fact]
		public void Signature_SameInputProducesSameResult()
		{
			string r1 = SampleInput.Signature();
			string r2 = SampleInput.Signature();
			Assert.Equal(r1, r2);
		}

		[Fact]
		public void Signature_DifferentInputProducesDifferentResult()
		{
			string r1 = "abc".Signature();
			string r2 = "xyz".Signature();
			Assert.NotEqual(r1, r2);
		}

		// ─── ComputeHash (SHA-512 default) ────────────────────────────────────

		[Fact]
		public void ComputeHash_Default_ReturnsNonEmptyString()
		{
			string result = SampleInput.ComputeHash();
			Assert.False(string.IsNullOrEmpty(result));
		}

		[Fact]
		public void ComputeHash_Default_NoDashes()
		{
			string result = SampleInput.ComputeHash(includeDashes: false);
			Assert.DoesNotContain("-", result);
		}

		[Fact]
		public void ComputeHash_Default_WithDashes()
		{
			string result = SampleInput.ComputeHash(includeDashes: true);
			Assert.Contains("-", result);
		}

		[Fact]
		public void ComputeHash_Default_IsDeterministic()
		{
			string r1 = SampleInput.ComputeHash();
			string r2 = SampleInput.ComputeHash();
			Assert.Equal(r1, r2);
		}

		// ─── ComputeHash (named algorithm) ───────────────────────────────────

		[Theory]
		[InlineData("SHA")]
		[InlineData("SHA1")]
		[InlineData("MD5")]
		[InlineData("SHA256")]
		[InlineData("SHA384")]
		[InlineData("SHA512")]
		public void ComputeHash_NamedAlgorithm_ReturnsNonEmptyString(string hashName)
		{
			string result = SampleInput.ComputeHash(hashName);
			Assert.False(string.IsNullOrEmpty(result));
		}

		[Fact]
		public void ComputeHash_InvalidAlgorithmName_ThrowsArgumentException()
		{
			Assert.Throws<ArgumentException>(() => SampleInput.ComputeHash("INVALID"));
		}

		[Theory]
		[InlineData("SHA256", "SHA512")]
		[InlineData("MD5", "SHA256")]
		public void ComputeHash_DifferentAlgorithmsProduceDifferentLengthHashes(string algo1, string algo2)
		{
			string r1 = SampleInput.ComputeHash(algo1);
			string r2 = SampleInput.ComputeHash(algo2);
			Assert.NotEqual(r1.Length, r2.Length);
		}

		// ─── ComputeHash<TItem> (collection) ─────────────────────────────────

		[Fact]
		public void ComputeHashCollection_ReturnsNonEmptyString()
		{
			IEnumerable<int> items = new[] { 1, 2, 3, 4, 5 };
			string result = items.ComputeHash();
			Assert.False(string.IsNullOrEmpty(result));
		}

		[Fact]
		public void ComputeHashCollection_SameItemsProduceSameHash()
		{
			IEnumerable<int> items1 = new[] { 1, 2, 3 };
			IEnumerable<int> items2 = new[] { 1, 2, 3 };
			Assert.Equal(items1.ComputeHash(), items2.ComputeHash());
		}

		[Fact]
		public void ComputeHashCollection_DifferentItemsProduceDifferentHash()
		{
			IEnumerable<int> items1 = new[] { 1, 2, 3 };
			IEnumerable<int> items2 = new[] { 4, 5, 6 };
			Assert.NotEqual(items1.ComputeHash(), items2.ComputeHash());
		}

		[Fact]
		public void ComputeHashCollection_InvalidAlgorithmName_ThrowsArgumentException()
		{
			IEnumerable<string> items = new[] { "a", "b" };
			Assert.Throws<ArgumentException>(() => items.ComputeHash("BOGUS"));
		}

		[Theory]
		[InlineData("SHA")]
		[InlineData("SHA1")]
		[InlineData("MD5")]
		[InlineData("SHA256")]
		[InlineData("SHA384")]
		[InlineData("SHA512")]
		public void ComputeHashCollection_NamedAlgorithm_Succeeds(string hashName)
		{
			IEnumerable<string> items = new[] { "a", "b", "c" };
			string result = items.ComputeHash(hashName);
			Assert.False(string.IsNullOrEmpty(result));
		}
	}
}
