using System;

namespace UPnPNet.Gena
{
	public interface IGenaSubscription
	{
		string Id { get; }

		GenaNotifyResponse LastOnNotify { get; }
		event EventHandler<GenaNotifyResponse> OnNotify;
	}
}