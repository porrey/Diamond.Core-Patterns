// ***
// *** Copyright(C) 2019-2020, Daniel M. Porrey. All rights reserved.
// *** 
// *** This program is free software: you can redistribute it and/or modify
// *** it under the terms of the GNU Lesser General Public License as published
// *** by the Free Software Foundation, either version 3 of the License, or
// *** (at your option) any later version.
// *** 
// *** This program is distributed in the hope that it will be useful,
// *** but WITHOUT ANY WARRANTY; without even the implied warranty of
// *** MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// *** GNU Lesser General Public License for more details.
// *** 
// *** You should have received a copy of the GNU Lesser General Public License
// *** along with this program. If not, see http://www.gnu.org/licenses/.
// *** 
using System;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Context
{
	public static class ExceptionContextDecorator
	{
		public static class WellKnown
		{
			public static class Context
			{
				public const string Exception = "Exception";
				public const string ExitCode = "ExitCode";
			}
		}

		public static bool HasException<TContext>(this IContextDecorator<TContext> contextDecorator)
			where TContext : IContext
		{
			return contextDecorator.Properties.ContainsKey(WellKnown.Context.Exception);
		}

		public static bool HasExitCode<TContext>(this IContextDecorator<TContext> contextDecorator)
			where TContext : IContext
		{
			return contextDecorator.Properties.ContainsKey(WellKnown.Context.ExitCode);
		}

		public static Exception GetException<TContext>(this IContextDecorator<TContext> contextDecorator)
			where TContext : IContext
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

		public static int GetExitCode<TContext>(this IContextDecorator<TContext> contextDecorator)
			where TContext : IContext
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

		public static void SetException<TContext>(this IContextDecorator<TContext> contextDecorator, Exception ex)
			where TContext : IContext
		{
			contextDecorator.Properties.Set(WellKnown.Context.Exception, ex);
		}

		public static void SetException<TContext>(this IContextDecorator<TContext> contextDecorator, int exitCode, Exception ex)
			where TContext : IContext
		{
			contextDecorator.Properties.Set(WellKnown.Context.Exception, ex);
			contextDecorator.Properties.Set(WellKnown.Context.ExitCode, exitCode);
		}

		public static void SetException<TContext>(this IContextDecorator<TContext> contextDecorator, string message)
			where TContext : IContext
		{
			contextDecorator.SetException(new Exception(message));
		}

		public static void SetException<TContext>(this IContextDecorator<TContext> contextDecorator, int exitCode, string message)
			where TContext : IContext
		{
			contextDecorator.SetException(new Exception(message));
			contextDecorator.Properties.Set(WellKnown.Context.ExitCode, exitCode);
		}

		public static void SetException<TContext>(this IContextDecorator<TContext> contextDecorator, string format, params object[] args)
			where TContext : IContext
		{
			contextDecorator.SetException(new Exception(String.Format(format, args)));
		}

		public static void SetException<TContext>(this IContextDecorator<TContext> contextDecorator, int exitCode, string format, params object[] args)
			where TContext : IContext
		{
			contextDecorator.SetException(new Exception(String.Format(format, args)));
			contextDecorator.Properties.Set(WellKnown.Context.ExitCode, exitCode);
		}
	}
}
