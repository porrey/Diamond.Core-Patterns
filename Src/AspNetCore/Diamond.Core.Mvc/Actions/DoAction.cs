using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.AspNet.DoAction {
	/// <summary>
	/// 
	/// </summary>
	public class DoAction<TInputs, TResult> : IDoAction<TInputs, TResult> {
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		public DoAction(ILogger<DoAction<TInputs, TResult>> logger) {
			this.Logger = logger;
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual ILogger<DoAction<TInputs, TResult>> Logger { get; set; }

		/// <summary>
		/// As a best practice, the name of this class should match the controller
		/// method name with the word "Action" appended to the end. The DoActionController
		/// uses [CallerMemberName] as the action key by default.
		/// </summary>
		public virtual string ActionKey => this.GetType().Name.Replace("Action", "");

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual Task<IControllerActionResult<TResult>> ExecuteActionAsync(TInputs item) {
			return this.OnExecuteActionAsync(item);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected virtual Task<IControllerActionResult<TResult>> OnExecuteActionAsync(TInputs item) {
			return Task.FromResult<IControllerActionResult<TResult>>(new ControllerActionResult<TResult>());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual Task<(bool, string)> ValidateModel(TInputs item) {
			return Task.FromResult<(bool, string)>((true, null));
		}
	}
}
