using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Context
{
	public class ApplicationContext : GenericContext, IApplicationContext
	{
		public virtual string[] Arguments { get; set; }
		public virtual IObjectFactory ObjectFactory { get; set; }
	}
}
