using System;

namespace UPnPNet.Gena
{
	internal class GenaSubscription : IGenaSubscription
	{
		public string Id { get; set; }
		public Uri Address { get; set; }
		public GenaNotifyResponse LastOnNotify { get; private set; }
		public GenaSubscriptionRequest RequestInfo { get; set; }

		public void FireOnNotify(GenaNotifyResponse response)
		{
			LastOnNotify = response;
			OnNotify?.Invoke(this, response);
		}

		public event EventHandler<GenaNotifyResponse> OnNotify;
	}
}
