using System;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.Command
{
	/// <summary>
	/// 
	/// </summary>
	public static class ServicesExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="hostBuilderFactory"></param>
		/// <param name="configureHost"></param>
		/// <returns></returns>
		public static CommandLineBuilder UseDiamondCommandPattern(this CommandLineBuilder builder, string[] args, Func<string[], IHostBuilder> hostBuilderFactory, Action<CommandLineBuilder> commandLineBuilderFactory) =>
			builder.UseMiddleware((invocation, next) =>
			{
				{
					if (hostBuilderFactory == null)
					{
						throw new ArgumentNullException(nameof(hostBuilderFactory));
					}

					IHostBuilder hostBuilder = hostBuilderFactory.Invoke(args);
					IHost host = hostBuilder.Build();

					//var invocation = hostBuilder.Properties[typeof(InvocationContext)];

					CommandLineBuilder commandLineBuilder = new CommandLineBuilder(null);
					commandLineBuilderFactory.Invoke(commandLineBuilder);

					next.Invoke();
				};
			};
}
