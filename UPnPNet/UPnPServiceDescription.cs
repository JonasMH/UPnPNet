using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPnPNet
{
    public class UPnPServiceDescription
    {
        public virtual IList<UPnPAction> Actions { get; set; } = new List<UPnPAction>();
        public virtual IList<UPnPServiceStateVariable> StateVariables { get; set; } = new List<UPnPServiceStateVariable>();
    }
}
