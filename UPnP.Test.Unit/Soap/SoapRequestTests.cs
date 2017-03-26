
using System.Collections.Generic;
using UPnPNet.Soap;
using Xunit;

namespace UPnP.Test.Unit.Soap
{
	public class SoapRequestTests
	{
		[Fact]
		public void GetBody_CheckXmlHeader_ShouldNotHaveXmlHeader()
		{
			SoapRequest request = new SoapRequest()
			{
				Action = "SomeAction",
				ServiceType = "SomeServiceType",
				ControlUrl = "/SomeUrl"
			};

			string body = request.GetBody();

			Assert.True(!body.StartsWith("<?xml"));
		}

		[Fact]
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

			Assert.True(body.Contains("<SomeArgument>SomeValue</SomeArgument>"));
		}
	}
}
