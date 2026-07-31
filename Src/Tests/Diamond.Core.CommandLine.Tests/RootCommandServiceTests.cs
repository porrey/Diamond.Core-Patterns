using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Threading;
using System.Threading.Tasks;
using Diamond.Core.CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.CommandLine.Tests
{
    // ─── Test Helpers ─────────────────────────────────────────────────────────────

    /// <summary>
    /// Test implementation of IRootCommand that also inherits from RootCommand,
    /// matching what InternalRootCommand does in production.
    /// </summary>
    internal class TestRootCommand : RootCommand, IRootCommand
    {
        public TestRootCommand(string[] args) : base("Test application") { Args = args; }
        public string[] Args { get; set; }
    }

    /// <summary>
    /// A concrete System.CommandLine Command that also implements ICommand, so it can
    /// be registered in DI and picked up by RootCommandService.
    /// </summary>
    internal class TestSubCommand : Command, ICommand
    {
        public TestSubCommand(string name) : base(name, $"The {name} command.") { }
    }

    /// <summary>
    /// Minimal fake for IHostApplicationLifetime. Lifetime events use
    /// CancellationToken.None so registered callbacks are never invoked,
    /// which is fine for StartAsync unit tests.
    /// </summary>
    internal class FakeHostApplicationLifetime : IHostApplicationLifetime
    {
        public CancellationToken ApplicationStarted => CancellationToken.None;
        public CancellationToken ApplicationStopping => CancellationToken.None;
        public CancellationToken ApplicationStopped => CancellationToken.None;
        public bool StopApplicationCalled { get; private set; }
        public void StopApplication() { StopApplicationCalled = true; }
    }

    /// <summary>Thin IServiceScope that delegates to a given IServiceProvider.</summary>
    internal class FakeServiceScope : IServiceScope
    {
        public FakeServiceScope(IServiceProvider sp) { ServiceProvider = sp; }
        public IServiceProvider ServiceProvider { get; }
        public void Dispose() { }
    }

    /// <summary>IServiceScopeFactory that always returns the same provider.</summary>
    internal class FakeServiceScopeFactory : IServiceScopeFactory
    {
        private readonly IServiceProvider _sp;
        public FakeServiceScopeFactory(IServiceProvider sp) { _sp = sp; }
        public IServiceScope CreateScope() => new FakeServiceScope(_sp);
    }

    /// <summary>
    /// Builds a RootCommandService wired with the supplied commands.
    /// </summary>
    internal static class ServiceBuilder
    {
        public static RootCommandService Build(
            TestRootCommand rootCommand,
            FakeHostApplicationLifetime lifetime,
            params ICommand[] commands)
        {
            var services = new ServiceCollection();
            foreach (ICommand cmd in commands)
            {
                services.AddSingleton(cmd);
            }
            // IEnumerable<ICommand> is resolved automatically from the individual registrations.
            IServiceProvider sp = services.BuildServiceProvider();

            return new RootCommandService(
                new NullLogger<RootCommandService>(),
                lifetime,
                rootCommand,
                new FakeServiceScopeFactory(sp));
        }
    }

    // ─── NoCommandsConfiguredException Tests ─────────────────────────────────────

    [TestFixture]
    public class NoCommandsConfiguredExceptionTests
    {
        [Test]
        public void NoCommandsConfiguredException_HasMessage()
        {
            var ex = new NoCommandsConfiguredException();
            Assert.That(ex.Message, Is.Not.Empty);
        }

        [Test]
        public void NoCommandsConfiguredException_MessageMentionsICommand()
        {
            var ex = new NoCommandsConfiguredException();
            Assert.That(ex.Message, Does.Contain("ICommand"));
        }

        [Test]
        public void NoCommandsConfiguredException_IsDiamondCommandLineException()
        {
            var ex = new NoCommandsConfiguredException();
            Assert.That(ex, Is.InstanceOf<DiamondCommandLineException>());
        }

        [Test]
        public void DiamondCommandLineException_IsException()
        {
            var ex = new NoCommandsConfiguredException();
            Assert.That(ex, Is.InstanceOf<Exception>());
        }
    }

    // ─── RootCommandService.StartAsync Tests ─────────────────────────────────────

    [TestFixture]
    public class RootCommandServiceStartAsyncTests
    {
        [Test]
        public async Task StartAsync_OneCommand_AddsItToRootCommand()
        {
            var root = new TestRootCommand(Array.Empty<string>());
            var lifetime = new FakeHostApplicationLifetime();
            var sub = new TestSubCommand("create");

            RootCommandService svc = ServiceBuilder.Build(root, lifetime, sub);
            await svc.StartAsync(CancellationToken.None);

            Assert.That(root.Subcommands.Count, Is.EqualTo(1));
            Assert.That(root.Subcommands[0].Name, Is.EqualTo("create"));
        }

        [Test]
        public async Task StartAsync_MultipleCommands_AddsAllToRootCommand()
        {
            var root = new TestRootCommand(Array.Empty<string>());
            var lifetime = new FakeHostApplicationLifetime();
            var sub1 = new TestSubCommand("create");
            var sub2 = new TestSubCommand("list");
            var sub3 = new TestSubCommand("delete");

            RootCommandService svc = ServiceBuilder.Build(root, lifetime, sub1, sub2, sub3);
            await svc.StartAsync(CancellationToken.None);

            Assert.That(root.Subcommands.Count, Is.EqualTo(3));
        }

        [Test]
        public void StartAsync_NoCommandsRegistered_ThrowsNoCommandsConfiguredException()
        {
            var root = new TestRootCommand(Array.Empty<string>());
            var lifetime = new FakeHostApplicationLifetime();

            RootCommandService svc = ServiceBuilder.Build(root, lifetime /* no commands */);

            Assert.ThrowsAsync<NoCommandsConfiguredException>(
                () => svc.StartAsync(CancellationToken.None));
        }

        [Test]
        public async Task StartAsync_RegistersApplicationLifetimeCallbacks()
        {
            // CancellationToken.None.Register(...) is a no-op but must not throw.
            var root = new TestRootCommand(Array.Empty<string>());
            var lifetime = new FakeHostApplicationLifetime();
            var sub = new TestSubCommand("ping");

            RootCommandService svc = ServiceBuilder.Build(root, lifetime, sub);

            Assert.DoesNotThrowAsync(() => svc.StartAsync(CancellationToken.None));
        }

        [Test]
        public async Task StartAsync_ReturnsCompletedTask()
        {
            var root = new TestRootCommand(Array.Empty<string>());
            var lifetime = new FakeHostApplicationLifetime();
            var sub = new TestSubCommand("go");

            RootCommandService svc = ServiceBuilder.Build(root, lifetime, sub);
            Task result = svc.StartAsync(CancellationToken.None);

            Assert.That(result.IsCompleted, Is.True);
        }
    }

    // ─── RootCommandService.StopAsync Tests ──────────────────────────────────────

    [TestFixture]
    public class RootCommandServiceStopAsyncTests
    {
        [Test]
        public async Task StopAsync_ReturnsCompletedTask()
        {
            var root = new TestRootCommand(Array.Empty<string>());
            var lifetime = new FakeHostApplicationLifetime();
            var sub = new TestSubCommand("go");

            RootCommandService svc = ServiceBuilder.Build(root, lifetime, sub);
            await svc.StartAsync(CancellationToken.None);

            Task result = svc.StopAsync(CancellationToken.None);
            Assert.That(result.IsCompleted, Is.True);
        }
    }

    // ─── End-to-end: CommandLineParser invocation through RootCommandService ──────

    [TestFixture]
    public class RootCommandServiceInvocationTests
    {
        /// <summary>
        /// A sub-command that records whether it was invoked and sets an exit code.
        /// Uses SetAction so CommandLineParser.Parse(...).InvokeAsync() will call it.
        /// </summary>
        private class TrackingSubCommand : Command, ICommand
        {
            public bool WasInvoked { get; private set; }

            public TrackingSubCommand() : base("track", "Track command.")
            {
                this.SetAction(_ =>
                {
                    WasInvoked = true;
                    return 0;
                });
            }
        }

        [Test]
        public async Task InvokeAsync_AfterStartAsync_RunsSubCommand()
        {
            var sub = new TrackingSubCommand();
            var root = new TestRootCommand(new[] { "track" });
            var lifetime = new FakeHostApplicationLifetime();

            var services = new ServiceCollection();
            services.AddSingleton<ICommand>(sub);
            IServiceProvider sp = services.BuildServiceProvider();

            var svc = new RootCommandService(
                new NullLogger<RootCommandService>(),
                lifetime,
                root,
                new FakeServiceScopeFactory(sp));

            await svc.StartAsync(CancellationToken.None);

            // Directly invoke the same way OnStarted does inside RootCommandService.
            int result = await System.CommandLine.Parsing.CommandLineParser
                .Parse((RootCommand)root, root.Args)
                .InvokeAsync();

            Assert.That(sub.WasInvoked, Is.True);
            Assert.That(result, Is.EqualTo(0));
        }
    }
}
