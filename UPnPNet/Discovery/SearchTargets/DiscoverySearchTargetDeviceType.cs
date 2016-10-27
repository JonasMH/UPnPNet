namespace UPnPNet.Discovery.SearchTargets
{
	internal class DiscoverySearchTargetDeviceType : IDiscoverySearchTarget
	{
		public DiscoverySearchTargetDeviceType(string deviceType, string version)
		{
			DeviceType = deviceType;
			Version = version;
		}

		public string DeviceType { get; set; }
		public string Version { get; set; }
		public string Target => $"urn:schemas-upnp-org:device:{DeviceType}:{Version}";
	}
}
