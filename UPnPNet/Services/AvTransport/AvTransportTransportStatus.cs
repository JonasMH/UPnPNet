namespace UPnPNet.Services.AvTransport
{
	public class AvTransportTransportStatus : TypeSafeEnum<AvTransportTransportStatus, string>
	{
		public static readonly AvTransportTransportStatus Ok = new AvTransportTransportStatus("OK");
		public static readonly AvTransportTransportStatus ErrorOccurred = new AvTransportTransportStatus("ERROR_OCCURRED");

		private AvTransportTransportStatus(string value) : base(value) { }

		public static AvTransportTransportStatus GetByValue(string value)
		{
			return GetByValueInternal(value, Ok);
		}
	}
}
