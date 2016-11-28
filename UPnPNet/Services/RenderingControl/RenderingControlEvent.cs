using System.Collections.Generic;
using System.Linq;

namespace UPnPNet.Services.RenderingControl
{
	public class RenderingControlEvent : UPnPLastChangeEvent
	{
		public IDictionary<RenderingControlChannel, int> Volumes => Values
			.Where(x => x.Key == "Volume")
			.ToDictionary(
				x => RenderingControlChannel.GetByValue(x.Attributes["channel"]),
				x => int.Parse(x.Attributes["val"]));

		public RenderingControlEvent(IList<UPnPLastChangeValue> values) : base(values) { }
	}
}
