using System;

namespace Diamond.Patterns.Extensions.DependencyInjection
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class ServiceDependencyAttribute : Attribute
	{
		public ServiceDependencyAttribute()
			: base()
		{
		}
	}
}
