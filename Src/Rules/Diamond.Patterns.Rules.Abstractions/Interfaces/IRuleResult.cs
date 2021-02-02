namespace Diamond.Patterns.Rules
{
	/// <summary>
	/// Contains details of the result of applying a rule.
	/// </summary>
	public interface IRuleResult
	{
		/// <summary>
		/// Indicates if the rule passed or not.
		/// </summary>
		bool Passed { get; set; }

		/// <summary>
		/// The error message in case the rule did not pass.
		/// </summary>
		string ErrorMessage { get; set; }
	}
}