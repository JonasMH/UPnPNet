using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UPnPNet.Discovery.SearchTargets;

namespace UPnPNet.Discovery
{
	// ReSharper disable once InconsistentNaming
	public class UPnPDiscovery
	{
		public int LocalPort { get; set; } = 2500;
		public int MulticastPort { get; set; } = 1900;
		public int TimeToLive { get; set; } = 32;
		public string MulticastAddress { get; set; } = "239.255.255.250";
		public int WaitTimeInSeconds { get; set; } = 2;
		public Encoding Encoder { get; set; } = new ASCIIEncoding();
		public IDiscoverySearchTarget SearchTarget { get; set; } = new DiscoverySearchTargetAll();
		
		private IPEndPoint MulticastEndPoint => new IPEndPoint(IPAddress.Parse(MulticastAddress), MulticastPort);

		public async Task<IList<UPnPDevice>> Search()
		{
			UdpClient client = new UdpClient();
			client.Client.Bind(new IPEndPoint(IPAddress.Parse("192.168.5.12"), LocalPort));
			client.JoinMulticastGroup(IPAddress.Parse(MulticastAddress), TimeToLive);
			client.MulticastLoopback = true;

			string request = $"M-SEARCH * HTTP/1.1\r\nHOST: {MulticastAddress}:{MulticastPort}\r\nMAN:\"ssdp:discover\"\r\nST: {SearchTarget.Target}\r\nMX: {WaitTimeInSeconds}\r\n\r\n";
			byte[] buffer = Encoder.GetBytes(request);

			await client.SendAsync(buffer, buffer.Length, MulticastEndPoint);

			IList<UPnPDevice> foundDevices = new List<UPnPDevice>();
			UPnPDeviceLoader deviceLoader = new UPnPDeviceLoader();
			UPnPServiceLoader serviceLoader = new UPnPServiceLoader();
			TaskFactory taskFactory = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);
			BlockingCollection<UPnPDevice> basicDevices = new BlockingCollection<UPnPDevice>();
			BlockingCollection<UPnPDevice> loadedDevices = new BlockingCollection<UPnPDevice>();
			BlockingCollection<UPnPDevice> loadedDevicesWithServices = new BlockingCollection<UPnPDevice>();

			Task deviceTask = taskFactory.StartNew(() => deviceLoader.LoadDevices(new[] { basicDevices }, loadedDevices));
			Task serviceTask = taskFactory.StartNew(() => serviceLoader.LoadServices(new[] { loadedDevices }, loadedDevicesWithServices));

			Stopwatch watch = Stopwatch.StartNew();
			
			while (watch.ElapsedMilliseconds < WaitTimeInSeconds * 1500)
			{
				if (client.Available <= 0)
					continue;
				
				UdpReceiveResult received = await client.ReceiveAsync();

				IDictionary<string, string> response = ParseResponse(Encoder.GetString(received.Buffer));

				if (!response.ContainsKey("LOCATION"))
					continue;

				UPnPDevice device = foundDevices.FirstOrDefault(x => x.Location == response["LOCATION"]);

				if (device == null)
				{
					device = CreateDeviceFromResponse(response);
					foundDevices.Add(device);
					basicDevices.Add(device);
				}

				device.Targets.Add(response["ST"]);

				Task.Delay(100).Wait();
			}

			basicDevices.CompleteAdding();

			await deviceTask;
			await serviceTask;

			return loadedDevicesWithServices.ToList();
		}

		private UPnPDevice CreateDeviceFromResponse(IDictionary<string, string> response)
		{
			return new UPnPDevice()
			{
				Location = response["LOCATION"],
				Server = response["SERVER"],
				UniqueServiceName = response["USN"]
			};
		}

		private IDictionary<string, string> ParseResponse(string input)
		{
			string[] lines = input.Split(new [] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
			Dictionary<string, string> output = new Dictionary<string, string>();

			if (lines[0] != "HTTP/1.1 200 OK")
				return null;

			foreach (string line in lines.Where(x => x != lines.First()))
			{
				int colonIndex = line.IndexOf(":", StringComparison.Ordinal);

				if (colonIndex < 0)
					continue;

				string key = line.Substring(0, colonIndex);
				string value = line.Substring(key.Length + 1).Trim();

				if (value.Length <= 0)
					continue;

				output.Add(key, value);
			}

			return output;
		}
	}
}
