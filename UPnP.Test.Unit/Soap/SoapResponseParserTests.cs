using UPnPNet.Soap;
using Xunit;

namespace UPnP.Test.Unit.Soap
{
	public class SoapResponseParserTests
	{
		[Fact]
		public void ParseResponse_IncludeOneValue_ValuePresent()
		{
			SoapResponseParser parser = new SoapResponseParser();
			const string xml = "<?xml version=\"1.0\"?>" +
							   "<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope/\" soap:encodingStyle=\"http://www.w3.org/2003/05/soap-encoding\">" +
							   "<soap:Body>" +
							   "<m:GetPriceResponse xmlns:m=\"http://www.w3schools.com/prices\">" +
							   "<m:Price>1.90</m:Price>" +
							   "</m:GetPriceResponse>" +
							   "</soap:Body>" +
							   "</soap:Envelope>";


			SoapResponse response = parser.ParseResponse(xml);

			Assert.Equal(1, response.Values.Count);
		}
	}
}
