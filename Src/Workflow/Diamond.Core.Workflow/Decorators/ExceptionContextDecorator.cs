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
using System;

namespace Diamond.Core.Workflow
{
	/// <summary>
	/// Provides extension methods for managing exception and exit code information within a workflow context. Enables
	/// setting, retrieving, and checking for exceptions and exit codes using well-known context property keys.
	/// </summary>
	/// <remarks>Use the methods in this class to record and access exception details and exit codes in a workflow's
	/// context. This facilitates consistent error handling and reporting across workflow steps. The well-known property
	/// keys defined in the nested classes ensure interoperability and standardization when storing exception-related
	/// information.</remarks>
	public static class ExceptionContextDecorator
	{
		/// <summary>
		/// Provides well-known constants for use in application context and metadata keys.
		/// </summary>
		/// <remarks>The constants defined in this class are intended to standardize context keys across the
		/// application, reducing the risk of errors from hard-coded strings. Use these values when storing or retrieving
		/// context information such as exception details or process exit codes.</remarks>
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
				public const string Exception = "Exception";

				/// <summary>
				/// 
				/// </summary>
				public const string ExitCode = "ExitCode";
			}
		}

		/// <summary>
		/// Retrieves the failure message from the context, providing details about the most deeply nested exception if
		/// present.
		/// </summary>
		/// <remarks>Use this method to obtain a user-friendly error message when handling failures in operations that
		/// utilize the context. If multiple nested exceptions are present, only the innermost exception's message is
		/// returned.</remarks>
		/// <param name="context">The context from which to extract the failure message. Must not be null and should provide exception information
		/// via HasException and GetException methods.</param>
		/// <returns>A string containing the message of the innermost exception if an exception exists in the context; otherwise,
		/// "Unknown failure.".</returns>
		public static string FailureMessage(this IContext context)
		{
			string returnValue = String.Empty;

			if (context.HasException())
			{
				Exception ex = context.GetException();

				while (ex.InnerException != null)
				{
					ex = ex.InnerException;
				}

				returnValue = ex.Message;
			}
			else
			{
				returnValue = "Unknown failure.";
			}

			return returnValue;
		}

		/// <summary>
		/// Determines whether the context contains an associated exception.
		/// </summary>
		/// <remarks>Use this method to verify if an exception has been recorded in the context before attempting to
		/// retrieve or handle it.</remarks>
		/// <param name="context">The context to check for an exception. Cannot be null.</param>
		/// <returns>true if the context contains an exception; otherwise, false.</returns>
		public static bool HasException(this IContext context)
		{
			return context.Properties.ContainsKey(WellKnown.Context.Exception);
		}

		/// <summary>
		/// Determines whether the context contains an exit code property.
		/// </summary>
		/// <param name="context">The context to check for the presence of an exit code property. Cannot be null.</param>
		/// <returns>true if the context contains an exit code property; otherwise, false.</returns>
		public static bool HasExitCode(this IContext context)
		{
			return context.Properties.ContainsKey(WellKnown.Context.ExitCode);
		}

		/// <summary>
		/// Retrieves the exception associated with the specified context.
		/// </summary>
		/// <param name="context">The context from which to obtain the exception. Must not be null and must contain an exception.</param>
		/// <returns>The exception stored in the context.</returns>
		/// <exception cref="NoExceptionException">Thrown if the context does not contain an exception.</exception>
		public static Exception GetException(this IContext context)
		{
			Exception returnValue = null;

			if (context.HasException())
			{
				returnValue = context.Properties.Get<Exception>(WellKnown.Context.Exception);
			}
			else
			{
				throw new NoExceptionException();
			}

			return returnValue;
		}

		/// <summary>
		/// Retrieves the exit code from the specified context.
		/// </summary>
		/// <param name="context">The context from which to obtain the exit code. Must support exit code retrieval.</param>
		/// <returns>The exit code value associated with the context.</returns>
		/// <exception cref="NoExitCodeException">Thrown if the context does not contain an exit code.</exception>
		public static int GetExitCode(this IContext context)
		{
			int returnValue = 0;

			if (context.HasExitCode())
			{
				returnValue = context.Properties.Get<int>(WellKnown.Context.ExitCode);
			}
			else
			{
				throw new NoExitCodeException();
			}

			return returnValue;
		}

		/// <summary>
		/// Associates the specified exception with the given context instance.
		/// </summary>
		/// <remarks>Use this method to record an exception in the context for later retrieval or handling. This is
		/// useful for propagating error information through context-aware operations.</remarks>
		/// <param name="context">The context in which to store the exception. Cannot be null.</param>
		/// <param name="ex">The exception to associate with the context. Cannot be null.</param>
		public static void SetException(this IContext context, Exception ex)
		{
			context.Properties.Set(WellKnown.Context.Exception, ex);
		}

		/// <summary>
		/// Sets the exception and exit code properties in the specified context to indicate an error state.
		/// </summary>
		/// <remarks>This method updates the context's properties to reflect an error, allowing downstream consumers
		/// to access the exception and exit code. Use this method to signal failure or error conditions within the
		/// context.</remarks>
		/// <param name="context">The context in which to set the exception and exit code properties. Cannot be null.</param>
		/// <param name="exitCode">The exit code representing the error condition to associate with the context.</param>
		/// <param name="ex">The exception to associate with the context. Cannot be null.</param>
		public static void SetException(this IContext context, int exitCode, Exception ex)
		{
			context.Properties.Set(WellKnown.Context.Exception, ex);
			context.Properties.Set(WellKnown.Context.ExitCode, exitCode);
		}

		/// <summary>
		/// Sets an exception on the specified context using the provided error message.
		/// </summary>
		/// <param name="context">The context in which to set the exception. Cannot be null.</param>
		/// <param name="message">The error message to associate with the exception. Cannot be null or empty.</param>
		public static void SetException(this IContext context, string message)
		{
			context.SetException(new Exception(message));
		}

		/// <summary>
		/// Sets an exception and exit code in the specified context, indicating an error condition with a custom message.
		/// </summary>
		/// <remarks>This method updates the context to reflect an error state, allowing downstream consumers to
		/// detect and handle failures based on the exception and exit code.</remarks>
		/// <param name="context">The context in which to record the exception and exit code. Cannot be null.</param>
		/// <param name="exitCode">The exit code to associate with the error. Typically used to indicate the reason for failure.</param>
		/// <param name="message">The error message to include in the exception. Cannot be null.</param>
		public static void SetException(this IContext context, int exitCode, string message)
		{
			context.SetException(new Exception(message));
			context.Properties.Set(WellKnown.Context.ExitCode, exitCode);
		}

		/// <summary>
		/// Sets an exception on the specified context using a formatted message.
		/// </summary>
		/// <remarks>The exception message is created using the specified format string and arguments. This method is
		/// useful for providing detailed error information in the context.</remarks>
		/// <param name="context">The context on which to set the exception. Cannot be null.</param>
		/// <param name="format">A composite format string that specifies the exception message.</param>
		/// <param name="args">An array of objects to format into the exception message.</param>
		public static void SetException(this IContext context, string format, params object[] args)
		{
			context.SetException(new Exception(String.Format(format, args)));
		}

		/// <summary>
		/// Sets an exception and exit code on the specified context using a formatted error message.
		/// </summary>
		/// <remarks>This method creates an exception with a formatted message and sets it on the context, along with
		/// the provided exit code. Use this to signal errors and communicate exit codes in a standardized way.</remarks>
		/// <param name="context">The context in which to set the exception and exit code. Cannot be null.</param>
		/// <param name="exitCode">The exit code to associate with the exception. Typically used to indicate the error type or severity.</param>
		/// <param name="format">A composite format string that defines the error message.</param>
		/// <param name="args">An array of objects to format into the error message string.</param>
		public static void SetException(this IContext context, int exitCode, string format, params object[] args)
		{
			context.SetException(new Exception(String.Format(format, args)));
			context.Properties.Set(WellKnown.Context.ExitCode, exitCode);
		}
	}
}
