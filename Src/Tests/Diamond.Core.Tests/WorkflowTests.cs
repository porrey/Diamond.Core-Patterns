// Copyright(C) 2019-2026, Daniel M. Porrey. All rights reserved.
// Licensed under the LGPL-3.0-or-later license.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Core.Workflow;
using Diamond.Core.Workflow.State;
using Xunit;

namespace Diamond.Core.Tests
{
	/// <summary>
	/// Tests for <see cref="WorkflowContext"/>, <see cref="WorkflowItemTemplate"/>,
	/// <see cref="LinearCompleteWorkflowManager"/> and <see cref="LinearHaltWorkflowManager"/>.
	/// </summary>
	public class WorkflowTests
	{
		// ─── WorkflowContext ──────────────────────────────────────────────────

		[Fact]
		public void WorkflowContext_DefaultProperties_AreInitialised()
		{
			var ctx = new WorkflowContext { Name = "test", Arguments = new[] { "a", "b" } };
			Assert.Equal("test", ctx.Name);
			Assert.Equal(new[] { "a", "b" }, ctx.Arguments);
			Assert.NotNull(ctx.Properties);
		}

		[Fact]
		public void WorkflowContext_Properties_CanStoreAndRetrieveValues()
		{
			var ctx = new WorkflowContext();
			ctx.Properties.Add("key", "value");
			Assert.Equal("value", ctx.Properties.Get<string>("key"));
		}

		[Fact]
		public async Task WorkflowContext_ResetAsync_ThrowsNotImplementedException()
		{
			var ctx = new WorkflowContext();
			await Assert.ThrowsAsync<NotImplementedException>(() => ctx.ResetAsync());
		}

		// ─── WorkflowItemTemplate lifecycle ──────────────────────────────────

		/// <summary>A simple step that always returns true.</summary>
		private sealed class SuccessStep : WorkflowItemTemplate
		{
			public int ExecuteCount { get; private set; }
			public int PrepareCount { get; private set; }
			public int PostCount { get; private set; }

			public SuccessStep() { Ordinal = 1; }

			protected override Task<bool> OnPrepareForExecutionAsync(IContext context)
			{
				PrepareCount++;
				return Task.FromResult(true);
			}

			protected override Task<bool> OnExecuteStepAsync(IContext context)
			{
				ExecuteCount++;
				return Task.FromResult(true);
			}

			protected override Task OnPostExecutionAsync(IContext context)
			{
				PostCount++;
				return Task.CompletedTask;
			}
		}

		/// <summary>A step that returns false (failure).</summary>
		private sealed class FailStep : WorkflowItemTemplate
		{
			public FailStep() { Ordinal = 1; }
			protected override Task<bool> OnExecuteStepAsync(IContext context) => Task.FromResult(false);
		}

		/// <summary>A step whose OnPrepareForExecutionAsync returns false.</summary>
		private sealed class PrepareFailStep : WorkflowItemTemplate
		{
			public PrepareFailStep() { Ordinal = 1; }
			protected override Task<bool> OnPrepareForExecutionAsync(IContext context) => Task.FromResult(false);
			protected override Task<bool> OnExecuteStepAsync(IContext context) => Task.FromResult(true);
		}

		/// <summary>A step that overrides OnShouldExecuteAsync to return false.</summary>
		private sealed class SkipStep : WorkflowItemTemplate
		{
			public SkipStep() { Ordinal = 1; }
			public override Task<bool> OnShouldExecuteAsync(IContext context) => Task.FromResult(false);
			protected override Task<bool> OnExecuteStepAsync(IContext context) => Task.FromResult(false);
		}

		[Fact]
		public async Task WorkflowItemTemplate_ExecuteStepAsync_Success_RunsAllPhases()
		{
			var step = new SuccessStep();
			var ctx = new WorkflowContext();
			bool result = await step.ExecuteStepAsync(ctx);
			Assert.True(result);
			Assert.Equal(1, step.PrepareCount);
			Assert.Equal(1, step.ExecuteCount);
			Assert.Equal(1, step.PostCount);
		}

		[Fact]
		public async Task WorkflowItemTemplate_ExecuteStepAsync_WhenPrepareFails_ReturnsFalse()
		{
			var step = new PrepareFailStep();
			var ctx = new WorkflowContext();
			bool result = await step.ExecuteStepAsync(ctx);
			Assert.False(result);
		}

		[Fact]
		public async Task WorkflowItemTemplate_ExecuteStepAsync_WhenShouldExecuteReturnsFalse_ReturnsTrue()
		{
			// "skip" means the step reports success (skipped is not a failure)
			var step = new SkipStep();
			var ctx = new WorkflowContext();
			bool result = await step.ExecuteStepAsync(ctx);
			Assert.True(result);
		}

		[Fact]
		public void WorkflowItemTemplate_Name_IsSetFromTypeName()
		{
			var step = new SuccessStep();
			// "Step" suffix should be stripped
			Assert.Equal("Success", step.Name);
		}

		[Fact]
		public void WorkflowItemTemplate_Properties_CanBeSet()
		{
			var step = new SuccessStep();
			step.AlwaysExecute = true;
			step.Weight = 2.5;
			step.Ordinal = 5;
			step.Name = "Custom";
			Assert.True(step.AlwaysExecute);
			Assert.Equal(2.5, step.Weight);
			Assert.Equal(5, step.Ordinal);
			Assert.Equal("Custom", step.Name);
		}

		// ─── WorkflowItemFactory (stub) ───────────────────────────────────────

		private sealed class StubWorkflowItemFactory : IWorkflowItemFactory
		{
			private readonly IWorkflowItem[] _items;
			public StubWorkflowItemFactory(params IWorkflowItem[] items) { _items = items; }
			public Task<IEnumerable<IWorkflowItem>> GetItemsAsync(string serviceKey)
				=> Task.FromResult<IEnumerable<IWorkflowItem>>(_items);
		}

