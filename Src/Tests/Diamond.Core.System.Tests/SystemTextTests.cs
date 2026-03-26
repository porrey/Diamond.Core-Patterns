using System;
using System.Collections.Generic;

namespace Diamond.Core.System.Tests
{
    [TestFixture]
    public class StringExtensionsTests
    {
        // ─── Limit ─────────────────────────────────────────────────────────────────

        [Test]
        public void Limit_ShorterThanMax_ReturnsUnchanged()
        {
            string result = "Hello".Limit(10);
            Assert.That(result, Is.EqualTo("Hello"));
        }

        [Test]
        public void Limit_ExactlyMax_ReturnsUnchanged()
        {
            string result = "Hello".Limit(5);
            Assert.That(result, Is.EqualTo("Hello"));
        }

        [Test]
        public void Limit_LongerThanMax_ReturnsTruncated()
        {
            string result = "Hello World".Limit(5);
            Assert.That(result, Is.EqualTo("Hello"));
        }

        [Test]
        public void Limit_MaxZero_ReturnsEmpty()
        {
            string result = "Hello".Limit(0);
            Assert.That(result, Is.EqualTo(""));
        }

        [Test]
        public void Limit_EmptyString_ReturnsEmpty()
        {
            string result = "".Limit(5);
            Assert.That(result, Is.EqualTo(""));
        }

        // ─── IsNullOrEmpty ─────────────────────────────────────────────────────────

        [Test]
        public void IsNullOrEmpty_Null_ReturnsTrue()
        {
            string value = null;
            Assert.That(value.IsNullOrEmpty(), Is.True);
        }

        [Test]
        public void IsNullOrEmpty_Empty_ReturnsTrue()
        {
            string value = "";
            Assert.That(value.IsNullOrEmpty(), Is.True);
        }

        [Test]
        public void IsNullOrEmpty_NonEmpty_ReturnsFalse()
        {
            string value = "hello";
            Assert.That(value.IsNullOrEmpty(), Is.False);
        }

        [Test]
        public void IsNullOrEmpty_WhitespaceOnly_ReturnsFalse()
        {
            string value = "   ";
            Assert.That(value.IsNullOrEmpty(), Is.False);
        }

        // ─── IsNullOrWhiteSpace ─────────────────────────────────────────────────────

        [Test]
        public void IsNullOrWhiteSpace_Null_ReturnsTrue()
        {
            string value = null;
            Assert.That(value.IsNullOrWhiteSpace(), Is.True);
        }

        [Test]
        public void IsNullOrWhiteSpace_Empty_ReturnsTrue()
        {
            string value = "";
            Assert.That(value.IsNullOrWhiteSpace(), Is.True);
        }

        [Test]
        public void IsNullOrWhiteSpace_WhitespaceOnly_ReturnsTrue()
        {
            string value = "   ";
            Assert.That(value.IsNullOrWhiteSpace(), Is.True);
        }

        [Test]
        public void IsNullOrWhiteSpace_NonEmpty_ReturnsFalse()
        {
            string value = "hello";
            Assert.That(value.IsNullOrWhiteSpace(), Is.False);
        }

        [Test]
        public void IsNullOrWhiteSpace_MixedContent_ReturnsFalse()
        {
            string value = " hello ";
            Assert.That(value.IsNullOrWhiteSpace(), Is.False);
        }
    }

    [TestFixture]
    public class SecureDataTests
    {
        // ─── Signature (MD5) ─────────────────────────────────────────────────────────

        [Test]
        public void Signature_ReturnsNonEmpty()
        {
            string hash = "hello".Signature();
            Assert.That(hash, Is.Not.Empty);
        }

        [Test]
        public void Signature_SameInput_SameOutput()
        {
            string hash1 = "hello".Signature();
            string hash2 = "hello".Signature();
            Assert.That(hash1, Is.EqualTo(hash2));
        }

        [Test]
        public void Signature_DifferentInput_DifferentOutput()
        {
            string hash1 = "hello".Signature();
            string hash2 = "world".Signature();
            Assert.That(hash1, Is.Not.EqualTo(hash2));
        }

        [Test]
        public void Signature_WithoutDashes_NoDashes()
        {
            string hash = "hello".Signature(includeDashes: false);
            Assert.That(hash, Does.Not.Contain("-"));
        }

        [Test]
        public void Signature_WithDashes_HasDashes()
        {
            string hash = "hello".Signature(includeDashes: true);
            Assert.That(hash, Does.Contain("-"));
        }

        [Test]
        public void Signature_KnownMd5_Hello()
        {
            // MD5("hello") = 5d41402abc4b2a76b9719d911017c592
            string hash = "hello".Signature(includeDashes: false);
            Assert.That(hash, Is.EqualTo("5D41402ABC4B2A76B9719D911017C592").IgnoreCase);
        }

