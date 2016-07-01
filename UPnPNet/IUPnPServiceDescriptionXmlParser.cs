namespace UPnPNet
{
    public interface IUPnPServiceDescriptionXmlParser
    {
        UPnPServiceDescription ParseDescription(UPnPService service, string xml);
    }
}