//
// Copyright(C) 2019-2026, Daniel M. Porrey. All rights reserved.
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
namespace Diamond.Core.Performance
{
	/// <summary>
	/// Defines a contract for measuring the execution of an action and returning its result.
	/// </summary>
	/// <remarks>Implementations of this interface can be used to record metrics, such as execution time or resource
	/// usage, for the specified action. The measurement details and how they are recorded depend on the
	/// implementation.</remarks>
	public interface IMeasureAction
	{
		/// <summary>
		/// Executes the specified action and measures its execution time, returning the result produced by the action.
		/// </summary>
		/// <remarks>Use this method to measure the duration of an operation while capturing its result. The action is
		/// executed synchronously. If the action throws an exception, the exception is propagated to the caller and no result
		/// is returned.</remarks>
		/// <typeparam name="T">The type of the value returned by the measured action.</typeparam>
		/// <param name="action">A delegate representing the action to execute and measure. Cannot be null.</param>
		/// <param name="actionName">An optional name used to identify the measured action in logs or reports. Defaults to "measured".</param>
		/// <returns>The value returned by the executed action.</returns>
		T Measure<T>(MeasurableActionDelegate<T> action, string actionName = "measured");
	}
}