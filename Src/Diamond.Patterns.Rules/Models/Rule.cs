using System.Threading.Tasks;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Rules
{
	public abstract class Rule<TItem> : IRule<TItem>
	{
		public string Group { get; set; }
		public abstract Task<IRuleResult> ValidateAsync(TItem item);
	}

	public abstract class Rule<TItem, TResult> : IRule<TItem, TResult>
	{
		public string Group { get; set; }
		public abstract Task<TResult> ValidateAsync(TItem item);
	}
}
