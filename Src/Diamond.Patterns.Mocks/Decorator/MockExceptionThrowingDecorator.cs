using System;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Mocks
{
	public class MockExceptionThrowingDecorator<TItem, TResult> : IDecorator<TItem, TResult>
	{
		public Task<TResult> TakeActionAsync(TItem item)
		{
			throw new Exception("This exception is thrown to simulate 500 internal error.");
		}
	}
}
