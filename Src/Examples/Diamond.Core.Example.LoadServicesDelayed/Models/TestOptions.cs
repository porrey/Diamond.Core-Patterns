namespace Diamond.Core.Example.LoadServicesDelayed
{
	public class TestOptions
	{
		public static string Key => nameof(TestOptions);
		public string Name { get; set; }
		public int[] Values { get; set; }
	}
}
