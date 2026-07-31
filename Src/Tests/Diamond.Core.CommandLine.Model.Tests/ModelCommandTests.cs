using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Diamond.Core.CommandLine.Model;

namespace Diamond.Core.CommandLine.Model.Tests
{
    // ─── Test Models ──────────────────────────────────────────────────────────────

    /// <summary>
    /// Minimal model with no attributes — property names drive option names.
    /// </summary>
    public class SimpleModel
    {
        public string Name { get; set; }
        public int Count { get; set; }
    }

    /// <summary>
    /// Model that uses [Display] and [Required] attributes, matching the
    /// shape of the Invoice model used in the ConsoleCommand example.
    /// </summary>
    public class AnnotatedModel
    {
        [Display(Name = "--number", ShortName = "-n", Description = "The invoice number.", Order = 1)]
        [Required]
        public string Number { get; set; }

        [Display(Name = "--description", ShortName = "-d", Description = "A description.", Order = 2)]
        public string Description { get; set; }

        [Display(Name = "--total", ShortName = "-t", Description = "The total amount.", Order = 3)]
        public decimal Total { get; set; }
    }

    /// <summary>
    /// Model where Order values are intentionally reversed so we can verify sorting.
    /// </summary>
    public class OrderedModel
    {
        [Display(Name = "--first", ShortName = "-f", Order = 10)]
        public string First { get; set; }

        [Display(Name = "--second", ShortName = "-s", Order = 1)]
        public string Second { get; set; }
    }

    // ─── Concrete command helpers ─────────────────────────────────────────────────

    /// <summary>
    /// Concrete command over SimpleModel that captures the parsed model.
    /// </summary>
    public class SimpleModelCommand : ModelCommand<SimpleModel>
    {
        public SimpleModel LastModel { get; private set; }
        public int HandleCallCount { get; private set; }

        public SimpleModelCommand() : base("simple", "A simple command.") { }

        protected override Task<int> OnHandleCommand(SimpleModel item)
        {
            LastModel = item;
            HandleCallCount++;
            return Task.FromResult(0);
        }
    }

    /// <summary>
    /// Concrete command over AnnotatedModel that captures the parsed model.
    /// </summary>
    public class AnnotatedModelCommand : ModelCommand<AnnotatedModel>
    {
        public AnnotatedModel LastModel { get; private set; }

        public AnnotatedModelCommand() : base("create", "Create an invoice.") { }

        protected override Task<int> OnHandleCommand(AnnotatedModel item)
        {
            LastModel = item;
            return Task.FromResult(0);
        }
    }

    /// <summary>
    /// Concrete command over AnnotatedModel that returns a non-zero exit code.
    /// </summary>
    public class FailingModelCommand : ModelCommand<AnnotatedModel>
    {
        public FailingModelCommand() : base("fail", "Always returns 2.") { }

        protected override Task<int> OnHandleCommand(AnnotatedModel item)
        {
            return Task.FromResult(2);
        }
    }

    // ─── ModelCommand Construction Tests ─────────────────────────────────────────

    [TestFixture]
    public class ModelCommandConstructionTests
    {
        [Test]
        public void Constructor_NoLogger_SetsCommandName()
        {
            var cmd = new SimpleModelCommand();
            Assert.That(cmd.Name, Is.EqualTo("simple"));
        }

        [Test]
        public void Constructor_SetsCommandDescription()
        {
            var cmd = new SimpleModelCommand();
            Assert.That(cmd.Description, Is.EqualTo("A simple command."));
        }

        [Test]
        public void Constructor_SimpleModel_RegistersTwoOptions()
        {
            var cmd = new SimpleModelCommand();
            // SimpleModel has Name and Count
            Assert.That(cmd.Options.Count, Is.EqualTo(2));
        }

        [Test]
        public void Constructor_AnnotatedModel_RegistersThreeOptions()
        {
            var cmd = new AnnotatedModelCommand();
            Assert.That(cmd.Options.Count, Is.EqualTo(3));
        }
    }

