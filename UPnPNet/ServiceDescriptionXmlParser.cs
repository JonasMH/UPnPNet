using System.Linq;
using System.Xml.Linq;

namespace UPnPNet
{
    public class ServiceDescriptionXmlParser : IServiceDescriptionXmlParser
    {
        public void ParseDescription(UPnPService service, string xmlstring)
        {
            XDocument xml = XDocument.Parse(xmlstring);

            //State variables
            ParseStateVariables(service, xml);
            ParseActions(service, xml);
        }

        private void ParseActions(UPnPService service, XDocument xml)
        {
            foreach (XElement statevar in xml.Descendants().Where(x => x.Name.LocalName == "actionList").Elements())
            {
                UPnPAction action = new UPnPAction();

                foreach (XElement descendant in statevar.Elements())
                {
                    switch (descendant.Name.LocalName)
                    {
                        case "name":
                            action.Name = descendant.Value;
                            break;
                        case "argumentList":
                            ActionArgument arg = new ActionArgument();

                            foreach (XElement xElement in descendant.Descendants())
                            {
                                switch (xElement.Name.LocalName)
                                {
                                    case "Name":
                                        arg.Name = xElement.Value;
                                        break;
                                    case "direction":
                                        arg.Direction = xElement.Value == "in"
                                            ? ActionArgument.ArgumentDirection.In
                                            : ActionArgument.ArgumentDirection.Out;
                                        break;
                                    case "relatedStateVariable":
                                        arg.RelatedStateVariable = xElement.Value;
                                        break;
                                }
                            }

                            action.Arguments.Add(arg);
                            break;
                    }
                }

                service.Actions.Add(action);//TODO Should properly check if allready exists
            }
        }

        private void ParseStateVariables(UPnPService service, XDocument xml)
        {
            foreach (XElement statevar in xml.Descendants().Where(x => x.Name.LocalName == "serviceStateTable").Elements())
            {
                UPnPServiceStateVariable state = new UPnPServiceStateVariable();

                XAttribute attr = statevar.Attributes().FirstOrDefault(x => x.Name.LocalName == "sendEvents");

                if (attr != null)
                {
                    state.SendEvent = attr.Value == "yes";
                }

                foreach (XElement descendant in statevar.Elements())
                {
                    switch (descendant.Name.LocalName)
                    {
                        case "name":
                            state.Name = descendant.Value;
                            break;
                        case "dataType":
                            state.DataType = descendant.Value;
                            break;
                        case "allowedValueList":
                            foreach (XElement xElement in descendant.Descendants())
                            {
                                state.AllowedValues.Add(xElement.Value);
                            }
                            break;
                    }
                }

                service.StateVariables.Add(state);//TODO Should properly check if allready exists
            }
        }
    }
}