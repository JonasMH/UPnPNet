namespace UPnPNet.Services.AvTransport
{
	public sealed class AvTransportPlayMode : TypeSafeEnum<AvTransportPlayMode, string>
	{
		public static readonly AvTransportPlayMode Normal = new AvTransportPlayMode("NORMAL");
		public static readonly AvTransportPlayMode RepeatAll = new AvTransportPlayMode("REPEAT_ALL");
		public static readonly AvTransportPlayMode RepeatOne = new AvTransportPlayMode("REPEAT_ONE");
		public static readonly AvTransportPlayMode ShuffleNoRepeat = new AvTransportPlayMode("SHUFFLE_NOREPEAT");
		public static readonly AvTransportPlayMode Shuffle = new AvTransportPlayMode("SHUFFLE");
		public static readonly AvTransportPlayMode ShuffleRepeatOne = new AvTransportPlayMode("SHUFFLE_REPEAT_ONE");

		private AvTransportPlayMode(string value) : base(value) { }

		public static AvTransportPlayMode GetByValue(string value)
		{
			return GetByValueInternal(value, Normal);
		}
	}
}