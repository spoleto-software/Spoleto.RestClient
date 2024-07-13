using Spoleto.RestClient;
using Spoleto.RestClient.Authentication.JWT;
using Spoleto.RestClient.Tests.Models;

namespace Spoleto.RestClient5.Tests
{
    internal class MyJWTDynamicAuthenticator(LoginModelCredentials credentials) : JWTDynamicAuthenticator<LoginModelCredentials, JsonTokenModel>(credentials)
    {
        protected override async Task<JsonTokenModel> GetAccessToken(IRestClient client, LoginModelCredentials credentials)
        {
            var restRequest = new RestRequestFactory(RestHttpMethod.Post, "api/Token")
                                    .WithJsonContent(credentials)
                                    .Build();

            var tokenModel = await client.ExecuteAsync<JsonTokenModel>(restRequest).ConfigureAwait(false);

            return tokenModel;
        }

        protected override async Task<JsonTokenModel> RefreshAccessToken(IRestClient client, LoginModelCredentials credentials, JsonTokenModel token)
        {
            var refreshModel = new RefreshTokenModel
            {
                ClientCode = credentials.ClientCode,
                ClientSecret = credentials.ClientSecret,
                RefreshToken = token.RefreshToken
            };

            var restRequest = new RestRequestFactory(RestHttpMethod.Post, "api/Token/Refresh")
                .WithJsonContent(refreshModel)
                .Build();

            var tokenModel = await client.ExecuteAsync<JsonTokenModel>(restRequest).ConfigureAwait(false);

            return tokenModel;
        }
    }
}