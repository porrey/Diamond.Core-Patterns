using System;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Linq;
using Microsoft.Extensions.Hosting;
using System.CommandLine.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.CommandLine;
using System.Collections.Generic;

namespace Diamond.Core.ConsoleCommands
{
	/// <summary>
	/// 
	/// </summary>
	public static class ServicesExtensions
	{
		private const string ConfigurationDirectiveName = "config";

		/// <summary>
		/// 
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="hostBuilderFactory"></param>
		/// <param name="configureHost"></param>
		/// <returns></returns>
		public static CommandLineBuilder UseDiamondCommandPattern(this CommandLineBuilder builder,
			RootCommand rootCommand,
			Func<string[], IHostBuilder> hostBuilderFactory,
			Action<IHostBuilder> configureHost = null) =>
				builder.UseMiddleware(async (invocation, next) =>
				{
					{
						if (hostBuilderFactory == null)
						{
							throw new ArgumentNullException(nameof(hostBuilderFactory));
						}

						IHostBuilder hostBuilder = hostBuilderFactory.Invoke(new string[] { });
						hostBuilder.Properties[typeof(InvocationContext)] = invocation;

						hostBuilder.ConfigureServices(services =>
						{
							services.AddSingleton(invocation);
							services.AddSingleton(invocation.BindingContext);
							services.AddSingleton(invocation.Console);
							services.AddTransient(_ => invocation.InvocationResult);
							services.AddTransient(_ => invocation.ParseResult);
						});

						hostBuilder.UseInvocationLifetime(invocation);
						configureHost?.Invoke(hostBuilder);

						using IHost host = hostBuilder.Build();
						invocation.BindingContext.AddService(typeof(IHost), _ => host);

						// ***
						// *** Add the commands from the services.
						// ***
						IEnumerable<ICommand> commands = host.Services.GetRequiredService<IEnumerable<ICommand>>();

						foreach (ICommand command in commands)
						{
							if (command is Command cmd)
							{
								rootCommand.AddCommand(cmd);
							}
						}

						string[] argsRemaining = invocation.ParseResult.UnparsedTokens.ToArray();

						hostBuilder.ConfigureHostConfiguration(config =>
						{
							config.AddCommandLineDirectives(invocation.ParseResult, ConfigurationDirectiveName);
						});

						await host.StartAsync();
						await next(invocation);
						await host.StopAsync();
					};
				});
	}
}
