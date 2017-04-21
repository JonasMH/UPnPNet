using System.Collections.Generic;

namespace UPnPNet.Models
{
	public class UPnPService
	{
		public string Id { get; set; }
		public string Type { get; set; }
		public string BaseUrl { get; set; }
		public string ControlUrl { get; set; }
		public string EventSubUrl { get; set; }
		public string ServiceDescriptionUrl { get; set; }
		public UPnPDevice ParentDevice { get; set; }
		public virtual IList<UPnPAction> Actions { get; set; } = new List<UPnPAction>();
		public virtual IList<UPnPServiceStateVariable> StateVariables { get; set; } = new List<UPnPServiceStateVariable>();

		public override string ToString()
		{
			return Id;
		}
	}
}
