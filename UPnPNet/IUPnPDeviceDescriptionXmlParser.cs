namespace UPnPNet
{
    public interface IUPnPDeviceDescriptionXmlParser
    {
        UPnPDeviceDescription ParseDescription(UPnPDevice device, string xml);
    }
}