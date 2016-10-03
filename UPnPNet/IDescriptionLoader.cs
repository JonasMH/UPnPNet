using System.Threading.Tasks;

namespace UPnPNet
{
	public interface IDescriptionLoader
	{
		Task<string> LoadDescription(string url);
	}
}