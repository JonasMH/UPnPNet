using System;
using System.Collections.Generic;

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

		public IUPnPDeviceDescriptionXmlParser Parser { private get; set; } = new UPnPDeviceDescriptionXmlParser();
		public IDescriptionLoader DescriptionLoader { private get; set; } = new HttpDescriptionLoader();

		public IList<string> Targets { get; } = new List<string>();

		public UPnPDeviceDescription LoadDescription()
		{
			return Parser.ParseDescription(this, DescriptionLoader.LoadDescription(Location));
		}

	}
}
