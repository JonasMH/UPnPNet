using System;
using System.Collections.Concurrent;
using System.Linq;

namespace UPnPNet.Discovery
{
	public class UPnPServiceLoader
	{
		public IUPnPServiceDescriptionXmlParser DescriptionParser { private get; set; } = new UPnPServiceDescriptionXmlParser();
		public IDescriptionLoader DescriptionLoader { private get; set; } = new HttpDescriptionLoader();

		public void LoadServices(BlockingCollection<UPnPDevice>[] input, BlockingCollection<UPnPDevice> output)
		{
			try
			{
				while (!input.All(bc => bc.IsCompleted))
				{
					UPnPDevice device;
					BlockingCollection<UPnPDevice>.TryTakeFromAny(input, out device);

					if(device == null)
						continue;

					LoadDeviceServices(device);
					output.Add(device);
				}
			}
			finally
			{
				output.CompleteAdding();
			}
		}

		private void LoadDeviceServices(UPnPDevice device)
		{
			foreach (UPnPService service in device.Services)
			{
				string descriptionXml = DescriptionLoader.LoadDescription(service.BaseUrl + service.ServiceDescriptionUrl).Result;
				DescriptionParser.ParseDescription(service, descriptionXml);
			}

			foreach (UPnPDevice deviceSubDevice in device.SubDevices)
			{
				LoadDeviceServices(deviceSubDevice);
			}
		}
	}
}