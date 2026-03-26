// Copyright(C) 2019-2026, Daniel M. Porrey. All rights reserved.
// Licensed under the LGPL-3.0-or-later license.
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Diamond.Core.Specification;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Diamond.Core.Tests
{
	/// <summary>
	/// Tests for <see cref="SpecificationTemplate{TResult}"/>,
	/// <see cref="SpecificationTemplate{TParameter,TResult}"/> and
	/// <see cref="SpecificationFactory"/>.
	/// </summary>
	public class SpecificationTests
	{
		// ─── Concrete specification templates ─────────────────────────────────

		private sealed class StringListSpecification : SpecificationTemplate<IEnumerable<string>>
		{
			protected override Task<IEnumerable<string>> OnExecuteSelectionAsync()
			{
				return Task.FromResult<IEnumerable<string>>(new[] { "apple", "banana" });
			}
		}

		private sealed class FilteredListSpecification : SpecificationTemplate<string, IEnumerable<string>>
		{
			protected override Task<IEnumerable<string>> OnExecuteSelectionAsync(string prefix)
			{
				return Task.FromResult<IEnumerable<string>>(new[] { $"{prefix}_1", $"{prefix}_2" });
			}
		}

		[Fact]
		public async Task SpecificationTemplate_ExecuteSelectionAsync_ReturnsExpectedResult()
		{
			var spec = new StringListSpecification();
			IEnumerable<string> result = await spec.ExecuteSelectionAsync();
			Assert.Contains("apple", result);
			Assert.Contains("banana", result);
		}

		[Fact]
		public async Task SpecificationTemplate_WithParameter_ExecuteSelectionAsync_ReturnsExpectedResult()
		{
			var spec = new FilteredListSpecification();
			IEnumerable<string> result = await spec.ExecuteSelectionAsync("foo");
			Assert.Contains("foo_1", result);
			Assert.Contains("foo_2", result);
		}

		[Fact]
		public async Task SpecificationTemplate_BaseNotImplemented_ThrowsNotImplementedException()
		{
			var spec = new NotImplementedSpecification();
			await Assert.ThrowsAsync<NotImplementedException>(() => spec.ExecuteSelectionAsync());
		}

		private sealed class NotImplementedSpecification : SpecificationTemplate<string>
		{
			// does not override OnExecuteSelectionAsync — falls through to base
		}

		// ─── SpecificationFactory ─────────────────────────────────────────────

		private static IServiceProvider BuildSp<TSpec>(string key, TSpec instance) where TSpec : ISpecification
		{
			var services = new ServiceCollection();
			services.AddKeyedSingleton<ISpecification>(key, (_, __) => instance);
			return services.BuildServiceProvider();
		}

		[Fact]
		public async Task SpecificationFactory_GetAsync_TResult_ReturnsMappedSpec()
		{
			var spec = new StringListSpecification();
			IServiceProvider sp = BuildSp("list-key", spec);
			var factory = new SpecificationFactory(sp);

			ISpecification<IEnumerable<string>> resolved = await factory.GetAsync<IEnumerable<string>>("list-key");
			Assert.NotNull(resolved);
		}

		[Fact]
		public async Task SpecificationFactory_GetAsync_TResult_NullKey_ThrowsArgumentException()
		{
			var services = new ServiceCollection();
			IServiceProvider sp = services.BuildServiceProvider();
			var factory = new SpecificationFactory(sp);

			await Assert.ThrowsAsync<ArgumentException>(() => factory.GetAsync<string>(null));
		}

		[Fact]
		public async Task SpecificationFactory_GetAsync_TResult_WhitespaceKey_ThrowsArgumentException()
		{
			var services = new ServiceCollection();
			IServiceProvider sp = services.BuildServiceProvider();
			var factory = new SpecificationFactory(sp);

			await Assert.ThrowsAsync<ArgumentException>(() => factory.GetAsync<string>("  "));
		}

		[Fact]
		public async Task SpecificationFactory_GetAsync_TResult_MissingKey_ThrowsNotFoundException()
		{
			var services = new ServiceCollection();
			IServiceProvider sp = services.BuildServiceProvider();
			var factory = new SpecificationFactory(sp);

			await Assert.ThrowsAsync<SpecificationNotFoundException<string>>(
				() => factory.GetAsync<string>("no-such-key"));
		}

		[Fact]
		public async Task SpecificationFactory_GetAsync_TParameterTResult_ReturnsMappedSpec()
		{
			var spec = new FilteredListSpecification();
			IServiceProvider sp = BuildSp("filtered-key", spec);
			var factory = new SpecificationFactory(sp);

			ISpecification<string, IEnumerable<string>> resolved =
				await factory.GetAsync<string, IEnumerable<string>>("filtered-key");
			Assert.NotNull(resolved);
		}

		[Fact]
		public async Task SpecificationFactory_GetAsync_TParameterTResult_NullKey_ThrowsArgumentException()
		{
			var services = new ServiceCollection();
			IServiceProvider sp = services.BuildServiceProvider();
			var factory = new SpecificationFactory(sp);

			await Assert.ThrowsAsync<ArgumentException>(
				() => factory.GetAsync<string, IEnumerable<string>>(null));
		}

		[Fact]
		public async Task SpecificationFactory_GetAsync_TParameterTResult_MissingKey_ThrowsNotFoundException()
		{
			var services = new ServiceCollection();
			IServiceProvider sp = services.BuildServiceProvider();
			var factory = new SpecificationFactory(sp);

			await Assert.ThrowsAsync<SpecificationNotFoundException<string, IEnumerable<string>>>(
				() => factory.GetAsync<string, IEnumerable<string>>("no-such-key"));
		}

		[Fact]
		public async Task SpecificationFactory_GetAsync_TResult_WrongType_ReturnsNull()
		{
			// Register a parameterised spec under a key; ask for a non-parameterised one.
			var spec = new FilteredListSpecification();
			IServiceProvider sp = BuildSp("wrong-type-key", spec);
			var factory = new SpecificationFactory(sp);

			// FilteredListSpecification does not implement ISpecification<IEnumerable<string>> (no-param),
			// so the factory should dispose and return null.
			ISpecification<IEnumerable<string>> resolved =
				await factory.GetAsync<IEnumerable<string>>("wrong-type-key");
			Assert.Null(resolved);
		}
	}
}
