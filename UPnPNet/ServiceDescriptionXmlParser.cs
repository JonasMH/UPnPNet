using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace UPnPNet
{
	public class UPnPServiceDescriptionXmlParser : IUPnPServiceDescriptionXmlParser
	{
		public UPnPService ParseDescription(UPnPService service, string xmlstring)
		{
			XDocument xml = XDocument.Parse(xmlstring);
			
			service.StateVariables = ParseStateVariables(xml);
			service.Actions = ParseActions(xml);

			return service;
		}

		private IList<UPnPAction> ParseActions(XContainer xml)
		{
			IList<UPnPAction> actions = new List<UPnPAction>();

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

				actions.Add(action);
			}

			return actions;
		}

		private IList<UPnPServiceStateVariable> ParseStateVariables(XContainer xml)
		{
			IList<UPnPServiceStateVariable> vars = new List<UPnPServiceStateVariable>();

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

				vars.Add(state);
			}

			return vars;
		}
	}
}