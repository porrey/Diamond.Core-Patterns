namespace Diamond.Patterns.Example
{
	public class Book : IBook
	{
		public string Title { get; set; }
		public string Isbn { get; set; }
		public bool CheckedOut { get; set; }
	}
}
