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

namespace Diamond.Core.Workflow
{
	/// <summary>
	/// 
	/// </summary>
	public static class ExceptionContextDecorator
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
				public const string Exception = "Exception";

				/// <summary>
				/// 
				/// </summary>
				public const string ExitCode = "ExitCode";
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
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
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static bool HasException(this IContext context)
		{
			return context.Properties.ContainsKey(WellKnown.Context.Exception);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static bool HasExitCode(this IContext context)
		{
			return context.Properties.ContainsKey(WellKnown.Context.ExitCode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
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
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
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
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="ex"></param>
		public static void SetException(this IContext context, Exception ex)
		{
			context.Properties.Set(WellKnown.Context.Exception, ex);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="exitCode"></param>
		/// <param name="ex"></param>
		public static void SetException(this IContext context, int exitCode, Exception ex)
		{
			context.Properties.Set(WellKnown.Context.Exception, ex);
			context.Properties.Set(WellKnown.Context.ExitCode, exitCode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="message"></param>
		public static void SetException(this IContext context, string message)
		{
			context.SetException(new Exception(message));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="exitCode"></param>
		/// <param name="message"></param>
		public static void SetException(this IContext context, int exitCode, string message)
		{
			context.SetException(new Exception(message));
			context.Properties.Set(WellKnown.Context.ExitCode, exitCode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void SetException(this IContext context, string format, params object[] args)
		{
			context.SetException(new Exception(String.Format(format, args)));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="exitCode"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void SetException(this IContext context, int exitCode, string format, params object[] args)
		{
			context.SetException(new Exception(String.Format(format, args)));
			context.Properties.Set(WellKnown.Context.ExitCode, exitCode);
		}
	}
}
