using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UPnPNet.Discovery;

namespace UPnPNet.Presentation.Cli
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Searching...");
			UPnPDiscovery discovery = new UPnPDiscovery {SearchTarget = DiscoverySearchTargets.All};
			IList<UPnPDevice> devices =  discovery.Search().Result;
			Console.WriteLine("Search done");
			Console.WriteLine("Devices found: " + devices.Count );

			foreach (UPnPDevice device in devices)
			{
				PrintDevice(device);
			}

			Console.ReadKey();
		}

		public static void PrintDevice(UPnPDevice device, int indentation = 0)
		{
			string identation = new string('\t', indentation);

			Console.WriteLine(identation + "==" + device.Properties["friendlyName"] + "==");
			Console.WriteLine(identation + "Services:");
			foreach (UPnPService service in device.Services)
			{
				Console.WriteLine(identation + "\t - " + service.Type);
			}

			Console.WriteLine(identation + "Actions:");
			foreach (string target in device.Targets)
			{
				Console.WriteLine(identation + "\t - " + target);
			}

			Console.WriteLine(identation + "SubDevices:");
			foreach (UPnPDevice subDevice in device.SubDevices)
			{
				PrintDevice(subDevice, indentation + 1);
			}
		}
	}
}
