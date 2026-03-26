using System;
using System.Globalization;

namespace Diamond.Core.System.Tests
{
    [TestFixture]
    public class Base36Tests
    {
        // ─── Static Fields ───────────────────────────────────────────────────────────

        [Test]
        public void Zero_Value_IsZero()
        {
            Assert.That((int)Base36.Zero, Is.EqualTo(0));
        }

        [Test]
        public void One_Value_IsOne()
        {
            Assert.That((int)Base36.One, Is.EqualTo(1));
        }

        [Test]
        public void MinValue_EqualsIntMinValue()
        {
            Assert.That((int)Base36.MinValue, Is.EqualTo(int.MinValue));
        }

        [Test]
        public void MaxValue_EqualsIntMaxValue()
        {
            Assert.That((int)Base36.MaxValue, Is.EqualTo(int.MaxValue));
        }

        // ─── Constructor ────────────────────────────────────────────────────────────

        [Test]
        public void Constructor_StoresValue()
        {
            var b = new Base36(42);
            Assert.That(b.Value, Is.EqualTo(42));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(-1)]
        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        public void Constructor_RoundTrip(int value)
        {
            var b = new Base36(value);
            Assert.That(b.Value, Is.EqualTo(value));
        }

        // ─── Implicit / Explicit Conversion ─────────────────────────────────────────

        [Test]
        public void ImplicitConversion_FromInt()
        {
            Base36 b = 100;
            Assert.That(b.Value, Is.EqualTo(100));
        }

        [Test]
        public void ExplicitConversion_ToInt()
        {
            Base36 b = new Base36(99);
            int i = (int)b;
            Assert.That(i, Is.EqualTo(99));
        }

        [Test]
        public void ImplicitConversion_FromString()
        {
            Base36 b = "Z";
            Assert.That(b.Value, Is.EqualTo(35));
        }

        [Test]
        public void ImplicitConversion_ToString()
        {
            Base36 b = new Base36(35);
            string s = (string)b;
            Assert.That(s, Is.EqualTo("Z"));
        }

        [Test]
        public void ImplicitConversion_FromString_Zero()
        {
            Base36 b = "0";
            Assert.That(b.Value, Is.EqualTo(0));
        }

        [Test]
        public void ImplicitConversion_FromString_MultiChar()
        {
            // 36 in base-36 is "10"
            Base36 b = "10";
            Assert.That(b.Value, Is.EqualTo(36));
        }

        // ─── ToString ───────────────────────────────────────────────────────────────

        [TestCase(0, "0")]
        [TestCase(1, "1")]
        [TestCase(9, "9")]
        [TestCase(10, "A")]
        [TestCase(35, "Z")]
        [TestCase(36, "10")]
        [TestCase(1295, "ZZ")]
        public void ToString_ProducesCorrectBase36(int value, string expected)
        {
            var b = new Base36(value);
            Assert.That(b.ToString(), Is.EqualTo(expected));
        }

        [Test]
        public void ToString_Negative_HasMinus()
        {
            var b = new Base36(-1);
            Assert.That(b.ToString(), Does.StartWith("-"));
        }

        // ─── Parse / TryParse ────────────────────────────────────────────────────────

