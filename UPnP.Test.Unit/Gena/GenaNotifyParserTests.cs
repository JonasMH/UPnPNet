using System.Collections.Generic;
using NUnit.Framework;
using UPnPNet.Gena;

namespace UPnP.Test.Unit.Gena
{
	[TestFixture]
	public class GenaNotifyParserTests
	{
		private GenaNotifyParser _parser = new GenaNotifyParser();

		private string _validBodyStringWithSingleValue;
		private IDictionary<string, string> _validHeaders;

		[SetUp]
		public void Setup()
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

		[Test]
		public void HandleNotify_BodyWithSingleValue_ValueShouldBeWrittenToSubscriber()
		{
			GenaNotifyResponse result = _parser.Parse(_validHeaders, _validBodyStringWithSingleValue);

			Assert.That(result.Values, Does.Contain(new KeyValuePair<string, string>("LastChange", "SomeValue")));
		}

		[Test]
		public void HandleNotify_AllHeaders_ShouldParseRelevantHeader()
		{
			GenaNotifyResponse result = _parser.Parse(_validHeaders, _validBodyStringWithSingleValue);

			Assert.That(result.SubscriptionId, Is.EqualTo("SomeSID"));
			Assert.That(result.NTS, Is.EqualTo("SomeNTS"));
			Assert.That(result.SequencyNumber, Is.EqualTo(2));
		}
	}
}
