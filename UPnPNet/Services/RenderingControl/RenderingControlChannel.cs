namespace UPnPNet.Services.RenderingControl
{
	public class RenderingControlChannel : TypeSafeEnum<RenderingControlChannel, string>
	{
		public static readonly RenderingControlChannel Master = new RenderingControlChannel("Master");
		public static readonly RenderingControlChannel LeftFront = new RenderingControlChannel("LF");
		public static readonly RenderingControlChannel RightFront = new RenderingControlChannel("RF");
		public static readonly RenderingControlChannel CenterFront = new RenderingControlChannel("CF");
		public static readonly RenderingControlChannel Subwoofer = new RenderingControlChannel("LFE");

		private RenderingControlChannel(string value) : base(value) { }

		public static RenderingControlChannel GetByValue(string value)
		{
			return GetByValueInternal(value, Master);
		}
	}
}
