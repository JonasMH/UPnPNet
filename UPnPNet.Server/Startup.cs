using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UPnPNet.Discovery;
using UPnPNet.Gena;
using UPnPNet.Models;
using UPnPNet.TestServer.Controllers;
using UPnPNet.TestServer.Repositories;

namespace UPnPNet.TestServer
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			IConfigurationBuilder builder = new ConfigurationBuilder()
				.SetBasePath(env.ContentRootPath)
				.AddJsonFile("appsettings.json", true, true)
				.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

			builder.AddEnvironmentVariables();
			Configuration = builder.Build();
		}

		public IConfigurationRoot Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			IList<UPnPDevice> devices = new UPnPDiscovery().Search().Result;

			services.AddSingleton<UPnPServiceControlRepository>();
			services.AddSingleton(new DeviceRepository { Devices = devices });
			services.AddSingleton<GenaSubscriptionHandler>();
			services.AddSingleton<NotifyRepository>();

			services.AddMvc().AddXmlSerializerFormatters();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
		{
			loggerFactory.AddConsole(Configuration.GetSection("Logging"));
			loggerFactory.AddDebug();


			app.UseStaticFiles();
			app.UseMvc();
		}
	}
}