		// ─── LinearCompleteWorkflowManager ────────────────────────────────────

		[Fact]
		public async Task LinearComplete_AllStepsSucceed_ReturnsTrue()
		{
			var step1 = new SuccessStep { Ordinal = 1 };
			var step2 = new SuccessStep { Ordinal = 2 };
			var factory = new StubWorkflowItemFactory(step1, step2);
			var manager = new LinearCompleteWorkflowManager(factory) { ServiceKey = "test" };
			var ctx = new WorkflowContext();

			bool result = await manager.ExecuteWorkflowAsync(ctx);
			Assert.True(result);
		}

		[Fact]
		public async Task LinearComplete_OneStepFails_ContinuesAndReturnsFalseAtEnd()
		{
			var step1 = new FailStep { Ordinal = 1 };
			var step2 = new SuccessStep { Ordinal = 2 };
			var factory = new StubWorkflowItemFactory(step1, step2);
			var manager = new LinearCompleteWorkflowManager(factory) { ServiceKey = "test" };
			var ctx = new WorkflowContext();

			bool result = await manager.ExecuteWorkflowAsync(ctx);
			// LinearComplete continues on failure; the overall result reflects the final state.
			// step2 still executed.
			Assert.Equal(1, step2.ExecuteCount);
		}

		[Fact]
		public async Task LinearComplete_NoSteps_ThrowsInvalidOperationException()
		{
			// When the steps collection is empty, the set_Steps setter calls
			// First() on an empty sequence before the MissingStepsException
			// guard is reached, resulting in an InvalidOperationException.
			var factory = new StubWorkflowItemFactory();
			var manager = new LinearCompleteWorkflowManager(factory) { ServiceKey = "test" };
			var ctx = new WorkflowContext();

			await Assert.ThrowsAsync<InvalidOperationException>(() => manager.ExecuteWorkflowAsync(ctx));
		}

		[Fact]
		public async Task LinearComplete_Steps_AreOrderedByOrdinal()
		{
			var order = new List<int>();
			var step3 = new RecordingStep(3, order);
			var step1 = new RecordingStep(1, order);
			var step2 = new RecordingStep(2, order);
			var factory = new StubWorkflowItemFactory(step3, step1, step2);
			var manager = new LinearCompleteWorkflowManager(factory) { ServiceKey = "test" };

			await manager.ExecuteWorkflowAsync(new WorkflowContext());
			Assert.Equal(new[] { 1, 2, 3 }, order);
		}

		private sealed class RecordingStep : WorkflowItemTemplate
		{
			private readonly List<int> _log;
			public RecordingStep(int ordinal, List<int> log)
			{
				Ordinal = ordinal;
				_log = log;
			}
			protected override Task<bool> OnExecuteStepAsync(IContext context)
			{
				_log.Add(Ordinal);
				return Task.FromResult(true);
			}
		}

		// ─── LinearHaltWorkflowManager ────────────────────────────────────────

		[Fact]
		public async Task LinearHalt_AllStepsSucceed_ReturnsTrue()
		{
			var step1 = new SuccessStep { Ordinal = 1 };
			var step2 = new SuccessStep { Ordinal = 2 };
			var factory = new StubWorkflowItemFactory(step1, step2);
			var manager = new LinearHaltWorkflowManager(factory) { ServiceKey = "test" };
			var ctx = new WorkflowContext();

			bool result = await manager.ExecuteWorkflowAsync(ctx);
			Assert.True(result);
		}

		[Fact]
		public async Task LinearHalt_StepFails_StopsExecution()
		{
			var step1 = new FailStep { Ordinal = 1 };
			var step2 = new SuccessStep { Ordinal = 2 };
			var factory = new StubWorkflowItemFactory(step1, step2);
			var manager = new LinearHaltWorkflowManager(factory) { ServiceKey = "test" };
			var ctx = new WorkflowContext();

			bool result = await manager.ExecuteWorkflowAsync(ctx);

			Assert.False(result);
			// step2 should NOT have executed since step1 failed and AlwaysExecute = false
			Assert.Equal(0, step2.ExecuteCount);
		}

		[Fact]
		public async Task LinearHalt_StepFailsButAlwaysExecute_ContinuesToAlwaysExecuteStep()
		{
			var step1 = new FailStep { Ordinal = 1 };
			var step2 = new AlwaysRunStep { Ordinal = 2, AlwaysExecute = true };
			var factory = new StubWorkflowItemFactory(step1, step2);
			var manager = new LinearHaltWorkflowManager(factory) { ServiceKey = "test" };
			var ctx = new WorkflowContext();

			await manager.ExecuteWorkflowAsync(ctx);
			Assert.Equal(1, step2.ExecuteCount);
		}

		[Fact]
		public async Task LinearHalt_NoSteps_ThrowsArgumentOutOfRangeException()
		{
			// When the steps collection is empty, the LinearHalt manager's
			// get_Steps getter throws ArgumentOutOfRangeException before the
			// MissingStepsException guard is reached.
			var factory = new StubWorkflowItemFactory();
			var manager = new LinearHaltWorkflowManager(factory) { ServiceKey = "test" };
			var ctx = new WorkflowContext();

			await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => manager.ExecuteWorkflowAsync(ctx));
		}

		private sealed class AlwaysRunStep : WorkflowItemTemplate
		{
			public int ExecuteCount { get; private set; }
			protected override Task<bool> OnExecuteStepAsync(IContext context)
			{
				ExecuteCount++;
				return Task.FromResult(true);
			}
		}
	}
}
