using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.Specification
{
	/// <summary>
	/// Provides the template for an <see cref="ISpecification"/> object.
	/// </summary>
	public abstract class SpecificationTemplate : DisposableObject, ISpecification
	{
		/// <summary>
		/// Creates an instance of <see cref="SpecificationTemplate"/> with the specified logger.
		/// </summary>
		/// <param name="logger">In instance of <see cref="ILogger{SpecificationTemplate}"/> used for logging.</param>
		public SpecificationTemplate(ILogger<SpecificationTemplate> logger)
			: this()
		{
			this.Logger = logger;
		}

		/// <summary>
		/// Creates a default instance of <see cref="SpecificationTemplate"/>.
		/// </summary>
		public SpecificationTemplate()
		{
			this.Name = this.GetType().Name.Replace("Specification", "");
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual ILogger<SpecificationTemplate> Logger { get; set; } = new NullLogger<SpecificationTemplate>();

		/// <summary>
		/// Gets the name used to uniquely identify this specification in a container. The name can be used by
		/// the factory when two or more specification shave the name signature/pattern. The default value is the
		/// name of the class with the term 'Specification' removed.
		/// </summary>
		public virtual string Name { get; set; }
	}
}
