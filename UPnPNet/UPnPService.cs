namespace UPnPNet
{
	public class UPnPService
	{
		public string Id { get; set; }
		public string Type { get; set; }

		public string BaseUrl { get; set; }
		public string ControlUrl { get; set; }
		public string EventSubUrl { get; set; }
		public string ServiceDescriptionUrl { get; set; }

		public IUPnPServiceDescriptionXmlParser DescriptionParser { private get; set; } = new UPnPServiceDescriptionXmlParser();
		public IDescriptionLoader DescriptionLoader { private get; set; } = new HttpDescriptionLoader();

		private UPnPServiceDescription _description;
		public UPnPServiceDescription Description
		{
			get
			{
				if (_description == null)
					_description = DescriptionParser.ParseDescription(this, DescriptionLoader.LoadDescription(BaseUrl + ServiceDescriptionUrl).Result);

				return _description;
			}
		}
	}
}
