namespace UPnPNet
{
    public interface IServiceDescriptionXmlParser
    {
        void ParseDescription(UPnPService device, string xml);
    }
}