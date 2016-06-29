using System;
using System.Linq;
using System.Xml.Linq;

namespace UPnPNet
{
    public class DeviceDescriptionXmlParser : IDeviceDescriptionXmlParser
    {
        public void ParseDescription(UPnPDevice device, string xmlstring)
        {
            XDocument xml = XDocument.Parse(xmlstring);

            //Services
            foreach (XElement element in xml.Descendants().Where(x => x.Name.LocalName == "serviceList").Elements())
            {
                UPnPService service = new UPnPService();
                Uri baseUri = new Uri(device.Location);

                service.BaseUrl = baseUri.Scheme + Uri.SchemeDelimiter + baseUri.Host + ":" + baseUri.Port;

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

                device.Services.Add(service);//Should properly check if allready exists
            }
        }
    }
}