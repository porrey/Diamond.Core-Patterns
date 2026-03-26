using System;
using Diamond.Core.Clonable;
using Diamond.Core.Clonable.NetCore;
using NewtonsoftFactory = Diamond.Core.Clonable.Newtonsoft.ObjectCloneFactory;

namespace Diamond.Core.Clonable.Tests
{
    // ─── Test Helpers ─────────────────────────────────────────────────────────────

    /// <summary>
    /// A simple concrete class for testing ObjectCloneFactory directly.
    /// Implements ICloneable directly (not via CloneableObject) to avoid the
    /// abstract-type limitation in System.Text.Json.
    /// </summary>
    public class SimpleCloneable : ICloneable
    {
        public string Name { get; set; }
        public int Number { get; set; }

        public object Clone() => new SimpleCloneable { Name = this.Name, Number = this.Number };
    }

    /// <summary>
    /// Concrete class extending CloneableObject for testing CloneableObject.Clone()
    /// with the Newtonsoft factory (which supports abstract base types).
    /// </summary>
    public class SampleCloneable : CloneableObject
    {
        public string Name { get; set; }
        public int Number { get; set; }
    }

    // ─── ClonableFactory Tests ────────────────────────────────────────────────────

    [TestFixture]
    public class ClonableFactoryTests
    {
        [SetUp]
        public void ResetFactory()
        {
            // Reset factory before each test to ensure test isolation
            ClonableFactory.SetFactory(null);
        }

        [TearDown]
        public void RestoreFactory()
        {
            // Restore to a usable state after tests
            ClonableFactory.SetFactory(null);
        }

        [Test]
        public void GetFactory_WhenNotSet_Throws()
        {
            Assert.Throws<NoClonableFactorySetException>(() => ClonableFactory.GetFactory());
        }

        [Test]
        public void SetFactory_ThenGetFactory_ReturnsSameInstance()
        {
            var factory = new ObjectCloneFactory();
            ClonableFactory.SetFactory(factory);
            var retrieved = ClonableFactory.GetFactory();
            Assert.That(retrieved, Is.SameAs(factory));
        }

        [Test]
        public void SetFactory_OverwritesPrevious()
        {
            var factory1 = new ObjectCloneFactory();
            var factory2 = new ObjectCloneFactory();
            ClonableFactory.SetFactory(factory1);
            ClonableFactory.SetFactory(factory2);
            var retrieved = ClonableFactory.GetFactory();
            Assert.That(retrieved, Is.SameAs(factory2));
        }

        [Test]
        public void SetFactory_Null_ThenGetFactory_Throws()
        {
            ClonableFactory.SetFactory(new ObjectCloneFactory());
            ClonableFactory.SetFactory(null);
            Assert.Throws<NoClonableFactorySetException>(() => ClonableFactory.GetFactory());
        }
    }

    // ─── Microsoft ObjectCloneFactory Tests ──────────────────────────────────────

    [TestFixture]
    public class ObjectCloneFactoryTests
    {
        private ObjectCloneFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new ObjectCloneFactory();
        }

        [Test]
        public void CloneInstance_Generic_ReturnsNewObject()
        {
            var original = new SimpleCloneable { Name = "Alice", Number = 42 };
            var clone = _factory.CloneInstance(original);
            Assert.That(clone, Is.Not.SameAs(original));
        }

        [Test]
        public void CloneInstance_Generic_HasSameProperties()
        {
            var original = new SimpleCloneable { Name = "Alice", Number = 42 };
            var clone = _factory.CloneInstance(original);
            Assert.That(clone.Name, Is.EqualTo(original.Name));
            Assert.That(clone.Number, Is.EqualTo(original.Number));
        }

        [Test]
        public void CloneInstance_Generic_IsDeepCopy()
        {
            var original = new SimpleCloneable { Name = "Alice" };
            var clone = _factory.CloneInstance(original);
            clone.Name = "Bob";
            Assert.That(original.Name, Is.EqualTo("Alice")); // original unchanged
        }

        [Test]
        public void CloneInstance_NonGeneric_ReturnsNonNull()
        {
            var original = new SimpleCloneable { Name = "Test", Number = 1 };
            object clone = _factory.CloneInstance((ICloneable)original);
            Assert.That(clone, Is.Not.Null);
        }

