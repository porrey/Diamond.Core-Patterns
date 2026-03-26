// Copyright(C) 2019-2026, Daniel M. Porrey. All rights reserved.
// Licensed under the LGPL-3.0-or-later license.
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Diamond.Core.Rules;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Diamond.Core.Tests
{
	/// <summary>
	/// Tests for <see cref="RuleResultTemplate"/>, <see cref="RuleTemplate{TItem}"/>,
	/// and <see cref="RulesFactory"/>.
	/// </summary>
	public class RulesTests
	{
		// ─── RuleResultTemplate ───────────────────────────────────────────────

		[Fact]
		public void RuleResultTemplate_DefaultValues_AreCorrect()
		{
			var r = new RuleResultTemplate();
			Assert.False(r.Passed);
			Assert.Null(r.ErrorMessage);
		}

		[Fact]
		public void RuleResultTemplate_SetProperties_AreStored()
		{
			var r = new RuleResultTemplate
			{
				Passed = true,
				ErrorMessage = "all good"
			};
			Assert.True(r.Passed);
			Assert.Equal("all good", r.ErrorMessage);
		}

		// ─── Concrete RuleTemplate<TItem> ─────────────────────────────────────

		private sealed class AlwaysPassRule : RuleTemplate<string>
		{
			protected override Task<IRuleResult> OnValidateAsync(string item)
			{
				return Task.FromResult<IRuleResult>(new RuleResultTemplate { Passed = true });
			}
		}

		private sealed class AlwaysFailRule : RuleTemplate<string>
		{
			public AlwaysFailRule() : base("test-group") { }

			protected override Task<IRuleResult> OnValidateAsync(string item)
			{
				return Task.FromResult<IRuleResult>(new RuleResultTemplate { Passed = false, ErrorMessage = "failed" });
			}
		}

		[Fact]
		public async Task RuleTemplate_ValidateAsync_DelegatesToOnValidateAsync()
		{
			var rule = new AlwaysPassRule();
			IRuleResult result = await rule.ValidateAsync("input");
			Assert.True(result.Passed);
		}

		[Fact]
		public async Task RuleTemplate_WithGroup_GroupIsSet()
		{
			var rule = new AlwaysFailRule();
			Assert.Equal("test-group", rule.Group);
			IRuleResult result = await rule.ValidateAsync("x");
			Assert.False(result.Passed);
		}

		[Fact]
		public void RuleTemplate_DefaultConstructor_GroupIsNull()
		{
			var rule = new AlwaysPassRule();
			Assert.Null(rule.Group);
		}

		[Fact]
		public async Task RuleTemplate_WithGroupAndLogger_Works()
		{
			var rule = new AlwaysPassRule();
			rule.Group = "dynamic-group";
			IRuleResult result = await rule.ValidateAsync("hello");
			Assert.True(result.Passed);
		}

		// ─── Concrete RuleTemplate<TItem, TResult> ────────────────────────────

		private sealed class IntScoreRule : RuleTemplate<string, int>
		{
			protected override Task<int> OnValidateAsync(string item)
			{
				return Task.FromResult(item.Length);
			}
		}

		[Fact]
		public async Task RuleTemplateGeneric_ValidateAsync_ReturnsCustomResult()
		{
			var rule = new IntScoreRule();
			int score = await rule.ValidateAsync("hello");
			Assert.Equal(5, score);
		}

		// ─── RulesFactory ─────────────────────────────────────────────────────

		private static IServiceProvider BuildServiceProvider(IEnumerable<IRule> rules, string key)
		{
			var services = new ServiceCollection();
			services.AddKeyedSingleton<IEnumerable<IRule>>(key, (_, __) => rules);
			return services.BuildServiceProvider();
		}

		[Fact]
		public async Task RulesFactory_GetAllAsync_WithKey_ReturnsMatchingRules()
		{
			var rules = new IRule[] { new AlwaysPassRule() };
			IServiceProvider sp = BuildServiceProvider(rules, "my-key");
			var factory = new RulesFactory(sp);

			IEnumerable<IRule<string, IRuleResult>> result = await factory.GetAllAsync<string, IRuleResult>("my-key");
			Assert.Single(result);
		}

		[Fact]
		public async Task RulesFactory_GetAllAsync_NullKey_ThrowsArgumentException()
		{
			var services = new ServiceCollection();
			IServiceProvider sp = services.BuildServiceProvider();
			var factory = new RulesFactory(sp);

			await Assert.ThrowsAsync<ArgumentException>(() => factory.GetAllAsync<string, IRuleResult>(null));
		}

		[Fact]
		public async Task RulesFactory_GetAllAsync_WhitespaceKey_ThrowsArgumentException()
		{
			var services = new ServiceCollection();
			IServiceProvider sp = services.BuildServiceProvider();
			var factory = new RulesFactory(sp);

			await Assert.ThrowsAsync<ArgumentException>(() => factory.GetAllAsync<string, IRuleResult>("   "));
		}

		[Fact]
		public async Task RulesFactory_GetAllAsync_UnregisteredKey_ThrowsRulesNotFoundException()
		{
			var services = new ServiceCollection();
			IServiceProvider sp = services.BuildServiceProvider();
			var factory = new RulesFactory(sp);

			await Assert.ThrowsAsync<RulesNotFoundException<string>>(
				() => factory.GetAllAsync<string, IRuleResult>("missing-key"));
		}

		[Fact]
		public async Task RulesFactory_EvaluateAsync_AllPass_ReturnsEmptyString()
		{
			var rules = new IRule[] { new AlwaysPassRule() };
			IServiceProvider sp = BuildServiceProvider(rules, "eval-key");
			var factory = new RulesFactory(sp);

			string errors = await factory.EvaluateAsync("eval-key", "test");
			Assert.Equal("", errors);
		}

		[Fact]
		public async Task RulesFactory_EvaluateAsync_SomeFail_ReturnsErrorMessages()
		{
			var rules = new IRule[] { new AlwaysFailRule() };
			IServiceProvider sp = BuildServiceProvider(rules, "fail-key");
			var factory = new RulesFactory(sp);

			string errors = await factory.EvaluateAsync("fail-key", "test");
			Assert.Contains("failed", errors);
		}

		[Fact]
		public async Task RulesFactory_GetAllAsync_NoKey_ReturnsMatchingRules()
		{
			var rules = new IRule[] { new AlwaysPassRule() };
			IServiceProvider sp = BuildServiceProvider(rules, null);
			var factory = new RulesFactory(sp);

			IEnumerable<IRule<string>> result = await factory.GetAllAsync<string>();
			Assert.Single(result);
		}

		[Fact]
		public async Task RulesFactory_GetAllAsync_NoKeyAndNoRegistration_ThrowsRulesNotFoundException()
		{
			var services = new ServiceCollection();
			IServiceProvider sp = services.BuildServiceProvider();
			var factory = new RulesFactory(sp);

			await Assert.ThrowsAsync<RulesNotFoundException<string>>(
				() => factory.GetAllAsync<string>());
		}
	}
}
