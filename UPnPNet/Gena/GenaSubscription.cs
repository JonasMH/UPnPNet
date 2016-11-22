using System;
using System.Threading.Tasks;

namespace UPnPNet.Gena
{
	internal class GenaSubscription : IGenaSubscription
	{
		public string Id { get; set; }
		public GenaSubscriptionRequest RequestInfo { get; set; }

		public void FireOnNotify(GenaNotifyResponse response)
		{
			OnNotify?.Invoke(this, response);
		}

		public event EventHandler<GenaNotifyResponse> OnNotify;
	}
}
