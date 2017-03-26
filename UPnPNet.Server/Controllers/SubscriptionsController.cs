using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UPnPNet.Gena;
using UPnPNet.Models;
using UPnPNet.Services;
using UPnPNet.Services.AvTransport;
using UPnPNet.Services.RenderingControl;
using UPnPNet.TestServer.Repositories;

namespace UPnPNet.TestServer.Controllers
{
	public class UPnPServiceControlRepository
	{
		public IList<UPnPServiceControl> ServiceControls = new List<UPnPServiceControl>();
	}

	[Route("api/[controller]")]
	public class SubscriptionsController : Controller
	{
		private readonly UPnPServiceControlRepository _upnpServiceControlRepository;
		private readonly GenaSubscriptionHandler _genaSubscriptionHandler;
		private readonly NotifyRepository _notifyRepository;
		private readonly DeviceRepository _deviceRepository;

		public SubscriptionsController(GenaSubscriptionHandler genaSubscriptionHandler, NotifyRepository notifyRepository, DeviceRepository deviceRepository, UPnPServiceControlRepository upnpServiceControlRepository)
		{
			_genaSubscriptionHandler = genaSubscriptionHandler;
			_notifyRepository = notifyRepository;
			_deviceRepository = deviceRepository;
			_upnpServiceControlRepository = upnpServiceControlRepository;
		}

		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_genaSubscriptionHandler.Subscriptions);
		}

		[HttpGet("notifies")]
		public IActionResult GetNotifies()
		{
			return Ok(_notifyRepository.Notifies);
		}

		[HttpPost]
		public IActionResult Subscribe([FromBody] GenaSubscriptionRequestDto dto)
		{
			UPnPService service = _deviceRepository.FindService(dto.ServiceId);
			UPnPServiceControl control = _upnpServiceControlRepository
				.ServiceControls
				.FirstOrDefault(x => x.Service == service);

			if (control == null)
			{
				switch (service.Id)
				{
					case UPnPServiceIds.AvTransport:
						control = new AvTransportServiceControl(service);
						break;
					case UPnPServiceIds.RenderingControl:
						control = new RenderingControlServiceControl(service);
						((RenderingControlServiceControl) control).OnLastChangeEvent += (sender, args) =>
						{
							_notifyRepository.Notifies.Add(args.Volumes.ToDictionary(x => x.Key.Value, x => x.Value.ToString()));
						};
						break;
					default:
						control = new UPnPServiceControl(service);
						break;
				}
				_upnpServiceControlRepository.ServiceControls.Add(control);
			}
			
			control.SubscribeToEvents(_genaSubscriptionHandler, "http://localhost:12823/api/subscriptions/notify/", 3600).Wait();
			
			return Ok();
		}

		[AcceptVerbs("NOTIFY")]
		[Route("notify")]
		public IActionResult Notify()
		{
			StreamReader reader = new StreamReader(Request.Body);
			IDictionary<string, string> headers =
				Request.Headers
				.ToDictionary(
					x => x.Key,
					x => x.Value.Aggregate((prev, next) => prev + " " + next));

			_genaSubscriptionHandler.HandleNotify(headers, reader.ReadToEnd());

			return Ok();
		}
	}

	public class PropertySet
	{
		public string Value { get; set; }
	}

	public class NotifyRepository
	{
		public IList<IDictionary<string, string>> Notifies = new List<IDictionary<string, string>>();
	}

	public class GenaSubscriptionRequestDto
	{
		public string ServiceId { get; set; }
		public int Timeout { get; set; }
	}
}
