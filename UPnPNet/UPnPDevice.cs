using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace UPnPNet
{
    // ReSharper disable once IconsistentNaming
    public class UPnPDevice
    {
        public string UUID
            =>
                UniqueServiceName.Substring(5,
                    UniqueServiceName.IndexOf("::", StringComparison.Ordinal) - 5);

        public string UniqueServiceName { get; set; }
        public string Location { get; set; }
        public string Server { get; set; }

        public IDeviceDescriptionXmlParser Parser { private get; set; } = new DeviceDescriptionXmlParser();
        public IDescriptionLoader DescriptionLoader { private get; set; } = new HttpDescriptionLoader();

        public IList<UPnPService> Services { get; } = new List<UPnPService>();
        public IList<string> Targets { get; } = new List<string>();

        public void LoadDescription()
        {
            Parser.ParseDescription(this, DescriptionLoader.LoadDescription(Location));
        }

    }
}
