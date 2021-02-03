using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Core.Specification;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	public class WidgetQualificationSpecification : ISpecification<IEnumerable<Widget>, IEnumerable<Widget>>
	{
		public WidgetQualificationSpecification(ILogger<WidgetQualificationSpecification> logger)
		{
			this.Name = WellKnown.Specification.QualifyWidget;
			this.Logger = logger;
		}

		protected ILogger<WidgetQualificationSpecification> Logger { get; set; }

		public string Name { get; set; }
		public readonly float Threshold = 500;

		public Task<IEnumerable<Widget>> ExecuteSelectionAsync(IEnumerable<Widget> inputs)
		{
			IEnumerable<Widget> returnValue = new Widget[0];

			this.Logger.LogInformation($"Qualifying all widgets less than or equal to {this.Threshold} lbs.");
			returnValue = inputs.Where(t => t.Weight <= this.Threshold).ToArray();

			return Task.FromResult(returnValue);
		}
	}
}
