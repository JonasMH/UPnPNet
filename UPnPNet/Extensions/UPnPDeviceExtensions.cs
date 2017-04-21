using UPnPNet.Models;

namespace UPnPNet.Extensions
{
	public static class UPnPDeviceExtensions
	{
		public static string GetInterfaceToHost(this UPnPDevice device)
		{
			while (true)
			{
				if (device.InterfaceToHost != null)
					return device.InterfaceToHost;

				if (device.ParentDevice == null)
					return null;

				device = device.ParentDevice;
			}
		}
	}
	public static class UPnPServiceExtensions
	{
		public static string GetInterfaceToHost(this UPnPService service)
		{
			return service.ParentDevice?.GetInterfaceToHost();
		}
	}
}