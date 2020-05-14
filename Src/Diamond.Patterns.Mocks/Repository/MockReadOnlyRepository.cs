using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Mocks
{
	public class MockReadOnlyRepository<TInterface> : IReadOnlyRepository<TInterface>
		where TInterface : IEntity
	{
		public MockReadOnlyRepository(IEnumerable<TInterface> items)
		{
			if (items != null)
			{
				this.Items = new List<TInterface>(items);
			}
			else
			{
				this.Items = new List<TInterface>();
			}
		}

		public IList<TInterface> Items { get; set; }

		public Task<IEnumerable<TInterface>> GetAllAsync()
		{
			return Task.FromResult(this.Items.AsEnumerable());
		}

		public Task<IEnumerable<TInterface>> GetAsync(Expression<Func<TInterface, bool>> predicate)
		{
			IEnumerable<TInterface> p = this.Items.Where(predicate.Compile());
			return Task.FromResult(p);
		}
	}
}
