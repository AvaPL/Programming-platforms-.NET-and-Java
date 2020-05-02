using System.Web;

namespace DiseaseTracker.DAL
{
    public interface IIpProvider
    {
        string GetIp(HttpServerUtilityBase server, HttpRequestBase request);
    }
}