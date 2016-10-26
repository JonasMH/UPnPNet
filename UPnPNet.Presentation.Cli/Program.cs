using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

			UPnPDevice sonosDevice = devices.FirstOrDefault(x => x.Properties["friendlyName"].ToLower().Contains("sonos"));
			UPnPService avService = sonosDevice.SubDevices.SelectMany(x => x.Services).FirstOrDefault(x => x.Type == "urn:schemas-upnp-org:service:AVTransport:1");

			ServiceControl avServiceControl = new ServiceControl(avService);

			avServiceControl.SendAction("Play",
				new Dictionary<string, string>() { { "InstanceID", "0" }, { "Speed", "1" } }).Wait();

			Task.Delay(2000).Wait();

			avServiceControl.SendAction("Stop",
				new Dictionary<string, string>() { { "InstanceID", "0" } }).Wait();
		}

		public static void PrintDevice(UPnPDevice device, int indentation = 0)
		{
			string identation = new string('\t', indentation);

			Console.WriteLine(identation + "==" + device.Properties["friendlyName"] + "==");

			Console.WriteLine(identation + "Properties:");
			foreach (KeyValuePair<string, string> keyValuePair in device.Properties)
			{
				Console.WriteLine(identation + "\t - " + keyValuePair.Key + ": " + keyValuePair.Value);
			}

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
