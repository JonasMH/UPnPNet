using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UPnPNet.Gena;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.Net.Http.Server;
using UPnPNet.Services;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace UPnPNet.Server
{
	public class UPnPServer
	{
		public GenaSubscriptionHandler Handler { get; }
		public string Url { get; private set; }
		private WebListener _listener;
		private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

		public UPnPServer()
		{
			Handler = new GenaSubscriptionHandler();
		}

		public UPnPServer(GenaSubscriptionHandler handler)
		{
			Handler = handler;
		}

		public void Start(IPEndPoint endpoint)
		{
			if (_listener != null)
				return;

			Url = $"http://{endpoint.Address}:{endpoint.Port}/";

			WebListenerSettings settings = new WebListenerSettings();
			settings.UrlPrefixes.Add(Url);
			_listener = new WebListener(settings);
			_listener.Start();

			var builder = new WebHostBuilder()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseUrls(Url)
				.UseWebListener(options =>
				{
				});

			builder.Build().Run();

			Task.Factory.StartNew(Listen, _cancellationTokenSource.Token);
		}

		public void Stop()
		{
			if (_listener == null)
				return;

			_cancellationTokenSource.Cancel();
			_listener.Dispose();
		}

		public async void SubscribeToControl<T>(UPnPLastChangeServiceControl<T> control)
		{
			await control.SubscribeToEvents(Handler, Url, 3600);
		}

		private async void Listen()
		{
			while (true)
			{
				try
				{
					Console.WriteLine("LISTENING");
					

					using (RequestContext client = await _listener.AcceptAsync())
					{
						if (!client.Request.Method.ToLower().StartsWith("notify"))
						{
							client.Response.StatusCode = 400;
							continue;
						}

						client.Response.StatusCode = 200;

						string body = await new StreamReader(client.Request.Body).ReadToEndAsync();

						Handler.HandleNotify(
							client.Request.Headers.ToDictionary(x => x.Key, x => x.Value.ToString()),
							body);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}
		}
		
		public HttpRequest ParseRequest(string request)
		{
			HttpRequest toReturn = new HttpRequest();

			using (StringReader sr = new StringReader(request))
			{
				string line;
				int linecount = 0;
				bool readingHeaders = true;

				while ((line = sr.ReadLine()) != null)
				{
					if (linecount == 0)
					{
						toReturn.Method = line.Substring(0, line.IndexOf(" ", StringComparison.Ordinal));
						linecount++;
						continue;
					}

					if (readingHeaders)
					{
						if (string.IsNullOrWhiteSpace(line))
						{
							readingHeaders = false;
						}
						else
						{
							toReturn.Headers.Add(line.Substring(0, line.IndexOf(":")), line.Substring(line.IndexOf(": ") + 2));
						}
					}
					else
					{
						toReturn.Body += line;
					}

					linecount++;
				}
			}

			toReturn.Body = WebUtility.HtmlDecode(toReturn.Body);

			return toReturn;
		}
	}

	public class HttpRequest
	{
		public string Body { get; set; }
		public string Method { get; set; }
		public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();
	}
}
