using System.Collections.Generic;

namespace UPnPNet.Models
{
	// ReSharper disable once IconsistentNaming
	public class UPnPDevice
	{
		public string UniqueServiceName { get; set; }
		public string Location { get; set; }
		public string Server { get; set; }
		public string InterfaceToHost { get; set; }
		public IList<string> Targets { get; } = new List<string>();

		public virtual IList<UPnPDevice> SubDevices { get; set; } = new List<UPnPDevice>();
		public virtual IList<UPnPService> Services { get; set; } = new List<UPnPService>();
		public virtual IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
		public UPnPDevice ParentDevice { get; set; }

		public override string ToString()
		{
			string text;
			if (!Properties.TryGetValue("friendlyName", out text))
			{
				text = Location;
			}

			return text;
		}
	}
}
