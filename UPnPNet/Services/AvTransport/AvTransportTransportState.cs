namespace UPnPNet.Services.AvTransport
{
	public sealed class AvTransportTransportState : TypeSafeEnum<AvTransportTransportState, string>
	{
		public static readonly AvTransportTransportState Stopped = new AvTransportTransportState("STOPPED");
		public static readonly AvTransportTransportState Playing = new AvTransportTransportState("PLAYING");
		public static readonly AvTransportTransportState PausedPlayback = new AvTransportTransportState("PAUSED_PLAYBACK");
		public static readonly AvTransportTransportState Transitioning = new AvTransportTransportState("TRANSITIONING");

		private AvTransportTransportState(string value) : base(value) { }

		public static AvTransportTransportState GetByValue(string value)
		{
			return GetByValueInternal(value, Stopped);
		}
	}
}