using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace UPnPNet.Services
{
	public class UPnPLastChangeEventParser
	{
		public IList<UPnPLastChangeValue> Parse(string xml)
		{
			XDocument xdocument = XDocument.Parse(xml);
			
			XElement instanceIdElement = xdocument.Root.Elements()
				.FirstOrDefault(x => x.Name.LocalName == "InstanceID");

			return instanceIdElement
				.Elements()
				.Select(x => new UPnPLastChangeValue
				{
					Key = x.Name.LocalName,
					Attributes = x.Attributes().ToDictionary(y => y.Name.LocalName, y => y.Value)
				}).ToList();
		}
	}
}