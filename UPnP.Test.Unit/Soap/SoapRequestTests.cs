
using NUnit.Framework;
using UPnPNet.Soap;

namespace UPnP.Test.Unit.Soap
{
	[TestFixture]
	public class SoapRequestTests
	{
		[Test]
		public void GetBody_VerifyVersionAndEncoding_ShouldBeV1AndUtf8()
		{
			SoapRequest request = new SoapRequest()
			{
				Action = "SomeAction",
				ServiceType = "SomeServiceType",
				ControlUrl = "/SomeUrl"
			};

			Assert.That(request.GetBody(), Does.StartWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>"));
		}
	}
}
