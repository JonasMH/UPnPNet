using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace UPnPNet.Soap
{
	public interface ISoapResponseParser
	{
		SoapResponse ParseResponse(string text);
	}

	public class SoapResponseParser : ISoapResponseParser
	{
		public SoapResponse ParseResponse(string text)
		{
			XDocument xml = XDocument.Parse(text);

			XElement envelopElement = xml.Root;
			XElement bodyElement = envelopElement.Elements().FirstOrDefault(x => x.Name.LocalName == "Body");

			IEnumerable<XElement> parameters = bodyElement.Elements().SelectMany(x => x.Elements());
			
			return new SoapResponse
			{
				Values = parameters.ToDictionary(x => x.Name.LocalName, y => y.Value)
			};
		}
	}
}
