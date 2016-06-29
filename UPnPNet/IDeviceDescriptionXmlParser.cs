namespace UPnPNet
{
    public interface IDeviceDescriptionXmlParser
    {
        void ParseDescription(UPnPDevice device, string xml);
    }
}