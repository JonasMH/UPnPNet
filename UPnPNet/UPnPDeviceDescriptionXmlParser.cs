using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace UPnPNet
{
	public class UPnPDeviceDescriptionXmlParser : IUPnPDeviceDescriptionXmlParser
	{
		public UPnPDeviceDescription ParseDescription(UPnPDevice device, string xmlstring)
		{
			XDocument xml = XDocument.Parse(xmlstring);
			UPnPDeviceDescription description = new UPnPDeviceDescription();

			ParseServices(xml.Root, device, description.Services);
			ParseProperties(xml.Root, device, description.Properties);

			return description;

		}

		public void ParseProperties(XElement xml, UPnPDevice device, IDictionary<string, string> dic)
		{
			foreach (XElement xElement in xml.Elements().Where(x => x.Name.LocalName == "device").Elements().Where(x => !x.HasElements))
			{
				dic.Add(xElement.Name.LocalName, xElement.Value);
			}
		}

		public void ParseServices(XElement xml, UPnPDevice device, IList<UPnPService> list)
		{
			foreach (XElement element in xml.Elements().Where(x => x.Name.LocalName == "device").Elements().Where(x => x.Name.LocalName == "serviceList"))
			{
				UPnPService service = new UPnPService();
				Uri baseUri = new Uri(device.Location);

				service.BaseUrl = baseUri.Scheme + "//" + baseUri.Host + ":" + baseUri.Port;

				foreach (XElement descendant in element.Descendants())
				{
					switch (descendant.Name.LocalName)
					{
						case "serviceType":
							service.Type = descendant.Value;
							break;
						case "serviceId":
							service.Id = descendant.Value;
							break;
						case "controlURL":
							service.ControlUrl = descendant.Value;
							break;
						case "eventSubURL":
							service.EventSubUrl = descendant.Value;
							break;
						case "SCPDURL":
							service.ServiceDescriptionUrl = descendant.Value;
							break;
					}
				}

				list.Add(service);
			}
		}
	}
}