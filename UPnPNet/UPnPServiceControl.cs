using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UPnPNet.Gena;
using UPnPNet.Models;
using UPnPNet.Services;
using UPnPNet.Soap;

namespace UPnPNet
{
	public class UPnPServiceControl
	{
		public UPnPService Service { get; }
		private IGenaSubscription _subscription;
		public UPnPServiceEvent LastEvent { get; private set; }
		public event EventHandler<UPnPServiceEvent> OnNewEvent;

		public UPnPServiceControl(UPnPService service)
		{
			Service = service;
		}

		public async Task<IDictionary<string, string>> SendAction(string action, IDictionary<string, string> arguments)
		{
			SoapClient client = new SoapClient()
			{
				BaseAddress = new Uri(Service.BaseUrl)
			};

			SoapRequest request = new SoapRequest()
			{
				Action = action,
				ServiceType = Service.Type,
				Arguments = arguments,
				ControlUrl = Service.ControlUrl
			};
			
			return (await client.SendAsync(request)).Values;
		}

		public async Task SubscribeToEvents(GenaSubscriptionHandler handler, string callbackUrl, int timeout)
		{
			if (_subscription != null)
				return;

			GenaSubscriptionRequest request = new GenaSubscriptionRequest()
			{
				BaseAddress = new Uri(Service.BaseUrl),
				PublisherPath = Service.EventSubUrl,
				Timeout = timeout,
				Callback = callbackUrl
			};

			_subscription = await handler.Subscribe(request);
			_subscription.OnNotify += HandleOnNotify;

			if (_subscription.LastOnNotify != null)
				HandleOnNotify(null, _subscription.LastOnNotify);
		}

		private void HandleOnNotify(object sender, GenaNotifyResponse e)
		{
			LastEvent = new UPnPServiceEvent(e.Values);
			OnNewEvent?.Invoke(this, LastEvent);
		}
	}
}
