using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Mocks
{
	public class MockSpecification<TResult> : ISpecification<TResult>
	{
		public MockSpecification(TResult item)
		{
			this.Item = item;
		}

		protected TResult Item { get; set; }

		public Task<TResult> ExecuteQueryAsync()
		{
			return Task.FromResult(Item);
		}
	}

	public class MockSpecification<TParameter, TResult> : ISpecification<TParameter, TResult>
	{
		public MockSpecification(TResult item)
		{
			this.Item = item;
		}

		protected TResult Item { get; set; }

		public Task<TResult> ExecuteQueryAsync(TParameter filter)
		{
			return Task.FromResult(Item);
		}
	}
}
