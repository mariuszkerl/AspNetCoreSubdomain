using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace AspNetCoreSubdomain.FunctionalTests
{
    public class UnitTest1
    {
        private HttpClient _client { get; }

        private TestServer _server { get; set; }
        public UnitTest1()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<AspNetCoreSubdomain.SubdomainsAreaWebSite.Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task AreaRouteValueRetrievedFromSubdomain()
        {
            // Arrange & Act
            var response = await _client.GetAsync("http://subdomain1.localhost/Home/Index");

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
                    { "area", "subdomain1" },
                    { "controller", "Home" },
                    { "action", "Index" },
                },
                result.RouteValues);
        }
    }
}