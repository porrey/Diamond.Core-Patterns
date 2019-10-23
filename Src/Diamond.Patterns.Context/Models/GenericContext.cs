using System;
using Diamond.Patterns.Abstractions;

namespace Diamond.Patterns.Context
{
	/// <summary>
	/// Defines a generic context that can be used for a work-flow. The
	/// context allows data to be shared between the multiple steps of
	/// a work-flow during execution.
	/// </summary>
	public class GenericContext : DisposableObject, IContext
	{
		/// <summary>
		/// Gets the name of the context. The name is used for logging purposes.
		/// </summary>
		public virtual string Name { get; set; }
	}
}
