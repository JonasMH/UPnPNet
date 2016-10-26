using System;
using System.Collections.Concurrent;
using System.Linq;

namespace UPnPNet.Discovery
{
	public class UPnPDeviceLoader
	{
		public IUPnPDeviceDescriptionXmlParser Parser { private get; set; } = new UPnPDeviceDescriptionXmlParser();
		public IDescriptionLoader DescriptionLoader { private get; set; } = new HttpDescriptionLoader();

		public void LoadDevices(BlockingCollection<UPnPDevice>[] input, BlockingCollection<UPnPDevice> output)
		{
			try
			{
				while (!input.All(bc => bc.IsCompleted))
				{
					UPnPDevice device;
					BlockingCollection<UPnPDevice>.TryTakeFromAny(input, out device);

					if (device != null)
					{
						string descriptionXml = DescriptionLoader.LoadDescription(device.Location).Result;
						output.Add(Parser.ParseDescription(device, descriptionXml));
					}
				}
			}
			finally
			{
				Console.WriteLine("Done1");
				output.CompleteAdding();
			}
		}
	}
}