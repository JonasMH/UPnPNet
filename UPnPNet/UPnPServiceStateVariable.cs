using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPnPNet
{
    public class UPnPServiceStateVariable
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public bool SendEvent { get; set; }
        public IList<string> AllowedValues { get; set; } = new List<string>();
    }
}