        [Test]
        public void CloneInstance_Generic_EmptyName_Preserved()
        {
            var original = new SimpleCloneable { Name = null, Number = 0 };
            var clone = _factory.CloneInstance(original);
            Assert.That(clone.Name, Is.Null);
            Assert.That(clone.Number, Is.EqualTo(0));
        }
    }

    // ─── Newtonsoft ObjectCloneFactory Tests ──────────────────────────────────────

    [TestFixture]
    public class NewtonsoftObjectCloneFactoryTests
    {
        private NewtonsoftFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new NewtonsoftFactory();
        }

        [Test]
        public void CloneInstance_Generic_ReturnsNewObject()
        {
            var original = new SimpleCloneable { Name = "Bob", Number = 7 };
            var clone = _factory.CloneInstance(original);
            Assert.That(clone, Is.Not.SameAs(original));
        }

        [Test]
        public void CloneInstance_Generic_HasSameProperties()
        {
            var original = new SimpleCloneable { Name = "Bob", Number = 7 };
            var clone = _factory.CloneInstance(original);
            Assert.That(clone.Name, Is.EqualTo("Bob"));
            Assert.That(clone.Number, Is.EqualTo(7));
        }

        [Test]
        public void CloneInstance_NonGeneric_ReturnsNonNull()
        {
            var original = new SimpleCloneable { Name = "NonGeneric", Number = 99 };
            object clone = _factory.CloneInstance((ICloneable)original);
            Assert.That(clone, Is.Not.Null);
        }
    }

    // ─── CloneableObject Tests (using Newtonsoft factory) ────────────────────────

    [TestFixture]
    public class CloneableObjectTests
    {
        [SetUp]
        public void Setup()
        {
            // Use Newtonsoft factory since it supports abstract base types via
            // TypeNameHandling.All (preserves type info in JSON).
            ClonableFactory.SetFactory(new NewtonsoftFactory());
        }

        [TearDown]
        public void TearDown()
        {
            ClonableFactory.SetFactory(null);
        }

        [Test]
        public void Clone_ReturnsNewInstance()
        {
            var obj = new SampleCloneable { Name = "Test", Number = 10 };
            var clone = obj.Clone();
            Assert.That(clone, Is.Not.SameAs(obj));
        }

        [Test]
        public void Clone_ReturnsCorrectProperties()
        {
            var obj = new SampleCloneable { Name = "Test", Number = 10 };
            var clone = (SampleCloneable)obj.Clone();
            Assert.That(clone.Name, Is.EqualTo("Test"));
            Assert.That(clone.Number, Is.EqualTo(10));
        }

        [Test]
        public void Clone_WhenFactoryNotSet_Throws()
        {
            ClonableFactory.SetFactory(null);

            var obj = new SampleCloneable { Name = "Test" };
            Assert.Throws<NoClonableFactorySetException>(() => obj.Clone());
        }

        [Test]
        public void Clone_ModifyingClone_DoesNotAffectOriginal()
        {
            var obj = new SampleCloneable { Name = "Original", Number = 1 };
            var clone = (SampleCloneable)obj.Clone();
            clone.Name = "Modified";
            Assert.That(obj.Name, Is.EqualTo("Original"));
        }
    }

    // ─── NoClonableFactorySetException Tests ─────────────────────────────────────

    [TestFixture]
    public class NoClonableFactorySetExceptionTests
    {
        [Test]
        public void NoClonableFactorySetException_HasMessage()
        {
            var ex = new NoClonableFactorySetException();
            Assert.That(ex.Message, Is.Not.Empty);
            Assert.That(ex.Message, Does.Contain("ClonableFactory"));
        }

        [Test]
        public void NoClonableFactorySetException_IsDiamondClonableException()
        {
            var ex = new NoClonableFactorySetException();
            Assert.That(ex, Is.InstanceOf<DiamondClonableException>());
        }

        [Test]
        public void DiamondClonableException_WithMessage_HasMessage()
        {
            // Test the abstract base class through a concrete subclass
            var ex = new NoClonableFactorySetException();
            Assert.That(ex, Is.InstanceOf<Diamond.Core.Abstractions.DiamondCoreException>());
        }
    }
}
