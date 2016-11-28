using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UPnPNet.Models;

namespace UPnPNet.Services.RenderingControl
{
	public class RenderingControlServiceControl : UPnPLastChangeServiceControl<RenderingControlEvent>
	{
		public RenderingControlServiceControl(UPnPService service) : base(service, x => new RenderingControlEvent(x))
		{
			if (service.Id != UPnPServiceIds.RenderingControl)
			{
				throw new ArgumentException("Service does not have correct id, is " + service.Id + ", should be " + UPnPServiceIds.AvTransport);
			}
		}

		public async Task SetVolume(int instanceId, RenderingControlChannel channel, int volume)
		{
			if (volume < 0)
				throw new ArgumentOutOfRangeException(nameof(volume), "Must be greater or equal to zero");

			await SendAction("SetVolume", new Dictionary<string, string>
			{
				{"InstanceId", instanceId.ToString()},
				{"Channel", channel.Value},
				{"DesiredVolume", volume.ToString() }
			});
		}

		public async Task<int> GetVolume(int instanceId, RenderingControlChannel channel)
		{
			IDictionary<string, string> result = await SendAction("SetVolume", new Dictionary<string, string>
			{
				{"InstanceId", instanceId.ToString()},
				{"Channel", channel.Value}
			});

			return int.Parse(result["CurrentVolume"]);
		}
	}
}
