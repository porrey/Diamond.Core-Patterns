using System;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Context
{
	public class ApplicationContext : DisposableObject, IApplicationContext
	{
		public virtual string Name { get; set; }
		public string[] Arguments { get; set; }
		public IObjectFactory ObjectFactory { get; set; }
	}
}
