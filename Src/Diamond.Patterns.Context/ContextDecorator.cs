using System;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Context
{
	public class ContextDecorator<T> : IContextDecorator<T> where T : IContext
	{
		public ContextDecorator(T item)
		{
			this.Item = item;
		}

		public IStateDictionary Properties { get; } = new StateDictionary();

		#region IContextDecorator
		public T Item { get; set; }

		public Task ResetAsync()
		{
			this.Properties.Clear();
			return Task.FromResult(0);
		}
		#endregion

		#region IExceptionDecorator
		public Exception Exception { get; protected set; }

		public void SetException(Exception ex)
		{
			this.Exception = ex;
		}

		public void SetException(string message)
		{
			this.SetException(new Exception(message));
		}

		public void SetException(string format, params object[] args)
		{
			this.SetException(new Exception(String.Format(format, args)));
		}
		#endregion
	}
}
