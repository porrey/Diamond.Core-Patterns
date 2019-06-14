using System.Threading.Tasks;

namespace Diamond.Patterns.Abstractions
{
	public interface ISpecification<TResult>
	{
		Task<TResult> ExecuteQueryAsync();
	}
}
