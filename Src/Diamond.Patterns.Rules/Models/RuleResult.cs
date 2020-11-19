using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Rules
{
	public class RuleResult : IRuleResult
	{
		public bool Passed { get; set; }
		public string ErrorMessage { get; set; }
	}
}
