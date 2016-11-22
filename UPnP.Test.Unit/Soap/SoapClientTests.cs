using System;
using System.Net.Http;
using NSubstitute;
using NUnit.Framework;
using UPnPNet.Soap;
using System.Linq;
using System.Text;

namespace UPnP.Test.Unit.Soap
{
	[TestFixture]
	public class SoapClientTests
	{
		private SoapRequest _soapRequest;
		private SoapClient _soapClient;
		private IHttpHandler _httpHandler;
		private ISoapResponseParser _responseParser;
		private HttpResponseMessage _reponseMessage;

		[SetUp]
		public void Setup()
		{
			_soapRequest = new SoapRequest
			{
				ControlUrl = "SomeControlUrl",
				ServiceType = "SomeServiceType",
				Action = "SomeAction"
			};

			_httpHandler = Substitute.For<IHttpHandler>();
			_responseParser = Substitute.For<ISoapResponseParser>();
			_reponseMessage = Substitute.For<HttpResponseMessage>();
			_reponseMessage.Content = new ByteArrayContent(Encoding.UTF8.GetBytes("SomeContent"));

			_httpHandler.SendAsync(Arg.Any<HttpRequestMessage>()).Returns(_reponseMessage);

			_soapClient = new SoapClient()
			{
				HttpHandler = _httpHandler,
				SoapResponseParser = _responseParser,
				BaseAddress = new Uri("http://SomeUri")
			};
		}

		[Test]
		public void SendAsync_VerifySoapActionHeader_ShouldIncludeServiceTypeAndAction()
		{
			HttpRequestMessage message = null;
			_httpHandler.SendAsync(Arg.Do<HttpRequestMessage>(x => message = x));

			_soapClient.SendAsync(_soapRequest).Wait();

			string headerValue = message.Headers.GetValues("SOAPACTION").FirstOrDefault();

			Assert.That(headerValue, Is.EqualTo("SomeServiceType#SomeAction"));
		}

		[Test]
		public void SendAsync_ByteContentUTF8Response_ShouldCallParseResponse()
		{
			_soapClient.SendAsync(_soapRequest).Wait();

			_responseParser.Received().ParseResponse("SomeContent");
		}
	}
}
