namespace UPnPNet.Discovery.SearchTargets
{
	internal class DiscoverySearchTargetDeviceUUDI : IDiscoverySearchTarget
	{
		public DiscoverySearchTargetDeviceUUDI(string deviceUUID)
		{
			DeviceUUID = deviceUUID;
		}

		public string DeviceUUID { get; set; }
		public string Target => "uuid:" + DeviceUUID;
	}
}