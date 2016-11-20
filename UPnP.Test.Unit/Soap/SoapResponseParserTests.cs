﻿using NUnit.Framework;
using UPnPNet.Soap;

namespace UPnP.Test.Unit.Soap
{
	[TestFixture]
	public class SoapResponseParserTests
	{
		[Test]
		public void ParseResponse_IncludeOneValue_ValuePresent()
		{
			string xml =
				"<?xml version=\"1.0\"?>" +
				"<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope/\" soap:encodingStyle=\"http://www.w3.org/2003/05/soap-encoding\">" +
				"<soap:Body>" +
				"<m:GetPriceResponse xmlns:m=\"http://www.w3schools.com/prices\">" +
				"<m:Price>1.90</m:Price>" +
				"</m:GetPriceResponse>" +
				"</soap:Body>" +
				"</soap:Envelope>";


			SoapResponse response = SoapResponseParser.ParseResponse(xml);

			Assert.That(response.Values, Has.Count.EqualTo(1));
		}
	}
}