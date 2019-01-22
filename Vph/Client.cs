using Newtonsoft.Json.Linq;
using RestSharp.Portable;
using RestSharp.Portable.OAuth2;
using RestSharp.Portable.OAuth2.Infrastructure;
using RestSharp.Portable.OAuth2.Models;

namespace Vph
{
    public class Client : OAuth2Client
    {
        public const string ApiBaseUrl = "https://www.strava.com";

        public Client(IRequestFactory factory, RestSharp.Portable.OAuth2.Configuration.IClientConfiguration configuration)
            : base(factory, configuration)
        {
        }

        protected override Endpoint AccessCodeServiceEndpoint => new Endpoint
        {
            BaseUri = ApiBaseUrl,
            Resource = "/oauth/authorize"
        };

        protected override Endpoint AccessTokenServiceEndpoint => new Endpoint
        {
            BaseUri = ApiBaseUrl,
            Resource = "/oauth/token"
        };

        public override string Name => "strava";

        protected override UserInfo ParseUserInfo(IRestResponse response)
        {
            return ParseUserInfo(response.Content);
        }

        protected virtual UserInfo ParseUserInfo(string content)
        {
            var obj = JObject.Parse(content);
            var userInfo = new UserInfo
            {
                Id = obj["id"].Value<string>(),
                FirstName = obj["firstname"].Value<string>(),
                LastName = obj["lastname"].Value<string>(),
                //Email = obj["email"].Value<string>(),
            };
            //userInfo.AvatarUri.Normal = obj["profile_medium"].Value<string>();
            //userInfo.AvatarUri.Large = obj["profile"].Value<string>();
            return userInfo;
        }

        protected override void BeforeGetUserInfo(BeforeAfterRequestArgs args)
        {
            args.Request.Parameters.Add(new Parameter { Name = "access_token", Value = AccessToken, Type = ParameterType.GetOrPost });
            base.BeforeGetUserInfo(args);
        }

        protected override Endpoint UserInfoServiceEndpoint
        {
            get
            {
                return new Endpoint
                {
                    BaseUri = ApiBaseUrl,
                    Resource = "/api/v3/athlete"
                };
            }
        }
    }
}