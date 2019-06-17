using System;

namespace Diamond.Patterns.Abstractions
{
	public interface IExceptionContext : IContext
	{
		void SetException(Exception ex);
		void SetException(string message);
		void SetException(string format, params object[] args);
		Exception Exception { get; }
	}
}
