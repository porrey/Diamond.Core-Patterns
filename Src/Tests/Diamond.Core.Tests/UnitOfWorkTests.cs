// Copyright(C) 2019-2026, Daniel M. Porrey. All rights reserved.
// Licensed under the LGPL-3.0-or-later license.
using System;
using System.Threading.Tasks;
using Diamond.Core.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Diamond.Core.Tests
{
	/// <summary>
	/// Tests for <see cref="UnitOfWorkTemplate{TResult,TSourceItem}"/> and
	/// <see cref="UnitOfWorkFactory"/>.
	/// </summary>
	public class UnitOfWorkTests
	{
		// ─── Concrete UnitOfWork implementation ───────────────────────────────

		private sealed class StringReverser : UnitOfWorkTemplate<string, string>
		{
			protected override Task<string> OnCommitAsync(string item)
			{
				char[] chars = item.ToCharArray();
				Array.Reverse(chars);
				return Task.FromResult(new string(chars));
			}
		}

		private sealed class CounterWork : UnitOfWorkTemplate<int, IUnitOfWork>
		{
			protected override Task<int> OnCommitAsync(IUnitOfWork item)
			{
				return Task.FromResult(42);
			}
		}

		// ─── UnitOfWorkTemplate ───────────────────────────────────────────────

		[Fact]
		public async Task UnitOfWorkTemplate_CommitAsync_DelegatesToOnCommitAsync()
		{
			var uow = new StringReverser();
			string result = await uow.CommitAsync("hello");
			Assert.Equal("olleh", result);
		}

		[Fact]
		public async Task UnitOfWorkTemplate_OnCommitAsync_Default_ThrowsNotImplementedException()
		{
			var uow = new DefaultUow();
			await Assert.ThrowsAsync<NotImplementedException>(() => uow.CommitAsync("input"));
		}

		private sealed class DefaultUow : UnitOfWorkTemplate<string, string>
		{
			// Does not override OnCommitAsync — falls through to base.
		}

		// ─── UnitOfWorkFactory ────────────────────────────────────────────────

		private static IServiceProvider BuildSp(string key, IUnitOfWork instance)
		{
			var services = new ServiceCollection();
			services.AddKeyedSingleton<IUnitOfWork>(key, (_, __) => instance);
			return services.BuildServiceProvider();
		}

		[Fact]
		public async Task UnitOfWorkFactory_GetAsync_ReturnsCorrectUnitOfWork()
		{
			var uow = new StringReverser();
			IServiceProvider sp = BuildSp("reverser", uow);
			var factory = new UnitOfWorkFactory(sp);

			IUnitOfWork<string, string> resolved = await factory.GetAsync<string, string>("reverser");
			Assert.NotNull(resolved);
			string result = await resolved.CommitAsync("abc");
			Assert.Equal("cba", result);
		}

		[Fact]
		public async Task UnitOfWorkFactory_GetAsync_NullKey_ThrowsArgumentException()
		{
			var services = new ServiceCollection();
			var factory = new UnitOfWorkFactory(services.BuildServiceProvider());
			await Assert.ThrowsAsync<ArgumentException>(() => factory.GetAsync<string, string>(null));
		}

		[Fact]
		public async Task UnitOfWorkFactory_GetAsync_WhitespaceKey_ThrowsArgumentException()
		{
			var services = new ServiceCollection();
			var factory = new UnitOfWorkFactory(services.BuildServiceProvider());
			await Assert.ThrowsAsync<ArgumentException>(() => factory.GetAsync<string, string>("  "));
		}

		[Fact]
		public async Task UnitOfWorkFactory_GetAsync_MissingKey_ThrowsUnitOfWorkNotFoundException()
		{
			var services = new ServiceCollection();
			var factory = new UnitOfWorkFactory(services.BuildServiceProvider());
			await Assert.ThrowsAsync<UnitOfWorkNotFoundException<string, string>>(
				() => factory.GetAsync<string, string>("no-such-key"));
		}

		[Fact]
		public async Task UnitOfWorkFactory_GetAsync_WrongType_ReturnsNull()
		{
			// Register a CounterWork under a key, then ask for <string,string>.
			var uow = new CounterWork();
			IServiceProvider sp = BuildSp("counter", uow);
			var factory = new UnitOfWorkFactory(sp);

			// CounterWork implements IUnitOfWork<int, IUnitOfWork>, not <string, string>.
			IUnitOfWork<string, string> resolved = await factory.GetAsync<string, string>("counter");
			Assert.Null(resolved);
		}
	}
}
