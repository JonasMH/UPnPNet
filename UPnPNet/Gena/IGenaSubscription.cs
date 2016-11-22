using System;

namespace UPnPNet.Gena
{
	public interface IGenaSubscription
	{
		string Id { get; }
		event EventHandler<GenaNotifyResponse> OnNotify;
	}
}