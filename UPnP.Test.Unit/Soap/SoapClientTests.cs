using System;
using System.Net.Http;
using NSubstitute;
using UPnPNet.Soap;
using System.Linq;
using System.Text;
using Xunit;

namespace UPnP.Test.Unit.Soap
{
	public class SoapClientTests
	{
		private SoapRequest _soapRequest;
		private SoapClient _soapClient;
		private IHttpHandler _httpHandler;
		private ISoapResponseParser _responseParser;
		private HttpResponseMessage _reponseMessage;
		public SoapClientTests()
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

		[Fact]
		public void SendAsync_VerifySoapActionHeader_ShouldIncludeServiceTypeAndAction()
		{
			HttpRequestMessage message = null;
			_httpHandler.SendAsync(Arg.Do<HttpRequestMessage>(x => message = x));

			_soapClient.SendAsync(_soapRequest).Wait();

			string headerValue = message.Headers.GetValues("SOAPACTION").FirstOrDefault();

			Assert.Equal("SomeServiceType#SomeAction", headerValue);
		}

		[Fact]
		public void SendAsync_ByteContentUTF8Response_ShouldCallParseResponse()
		{
			_soapClient.SendAsync(_soapRequest).Wait();

			_responseParser.Received().ParseResponse("SomeContent");
		}
	}
}
