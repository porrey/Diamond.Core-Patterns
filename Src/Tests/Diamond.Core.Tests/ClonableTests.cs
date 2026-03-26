// Copyright(C) 2019-2026, Daniel M. Porrey. All rights reserved.
// Licensed under the LGPL-3.0-or-later license.
using System;
using Diamond.Core.Clonable;
using Diamond.Core.Clonable.NetCore;
using Diamond.Core.Clonable.Newtonsoft;
using Xunit;

namespace Diamond.Core.Tests
{
	/// <summary>
	/// Tests for <see cref="ClonableFactory"/>, <see cref="CloneableObject"/> and the
	/// two <see cref="IObjectCloneFactory"/> implementations.
	/// </summary>
	public class ClonableTests
	{
		// ─── Helper model ─────────────────────────────────────────────────────

		private sealed class SampleModel : CloneableObject
		{
			public string Name { get; set; }
			public int Value { get; set; }
		}

		// ─── ClonableFactory ──────────────────────────────────────────────────

		[Fact]
		public void ClonableFactory_GetFactory_WhenNotSet_ThrowsNoClonableFactorySetException()
		{
			// Reset any previously set factory.
			ClonableFactory.SetFactory(null);
			Assert.Throws<NoClonableFactorySetException>(() => ClonableFactory.GetFactory());
		}

		[Fact]
		public void ClonableFactory_SetFactory_ThenGetFactory_ReturnsSetFactory()
		{
			var factory = new Diamond.Core.Clonable.NetCore.ObjectCloneFactory();
			ClonableFactory.SetFactory(factory);
			IObjectCloneFactory retrieved = ClonableFactory.GetFactory();
			Assert.Same(factory, retrieved);
		}

		// ─── Microsoft (System.Text.Json) ObjectCloneFactory ──────────────────

		[Fact]
		public void MicrosoftCloneFactory_CloneInstance_Generic_ReturnsDeepCopy()
		{
			var factory = new Diamond.Core.Clonable.NetCore.ObjectCloneFactory();
			var original = new SampleModel { Name = "Test", Value = 42 };

			SampleModel clone = factory.CloneInstance(original);

			Assert.NotNull(clone);
			Assert.Equal("Test", clone.Name);
			Assert.Equal(42, clone.Value);
			Assert.NotSame(original, clone);
		}

		[Fact]
		public void MicrosoftCloneFactory_CloneInstance_NonGeneric_ReturnsObject()
		{
			var factory = new Diamond.Core.Clonable.NetCore.ObjectCloneFactory();
			var original = new SampleModel { Name = "Test", Value = 99 };

			object clone = factory.CloneInstance((ICloneable)original);
			Assert.NotNull(clone);
		}

		// ─── Newtonsoft ObjectCloneFactory ────────────────────────────────────

		[Fact]
		public void NewtonsoftCloneFactory_CloneInstance_Generic_ReturnsDeepCopy()
		{
			var factory = new Diamond.Core.Clonable.Newtonsoft.ObjectCloneFactory();
			var original = new SampleModel { Name = "Newton", Value = 7 };

			SampleModel clone = factory.CloneInstance(original);

			Assert.NotNull(clone);
			Assert.Equal("Newton", clone.Name);
			Assert.Equal(7, clone.Value);
			Assert.NotSame(original, clone);
		}

		[Fact]
		public void NewtonsoftCloneFactory_CloneInstance_NonGeneric_ReturnsObject()
		{
			var factory = new Diamond.Core.Clonable.Newtonsoft.ObjectCloneFactory();
			var original = new SampleModel { Name = "Newton", Value = 7 };

			object clone = factory.CloneInstance((ICloneable)original);
			Assert.NotNull(clone);
		}

		// ─── CloneableObject.Clone() using ClonableFactory ───────────────────

		[Fact]
		public void CloneableObject_Clone_WithNewtonsoftFactory_ReturnsDeepCopy()
		{
			ClonableFactory.SetFactory(new Diamond.Core.Clonable.Newtonsoft.ObjectCloneFactory());
			var original = new SampleModel { Name = "Newtonsoft", Value = 2 };

			var clone = (SampleModel)original.Clone();

			Assert.NotNull(clone);
			Assert.Equal("Newtonsoft", clone.Name);
			Assert.Equal(2, clone.Value);
			Assert.NotSame(original, clone);
		}

		[Fact]
		public void CloneableObject_Clone_WithoutFactory_ThrowsNoClonableFactorySetException()
		{
			ClonableFactory.SetFactory(null);
			var original = new SampleModel { Name = "No factory", Value = 0 };
			Assert.Throws<NoClonableFactorySetException>(() => original.Clone());
		}

		[Fact]
		public void CloneableObject_Clone_MutatingClone_DoesNotAffectOriginal()
		{
			// Use Newtonsoft since the Microsoft factory requires a concrete (non-abstract)
			// deserialization target, and CloneableObject.Clone() passes 'this' typed as
			// the abstract base class to the factory.
			ClonableFactory.SetFactory(new Diamond.Core.Clonable.Newtonsoft.ObjectCloneFactory());
			var original = new SampleModel { Name = "Original", Value = 10 };

			var clone = (SampleModel)original.Clone();
			clone.Name = "Modified";
			clone.Value = 99;

			Assert.Equal("Original", original.Name);
			Assert.Equal(10, original.Value);
		}
	}
}
