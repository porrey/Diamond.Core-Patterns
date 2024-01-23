﻿//
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
using System.Threading;
using System.Threading.Tasks;
using Diamond.Core.Rules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	public class RulesExampleHostedService : IHostedService
	{
		private readonly ILogger<RulesExampleHostedService> _logger = null;
		private readonly IHostApplicationLifetime _appLifetime = null;
		private readonly IConfiguration _configuration = null;
		private readonly IRulesFactory _rulesFactory = null;

		private int _exitCode = 0;

		public RulesExampleHostedService(ILogger<RulesExampleHostedService> logger, IHostApplicationLifetime appLifetime, IConfiguration configuration, IRulesFactory rulesFactory)
		{
			_logger = logger;
			_appLifetime = appLifetime;
			_configuration = configuration;
			_rulesFactory = rulesFactory;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation($"Starting {nameof(RulesExampleHostedService)} service.");

			//
			// Create the shipments to validate.
			//
			Random rnd = new Random();

			IShipmentModel shipment = new ShipmentModel()
			{
				ProNumber = $"PRO{rnd.Next(100000000, 999999999):000000000}",
				Weight = rnd.Next(100, 14000),
				PickupAddress = rnd.Next(1, 2) == 1 ? "1234 MAIN ST, CHICAGO IL 60601" : "",
				DeliveryAddress = rnd.Next(1, 2) == 1 ? "125 SAINT AVE, NEW YORK, NY 10116" : "",
				PalletCount = rnd.Next(0, 10)
			};

			//
			// Get the required specification from the services.
			//
			_logger.LogDebug("Retrieving rules to validate shipment.");
			IEnumerable<IRule<IShipmentModel>> rules = await _rulesFactory.GetAllAsync<IShipmentModel>(WellKnown.Rules.Shipment);

			//
			// Execute the specification to get the list of qualified widgets.
			//
			_logger.LogDebug("Executing rules on shipment.");
			IEnumerable<IRuleResult> results = rules.Select(t => t.ValidateAsync(shipment).Result);

			//
			// Compile a list of messages.
			//
			IEnumerable<IRuleResult> messages = results.Where(t => !t.Passed);

			//
			//
			//
			if (messages.Any())
			{
				string message = string.Join(" ", messages.Select(t => t.ErrorMessage));
				_logger.LogError($"Shipment '{shipment.ProNumber}' failed validation: {message}");
				_exitCode = 1;
			}
			else
			{
				_logger.LogInformation($"Shipment '{shipment.ProNumber}' passed validation.");
				_exitCode = 0;
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogDebug($"Exiting service {nameof(RulesExampleHostedService)} with return code: {_exitCode}");

			//
			// Exit code.
			//
			Environment.ExitCode = _exitCode;
			return Task.CompletedTask;
		}
	}
}
