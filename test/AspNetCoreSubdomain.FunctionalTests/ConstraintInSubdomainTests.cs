using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreSubdomain.FunctionalTests
{
    public class ConstraintInSubdomainTests
    {
        private HttpClient _client { get; }

        private TestServer _server { get; set; }
        public ConstraintInSubdomainTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<AspNetCoreSubdomain.IntConstraintWebSite.Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task InlineIntConstraintIsWorkingOnSubdomain()
        {
            // Arrange & Act
            var response = await _client.GetAsync("http://1.localhost/Home/Index");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SubdomainRoutingResult>(body);

            Assert.Contains("/Home/Index", result.ExpectedUrls);
            Assert.Equal("Home", result.Controller);
            Assert.Equal("Index", result.Action);
            Assert.Equal(
                new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                {
                    { "parameter", "1" },
                    { "controller", "Home" },
                    { "action", "Index" },
                },
                result.RouteValues);
        }

    }
}
