using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace UPnPNet.Soap
{
	public class SoapClient
	{
		public Uri BaseAddress { get; set; }

		public async Task<SoapResponse> SendAsync(SoapRequest request)
		{
			HttpClient client = new HttpClient
			{
				BaseAddress = BaseAddress
			};

			HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, request.ControlUrl);

			message.Headers.Add("SOAPACTION", request.ServiceType + "#" + request.Action);
			message.Content = new StringContent(request.GetBody(), Encoding.UTF8, "text/xml");

			HttpResponseMessage response = await client.SendAsync(message);

			return SoapResponseParser.ParseResponse(await response.Content.ReadAsStringAsync());
		}
	}

	public class SoapRequest
	{
		public IDictionary<string, string> Arguments { get; set; } = new Dictionary<string, string>();
		public string ControlUrl { get; set; }
		public string Action { get; set; }
		public string ServiceType { get; set; }

		public string GetBody()
		{
			string argumentStr = Arguments.Aggregate("", (current, c) => current + $"<{c.Key}>{c.Value}</{c.Key}>");

			XNamespace ns = "http://schemas.xmlsoap.org/soap/envelope/";
			XNamespace serviceNs = ServiceType;

			XElement actionElement = new XElement(serviceNs + Action, new XAttribute(XNamespace.Xmlns + "service", serviceNs), argumentStr);
			XElement bodyElement = new XElement(ns + "Body", actionElement);
			XElement envelopeElement = new XElement(ns + "Envelope", new XAttribute(XNamespace.Xmlns + "soap", ns), bodyElement);

			XDocument document = new XDocument(envelopeElement);
			Utf8StringWriter writer = new Utf8StringWriter();

			document.Save(writer);

			return writer.ToString();
		}
	}

	class Utf8StringWriter : StringWriter
	{
		public override Encoding Encoding => Encoding.UTF8;
	}
}
