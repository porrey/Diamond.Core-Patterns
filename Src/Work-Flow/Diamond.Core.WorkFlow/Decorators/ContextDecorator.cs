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

namespace Diamond.Core.WorkFlow
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
		/// <param name="contextDecorator"></param>
		/// <returns></returns>
		public static bool HasException(this IContext contextDecorator)
		{
			return contextDecorator.Properties.ContainsKey(WellKnown.Context.Exception);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="contextDecorator"></param>
		/// <returns></returns>
		public static bool HasExitCode(this IContext contextDecorator)
		{
			return contextDecorator.Properties.ContainsKey(WellKnown.Context.ExitCode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="contextDecorator"></param>
		/// <returns></returns>
		public static Exception GetException(this IContext contextDecorator)
		{
			Exception returnValue = null;

			if (contextDecorator.HasException())
			{
				returnValue = contextDecorator.Properties.Get<Exception>(WellKnown.Context.Exception);
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
		/// <param name="contextDecorator"></param>
		/// <returns></returns>
		public static int GetExitCode(this IContext contextDecorator)
		{
			int returnValue = 0;

			if (contextDecorator.HasExitCode())
			{
				returnValue = contextDecorator.Properties.Get<int>(WellKnown.Context.ExitCode);
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
		/// <param name="contextDecorator"></param>
		/// <param name="ex"></param>
		public static void SetException(this IContext contextDecorator, Exception ex)
		{
			contextDecorator.Properties.Set(WellKnown.Context.Exception, ex);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="contextDecorator"></param>
		/// <param name="exitCode"></param>
		/// <param name="ex"></param>
		public static void SetException(this IContext contextDecorator, int exitCode, Exception ex)
		{
			contextDecorator.Properties.Set(WellKnown.Context.Exception, ex);
			contextDecorator.Properties.Set(WellKnown.Context.ExitCode, exitCode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="contextDecorator"></param>
		/// <param name="message"></param>
		public static void SetException(this IContext contextDecorator, string message)
		{
			contextDecorator.SetException(new Exception(message));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="contextDecorator"></param>
		/// <param name="exitCode"></param>
		/// <param name="message"></param>
		public static void SetException(this IContext contextDecorator, int exitCode, string message)
		{
			contextDecorator.SetException(new Exception(message));
			contextDecorator.Properties.Set(WellKnown.Context.ExitCode, exitCode);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="contextDecorator"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void SetException(this IContext contextDecorator, string format, params object[] args)
		{
			contextDecorator.SetException(new Exception(String.Format(format, args)));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="contextDecorator"></param>
		/// <param name="exitCode"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void SetException(this IContext contextDecorator, int exitCode, string format, params object[] args)
		{
			contextDecorator.SetException(new Exception(String.Format(format, args)));
			contextDecorator.Properties.Set(WellKnown.Context.ExitCode, exitCode);
		}
	}
}
