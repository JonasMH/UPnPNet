using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UPnPNet.Models;

namespace UPnPNet.Services.AvTransport
{
	public class AvTransportServiceControl : UPnPLastChangeServiceControl<AvTransportEvent>
	{
		public AvTransportServiceControl(UPnPService service) : base(service, x => new AvTransportEvent(x))
		{
			if (service.Id != UPnPServiceIds.AvTransport)
			{
				throw new ArgumentException("Service does not have correct id, is " + service.Id + ", should be " + UPnPServiceIds.AvTransport);
			}
		}

		public Task Stop(int instanceId)
		{
			return SendAction("Stop", new Dictionary<string, string> { { "InstanceID", instanceId.ToString() } });
		}

		public Task Play(int instanceId, int speed)
		{
			return SendAction("Play", new Dictionary<string, string>
			{
				{ "InstanceID", instanceId.ToString()},
				{ "Speed", speed.ToString() }
			});
		}

		public Task Pause(int instanceId)
		{
			return SendAction("Pause", new Dictionary<string, string> { { "InstanceID", instanceId.ToString() } });
		}

		public Task Next(int instanceId)
		{
			return SendAction("Next", new Dictionary<string, string> { { "InstanceID", instanceId.ToString() } });
		}

		public Task Previous(int instanceId)
		{
			return SendAction("Previous", new Dictionary<string, string> { { "InstanceID", instanceId.ToString() } });
		}

		public Task SetPlayMode(int instanceId, AvTransportPlayMode playMode)
		{
			return SendAction("SetPlayMode", new Dictionary<string, string>
			{
				{ "InstanceID", instanceId.ToString() },
				{ "NewPlayMode", playMode.Value }
			});
		}
	}
}