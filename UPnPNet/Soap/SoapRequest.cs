using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace UPnPNet.Soap
{
	public class SoapRequest
	{
		public IDictionary<string, string> Arguments { get; set; } = new Dictionary<string, string>();
		public string ControlUrl { get; set; }
		public string Action { get; set; }
		public string ServiceType { get; set; }

		public string GetBody()
		{
			XNamespace ns = "http://schemas.xmlsoap.org/soap/envelope/";
			XNamespace serviceNs = ServiceType;

			IList<XElement> xArguments = Arguments.Select(x => new XElement(x.Key, x.Value)).ToList();
			XElement actionElement = new XElement(serviceNs + Action, new XAttribute(XNamespace.Xmlns + "service", serviceNs), xArguments);
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
