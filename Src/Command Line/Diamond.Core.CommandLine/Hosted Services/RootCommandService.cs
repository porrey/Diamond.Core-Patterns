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
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.CommandLine.Hosting;
using System.CommandLine.Rendering;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Diamond.Core.CommandLine
{
	/// <summary>
	/// 
	/// </summary>
	public class RootCommandService : IHostedService
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="logger"></param>
		/// <param name="hostApplicationLifetime"></param>
		/// <param name="rootCommand"></param>
		/// <param name="serviceScopeFactory"></param>
		public RootCommandService(ILogger<RootCommandService> logger, IHostApplicationLifetime hostApplicationLifetime, IRootCommand rootCommand, IServiceScopeFactory serviceScopeFactory)
		{
			this.Logger = logger;
			this.HostApplicationLifetime = hostApplicationLifetime;
			this.RootCommand = rootCommand;
			this.ServiceScopeFactory = serviceScopeFactory;
		}

		/// <summary>
		/// 
		/// </summary>
		protected virtual ILogger<RootCommandService> Logger { get; set; } = new NullLogger<RootCommandService>();

		/// <summary>
		/// 
		/// </summary>
		protected virtual IHostApplicationLifetime HostApplicationLifetime { get; set; }

		/// <summary>
		/// 
		/// </summary>
		protected virtual IRootCommand RootCommand { get; set; }

		/// <summary>
		/// 
		/// </summary>
		protected virtual IServiceScopeFactory ServiceScopeFactory { get; set; }

		private IServiceScope Scope { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public virtual Task StartAsync(CancellationToken cancellationToken)
		{
			//
			// Since this hosted service runs as a singleton we need
			// a scope to get access to scoped services.
			//
			this.Scope = this.ServiceScopeFactory.CreateScope();

			//
			// Get any commands configured.
			//
			IEnumerable<ICommand> commands = this.Scope.ServiceProvider.GetRequiredService<IEnumerable<ICommand>>();
			this.Logger.LogDebug("Found {count} ICommand objects registered.", commands.Count());

			//
			// There should be at least one command.
			//
			if (commands.Any())
			{
				//
				// Add the commands.
				//
				foreach (ICommand cmd in commands)
				{
					if (cmd is Command c)
					{
						this.Logger.LogDebug("Adding '{name}' command to Root Command.", c.Name);
						((RootCommand)this.RootCommand).AddCommand(c);
					}
				}
			}
			else
			{
				throw new NoCommandsConfiguredException();
			}

			this.Logger.LogDebug("Registering 'ApplicationStarted' in Root Command Service.");
			this.HostApplicationLifetime.ApplicationStarted.Register(this.OnStarted);
			this.Logger.LogDebug("Registering 'ApplicationStopping' in Root Command Service.");
			this.HostApplicationLifetime.ApplicationStopping.Register(this.OnStopping);
			this.Logger.LogDebug("Registering 'ApplicationStopped' in Root Command Service.");
			this.HostApplicationLifetime.ApplicationStopped.Register(this.OnStopped);

			return Task.CompletedTask;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cancellationToken"></param>
		/// <returns></returns>
		public virtual Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		private void OnStarted()
		{
			//
			// Start this as a new Task so the remainder of the application
			// can continue to execute,
			//
			_ = Task.Factory.StartNew(async () =>
			  {
				  try
				  {
					  this.Logger.LogDebug("OnStarted has been called on Root Command Service.");
					  this.Logger.LogDebug("Executing InvokeAsync on Root Command.");

					  int result = await new CommandLineBuilder((RootCommand)this.RootCommand)
												   .UseDefaults()
												   .UseAnsiTerminalWhenAvailable()
												   .Build()
												   .InvokeAsync(this.RootCommand.Args);

					  this.Logger.LogDebug("Root Command returned integer value of {result}.", result);
					  this.Logger.LogDebug("Setting Environment.ExitCode to {result}.", result);
					  Environment.ExitCode = result;
				  }
				  catch (Exception ex)
				  {
					  this.Logger.LogError(ex, "Caught exception in RootCommandService.OnStarted().");
				  }
				  finally
				  {
					  this.Logger.LogDebug("Calling StopApplication() on IHostApplicationLifetime.");
					  this.HostApplicationLifetime.StopApplication();
				  }
			  });
		}

		private void OnStopping()
		{
			if (this.Scope != null)
			{
				this.Scope.Dispose();
				this.Scope = null;
			}

			this.Logger.LogDebug("OnStopping has been called on Root Command Service.");
		}

		private void OnStopped()
		{
			this.Logger.LogDebug("OnStopped has been called on Root Command Service.");
		}
	}
}
