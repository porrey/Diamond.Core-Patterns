using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Mocks
{
	public class MockSpecificationFactory : ISpecificationFactory
	{
		public MockSpecificationFactory(IDictionary<string, ISpecification> items)
		{
			this.Items = items;
		}

		public ILoggerSubscriber LoggerSubscriber { get; set; }
		protected IDictionary<string, ISpecification> Items { get; set; }

		public Task<ISpecification<TResult>> GetAsync<TResult>()
		{
			ISpecification<TResult> item = (from tbl in this.Items
											where tbl.Value is ISpecification<TResult>
											select tbl.Value).Single() as ISpecification<TResult>;

			return Task.FromResult(item);
		}

		public Task<ISpecification<TResult>> GetAsync<TResult>(string name)
		{
			ISpecification<TResult> item = (from tbl in this.Items
											where tbl.Value is ISpecification<TResult> &&
											tbl.Key == name
											select tbl.Value).Single() as ISpecification<TResult>;

			return Task.FromResult(item);
		}

		public Task<ISpecification<TParameter, TResult>> GetAsync<TParameter, TResult>()
		{
			ISpecification<TParameter, TResult> item = (from tbl in this.Items
														where tbl.Value is ISpecification<TParameter, TResult>
														select tbl.Value).Single() as ISpecification<TParameter, TResult>;

			return Task.FromResult(item);
		}

		public Task<ISpecification<TParameter, TResult>> GetAsync<TParameter, TResult>(string name)
		{
			ISpecification<TParameter, TResult> item = (from tbl in this.Items
														where tbl.Value is ISpecification<TParameter, TResult> &&
														tbl.Key == name
														select tbl.Value).Single() as ISpecification<TParameter, TResult>;

			return Task.FromResult(item);
		}
	}
}
