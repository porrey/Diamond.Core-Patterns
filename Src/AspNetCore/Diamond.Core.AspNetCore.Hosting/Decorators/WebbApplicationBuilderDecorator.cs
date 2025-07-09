using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Diamond.Core.AspNetCore.Hosting
{
	/// <summary>
	/// Configures the <see cref="IHostBuilder"/> of the specified <see cref="WebApplicationBuilder"/> using the provided
	/// configuration action.
	/// </summary>
	/// <remarks>This method allows additional configuration of the host builder within the web application builder,
	/// enabling customization of the hosting environment.</remarks>
	public static class WebbApplicationBuilderDecorator
	{
		/// <summary>
		/// Configures the host builder for the specified <see cref="WebApplicationBuilder"/>.
		/// </summary>
		/// <remarks>This method allows additional configuration of the host builder used by the web application. The
		/// provided <paramref name="configuration"/> action is invoked with the <see cref="IHostBuilder"/> associated with
		/// the <paramref name="builder"/>.</remarks>
		/// <param name="builder">The <see cref="WebApplicationBuilder"/> to configure.</param>
		/// <param name="configuration">An <see cref="Action{T}"/> that configures the <see cref="IHostBuilder"/>.</param>
		/// <returns>The configured <see cref="WebApplicationBuilder"/>.</returns>
		public static WebApplicationBuilder WithHostBuilder(this WebApplicationBuilder builder, Action<IHostBuilder, WebApplicationBuilder> configuration)
		{
			configuration.Invoke(builder.Host, builder);
			return builder;
		}

		/// <summary>
		/// Configures the specified <see cref="WebApplication"/> using the provided configuration action.
		/// </summary>
		/// <remarks>This method allows for chaining configuration actions on a <see cref="WebApplication"/>
		/// instance.</remarks>
		/// <param name="app">The <see cref="WebApplication"/> instance to configure.</param>
		/// <param name="configuration">An <see cref="Action{T}"/> that defines the configuration logic to apply to the <paramref name="app"/>.</param>
		/// <returns>The configured <see cref="WebApplication"/> instance.</returns>
		public static WebApplication ConfigureApp(this WebApplication app, Action<WebApplication> configuration)
		{
			configuration.Invoke(app);
			return app;
		}
	}
}
