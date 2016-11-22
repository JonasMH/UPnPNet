using System;

namespace UPnPNet.Gena
{
	public class GenaSubscriptionRequest
	{
		public string PublisherPath { get; set; }
		public Uri BaseAddress { get; set; }
		/// <summary>
		/// Timeout in seconds
		/// </summary>
		public int Timeout { get; set; }
		public string Callback { get; set; }
	}
}