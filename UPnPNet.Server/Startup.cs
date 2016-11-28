using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UPnPNet.Discovery;
using UPnPNet.Gena;
using UPnPNet.Models;
using UPnPNet.Server.Controllers;
using UPnPNet.Server.Repositories;

namespace UPnPNet.Server
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			IConfigurationBuilder builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", true, true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

			if (env.IsEnvironment("Development"))
			{
				builder.AddApplicationInsightsSettings(true);
			}
			
			builder.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; }
		
		public void ConfigureServices(IServiceCollection services)
		{
			IList<UPnPDevice> devices = new UPnPDiscovery().Search().Result;

			services.AddSingleton<UPnPServiceControlRepository>();
			services.AddSingleton(new DeviceRepository {Devices = devices });
			services.AddSingleton<GenaSubscriptionHandler>();
			services.AddSingleton<NotifyRepository>();

			services.AddApplicationInsightsTelemetry(Configuration);
			services.AddMvc().AddXmlSerializerFormatters();
		}
		
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();

			app.UseApplicationInsightsRequestTelemetry();

			app.UseApplicationInsightsExceptionTelemetry();
			app.UseStaticFiles();
			app.UseMvc();
		}
	}
}
