using System.Collections.Generic;

namespace UPnPNet
{
	public class UPnPServiceDescription
	{
		public virtual IList<UPnPAction> Actions { get; set; } = new List<UPnPAction>();
		public virtual IList<UPnPServiceStateVariable> StateVariables { get; set; } = new List<UPnPServiceStateVariable>();
	}
}
