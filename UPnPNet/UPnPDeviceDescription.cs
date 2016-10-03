using System.Collections.Generic;

namespace UPnPNet
{
	public class UPnPDeviceDescription
	{
		public virtual IList<UPnPService> Services { get; set; } = new List<UPnPService>();
		public virtual IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();
	}
}
