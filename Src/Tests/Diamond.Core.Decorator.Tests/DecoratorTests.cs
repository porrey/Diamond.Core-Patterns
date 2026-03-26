using System;
using System.Threading.Tasks;
using Diamond.Core.Decorator;
using Microsoft.Extensions.DependencyInjection;

namespace Diamond.Core.Decorator.Tests
{
    // ─── Test Helpers ─────────────────────────────────────────────────────────────

    public class SampleDto
    {
        public string Value { get; set; }
    }

    /// <summary>Concrete decorator that returns the length of the dto's Value string.</summary>
    public class LengthDecorator : DecoratorTemplate<SampleDto, int>
    {
        protected override Task<int> OnTakeActionAsync(SampleDto item)
        {
            return Task.FromResult(item.Value?.Length ?? 0);
        }
    }

    /// <summary>Concrete decorator that returns a formatted string.</summary>
    public class FormatDecorator : DecoratorTemplate<SampleDto, string>
    {
        protected override Task<string> OnTakeActionAsync(SampleDto item)
        {
            return Task.FromResult($"formatted:{item.Value}");
        }
    }

    // ─── DecoratorTemplate Tests ──────────────────────────────────────────────────

    [TestFixture]
    public class DecoratorTemplateTests
    {
        // ── Item property ────────────────────────────────────────────────────────

        [Test]
        public void Item_SetOnce_Succeeds()
        {
            var dec = new LengthDecorator();
            dec.Item = new SampleDto { Value = "hello" };
            Assert.That(dec.Item.Value, Is.EqualTo("hello"));
        }

        [Test]
        public void Item_SetTwiceToDifferentValue_Throws()
        {
            var dec = new LengthDecorator();
            dec.Item = new SampleDto { Value = "first" };
            Assert.Throws<DecoratedItemInstanceAlreadySetException>(
                () => dec.Item = new SampleDto { Value = "second" });
        }

        [Test]
        public void Item_SetTwice_SecondNull_DoesNotThrow()
        {
            // Setting to default/null after setting should be allowed
            var dec = new LengthDecorator();
            dec.Item = new SampleDto { Value = "first" };
            Assert.DoesNotThrow(() => dec.Item = null);
        }

        // ── TakeActionAsync (no parameter) ───────────────────────────────────────

