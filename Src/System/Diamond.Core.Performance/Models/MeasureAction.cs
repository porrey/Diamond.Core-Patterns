//
// Copyright(C) 2019-2025, Daniel M. Porrey. All rights reserved.
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
using System.Diagnostics;
using Humanizer;
using Microsoft.Extensions.Logging;

/// <summary>
/// 
/// </summary>
namespace Diamond.Core.Performance
{
	/// <summary>
	/// Public delegate that defines an action with a return type.
	/// </summary>
	/// <typeparam name="T">Specifies the return type.</typeparam>
	/// <returns>Returns an instance of T.</returns>
	public delegate T MeasurableActionDelegate<T>();

	/// <summary>
	/// Allows any method or block of code to be measured for performance.
	/// </summary>
	public class MeasureAction : IMeasureAction
	{
		/// <summary>
		/// Creates an instance of <see cref="MeasureAction"/> with the specified logger./>
		/// </summary>
		/// <param name="logger"></param>
		public MeasureAction(ILogger<MeasureAction> logger)
		{
			this.Logger = logger;
		}

		/// <summary>
		/// Gets/sets the logger for this instance.
		/// </summary>
		protected ILogger<MeasureAction> Logger { get; set; }

		/// <summary>
		/// Measure the method or block of code.
		/// </summary>
		/// <typeparam name="T">The return type.</typeparam>
		/// <param name="action">The method or block of code to run.</param>
		/// <param name="actionName">The name of the action used in the log.</param>
		/// <returns>Returns an instance of T.</returns>
		public T Measure<T>(MeasurableActionDelegate<T> action, string actionName = "measured")
		{
			T returnValue = default;
			Stopwatch sw = new();

			try
			{
				sw.Start();
				returnValue = action.Invoke();
			}
			finally
			{
				sw.Stop();
				this.Logger.LogInformation("The '{actionName}' action completed in {time}.", actionName, sw.Elapsed.Humanize());
			}

			return returnValue;
		}

		/// <summary>
		/// Measure the method or block of code.
		/// </summary>
		/// <param name="action">The method or block of code to run.</param>
		/// <param name="actionName">The name of the action used in the log.</param>
		public void Measure<T>(Action action, string actionName = "measured")
		{
			Stopwatch sw = new();

			try
			{
				sw.Start();
				action.Invoke();
			}
			finally
			{
				sw.Stop();
				this.Logger.LogInformation("The '{actionName}' action completed in {time}.", actionName, sw.Elapsed.Humanize());
			}
		}
	}
}