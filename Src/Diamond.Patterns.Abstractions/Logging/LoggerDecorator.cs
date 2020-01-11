using System;

namespace Diamond.Patterns.Abstractions
{
	/// <summary>
	/// Provides logging methods for an object that implements ILoggerSource.
	/// </summary>
	public static class LoggerDecorator
	{
		/// <summary>
		/// Creates a log entry on the specified interface with the
		/// given <see cref="LoggingLevel"/> and message.
		/// </summary>
		/// <param name="loggerSubscriber">The source interface.</param>
		/// <param name="loggingLevel">The level of the log entry.</param>
		/// <param name="message">The log entry message.</param>
		public static void Log(this ILoggerSubscriber loggerSubscriber, LoggingLevel loggingLevel, string message)
		{
			loggerSubscriber?.Logger?.Invoke(loggingLevel, message);
		}

		/// <summary>
		/// Creates a log entry on the specified interface with the
		/// given <see cref="LoggingLevel"/> and formatted string and
		/// arguments.
		/// </summary>
		/// <param name="loggerSubscriber">The source interface.</param>
		/// <param name="loggingLevel">The level of the log entry.</param>
		/// <param name="format">A composite format string.</param>
		/// <param name="args">An object array that contains zero or more objects to format.</param>
		public static void Log(this ILoggerSubscriber loggerSubscriber, LoggingLevel loggingLevel, string format, params object[] args)
		{
			loggerSubscriber?.Logger?.Invoke(loggingLevel, String.Format(format, args));
		}

		/// <summary>
		/// Creates a log entry on the specified interface with the
		/// given <see cref="LoggingLevel"/> and exception. This method
		/// will create an entry for all inner exceptions as well.
		/// </summary>
		/// <param name="loggerSubscriber">The source interface.</param>
		/// <param name="exception">Represents the error that occurred during application execution.</param>
		public static void Log(this ILoggerSubscriber loggerSubscriber, Exception exception)
		{
			Exception ex = exception;

			while (ex != null)
			{
				loggerSubscriber?.Logger?.Invoke(LoggingLevel.Error, $"Exception: '{ex.Message}'");
				ex = ex.InnerException;
			}
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="loggerSubscriber"></param>
		/// <param name="exception"></param>
		public static void Exception(this ILoggerSubscriber loggerSubscriber, Exception exception)
		{
			loggerSubscriber.Log(exception);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="loggerSubscriber"></param>
		/// <param name="message"></param>
		public static void Information(this ILoggerSubscriber loggerSubscriber, string message)
		{
			loggerSubscriber.Log(LoggingLevel.Information, message);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="loggerSubscriber"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void Information(this ILoggerSubscriber loggerSubscriber, string format, params object[] args)
		{
			loggerSubscriber.Log(LoggingLevel.Information, format, args);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="loggerSubscriber"></param>
		/// <param name="message"></param>
		public static void Warning(this ILoggerSubscriber loggerSubscriber, string message)
		{
			loggerSubscriber.Log(LoggingLevel.Warning, message);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="loggerSubscriber"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void Warning(this ILoggerSubscriber loggerSubscriber, string format, params object[] args)
		{
			loggerSubscriber.Log(LoggingLevel.Warning, format, args);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="loggerSubscriber"></param>
		/// <param name="message"></param>
		public static void Error(this ILoggerSubscriber loggerSubscriber, string message)
		{
			loggerSubscriber.Log(LoggingLevel.Error, message);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="loggerSubscriber"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void Error(this ILoggerSubscriber loggerSubscriber, string format, params object[] args)
		{
			loggerSubscriber.Log(LoggingLevel.Error, format, args);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="loggerSubscriber"></param>
		/// <param name="message"></param>
		public static void Fatal(this ILoggerSubscriber loggerSubscriber, string message)
		{
			loggerSubscriber.Log(LoggingLevel.Fatal, message);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="loggerSubscriber"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void Fatal(this ILoggerSubscriber loggerSubscriber, string format, params object[] args)
		{
			loggerSubscriber.Log(LoggingLevel.Fatal, format, args);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="loggerSubscriber"></param>
		/// <param name="message"></param>
		public static void Debug(this ILoggerSubscriber loggerSubscriber, string message)
		{
			loggerSubscriber.Log(LoggingLevel.Debug, message);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="loggerSubscriber"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void Debug(this ILoggerSubscriber loggerSubscriber, string format, params object[] args)
		{
			loggerSubscriber.Log(LoggingLevel.Debug, format, args);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="loggerSubscriber"></param>
		/// <param name="message"></param>
		public static void Verbose(this ILoggerSubscriber loggerSubscriber, string message)
		{
			loggerSubscriber.Log(LoggingLevel.Verbose, message);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="loggerSubscriber"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void Verbose(this ILoggerSubscriber loggerSubscriber, string format, params object[] args)
		{
			loggerSubscriber.Log(LoggingLevel.Verbose, format, args);
		}
	}
}