        [Test]
        public async Task TakeActionAsync_NoParam_ItemSet_Succeeds()
        {
            var dec = new LengthDecorator();
            dec.Item = new SampleDto { Value = "hello" };
            int result = await dec.TakeActionAsync();
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public void TakeActionAsync_NoParam_ItemNotSet_Throws()
        {
            var dec = new LengthDecorator();
            Assert.ThrowsAsync<DecoratedItemInstanceNotSetException>(
                () => dec.TakeActionAsync());
        }

        // ── TakeActionAsync (with parameter) ─────────────────────────────────────

        [Test]
        public async Task TakeActionAsync_WithParam_Succeeds()
        {
            var dec = new LengthDecorator();
            var dto = new SampleDto { Value = "world" };
            int result = await dec.TakeActionAsync(dto);
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public void TakeActionAsync_WithParam_ItemAlreadySet_Throws()
        {
            var dec = new LengthDecorator();
            dec.Item = new SampleDto { Value = "first" };
            Assert.ThrowsAsync<DecoratedItemInstanceAlreadySetException>(
                () => dec.TakeActionAsync(new SampleDto { Value = "second" }));
        }

        [Test]
        public async Task TakeActionAsync_WithParam_SetsItem()
        {
            var dec = new LengthDecorator();
            var dto = new SampleDto { Value = "hi" };
            await dec.TakeActionAsync(dto);
            Assert.That(dec.Item, Is.SameAs(dto));
        }

        // ── Dispose ──────────────────────────────────────────────────────────────

        [Test]
        public void Dispose_DoesNotThrow()
        {
            var dec = new LengthDecorator();
            dec.Item = new SampleDto { Value = "data" };
            Assert.DoesNotThrow(() => dec.Dispose());
        }

        // ── String result decorator ───────────────────────────────────────────────

        [Test]
        public async Task TakeActionAsync_StringResult_Succeeds()
        {
            var dec = new FormatDecorator();
            var dto = new SampleDto { Value = "test" };
            string result = await dec.TakeActionAsync(dto);
            Assert.That(result, Is.EqualTo("formatted:test"));
        }
    }

    // ─── DecoratorFactory Tests ───────────────────────────────────────────────────

    [TestFixture]
    public class DecoratorFactoryTests
    {
        private ServiceProvider BuildProvider(Action<IServiceCollection> configure)
        {
            var services = new ServiceCollection();
            configure(services);
            return services.BuildServiceProvider();
        }

        [Test]
        public async Task GetAsync_ByKey_Found_ReturnsDecorator()
        {
            var provider = BuildProvider(s =>
                s.AddKeyedTransient<IDecorator, LengthDecorator>("length"));

            var factory = new DecoratorFactory(provider);
            var decorator = await factory.GetAsync<SampleDto, int>("length");
            Assert.That(decorator, Is.Not.Null);
        }

        [Test]
        public async Task GetAsync_ByKeyAndItem_SetsItem()
        {
            var provider = BuildProvider(s =>
                s.AddKeyedTransient<IDecorator, LengthDecorator>("length"));

            var factory = new DecoratorFactory(provider);
            var dto = new SampleDto { Value = "hello" };
            var decorator = await factory.GetAsync<SampleDto, int>("length", dto);
            Assert.That(decorator.Item, Is.SameAs(dto));
        }

        [Test]
        public async Task GetAsync_ByKeyAndItem_TakeAction_ReturnsResult()
        {
            var provider = BuildProvider(s =>
                s.AddKeyedTransient<IDecorator, LengthDecorator>("length"));

            var factory = new DecoratorFactory(provider);
            var dto = new SampleDto { Value = "hello" };
            var decorator = await factory.GetAsync<SampleDto, int>("length", dto);
            int result = await decorator.TakeActionAsync();
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public void GetAsync_NotFound_Throws()
        {
            var provider = BuildProvider(_ => { });
            var factory = new DecoratorFactory(provider);

            Assert.ThrowsAsync<DecoratorNotFoundException<SampleDto, int>>(
                () => factory.GetAsync<SampleDto, int>("missing"));
        }

        [Test]
        public void GetAsync_NullItem_Throws()
        {
            var provider = BuildProvider(s =>
                s.AddKeyedTransient<IDecorator, LengthDecorator>("length"));

            var factory = new DecoratorFactory(provider);

            Assert.ThrowsAsync<ArgumentNullException>(
                () => factory.GetAsync<SampleDto, int>("length", null));
        }
    }

    // ─── Exception Tests ──────────────────────────────────────────────────────────

    [TestFixture]
    public class DecoratorExceptionTests
    {
        [Test]
        public void DecoratedItemInstanceAlreadySetException_HasMessage()
        {
            var ex = new DecoratedItemInstanceAlreadySetException();
            Assert.That(ex.Message, Is.Not.Empty);
            Assert.That(ex.Message, Does.Contain("already been set"));
        }

        [Test]
        public void DecoratedItemInstanceNotSetException_HasMessage()
        {
            var ex = new DecoratedItemInstanceNotSetException();
            Assert.That(ex.Message, Is.Not.Empty);
        }

        [Test]
        public void DecoratorNotFoundException_NoName_HasMessage()
        {
            var ex = new DecoratorNotFoundException<SampleDto, int>();
            Assert.That(ex.Message, Does.Contain("SampleDto"));
            Assert.That(ex.Message, Does.Contain("Int32"));
        }

        [Test]
        public void DecoratorNotFoundException_WithName_HasNameInMessage()
        {
            var ex = new DecoratorNotFoundException<SampleDto, int>("my-decorator");
            Assert.That(ex.Message, Does.Contain("my-decorator"));
        }
    }
}
