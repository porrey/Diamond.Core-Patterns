using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface IWorkFlowItem
	{
		/// <summary>
		/// Specifies the order in which the specified step is executed in a given
		/// work flow.
		/// </summary>
		int Ordinal { get; set; }

		/// <summary>
		/// A unique name for the specified step usually used for display or logging
		/// purposes.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// A unique name that groups one or more steps together for a given
		/// work flow.
		/// </summary>
		string Group { get; }

		/// <summary>
		/// Defines the weight applied to this step when showing progress. The default
		/// is 1 which makes it "evenly" weighted. A smaller value indicates this
		/// step should have less of an impact on the progress *takes less time to complete)
		/// while larger numbers have more of an impact (they take more time to complete).
		/// These numbers are totally arbitrary and are evaluated in relation to all
		/// other weights in the work flow.
		/// </summary>
		double Weight { get; }

		/// <summary>
		/// Indicates that regardless of the result of previous steps, this step
		/// should always execute. This is used in linear work flow managers that
		/// stop executing when one of the steps fail. A step marked with this
		/// attribute usually a "clean-up" step that must execute every time. This
		///  property may be ignored by certain work flow managers.
		/// </summary>
		bool AlwaysExecute { get; }
	}

	public interface IWorkFlowItem<TContextDecorator, TContext> : IWorkFlowItem
		where TContext : IContext
		where TContextDecorator : IContextDecorator<TContext>
	{
		/// <summary>
		/// Performs the work for the specified step.
		/// </summary>
		Task<bool> ExecuteStepAsync(TContextDecorator context);

		/// <summary>
		/// Indicates whether or not a step should be executed during a work flow.
		/// his is used by conditional work flow managers where every step in the
		/// work flow is executed from start to finish unless tis property returns
		/// false. This property may be ignored by certain work flow managers.
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		bool ShouldExecute(TContextDecorator context);
	}
}
