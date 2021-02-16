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
namespace Diamond.Core.Workflow
{
	/// <summary>
	/// 
	/// </summary>
	public static class DiamondWorkflow
	{
		/// <summary>
		/// 
		/// </summary>
		public static class WellKnown
		{
			/// <summary>
			/// 
			/// </summary>
			public static class Context
			{
				/// <summary>
				/// 
				/// </summary>
				public const string LastStepSuccess = "LastStepSuccess";

				/// <summary>
				/// 
				/// </summary>
				public const string WorkflowError = "WorkflowError";

				/// <summary>
				/// 
				/// </summary>
				public const string WorkflowErrorMessage = "WorkflowErrorMessage";

				/// <summary>
				/// 
				/// </summary>
				public const string WorkflowFailed = "WorkflowFailed";

				/// <summary>
				/// 
				/// </summary>
				public const string TemporaryFolder = "TemporaryFolder";

				/// <summary>
				/// 
				/// </summary>
				public const string IStateDictionaryArray = "IStateDictionaryArray";
			}
		}
	}
}
