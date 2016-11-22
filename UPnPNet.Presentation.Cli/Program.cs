using System;
using System.Collections.Generic;
using System.Linq;
using UPnPNet.Discovery;
using UPnPNet.Discovery.SearchTargets;
using UPnPNet.Models;

namespace UPnPNet.Presentation.Cli
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Searching...");
			UPnPDiscovery discovery = new UPnPDiscovery { SearchTarget = DiscoverySearchTargetFactory.ServiceTypeSearch("AVTransport", "1") };
			IList<UPnPDevice> devices = discovery.Search().Result;
			Console.WriteLine("Search done");
			Console.WriteLine("Devices found: " + devices.Count);

			IList<UPnPDevice> sonosDevices = devices.Where(x => x.Properties["friendlyName"].ToLower().Contains("sonos")).ToList();
			IList<UPnPService> avServices = sonosDevices.SelectMany(x => x.SubDevices).SelectMany(x => x.Services)
				.Where(x => x.Type == "urn:schemas-upnp-org:service:AVTransport:1").ToList();

			IList<UPnPServiceControl> speakers = avServices.Select(x => new UPnPServiceControl(x)).ToList();


			while (true)
			{
				ConsoleKeyInfo info = Console.ReadKey();

				switch (info.Key)
				{
					case ConsoleKey.Q:
						return;
					case ConsoleKey.A:
						ForEach(speakers, x => x.SendAction("Play", new Dictionary<string, string>() { { "InstanceID", "0" }, { "Speed", "1" } }).Wait());
						break;
					case ConsoleKey.S:
						ForEach(speakers, x => x.SendAction("Pause", new Dictionary<string, string>() { { "InstanceID", "0" } }).Wait());
						break;
				}
			}
		}

		private static void ForEach<T>(IList<T> list, Action<T> command)
		{
			foreach (T x1 in list)
			{
				command(x1);
			}
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