    // ─── BuildOptions: option name/alias/description ─────────────────────────────

    [TestFixture]
    public class ModelCommandBuildOptionsTests
    {
        private AnnotatedModelCommand _cmd;
        private IList<Option> _options;

        [SetUp]
        public void Setup()
        {
            _cmd = new AnnotatedModelCommand();
            _options = _cmd.Options;
        }

        [Test]
        public void BuildOptions_UsesDisplayName_ForOptionName()
        {
            Assert.That(_options.Any(o => o.Name == "--number"), Is.True);
        }

        [Test]
        public void BuildOptions_UsesDisplayShortName_ForAlias()
        {
            Option numberOption = _options.First(o => o.Name == "--number");
            Assert.That(numberOption.Aliases.Contains("-n"), Is.True);
        }

        [Test]
        public void BuildOptions_UsesDisplayDescription()
        {
            Option numberOption = _options.First(o => o.Name == "--number");
            Assert.That(numberOption.Description, Is.EqualTo("The invoice number."));
        }

        [Test]
        public void BuildOptions_AllAnnotatedOptionsPresent()
        {
            var names = _options.Select(o => o.Name).ToList();
            Assert.That(names, Does.Contain("--number"));
            Assert.That(names, Does.Contain("--description"));
            Assert.That(names, Does.Contain("--total"));
        }

        [Test]
        public void BuildOptions_WithoutDisplayAttribute_UsesPropertyNameAsOptionName()
        {
            var cmd = new SimpleModelCommand();
            var names = cmd.Options.Select(o => o.Name).ToList();
            // Property "Name" → "--name", Property "Count" → "--count"
            Assert.That(names, Does.Contain("--name"));
            Assert.That(names, Does.Contain("--count"));
        }

        [Test]
        public void BuildOptions_WithoutDisplayAttribute_UsesFirstCharAsAlias()
        {
            var cmd = new SimpleModelCommand();
            Option nameOption = cmd.Options.First(o => o.Name == "--name");
            Assert.That(nameOption.Aliases.Contains("-n"), Is.True);
        }

        [Test]
        public void BuildOptions_SortsOptionsByDisplayOrder()
        {
            // OrderedModel: --second has Order=1, --first has Order=10
            // After sorting by Order, --second should be first in the options list.
            var cmd = new ModelCommand<OrderedModel>("ordered");
            var names = cmd.Options.Select(o => o.Name).ToList();
            Assert.That(names.IndexOf("--second"), Is.LessThan(names.IndexOf("--first")));
        }

        [Test]
        public void BuildOptions_NullModel_RegistersNoOptions()
        {
            var cmd = new ModelCommand<NullModel>("list");
            Assert.That(cmd.Options.Count, Is.EqualTo(0));
        }
    }

    // ─── ModelCommand Invocation (parse → model binding) ─────────────────────────

    [TestFixture]
    public class ModelCommandInvocationTests
    {
        [Test]
        public async Task Invoke_StringProperty_IsBoundFromArgs()
        {
            var root = new RootCommand("test");
            var cmd = new SimpleModelCommand();
            root.Add(cmd);

            await CommandLineParser.Parse(root, new[] { "simple", "--name", "Alice" }).InvokeAsync();

            Assert.That(cmd.LastModel, Is.Not.Null);
            Assert.That(cmd.LastModel.Name, Is.EqualTo("Alice"));
        }

        [Test]
        public async Task Invoke_IntProperty_IsBoundFromArgs()
        {
            var root = new RootCommand("test");
            var cmd = new SimpleModelCommand();
            root.Add(cmd);

            await CommandLineParser.Parse(root, new[] { "simple", "--count", "42" }).InvokeAsync();

            Assert.That(cmd.LastModel.Count, Is.EqualTo(42));
        }

