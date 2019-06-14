namespace Diamond.Patterns.Abstractions
{
	public interface IApplicationContext : IContext
	{
		string[] Arguments { get; set; }
		IObjectFactory ObjectFactory { get; set; }
	}
}