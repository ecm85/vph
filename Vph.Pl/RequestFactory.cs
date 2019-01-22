using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable.OAuth2.Infrastructure;

namespace Vph.Pl
{
    public class RequestFactory : IRequestFactory
    {
        public RestSharp.Portable.IRestClient CreateClient()
        {
            return new RestClient();
        }

        public RestSharp.Portable.IRestRequest CreateRequest(string resource)
        {
            return new RestRequest(resource);
        }
    }
}
