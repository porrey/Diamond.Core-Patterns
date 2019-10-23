using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.UnitOfWork
{
	public class UnitOfWorkNotFoundException<TResult, TSourceItem> : DiamondPatternsException
	{
		public UnitOfWorkNotFoundException(string name)
			: base($"A Unit Of Work of type 'IUnitOfWork<{typeof(TResult).Name}, {typeof(TSourceItem).Name}>' with name '{name}' has not been configured.")
		{
		}
	}
}
