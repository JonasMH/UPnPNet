using System.Collections.Generic;
using UPnPNet.Gena;
using Xunit;

namespace UPnP.Test.Unit.Gena
{
	public class GenaNotifyParserTests
	{
		private readonly GenaNotifyParser _parser;

		private readonly string _validBodyStringWithSingleValue;
		private readonly IDictionary<string, string> _validHeaders;
		
		public GenaNotifyParserTests()
		{
			_parser = new GenaNotifyParser();
			_validBodyStringWithSingleValue = "<e:propertyset xmlns:e=\"urn:schemas-upnp-org:event-1-0\"><e:property><LastChange>SomeValue</LastChange></e:property></e:propertyset>";
			_validHeaders = new Dictionary<string, string>
			{
				{"SID", "SomeSID"},
				{"NTS", "SomeNTS"},
				{"SEQ", "2"}
			};
		}

		[Fact]
		public void HandleNotify_BodyWithSingleValue_ValueShouldBeWrittenToSubscriber()
		{
			GenaNotifyResponse result = _parser.Parse(_validHeaders, _validBodyStringWithSingleValue);

			Assert.True(result.Values.Contains(new KeyValuePair<string, string>("LastChange", "SomeValue")));
		}

		[Fact]
		public void HandleNotify_AllHeaders_ShouldParseRelevantHeader()
		{
			GenaNotifyResponse result = _parser.Parse(_validHeaders, _validBodyStringWithSingleValue);

			Assert.Equal("SomeSID", result.SubscriptionId);
			Assert.Equal("SomeNTS", result.NTS);
			Assert.Equal(2, result.SequencyNumber);
		}
	}
}
