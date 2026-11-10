using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Rules.Tests
{
    // ─── Test Helpers ─────────────────────────────────────────────────────────────

    public class SampleModel
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    /// <summary>Rule that passes when the model's name is not empty.</summary>
    public class NameNotEmptyRule : RuleTemplate<SampleModel>
    {
        protected override Task<IRuleResult> OnValidateAsync(SampleModel item)
        {
            return Task.FromResult<IRuleResult>(new RuleResultTemplate
            {
                Passed = !string.IsNullOrEmpty(item.Name),
                ErrorMessage = "Name is required."
            });
        }
    }

    /// <summary>Rule that passes when age is >= 18.</summary>
    public class AgeAdultRule : RuleTemplate<SampleModel>
    {
        protected override Task<IRuleResult> OnValidateAsync(SampleModel item)
        {
            return Task.FromResult<IRuleResult>(new RuleResultTemplate
            {
                Passed = item.Age >= 18,
                ErrorMessage = "Must be 18 or older."
            });
        }
    }

    /// <summary>Rule that always fails - used for testing failure paths.</summary>
    public class AlwaysFailRule : RuleTemplate<SampleModel, IRuleResult>
    {
        protected override Task<IRuleResult> OnValidateAsync(SampleModel item)
        {
            return Task.FromResult<IRuleResult>(new RuleResultTemplate
            {
                Passed = false,
                ErrorMessage = "Always fails."
            });
        }
    }

    // ─── RuleResultTemplate Tests ─────────────────────────────────────────────────

    [TestFixture]
    public class RuleResultTemplateTests
    {
        [Test]
        public void RuleResultTemplate_DefaultPassedIsFalse()
        {
            RuleResultTemplate result = new RuleResultTemplate();
            Assert.That(result.Passed, Is.False);
        }

        [Test]
        public void RuleResultTemplate_SetPassed_True()
        {
            RuleResultTemplate result = new RuleResultTemplate { Passed = true };
            Assert.That(result.Passed, Is.True);
        }

        [Test]
        public void RuleResultTemplate_SetErrorMessage()
        {
            RuleResultTemplate result = new RuleResultTemplate { ErrorMessage = "Error occurred." };
            Assert.That(result.ErrorMessage, Is.EqualTo("Error occurred."));
        }
    }

    // ─── RuleTemplate Tests ───────────────────────────────────────────────────────

    [TestFixture]
    public class RuleTemplateTests
    {
        [Test]
        public async Task ValidateAsync_Rule_CallsOnValidate()
        {
            NameNotEmptyRule rule = new NameNotEmptyRule();
            SampleModel model = new SampleModel { Name = "Alice" };
            IRuleResult result = await rule.ValidateAsync(model);
            Assert.That(result.Passed, Is.True);
        }

        [Test]
        public async Task ValidateAsync_Rule_FailsWhenInvalid()
        {
            NameNotEmptyRule rule = new NameNotEmptyRule();
            SampleModel model = new SampleModel { Name = "" };
            IRuleResult result = await rule.ValidateAsync(model);
            Assert.That(result.Passed, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("Name is required."));
        }

        [Test]
        public void RuleTemplate_DefaultGroup_IsNull()
        {
            NameNotEmptyRule rule = new NameNotEmptyRule();
            Assert.That(rule.Group, Is.Null);
        }
    }

    // ─── RulesFactory Tests ───────────────────────────────────────────────────────

    [TestFixture]
    public class RulesFactoryTests
    {
        private ServiceProvider BuildProvider(Action<IServiceCollection> configure)
        {
            ServiceCollection services = new ServiceCollection();
            configure(services);
            return services.BuildServiceProvider();
        }

        // ── GetAllAsync<TItem> (no key) ───────────────────────────────────────────

        [Test]
        public async Task GetAllAsync_NoKey_ReturnsMatchingRules()
        {
            ServiceProvider provider = this.BuildProvider(s =>
            {
                s.AddKeyedSingleton<IEnumerable<IRule>>(null as string,
                    (sp, key) => new IRule[] { new NameNotEmptyRule(), new AgeAdultRule() });
            });

            RulesFactory factory = new RulesFactory(provider);
			IEnumerable<IRule<SampleModel>> rules = await factory.GetAllAsync<SampleModel>();
            Assert.That(rules, Has.Exactly(2).Items);
        }

        [Test]
        public async System.Threading.Tasks.Task GetAllAsync_NoKey_EmptyList_Throws()
        {
            ServiceProvider provider = this.BuildProvider(s =>
            {
                s.AddKeyedSingleton<IEnumerable<IRule>>(null as string,
                    (sp, key) => Array.Empty<IRule>());
            });

            RulesFactory factory = new RulesFactory(provider);
			await Assert.ThrowsAsync<RulesNotFoundException<SampleModel>>(
                () => factory.GetAllAsync<SampleModel>());
        }

        [Test]
        public async Task GetAllAsync_NoKey_NoRegistration_Throws()
        {
            ServiceProvider provider = this.BuildProvider(_ => { });
            RulesFactory factory = new RulesFactory(provider);
            await Assert.ThrowsAsync<RulesNotFoundException<SampleModel>>(
                () => factory.GetAllAsync<SampleModel>());
        }

        // ── GetAllAsync<TItem>(key) ───────────────────────────────────────────────

        [Test]
        public async Task GetAllAsync_WithKey_ReturnsMatchingRules()
        {
			ServiceProvider provider = this.BuildProvider(s =>
            {
                s.AddKeyedSingleton<IEnumerable<IRule>>("sample-rules",
                    (sp, key) => new IRule[] { new NameNotEmptyRule() });
            });

            RulesFactory factory = new RulesFactory(provider);
            IEnumerable<IRule<SampleModel>> rules = await factory.GetAllAsync<SampleModel>("sample-rules");
            Assert.That(rules, Has.Exactly(1).Items);
        }

        [Test]
        public async System.Threading.Tasks.Task GetAllAsync_WithKey_NotFound_Throws()
        {
            ServiceProvider provider = this.BuildProvider(_ => { });
            RulesFactory factory = new RulesFactory(provider);
            await Assert.ThrowsAsync<RulesNotFoundException<SampleModel>>(
                () => factory.GetAllAsync<SampleModel>("missing-key"));
        }

        // ── GetAllAsync<TItem, TResult> with serviceKey ───────────────────────────

        [Test]
        public async Task GetAllAsync_TItemTResult_ReturnsMatchingRules()
        {
            ServiceProvider provider = this.BuildProvider(s =>
            {
                s.AddKeyedSingleton<IEnumerable<IRule>>("typed-rules",
                    (sp, key) => new IRule[] { new AlwaysFailRule() });
            });

            RulesFactory factory = new RulesFactory(provider);
			IEnumerable<IRule<SampleModel, IRuleResult>> rules = await factory.GetAllAsync<SampleModel, IRuleResult>("typed-rules");
            Assert.That(rules, Has.Exactly(1).Items);
        }

        [Test]
        public async System.Threading.Tasks.Task GetAllAsync_TItemTResult_NullKey_Throws()
        {
            ServiceProvider provider = this.BuildProvider(_ => { });
            RulesFactory factory = new RulesFactory(provider);
            await Assert.ThrowsAsync<ArgumentException>(
                () => factory.GetAllAsync<SampleModel, IRuleResult>(null));
        }

        [Test]
        public async System.Threading.Tasks.Task GetAllAsync_TItemTResult_EmptyKey_Throws()
        {
            ServiceProvider provider = this.BuildProvider(_ => { });
            RulesFactory factory = new RulesFactory(provider);
            await Assert.ThrowsAsync<ArgumentException>(
                () => factory.GetAllAsync<SampleModel, IRuleResult>(""));
        }

        [Test]
        public async System.Threading.Tasks.Task GetAllAsync_TItemTResult_NotFound_Throws()
        {
            ServiceProvider provider = this.BuildProvider(_ => { });
            RulesFactory factory = new RulesFactory(provider);
            await Assert.ThrowsAsync<RulesNotFoundException<SampleModel>>(
                () => factory.GetAllAsync<SampleModel, IRuleResult>("no-such-key"));
        }

        // ── EvaluateAsync ─────────────────────────────────────────────────────────

        [Test]
        public async Task EvaluateAsync_AllPassed_ReturnsEmpty()
        {
			SampleModel model = new SampleModel { Name = "Alice", Age = 25 };

			ServiceProvider provider = this.BuildProvider(s =>
            {
                s.AddKeyedSingleton<IEnumerable<IRule>>("validation",
                    (sp, key) => new IRule[] { new NameNotEmptyRule(), new AgeAdultRule() });
            });

            RulesFactory factory = new RulesFactory(provider);
            string errors = await factory.EvaluateAsync("validation", model);
            Assert.That(errors, Is.Empty);
        }

        [Test]
        public async Task EvaluateAsync_SomeFailed_ReturnsErrorMessages()
        {
            SampleModel model = new SampleModel { Name = "", Age = 15 };

            ServiceProvider provider = this.BuildProvider(s =>
            {
                s.AddKeyedSingleton<IEnumerable<IRule>>("validation",
                    (sp, key) => new IRule[] { new NameNotEmptyRule(), new AgeAdultRule() });
            });

            RulesFactory factory = new RulesFactory(provider);
            string errors = await factory.EvaluateAsync("validation", model);
            Assert.That(errors, Does.Contain("Name is required."));
            Assert.That(errors, Does.Contain("Must be 18 or older."));
        }

        [Test]
        public async Task EvaluateAsync_SingleFailure_ReturnsOneMessage()
        {
            SampleModel model = new SampleModel { Name = "Bob", Age = 15 };

            ServiceProvider provider = this.BuildProvider(s =>
            {
                s.AddKeyedSingleton<IEnumerable<IRule>>("validation",
                    (sp, key) => new IRule[] { new NameNotEmptyRule(), new AgeAdultRule() });
            });

            RulesFactory factory = new RulesFactory(provider);
            string errors = await factory.EvaluateAsync("validation", model);
            Assert.That(errors, Does.Contain("Must be 18 or older."));
            Assert.That(errors, Does.Not.Contain("Name is required."));
        }
    }

    // ─── RulesNotFoundException Tests ─────────────────────────────────────────────

    [TestFixture]
    public class RulesNotFoundExceptionTests
    {
        [Test]
        public void RulesNotFoundException_NoGroup_HasMessage()
        {
            RulesNotFoundException<SampleModel> ex = new RulesNotFoundException<SampleModel>();
            Assert.That(ex.Message, Does.Contain("SampleModel"));
        }

        [Test]
        public void RulesNotFoundException_WithGroup_HasGroupInMessage()
        {
            RulesNotFoundException<SampleModel> ex = new RulesNotFoundException<SampleModel>("my-group");
            Assert.That(ex.Message, Does.Contain("SampleModel"));
            Assert.That(ex.Message, Does.Contain("my-group"));
        }
    }
}
