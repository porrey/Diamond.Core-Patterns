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
namespace Diamond.Core.Workflow
{
	/// <summary>
	/// Provides well-known constants and workflow context keys used throughout the Diamond workflow system.
	/// </summary>
	/// <remarks>This class contains nested static classes and string constants that represent standard context keys
	/// and identifiers for workflow operations. Use these constants to ensure consistency when accessing or setting
	/// workflow-related context values.</remarks>
	public static class DiamondWorkflow
	{
		/// <summary>
		/// Provides well-known constants for use in workflow context and state management scenarios.
		/// </summary>
		/// <remarks>The constants defined in this class represent standard keys commonly used to store and retrieve
		/// workflow-related information, such as error states, success indicators, and temporary data. These values help
		/// ensure consistency when interacting with workflow context dictionaries or state objects across different
		/// components.</remarks>
		public static class WellKnown
		{
			/// <summary>
			/// Provides well-known keys for storing and retrieving workflow-related context information in a state dictionary or
			/// similar data structure.
			/// </summary>
			/// <remarks>Use these constants to ensure consistent access to workflow state, error details, and temporary
			/// resources across workflow steps. The keys are intended for use in scenarios where workflow execution state, error
			/// handling, and temporary data management are required.</remarks>
			public static class Context
			{
				/// <summary>
				/// Represents the key used to indicate whether the last step was successful.
				/// </summary>
				public const string LastStepSuccess = "LastStepSuccess";

				/// <summary>
				/// Represents the event name used to indicate a workflow error.
				/// </summary>
				public const string WorkflowError = "WorkflowError";

				/// <summary>
				/// Represents the string value used to identify exception-related log entries or categories.
				/// </summary>
				public const string Exception = "Exception";

				/// <summary>
				/// Represents the key used to identify workflow error messages in configuration or data stores.
				/// </summary>
				public const string WorkflowErrorMessage = "WorkflowErrorMessage";

				/// <summary>
				/// Represents the event name used to indicate that a workflow has failed.
				/// </summary>
				public const string WorkflowFailed = "WorkflowFailed";

				/// <summary>
				/// Represents the name of the temporary folder used for storing transient files or data.
				/// </summary>
				public const string TemporaryFolder = "TemporaryFolder";

				/// <summary>
				/// Represents the name of the array type for state dictionaries used in serialization or type identification
				/// scenarios.
				/// </summary>
				public const string IStateDictionaryArray = "IStateDictionaryArray";
			}
		}
	}
}
