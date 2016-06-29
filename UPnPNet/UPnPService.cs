using System.Collections.Generic;

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


        public IServiceDescriptionXmlParser Parser { private get; set; } = new ServiceDescriptionXmlParser();
        public IDescriptionLoader DescriptionLoader { private get; set; } = new HttpDescriptionLoader();

        public void LoadDescription()
        {
            Parser.ParseDescription(this, DescriptionLoader.LoadDescription(BaseUrl + ServiceDescriptionUrl));
        }

        public virtual IList<UPnPAction> Actions { get; set; } = new List<UPnPAction>();
        public virtual IList<UPnPServiceStateVariable> StateVariables { get; set; } = new List<UPnPServiceStateVariable>();
    }
}
