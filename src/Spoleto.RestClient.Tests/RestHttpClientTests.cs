using System.Net;
using Spoleto.RestClient;
using Spoleto.RestClient.Tests;
using Spoleto.RestClient.Tests.Models;

namespace Spoleto.RestClient5.Tests
{
    public partial class RestHttpClientTests
    {
        private TestOptions _settings;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _settings = ConfigurationHelper.GetTestOptions();
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task NTLMAuth()
        {
            // Arange
            var messageHandler = new SocketsHttpHandler
            {
                Credentials = CredentialCache.DefaultNetworkCredentials // Win auth
            };
            var httpClient = new HttpClient(messageHandler)
            {
                BaseAddress = new Uri(_settings.ServiceUrl)
            };

            var restClient = new RestClientFactory().WithHttpClient(httpClient, true).Build();

            var restRequest = new RestRequestFactory(RestHttpMethod.Post, _settings.TestUrl1).WithJsonContent(_settings.ObjectModel1).Build();

            // Act
            var result = await restClient.ExecuteAsBytesAsync(restRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task JWTAuth()
        {
            // Arrange
            var httpClient = new HttpClient()
            {
                BaseAddress = new Uri(_settings.ServiceUrl)
            };

            var restClient = new RestClientFactory()
                .WithHttpClient(httpClient, true)
                .WithAuthenticator(new MyJWTDynamicAuthenticator(_settings.Credentials))
                .Build();

            var restRequest = new RestRequestFactory(RestHttpMethod.Post, _settings.TestUrl2).WithJsonContent(_settings.ObjectModel2).Build();

            var restRequest2 = new RestRequestFactory(RestHttpMethod.Post, _settings.TestUrl2).WithJsonContent(_settings.ObjectModel2).Build();

            var restRequest3 = new RestRequestFactory(RestHttpMethod.Post, _settings.TestUrl2).WithJsonContent(_settings.ObjectModel2).Build();

            // Act
            var result = await restClient.ExecuteAsBytesAsync(restRequest);

            await Task.Delay(1500);

            var result2 = await restClient.ExecuteAsBytesAsync(restRequest2);

            await Task.Delay(2500);

            var result3 = await restClient.ExecuteAsBytesAsync(restRequest3);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.Not.Null);
                Assert.That(result2, Is.Not.Null);
                Assert.That(result3, Is.Not.Null);
            });
        }
    }
}