        [Test]
        public async Task Invoke_AllPropertiesPopulated()
        {
            var root = new RootCommand("test");
            var cmd = new AnnotatedModelCommand();
            root.Add(cmd);

            await CommandLineParser.Parse(root, new[] { "create", "--number", "INV001", "--description", "Test invoice", "--total", "99.99" }).InvokeAsync();

            Assert.That(cmd.LastModel.Number, Is.EqualTo("INV001"));
            Assert.That(cmd.LastModel.Description, Is.EqualTo("Test invoice"));
            Assert.That(cmd.LastModel.Total, Is.EqualTo(99.99m));
        }

        [Test]
        public async Task Invoke_AliasUsedInsteadOfFullName_BindsCorrectly()
        {
            var root = new RootCommand("test");
            var cmd = new AnnotatedModelCommand();
            root.Add(cmd);

            await CommandLineParser.Parse(root, new[] { "create", "-n", "INV002" }).InvokeAsync();

            Assert.That(cmd.LastModel.Number, Is.EqualTo("INV002"));
        }

        [Test]
        public async Task Invoke_OnHandleCommandCalled_Once()
        {
            var root = new RootCommand("test");
            var cmd = new SimpleModelCommand();
            root.Add(cmd);

            await CommandLineParser.Parse(root, new[] { "simple", "--name", "Bob" }).InvokeAsync();

            Assert.That(cmd.HandleCallCount, Is.EqualTo(1));
        }

        [Test]
        public async Task Invoke_ReturnsExitCodeFromOnHandleCommand()
        {
            var root = new RootCommand("test");
            var cmd = new FailingModelCommand();
            root.Add(cmd);

            int result = await CommandLineParser.Parse(root, new[] { "fail", "--number", "X" }).InvokeAsync();

            Assert.That(result, Is.EqualTo(2));
        }

        [Test]
        public async Task Invoke_DefaultOnHandleCommand_ReturnsZero()
        {
            var root = new RootCommand("test");
            var cmd = new ModelCommand<SimpleModel>("noop");
            root.Add(cmd);

            int result = await CommandLineParser.Parse(root, new[] { "noop" }).InvokeAsync();

            Assert.That(result, Is.EqualTo(0));
        }
    }

    // ─── OptionDescriptor Tests ───────────────────────────────────────────────────

    [TestFixture]
    public class OptionDescriptorTests
    {
        [Test]
        public void OptionDescriptor_DefaultValues()
        {
            var descriptor = new OptionDescriptor();
            Assert.That(descriptor.Name, Is.Null);
            Assert.That(descriptor.Alias, Is.Null);
            Assert.That(descriptor.Description, Is.Null);
            Assert.That(descriptor.Order, Is.EqualTo(0));
            Assert.That(descriptor.IsRequired, Is.False);
            Assert.That(descriptor.PropertyType, Is.Null);
            Assert.That(descriptor.ModelProperty, Is.Null);
        }

        [Test]
        public void OptionDescriptor_SetProperties()
        {
            var descriptor = new OptionDescriptor
            {
                Name = "--name",
                Alias = "-n",
                Description = "A name option.",
                Order = 5,
                IsRequired = true
            };

            Assert.That(descriptor.Name, Is.EqualTo("--name"));
            Assert.That(descriptor.Alias, Is.EqualTo("-n"));
            Assert.That(descriptor.Description, Is.EqualTo("A name option."));
            Assert.That(descriptor.Order, Is.EqualTo(5));
            Assert.That(descriptor.IsRequired, Is.True);
        }
    }

    // ─── NullModel Tests ──────────────────────────────────────────────────────────

    [TestFixture]
    public class NullModelTests
    {
        [Test]
        public void NullModel_CanInstantiate()
        {
            var model = new NullModel();
            Assert.That(model, Is.Not.Null);
        }

        [Test]
        public void ModelCommand_WithNullModel_HasNoOptions()
        {
            var cmd = new ModelCommand<NullModel>("list", "List everything.");
            Assert.That(cmd.Options.Count, Is.EqualTo(0));
        }
    }
}
