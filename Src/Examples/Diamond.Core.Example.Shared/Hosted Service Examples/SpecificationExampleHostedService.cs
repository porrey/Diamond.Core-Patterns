//
// Copyright(C) 2019-2023, Daniel M. Porrey. All rights reserved.
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
using System.Threading;
using System.Threading.Tasks;
using Diamond.Core.Specification;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	public class SpecificationExampleHostedService : IHostedService
	{
		private readonly ILogger<SpecificationExampleHostedService> _logger = null;
		private readonly IHostApplicationLifetime _appLifetime = null;
		private readonly IConfiguration _configuration = null;
		private readonly ISpecificationFactory _specificationFactory = null;

		private int _exitCode = 0;

		public SpecificationExampleHostedService(ILogger<SpecificationExampleHostedService> logger, IHostApplicationLifetime appLifetime, IConfiguration configuration, ISpecificationFactory specificationFactory)
		{
			_logger = logger;
			_appLifetime = appLifetime;
			_configuration = configuration;
			_specificationFactory = specificationFactory;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Starting {nameof(SpecificationExampleHostedService)} service.");

			//
			// Create the widgets to qualify.
			//
			Random rnd = new Random();
			Widget[] widgets = new Widget[]
			{
				new Widget() { ItemName = "Widget 1", Weight = rnd.Next(1, 1000) },
				new Widget() { ItemName = "Widget 2", Weight = rnd.Next(1, 1000) },
				new Widget() { ItemName = "Widget 3", Weight = rnd.Next(1, 1000) },
				new Widget() { ItemName = "Widget 4", Weight = rnd.Next(1, 1000) },
				new Widget() { ItemName = "Widget 5", Weight = rnd.Next(1, 1000) },
				new Widget() { ItemName = "Widget 6", Weight = rnd.Next(1, 1000) },
				new Widget() { ItemName = "Widget 7", Weight = rnd.Next(1, 1000) },
				new Widget() { ItemName = "Widget 8", Weight = rnd.Next(1, 1000) },
				new Widget() { ItemName = "Widget 9", Weight = rnd.Next(1, 1000) },
				new Widget() { ItemName = "Widget 10", Weight = rnd.Next(1, 1000) },
			};

			this.DisplayWidgets(widgets);

			//
			// Get the required specification from the services.
			//
			_logger.LogDebug("Retrieving specification for widget qualification.");
			ISpecification<IEnumerable<Widget>, IEnumerable<Widget>> specification = await _specificationFactory.GetAsync<IEnumerable<Widget>, IEnumerable<Widget>>(WellKnown.Specification.QualifyWidget);

			//
			// Execute the specification to get the list of qualified widgets.
			//
			_logger.LogDebug("Executing specification on widgets.");
			IEnumerable<Widget> qualifiedItems = await specification.ExecuteSelectionAsync(widgets);

			_logger.LogInformation($"{qualifiedItems.Count()} widget(s) were qualified.");
			this.DisplayWidgets(qualifiedItems);

			_exitCode = 0;
		}

		private void DisplayWidgets(IEnumerable<Widget> items)
		{
			foreach (var item in items)
			{
				_logger.LogInformation($"Widget '{item.ItemName}' qualified at {item.Weight} lbs.");
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogDebug($"Exiting service {nameof(SpecificationExampleHostedService)} with return code: {_exitCode}");

			//
			// Exit code.
			//
			Environment.ExitCode = _exitCode;
			return Task.CompletedTask;
		}
	}
}
