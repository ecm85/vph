using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RestSharp.Portable;
using RestSharp.Portable.OAuth2;
using RestSharp.Portable.OAuth2.Infrastructure;

namespace Vph.Pl
{
    public class Authenticator : OAuth2Authenticator
    {
        public string AccessToken
        {
            get => Context.Session.GetString("AccessToken");
            set => Context.Session.SetString("AccessToken", value);
        }

        public bool IsAuthenticated => !string.IsNullOrEmpty(AccessToken);
        private HttpContext Context { get; }

        public Authenticator(OAuth2Client client, HttpContext context) : base(client)
        {
            Context = context;
        }

        public async Task<Uri> GetLoginLinkUri()
        {
            var uri = await Client.GetLoginLinkUri();
            return new Uri(uri);
        }

        public async Task<bool> OnPageLoaded(Uri uri)
        {
            Debug.WriteLine("Navigated to redirect url.");
            var parameters = uri.Query.Remove(0, 1).ParseQueryString(); 
            await Client.GetUserInfo(parameters);

            if (!string.IsNullOrEmpty(Client.AccessToken))
            {
                AccessToken = Client.AccessToken;
                return true;
            }
            return false;
        }

        public override bool CanPreAuthenticate(IRestClient client, IRestRequest request, ICredentials credentials)
        {
            return true;
        }

        public override bool CanPreAuthenticate(IHttpClient client, IHttpRequestMessage request, ICredentials credentials)
        {
            return false;
        }

        public override Task PreAuthenticate(IRestClient client, IRestRequest request, ICredentials credentials)
        {
            if (!string.IsNullOrEmpty(AccessToken))
                request.AddHeader("Authorization", "Bearer " + AccessToken);
            return Task.CompletedTask;
        }

        public override Task PreAuthenticate(IHttpClient client, IHttpRequestMessage request, ICredentials credentials)
        {
            throw new NotImplementedException();
        }
    }
}
