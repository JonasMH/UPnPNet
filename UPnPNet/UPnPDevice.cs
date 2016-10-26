using System;
using System.Collections.Generic;

namespace UPnPNet
{
	// ReSharper disable once IconsistentNaming
	public class UPnPDevice
	{
		public string UUID =>
				UniqueServiceName.Substring(5,
					UniqueServiceName.IndexOf("::", StringComparison.Ordinal) - 5);
		
		public string UniqueServiceName { get; set; }
		public string Location { get; set; }
		public string Server { get; set; }
		public IList<string> Targets { get; } = new List<string>();

		public virtual IList<UPnPDevice> SubDevices { get; set; } = new List<UPnPDevice>();
		public virtual IList<UPnPService> Services { get; set; } = new List<UPnPService>();
		public virtual IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
	}
}
