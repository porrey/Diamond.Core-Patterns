namespace Diamond.Patterns.Mocks
{
	public class TestItem<TItem, TResult>
	{
		public TItem SourceData { get; set; }
		public TResult ExpectedResult { get; set; }
	}
}
