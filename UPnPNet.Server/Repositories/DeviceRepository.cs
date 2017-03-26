using System.Collections.Generic;
using System.Linq;
using UPnPNet.Models;

namespace UPnPNet.TestServer.Repositories
{
	public class DeviceRepository
	{
		public IList<UPnPDevice> Devices { get; set; }

		public UPnPService FindService(string id)
		{
			return Devices.SelectMany(x => x.SubDevices).SelectMany(x => x.Services).FirstOrDefault(x => x.Id == id);
		}
	}
}
