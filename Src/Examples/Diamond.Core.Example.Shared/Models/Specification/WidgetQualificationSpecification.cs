//
// Copyright(C) 2019-2024, Daniel M. Porrey. All rights reserved.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published
// by the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see http://www.gnu.org/licenses/.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diamond.Core.Specification;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	public class WidgetQualificationSpecification : DisposableObject, ISpecification<IEnumerable<Widget>, IEnumerable<Widget>>
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
			IEnumerable<Widget> returnValue = Array.Empty<Widget>();

			this.Logger.LogInformation("Qualifying all widgets less than or equal to {threshold} lbs.", this.Threshold);
			returnValue = inputs.Where(t => t.Weight <= this.Threshold).ToArray();

			return Task.FromResult(returnValue);
		}
	}
}
