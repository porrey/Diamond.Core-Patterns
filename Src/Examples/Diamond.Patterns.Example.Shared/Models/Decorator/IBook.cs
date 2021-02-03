namespace Diamond.Patterns.Example
{
	public interface IBook
	{
		string Title { get; set; }
		string Isbn { get; set; }
		bool CheckedOut { get; set; }
	}
}
