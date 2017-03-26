using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UPnPNet.Gena;
using System.Threading;
using Microsoft.Net.Http.Server;
using UPnPNet.Services;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace UPnPNet.Server
{
	public class UPnPServer
	{
		public GenaSubscriptionHandler Handler { get; }
		public string Url { get; private set; }
		private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
		private TcpListener _listener;

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
			Url = $"http://{endpoint.Address}:{endpoint.Port}";

			_listener = new TcpListener(endpoint);
			_listener.Start();
			
			
			Task.Factory.StartNew(() => Listen(_listener), _cancellationTokenSource.Token);
		}

		public void Stop()
		{
			_cancellationTokenSource.Cancel();
		}

		public async void SubscribeToControl<T>(UPnPLastChangeServiceControl<T> control)
		{
			await control.SubscribeToEvents(Handler, Url, 3600);
		}

		private async void Listen(TcpListener listener)
		{
			while (true)
			{
				try
				{
					Console.WriteLine("LISTENING");
					using (TcpClient client = await listener.AcceptTcpClientAsync())
					{
						NetworkStream stream = client.GetStream();

						StreamReader reader = new StreamReader(stream);
						StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

						string request = reader.ReadToEnd();
						string response = "HTTP/1.1 200 Ok\r\nConnection: close\r\n\r\n";

						string[] lines = request.Split(new [] { "\r\n" }, StringSplitOptions.None);
						string body = "";
						IDictionary<string, string> headers = new Dictionary<string, string>();


						for(int i = 1; i < lines.Length; i++)
						{
							if (string.IsNullOrEmpty(lines[i])) //Assume we hit body
							{
								for (int x = i; x < lines.Length; x++)
								{
									body += lines[x];
								}
								break;
							}

							int colonIndex = lines[i].IndexOf(":");
							headers.Add(lines[i].Substring(0, colonIndex), lines[i].Substring(colonIndex + 2));
						}


						writer.Write(response);
						
						Handler.HandleNotify(headers, body);
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
