using System;
using System.Collections.Generic;
using UPnPNet.Discovery;

namespace UPnPNet.Presentation.Cli
{
	public class Program
	{
		public static void Main(string[] args)
		{
			UPnPDiscovery discovery = new UPnPDiscovery();
			IList<UPnPDevice> devices =  discovery.SearchAsync().Result;

			foreach (UPnPDevice device in devices)
			{
				Console.WriteLine(device.UniqueServiceName);
				foreach (UPnPService service in device.LoadDescription().Services)
				{
					service.
				}
			}

			Console.ReadKey();
		}
	}
}
