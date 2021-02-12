//
// Copyright(C) 2019-2021, Daniel M. Porrey. All rights reserved.
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
using System.Threading;
using System.Threading.Tasks;
using Diamond.Core.Decorator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Diamond.Core.Example
{
	public class DecoratorExampleHostedService : IHostedService
	{
		private readonly ILogger<DecoratorExampleHostedService> _logger = null;
		private readonly IHostApplicationLifetime _appLifetime = null;
		private readonly IConfiguration _configuration = null;
		private readonly IDecoratorFactory _decoratorFactory = null;

		private int _exitCode = 0;

		public DecoratorExampleHostedService(ILogger<DecoratorExampleHostedService> logger, IHostApplicationLifetime appLifetime, IConfiguration configuration, IDecoratorFactory decoratorFactory)
		{
			_logger = logger;
			_appLifetime = appLifetime;
			_configuration = configuration;
			_decoratorFactory = decoratorFactory;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Starting application.");

			//*
			//* Create a book.
			//*
			Random rnd = new Random();

			IBook book = new Book()
			{
				Title = "Design Patterns in C#",
				Isbn = "978-1484260616",
				CheckedOut = rnd.Next(1, 2) == 1 ? true : false
			};

			//*
			//* Get a decorator to check out the book.
			//*
			IDecorator<IBook, bool> decorator = await _decoratorFactory.GetAsync<IBook, bool>(WellKnown.Decorator.BookTransaction);

			//*
			//* Check the book out of the library.
			//*
			if (await decorator.TakeActionAsync(book))
			{
				_logger.LogInformation("The book was successfully checked out.");
			}
			else
			{
				_logger.LogWarning("The book is not available for check out.");
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogDebug($"Exiting with return code: {_exitCode}");

			//
			// Exit code.
			//
			Environment.ExitCode = _exitCode;
			return Task.CompletedTask;
		}
	}
}
