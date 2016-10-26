namespace UPnPNet
{
	public interface IUPnPDeviceDescriptionXmlParser
	{
		UPnPDevice ParseDescription(UPnPDevice device, string xml);
	}
}