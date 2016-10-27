namespace UPnPNet.Discovery.SearchTargets
{
	public class DiscoverySearchTargets
	{
		public static IDiscoverySearchTarget AllSearch()
		{
			return new DiscoverySearchTargetAll();
		}

		public static IDiscoverySearchTarget RootDevicesSearch()
		{
			return new DiscoverySearchTargetRootDevices();
		}

		public static IDiscoverySearchTarget DeviceSearch(string uuid)
		{
			return new DiscoverySearchTargetDeviceUUDI(uuid);
		}

		public static IDiscoverySearchTarget DeviceTypeSearch(string deviceType, string version)
		{
			return new DiscoverySearchTargetDeviceType(deviceType, version);
		}

		public static IDiscoverySearchTarget ServiceTypeSearch(string serviceType, string version)
		{
			return new DiscoverySearchTargetServiceType(serviceType, version);
		}
	}
}