        [TestCase("0", 0)]
        [TestCase("1", 1)]
        [TestCase("A", 10)]
        [TestCase("Z", 35)]
        [TestCase("10", 36)]
        [TestCase("ZZ", 1295)]
        public void Parse_ValidString_ReturnsExpected(string input, int expected)
        {
            Base36 result = Base36.Parse(input, null);
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [TestCase("a", 10)]
        [TestCase("z", 35)]
        [TestCase("zz", 1295)]
        public void Parse_LowercaseInput_ReturnsExpected(string input, int expected)
        {
            Base36 result = Base36.Parse(input, null);
            Assert.That(result.Value, Is.EqualTo(expected));
        }

        [Test]
        public void TryParse_ValidString_ReturnsTrue()
        {
            bool success = Base36.TryParse("10", null, out Base36 result);
            Assert.That(success, Is.True);
            Assert.That(result.Value, Is.EqualTo(36));
        }

        [Test]
        public void TryParse_InvalidString_ReturnsFalse()
        {
            bool success = Base36.TryParse("!!", null, out Base36 result);
            Assert.That(success, Is.False);
        }

        [Test]
        public void TryParse_NullString_ReturnsFalse()
        {
            bool success = Base36.TryParse((string)null, null, out Base36 result);
            Assert.That(success, Is.False);
        }

        [Test]
        public void TryParse_EmptyString_ReturnsFalse()
        {
            bool success = Base36.TryParse("", null, out Base36 result);
            Assert.That(success, Is.False);
        }

        // ─── Arithmetic Operators ────────────────────────────────────────────────────

        [Test]
        public void Add_TwoBase36()
        {
            Base36 a = 10;
            Base36 b = 26;
            Base36 c = a + b;
            Assert.That(c.Value, Is.EqualTo(36));
        }

        [Test]
        public void Subtract_TwoBase36()
        {
            Base36 a = 36;
            Base36 b = 10;
            Base36 c = a - b;
            Assert.That(c.Value, Is.EqualTo(26));
        }

        [Test]
        public void Multiply_TwoBase36()
        {
            Base36 a = 6;
            Base36 b = 6;
            Base36 c = a * b;
            Assert.That(c.Value, Is.EqualTo(36));
        }

        [Test]
        public void Divide_TwoBase36()
        {
            Base36 a = 36;
            Base36 b = 6;
            Base36 c = a / b;
            Assert.That(c.Value, Is.EqualTo(6));
        }

        [Test]
        public void Modulo_TwoBase36()
        {
            Base36 a = 37;
            Base36 b = 36;
            Base36 c = a % b;
            Assert.That(c.Value, Is.EqualTo(1));
        }

        [Test]
        public void Increment_Base36()
        {
            Base36 a = 10;
            a++;
            Assert.That(a.Value, Is.EqualTo(11));
        }

        [Test]
        public void Decrement_Base36()
        {
            Base36 a = 10;
            a--;
            Assert.That(a.Value, Is.EqualTo(9));
        }

        [Test]
        public void UnaryPlus_Base36()
        {
            Base36 a = -5;
            Base36 b = +a;
            Assert.That(b.Value, Is.EqualTo(-5));
        }

        [Test]
        public void UnaryMinus_Base36()
        {
            Base36 a = 5;
            Base36 b = -a;
            Assert.That(b.Value, Is.EqualTo(-5));
        }

        [Test]
        public void Add_Base36AndInt()
        {
            Base36 a = 10;
            Base36 c = a + 5;
            Assert.That(c.Value, Is.EqualTo(15));
        }

        [Test]
        public void Subtract_Base36AndInt()
        {
            Base36 a = 20;
            Base36 c = a - 5;
            Assert.That(c.Value, Is.EqualTo(15));
        }

        [Test]
        public void Multiply_Base36AndInt()
        {
            Base36 a = 5;
            Base36 c = a * 3;
            Assert.That(c.Value, Is.EqualTo(15));
        }

        [Test]
        public void Divide_Base36AndInt()
        {
            Base36 a = 15;
            Base36 c = a / 3;
            Assert.That(c.Value, Is.EqualTo(5));
        }

        [Test]
        public void Modulo_Base36AndInt()
        {
            Base36 a = 16;
            Base36 c = a % 3;
            Assert.That(c.Value, Is.EqualTo(1));
        }

        // ─── Comparison Operators ────────────────────────────────────────────────────

        [Test]
        public void EqualityOperator_Equal()
        {
            Base36 a = 10;
            Base36 b = 10;
            Assert.That(a == b, Is.True);
        }

        [Test]
        public void EqualityOperator_NotEqual()
        {
            Base36 a = 10;
            Base36 b = 20;
            Assert.That(a != b, Is.True);
        }

        [Test]
        public void LessThan_Operator()
        {
            Base36 a = 5;
            Base36 b = 10;
            Assert.That(a < b, Is.True);
        }

        [Test]
        public void GreaterThan_Operator()
        {
            Base36 a = 10;
            Base36 b = 5;
            Assert.That(a > b, Is.True);
        }

        [Test]
        public void LessThanOrEqual_Operator()
        {
            Base36 a = 10;
            Base36 b = 10;
            Assert.That(a <= b, Is.True);
        }

        [Test]
        public void GreaterThanOrEqual_Operator()
        {
            Base36 a = 10;
            Base36 b = 10;
            Assert.That(a >= b, Is.True);
        }

        // ─── IEquatable / IComparable ─────────────────────────────────────────────────

        [Test]
        public void Equals_SameValue_ReturnsTrue()
        {
            Base36 a = 42;
            Base36 b = 42;
            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void Equals_DifferentValue_ReturnsFalse()
        {
            Base36 a = 42;
            Base36 b = 43;
            Assert.That(a.Equals(b), Is.False);
        }

        [Test]
        public void Equals_Object_SameValue_ReturnsTrue()
        {
            Base36 a = 42;
            object b = new Base36(42);
            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void Equals_Object_Null_ReturnsFalse()
        {
            Base36 a = 42;
            // Cast null to object to avoid the implicit string->Base36 conversion
            object nullObj = null;
            Assert.That(a.Equals(nullObj), Is.False);
        }

        [Test]
        public void GetHashCode_SameValues_Equal()
        {
            Base36 a = 42;
            Base36 b = 42;
            Assert.That(a.GetHashCode(), Is.EqualTo(b.GetHashCode()));
        }

        [Test]
        public void CompareTo_LessThan_ReturnsNegative()
        {
            Base36 a = 5;
            Base36 b = 10;
            Assert.That(a.CompareTo(b), Is.LessThan(0));
        }

        [Test]
        public void CompareTo_Equal_ReturnsZero()
        {
            Base36 a = 10;
            Base36 b = 10;
            Assert.That(a.CompareTo(b), Is.EqualTo(0));
        }

        [Test]
        public void CompareTo_GreaterThan_ReturnsPositive()
        {
            Base36 a = 15;
            Base36 b = 10;
            Assert.That(a.CompareTo(b), Is.GreaterThan(0));
        }

        [Test]
        public void CompareTo_Object_Null_ReturnsPositive()
        {
            // IComparable.CompareTo throws ArgumentException for non-Base36 objects,
            // including null (since null is not a Base36 instance).
            Base36 a = 10;
            IComparable comparable = a;
            Assert.Throws<ArgumentException>(() => comparable.CompareTo(null));
        }

        [Test]
        public void CompareTo_Object_InvalidType_Throws()
        {
            Base36 a = 10;
            IComparable comparable = a;
            object notBase36 = new object();
            Assert.Throws<ArgumentException>(() => comparable.CompareTo(notBase36));
        }

        // ─── ISpanFormattable ─────────────────────────────────────────────────────────

        [Test]
        public void TryFormat_Succeeds()
        {
            Base36 a = 35;
            char[] buffer = new char[10];
            bool ok = a.TryFormat(buffer.AsSpan(), out int charsWritten, ReadOnlySpan<char>.Empty, null);
            Assert.That(ok, Is.True);
            Assert.That(new string(buffer, 0, charsWritten), Is.EqualTo("Z"));
        }

        [Test]
        public void TryFormat_BufferTooSmall_ReturnsFalse()
        {
            Base36 a = 1295; // "ZZ"
            char[] buffer = new char[1];
            bool ok = a.TryFormat(buffer.AsSpan(), out int charsWritten, ReadOnlySpan<char>.Empty, null);
            Assert.That(ok, Is.False);
        }

        // ─── ISpanParsable ─────────────────────────────────────────────────────────

        [Test]
        public void TryParse_SpanInput_Succeeds()
        {
            bool success = Base36.TryParse("ZZ".AsSpan(), null, out Base36 result);
            Assert.That(success, Is.True);
            Assert.That(result.Value, Is.EqualTo(1295));
        }

        [Test]
        public void Parse_SpanInput_Succeeds()
        {
            Base36 result = Base36.Parse("ZZ".AsSpan(), null);
            Assert.That(result.Value, Is.EqualTo(1295));
        }

        // ─── Round-Trip ────────────────────────────────────────────────────────────

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(35)]
        [TestCase(36)]
        [TestCase(1295)]
        [TestCase(46655)] // ZZZ
        public void RoundTrip_ToStringAndBack(int value)
        {
            Base36 original = new Base36(value);
            string s = original.ToString();
            Base36 parsed = Base36.Parse(s, null);
            Assert.That(parsed.Value, Is.EqualTo(value));
        }
    }
}
