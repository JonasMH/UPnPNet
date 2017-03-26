using System.Collections.Generic;
using UPnPNet.Services;
using UPnPNet.Services.RenderingControl;
using Xunit;

namespace UPnP.Test.Unit.Services.RenderingControl
{
	public class RenderingControlEventTests
	{
		[Fact]
		public void Volumes_MultipleVolumes_ShouldParseThemAll()
		{
		IList<UPnPLastChangeValue> _testData = new List<UPnPLastChangeValue>();
			RenderingControlEvent _controlEvent;

		_testData.Add(new UPnPLastChangeValue
			{
				Key = "Volume",
				Attributes = new Dictionary<string, string>
				{
					{"channel", "Master"},
					{"val", "17"}
				}
			});
			_testData.Add(new UPnPLastChangeValue
			{
				Key = "Volume",
				Attributes = new Dictionary<string, string>
				{
					{"channel", "LF"},
					{"val", "100"}
				}
			});

			Assert.NotNull(RenderingControlChannel.GetByValue("Master"));

			_controlEvent = new RenderingControlEvent(_testData);

			Assert.Equal(2, _controlEvent.Volumes.Count);
			Assert.Equal(17, _controlEvent.Volumes[RenderingControlChannel.Master]);
			Assert.Equal(100, _controlEvent.Volumes[RenderingControlChannel.LeftFront]);
		}
	}
}
