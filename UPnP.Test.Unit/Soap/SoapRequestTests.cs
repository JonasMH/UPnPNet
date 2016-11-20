
using System.Collections.Generic;
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

			string body = request.GetBody();

			Assert.That(body, Does.StartWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>"));
		}

		[Test]
		public void GetBody_SingleArgument_ArgumentShouldBePresentInXml()
		{
			SoapRequest request = new SoapRequest
			{
				Action = "SomeAction",
				ServiceType = "SomeServiceType",
				ControlUrl = "/SomeUrl",
				Arguments = new Dictionary<string, string> { { "SomeArgument", "SomeValue"} }
			};

			string body = request.GetBody();

			Assert.That(body, Does.Contain("<SomeArgument>SomeValue</SomeArgument>"));
		}
	}
}
