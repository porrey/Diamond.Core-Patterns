//
// Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see http://www.gnu.org/licenses/.
//
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.AspNetCore.DoAction
{
	/// <summary>
	/// 
	/// </summary>
	[Obsolete("Use DoActionTemplate instead.")]
	public class DoAction<TInputs, TResult> : DoActionTemplate<TInputs, TResult>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		public DoAction(ILogger<DoAction<TInputs, TResult>> logger)
			: base(logger)
		{
			this.Logger = logger;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class DoActionTemplate<TInputs, TResult> : IDoAction<TInputs, TResult>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		public DoActionTemplate(ILogger<DoActionTemplate<TInputs, TResult>> logger)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// 
		/// </summary>
		public DoActionTemplate()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual ILogger<DoActionTemplate<TInputs, TResult>> Logger { get; set; }

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
		public virtual Task<IControllerActionResult<TResult>> ExecuteActionAsync(TInputs item)
		{
			return this.OnExecuteActionAsync(item);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected virtual Task<IControllerActionResult<TResult>> OnExecuteActionAsync(TInputs item)
		{
			return Task.FromResult<IControllerActionResult<TResult>>(new ControllerActionResult<TResult>());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public virtual Task<(bool, string)> ValidateModel(TInputs item)
		{
			return Task.FromResult<(bool, string)>((true, null));
		}
	}
}