        // ─── ComputeHash (SHA512) ────────────────────────────────────────────────────

        [Test]
        public void ComputeHash_Default_ReturnsNonEmpty()
        {
            string hash = "hello".ComputeHash();
            Assert.That(hash, Is.Not.Empty);
        }

        [Test]
        public void ComputeHash_SameInput_SameOutput()
        {
            string hash1 = "hello".ComputeHash();
            string hash2 = "hello".ComputeHash();
            Assert.That(hash1, Is.EqualTo(hash2));
        }

        [Test]
        public void ComputeHash_WithoutDashes_NoDashes()
        {
            string hash = "hello".ComputeHash(includeDashes: false);
            Assert.That(hash, Does.Not.Contain("-"));
        }

        [Test]
        public void ComputeHash_WithDashes_HasDashes()
        {
            string hash = "hello".ComputeHash(includeDashes: true);
            Assert.That(hash, Does.Contain("-"));
        }

        // ─── ComputeHash with algorithm name ─────────────────────────────────────────

        [TestCase("SHA1")]
        [TestCase("SHA")]
        [TestCase("MD5")]
        [TestCase("SHA256")]
        [TestCase("SHA384")]
        [TestCase("SHA512")]
        public void ComputeHash_WithAlgorithmName_ReturnsNonEmpty(string algorithm)
        {
            string hash = "hello".ComputeHash(algorithm);
            Assert.That(hash, Is.Not.Empty);
        }

        [Test]
        public void ComputeHash_SHA1_IsSameAsSHA()
        {
            string sha1Hash = "hello".ComputeHash("SHA1");
            string shaHash = "hello".ComputeHash("SHA");
            Assert.That(sha1Hash, Is.EqualTo(shaHash));
        }

        [Test]
        public void ComputeHash_InvalidAlgorithm_Throws()
        {
            Assert.Throws<ArgumentException>(() => "hello".ComputeHash("INVALID"));
        }

        [Test]
        public void ComputeHash_SHA256_CorrectLength()
        {
            // SHA256 produces 32 bytes = 64 hex chars
            string hash = "hello".ComputeHash("SHA256", includeDashes: false);
            Assert.That(hash.Length, Is.EqualTo(64));
        }

        [Test]
        public void ComputeHash_SHA512_CorrectLength()
        {
            // SHA512 produces 64 bytes = 128 hex chars
            string hash = "hello".ComputeHash("SHA512", includeDashes: false);
            Assert.That(hash.Length, Is.EqualTo(128));
        }

        [Test]
        public void ComputeHash_MD5_CorrectLength()
        {
            // MD5 produces 16 bytes = 32 hex chars
            string hash = "hello".ComputeHash("MD5", includeDashes: false);
            Assert.That(hash.Length, Is.EqualTo(32));
        }

        // ─── ComputeHash for IEnumerable ──────────────────────────────────────────────

        [Test]
        public void ComputeHash_IEnumerable_ReturnsNonEmpty()
        {
            var items = new List<string> { "a", "b", "c" };
            string hash = items.ComputeHash();
            Assert.That(hash, Is.Not.Empty);
        }

        [Test]
        public void ComputeHash_IEnumerable_SameItems_SameHash()
        {
            var items1 = new List<int> { 1, 2, 3 };
            var items2 = new List<int> { 1, 2, 3 };
            string hash1 = items1.ComputeHash();
            string hash2 = items2.ComputeHash();
            Assert.That(hash1, Is.EqualTo(hash2));
        }

        [Test]
        public void ComputeHash_IEnumerable_DifferentItems_DifferentHash()
        {
            var items1 = new List<int> { 1, 2, 3 };
            var items2 = new List<int> { 3, 2, 1 };
            string hash1 = items1.ComputeHash();
            string hash2 = items2.ComputeHash();
            Assert.That(hash1, Is.Not.EqualTo(hash2));
        }

        [Test]
        public void ComputeHash_IEnumerable_InvalidAlgorithm_Throws()
        {
            var items = new List<string> { "a" };
            Assert.Throws<ArgumentException>(() => items.ComputeHash("INVALID"));
        }

        [TestCase("SHA1")]
        [TestCase("MD5")]
        [TestCase("SHA256")]
        [TestCase("SHA384")]
        [TestCase("SHA512")]
        public void ComputeHash_IEnumerable_AllAlgorithms(string algorithm)
        {
            var items = new List<string> { "test" };
            string hash = items.ComputeHash(algorithm);
            Assert.That(hash, Is.Not.Empty);
        }
    }
}
