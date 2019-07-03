namespace Diamond.Patterns.Abstractions
{
	public interface IApplicationContext : IContext
	{
		string[] Arguments { get; }
		IObjectFactory ObjectFactory { get; }
	}
}