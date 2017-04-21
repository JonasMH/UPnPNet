using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UPnPNet.Models;

namespace UPnPNet
{
	public class UPnPDeviceDescriptionXmlParser : IUPnPDeviceDescriptionXmlParser
	{
		public UPnPDevice ParseDescription(UPnPDevice device, string xmlstring)
		{
			XDocument xml = XDocument.Parse(xmlstring);
			return ParseDescription(device, GetDeviceSubElements(xml.Root));
		}

		public UPnPDevice ParseDescription(UPnPDevice device, IEnumerable<XElement> xml)
		{
			Uri baseUri = new Uri(device.Location);
			IEnumerable<XElement> xElements = xml as IList<XElement> ?? xml.ToList();

			device.Services = ParseServices(xElements, baseUri);

			foreach (UPnPService service in device.Services)
			{
				service.ParentDevice = device;
			}

			device.Properties = ParseProperties(xElements);
			device.SubDevices = LoadSubDevices(xElements, device.Location);
			
			foreach (UPnPDevice service in device.SubDevices)
			{
				service.ParentDevice = device;
			}

			return device;
		}

		public IList<UPnPDevice> LoadSubDevices(IEnumerable<XElement> xml, string location)
		{
			IList<UPnPDevice> devices = new List<UPnPDevice>();

			foreach (XElement element in xml.Where(x => x.Name.LocalName == "deviceList").Elements())
			{
				UPnPDevice device = new UPnPDevice { Location = location };

				device = ParseDescription(device, element.Elements());

				devices.Add(device);
			}

			return devices;
		}

		public IDictionary<string, string> ParseProperties(IEnumerable<XElement> xml)
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>();

			foreach (XElement xElement in xml.Where(x => !x.HasElements))
			{
				dictionary.Add(xElement.Name.LocalName, xElement.Value);
			}

			return dictionary;
		}

		public IList<UPnPService> ParseServices(IEnumerable<XElement> xml, Uri baseUri)
		{
			IList<UPnPService> services = new List<UPnPService>();

			foreach (XElement element in xml.Where(x => x.Name.LocalName == "serviceList").Descendants())
			{
				UPnPService service = new UPnPService {BaseUrl = baseUri.Scheme + "://" + baseUri.Host + ":" + baseUri.Port};


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

				if (service.Type != null)
				{
					services.Add(service);
				}

			}

			return services;
		}

		private static IEnumerable<XElement> GetDeviceSubElements(XElement xml)
		{
			return xml.Elements().Where(x => x.Name.LocalName == "device").Elements();
		}
	}
}