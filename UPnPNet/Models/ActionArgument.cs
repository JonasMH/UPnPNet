namespace UPnPNet.Models
{
	public struct ActionArgument
	{
		public enum ArgumentDirection
		{
			In,
			Out
		}

		public string Name { get; set; }
		public ArgumentDirection Direction { get; set; }
		public string RelatedStateVariable { get; set; }
	}
}