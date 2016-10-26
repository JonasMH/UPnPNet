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

					foreach (UPnPService service in device.Services)
					{
						string descriptionXml = DescriptionLoader.LoadDescription(service.BaseUrl + service.ServiceDescriptionUrl).Result;
						DescriptionParser.ParseDescription(service, descriptionXml);
					}

					output.Add(device);
				}
			}
			finally
			{
				Console.WriteLine("Done2");
				output.CompleteAdding();
			}
		}
	}
}