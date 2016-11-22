using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UPnPNet.Soap;

namespace UPnPNet.Gena
{
	public class GenaSubscriptionHandler
	{
		public IHttpHandler HttpHandler { get; set; } = new HttpHandler();
		private IGenaNotifyParser NotifyParser { get; set; } = new GenaNotifyParser();
		private readonly List<GenaSubscription> subscriptions = new List<GenaSubscription>();

		public IReadOnlyList<IGenaSubscription> Subscriptions => subscriptions.AsReadOnly();

		public async Task<IGenaSubscription> Subscribe(GenaSubscriptionRequest request)
		{
			Uri address = new Uri(request.BaseAddress, request.PublisherPath);
			HttpRequestMessage httpRequest = new HttpRequestMessage(new HttpMethod("SUBSCRIBE"), address);
			
			httpRequest.Headers.Clear();

			httpRequest.Headers.Add("HOST", address.Authority);
			httpRequest.Headers.Add("CONNECTION", (string)null);
			httpRequest.Headers.Add("NT", "upnp:event");
			httpRequest.Headers.Add("TIMEOUT", "Second-" + request.Timeout);
			httpRequest.Headers.Add("CALLBACK", "<" + request.Callback + ">");
		
			HttpResponseMessage response = await HttpHandler.SendAsync(httpRequest);

			GenaSubscription subscribtion = new GenaSubscription()
			{
				Id = response.Headers.GetValues("SID").FirstOrDefault(),
			};

			subscriptions.Add(subscribtion);

			return subscribtion;
		}

		public async Task Unsubscribe(IGenaSubscription subscription)
		{
			GenaSubscription sub = subscription as GenaSubscription;

			if (sub == null)
				return;

			HttpRequestMessage httpRequest = new HttpRequestMessage
			{
				Method = new HttpMethod("SUBSCRIBE"),
			};

			httpRequest.Headers.Add("SID", subscription.Id);

			await HttpHandler.SendAsync(httpRequest);

		}

		public void HandleNotify(string methodVersion, IDictionary<string, string> headers, string body)
		{
			GenaNotifyResponse response = NotifyParser.Parse(headers, body);

			IEnumerable<GenaSubscription> subs = Subscriptions
				.Where(x => x.Id == response.SubscriptionId)
				.Select(x => x as GenaSubscription);

			foreach (GenaSubscription genaSubscription in subs)
			{
				genaSubscription.FireOnNotify(response);
			}
		}
	}
}