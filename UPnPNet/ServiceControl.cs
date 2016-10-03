using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace UPnPNet
{
	public class ServiceControl
	{
		private readonly UPnPService _service;

		public ServiceControl(UPnPService service)
		{
			_service = service;
		}

		public async Task<bool> SendAction(string action, IDictionary<string, string> arguments)
		{
			string argumentStr = arguments.Aggregate("", (current, c) => current + $"<{c.Key}>{c.Value}</{c.Key}>");

			string body =
				"<?xml version=\"1.0\"?>\r\n" +
				"<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\">\r\n" +
					"<s:Body>\r\n" +
						$"<u:{action} xmlns:u=\"{_service.Type}\">\r\n" +
							argumentStr +
						$"</u:{action}>\r\n" +
					"</s:Body>\r\n" +
				"</s:Envelope>\r\n";

			HttpClient client = new HttpClient
			{
				BaseAddress = new Uri(_service.BaseUrl),
				DefaultRequestHeaders =
				{
					{ "SOAPACTION", _service.Type + "#" + action },
					{"Content-Type", "text/xml; charset =\"utf-8\"" }
				}
			};

			await client.PostAsync(_service.ControlUrl, new StringContent(body));

			return true;
		}
	}
}
