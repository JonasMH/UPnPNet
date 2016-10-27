namespace UPnPNet.Discovery.SearchTargets
{
	internal class DiscoverySearchTargetServiceType : IDiscoverySearchTarget
	{
		public DiscoverySearchTargetServiceType(string serviceType, string version)
		{
			ServiceType = serviceType;
			Version = version;
		}

		public string ServiceType { get; set; }
		public string Version { get; set; }
		public string Target => $"urn:schemas-upnp-org:service:{ServiceType}:{Version}";
	}
}
