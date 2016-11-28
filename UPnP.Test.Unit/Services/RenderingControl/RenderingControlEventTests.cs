using System.Collections.Generic;
using NUnit.Framework;
using UPnPNet.Services;
using UPnPNet.Services.RenderingControl;

namespace UPnP.Test.Unit.Services.RenderingControl
{
	[TestFixture]
	public class RenderingControlEventTests
	{
		public IList<UPnPLastChangeValue> _testData;
		public RenderingControlEvent _controlEvent;

		[SetUp]
		public void Setup()
		{
			_testData = new List<UPnPLastChangeValue>();
		}

		[Test]
		public void Volumes_MultipleVolumes_ShouldParseThemAll()
		{
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

			Assert.That(RenderingControlChannel.GetByValue("Master"), Is.Not.Null);

			_controlEvent = new RenderingControlEvent(_testData);

			Assert.That(_controlEvent.Volumes, Has.Count.EqualTo(2));
			Assert.That(_controlEvent.Volumes[RenderingControlChannel.Master], Is.EqualTo(17));
			Assert.That(_controlEvent.Volumes[RenderingControlChannel.LeftFront], Is.EqualTo(100));
		}
	}
}
