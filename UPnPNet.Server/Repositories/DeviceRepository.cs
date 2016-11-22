using System.Collections.Generic;
using UPnPNet.Models;

namespace UPnPNet.Server.Repositories
{
	public class DeviceRepository
	{
		public IList<UPnPDevice> Devices { get; set; }
	}
}
