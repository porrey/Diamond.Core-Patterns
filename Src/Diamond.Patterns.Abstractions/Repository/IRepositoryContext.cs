using System;

namespace Diamond.Patterns.Abstractions
{
	/// <summary>
	/// This interface is used as a handle for any type of context
	/// without the need to expose the type.
	/// </summary>
	public interface IRepositoryContext : IDisposable
	{
	}
}
