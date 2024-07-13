using Microsoft.Extensions.Configuration;
using Spoleto.RestClient.Tests.Models;

namespace Spoleto.RestClient.Tests
{
    internal static class ConfigurationHelper
    {
        private static readonly IConfigurationRoot _config;

        static ConfigurationHelper()
        {
            _config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: true)
               .AddUserSecrets("bde1af50-fb9b-47f7-85a1-831eff80f1c8")
               .Build();
        }

        public static TestOptions GetTestOptions()
        {
            var settings = _config.GetSection(nameof(TestOptions)).Get<TestOptions>();

            return settings;
        }
    }
}
