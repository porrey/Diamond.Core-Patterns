using System;
using System.Threading.Tasks;
using Diamond.Core.Specification;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Specification.Tests
{
    // ─── Test Helpers ─────────────────────────────────────────────────────────────

    /// <summary>Concrete specification that returns a string result (no parameter).</summary>
    public class StringResultSpecification : SpecificationTemplate<string>
    {
        private readonly string _result;

        public StringResultSpecification(string result)
        {
            _result = result;
        }

        protected override Task<string> OnExecuteSelectionAsync()
        {
            return Task.FromResult(_result);
        }
    }

    /// <summary>Concrete specification that returns a string result (with int parameter).</summary>
    public class IntStringSpecification : SpecificationTemplate<int, string>
    {
        protected override Task<string> OnExecuteSelectionAsync(int input)
        {
            return Task.FromResult($"Value={input}");
        }
    }

    // ─── SpecificationTemplate Tests ─────────────────────────────────────────────

    [TestFixture]
    public class SpecificationTemplateTests
    {
        [Test]
        public async Task ExecuteSelectionAsync_CallsOnExecuteSelectionAsync()
        {
			StringResultSpecification spec = new StringResultSpecification("hello");
            string result = await spec.ExecuteSelectionAsync();
            Assert.That(result, Is.EqualTo("hello"));
        }

        [Test]
        public async Task ExecuteSelectionAsync_BaseTemplate_ThrowsNotImplemented()
        {
            // The base SpecificationTemplate<TResult> throws NotImplementedException
            // if OnExecuteSelectionAsync is not overridden.
            ThrowingSpecification spec = new ThrowingSpecification();
			await Assert.ThrowsAsync<NotImplementedException>(() => spec.ExecuteSelectionAsync());
        }

        [Test]
        public void Dispose_DoesNotThrow()
        {
            StringResultSpecification spec = new StringResultSpecification("test");
            Assert.DoesNotThrow(() => spec.Dispose());
        }

        // Minimal subclass that does NOT override OnExecuteSelectionAsync
        private class ThrowingSpecification : SpecificationTemplate<string> { }
    }

    // ─── SpecificationTemplate<TParameter, TResult> Tests ────────────────────────

    [TestFixture]
    public class SpecificationTemplateParameterTests
    {
        [Test]
        public async Task ExecuteSelectionAsync_WithParameter_ReturnsExpected()
        {
            IntStringSpecification spec = new IntStringSpecification();
            string result = await spec.ExecuteSelectionAsync(42);
            Assert.That(result, Is.EqualTo("Value=42"));
        }
    }

    // ─── SpecificationFactory Tests ───────────────────────────────────────────────

    [TestFixture]
    public class SpecificationFactoryTests
    {
        private ServiceProvider BuildProvider(Action<IServiceCollection> configure)
        {
            ServiceCollection services = new ServiceCollection();
            configure(services);
            return services.BuildServiceProvider();
        }

        // ── GetAsync<TResult> ─────────────────────────────────────────────────────

        [Test]
        public async Task GetAsync_TResult_Found_ReturnsSpecification()
        {
			ServiceProvider provider = BuildProvider(s =>
                s.AddKeyedTransient<ISpecification, StringResultSpecification>("my-spec",
                    (sp, key) => new StringResultSpecification("found")));

            SpecificationFactory factory = new SpecificationFactory(provider);

            ISpecification<string> spec = await factory.GetAsync<string>("my-spec");
            Assert.That(spec, Is.Not.Null);
        }

        [Test]
        public async Task GetAsync_TResult_Executes_Correctly()
        {
            ServiceProvider provider = BuildProvider(s =>
                s.AddKeyedTransient<ISpecification, StringResultSpecification>("greeting",
                    (sp, key) => new StringResultSpecification("Hello!")));

            SpecificationFactory factory = new SpecificationFactory(provider);
            ISpecification<string> spec = await factory.GetAsync<string>("greeting");
            string result = await spec.ExecuteSelectionAsync();
            Assert.That(result, Is.EqualTo("Hello!"));
        }

        [Test]
        public async Task GetAsync_TResult_NotFound_Throws()
        {
            ServiceProvider provider = this.BuildProvider(_ => { });
            SpecificationFactory factory = new SpecificationFactory(provider);
			await Assert.ThrowsAsync<SpecificationNotFoundException<string>>(
                () => factory.GetAsync<string>("missing"));
        }

        [Test]
        public async Task GetAsync_TResult_NullKey_Throws()
        {
            ServiceProvider provider = this.BuildProvider(_ => { });
            SpecificationFactory factory = new SpecificationFactory(provider);
			await Assert.ThrowsAsync<ArgumentException>(() => factory.GetAsync<string>(null));
        }

        [Test]
        public async Task GetAsync_TResult_WhitespaceKey_Throws()
        {
			ServiceProvider provider = this.BuildProvider(_ => { });
			SpecificationFactory factory = new SpecificationFactory(provider);
			await Assert.ThrowsAsync<ArgumentException>(() => factory.GetAsync<string>("  "));
        }

        // ── GetAsync<TParameter, TResult> ─────────────────────────────────────────

        [Test]
        public async Task GetAsync_TParameterTResult_Found_ReturnsSpecification()
        {
            ServiceProvider provider = this.BuildProvider(s =>
                s.AddKeyedTransient<ISpecification, IntStringSpecification>("int-spec"));

            SpecificationFactory factory = new SpecificationFactory(provider);
            ISpecification<int, string> spec = await factory.GetAsync<int, string>("int-spec");
            Assert.That(spec, Is.Not.Null);
        }

        [Test]
        public async Task GetAsync_TParameterTResult_Executes_Correctly()
        {
            ServiceProvider provider = this.BuildProvider(s =>
                s.AddKeyedTransient<ISpecification, IntStringSpecification>("int-spec"));

            SpecificationFactory factory = new SpecificationFactory(provider);
            ISpecification<int, string> spec = await factory.GetAsync<int, string>("int-spec");
            string result = await spec.ExecuteSelectionAsync(99);
            Assert.That(result, Is.EqualTo("Value=99"));
        }

        [Test]
        public async Task GetAsync_TParameterTResult_NotFound_Throws()
        {
            ServiceProvider provider = this.BuildProvider(_ => { });
            SpecificationFactory factory = new SpecificationFactory(provider);

            await Assert.ThrowsAsync<SpecificationNotFoundException<int, string>>(
                () => factory.GetAsync<int, string>("missing"));
        }

        [Test]
        public async Task GetAsync_TParameterTResult_NullKey_Throws()
        {
            ServiceProvider provider = this.BuildProvider(_ => { });
            SpecificationFactory factory = new SpecificationFactory(provider);

            await Assert.ThrowsAsync<ArgumentException>(() => factory.GetAsync<int, string>(null));
        }

        [Test]
        public async Task GetAsync_TParameterTResult_EmptyKey_Throws()
        {
            ServiceProvider provider = this.BuildProvider(_ => { });
            SpecificationFactory factory = new SpecificationFactory(provider);

            await Assert.ThrowsAsync<ArgumentException>(() => factory.GetAsync<int, string>(""));
        }
    }

    // ─── SpecificationNotFoundException Tests ────────────────────────────────────

    [TestFixture]
    public class SpecificationNotFoundExceptionTests
    {
        [Test]
        public void SpecificationNotFoundException_TResult_HasMessage()
        {
			SpecificationNotFoundException<string> ex = new SpecificationNotFoundException<string>("my-key");
            Assert.That(ex.Message, Does.Contain("my-key"));
            Assert.That(ex.Message, Does.Contain("String"));
        }

        [Test]
        public void SpecificationNotFoundException_TParameterTResult_HasMessage()
        {
			SpecificationNotFoundException<int, string> ex = new SpecificationNotFoundException<int, string>("my-key");
            Assert.That(ex.Message, Does.Contain("my-key"));
        }

        [Test]
        public void SpecificationNotFoundException_IsDisposable()
        {
            // Verify exception hierarchy inherits from DiamondSpecificationException
			SpecificationNotFoundException<string> ex = new SpecificationNotFoundException<string>("x");
            Assert.That(ex, Is.InstanceOf<DiamondSpecificationException>());
        }
    }
}
