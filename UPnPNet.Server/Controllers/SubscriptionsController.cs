using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using UPnPNet.Gena;
using System.Linq;

namespace UPnPNet.Server.Controllers
{
	[Route("api/[controller]")]
	public class SubscriptionsController : Controller
	{
		private readonly GenaSubscriptionHandler _genaSubscriptionHandler;
		private readonly NotifyRepository _notifyRepository;

		public SubscriptionsController(GenaSubscriptionHandler genaSubscriptionHandler, NotifyRepository notifyRepository)
		{
			_genaSubscriptionHandler = genaSubscriptionHandler;
			_notifyRepository = notifyRepository;
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
			GenaSubscriptionRequest request = new GenaSubscriptionRequest()
			{
				BaseAddress = new Uri(dto.BaseAddress),
				PublisherPath = dto.PublisherPath,
				Timeout = dto.Timeout,
				Callback = "http://192.168.5.12:12824/api/subscriptions/notify/"
			};

			IGenaSubscription sub = _genaSubscriptionHandler.Subscribe(request).Result;

			sub.OnNotify += Sub_OnNotify;

			return Ok(sub);
		}

		[AcceptVerbs("NOTIFY")]
		[Route("notify")]
		public IActionResult Notify()
		{
			StreamReader reader = new StreamReader(Request.Body);
			IDictionary<string, string> headers = new Dictionary<string, string>();

			foreach (KeyValuePair<string, StringValues> requestHeader in Request.Headers)
			{
				headers.Add(requestHeader.Key, requestHeader.Value.Aggregate((prev, next) => prev + " " + next));
			}

			_genaSubscriptionHandler.HandleNotify(Request.Method, headers, reader.ReadToEnd());

			return Ok();
		}

		private void Sub_OnNotify(object sender, GenaNotifyResponse genaNotifyResponse)
		{
			_notifyRepository.Notifies.Add(genaNotifyResponse.Values);
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
		public string PublisherPath { get; set; }
		public string BaseAddress { get; set; }
		public int Timeout { get; set; }
		public string LocalUrl { get; set; }
	}
}
