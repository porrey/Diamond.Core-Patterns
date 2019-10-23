using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Specification
{
	public class SpecificationNotFoundException<TResult> : DiamondPatternsException
	{
		public SpecificationNotFoundException(string name)
			: base($"A Specification of type 'ISpeciification<{typeof(TResult).Name}>' with name '{name}' has not been configured.")
		{
		}
	}

	public class SpecificationNotFoundException<TParameter, TResult> : DiamondPatternsException
	{
		public SpecificationNotFoundException(string name)
			: base($"A Specification of type 'ISpeciification<{typeof(TParameter).Name}, {typeof(TResult).Name}>' with name '{name}' has not been configured.")
		{
		}
	}
}
