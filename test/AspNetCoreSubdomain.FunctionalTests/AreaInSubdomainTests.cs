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
    public class AreaInSubdomainTests
    {
        private HttpClient _client { get; }

        private TestServer _server { get; set; }
        public AreaInSubdomainTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<SubdomainsAreaWebSite.Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task UrlWithSubdomainHasCorrectRouteValues()
        {
            // Arrange & Act
            var response = await _client.GetAsync("http://area1.localhost/Home/Index");

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
                    { "area", "area1" },
                    { "controller", "Home" },
                    { "action", "Index" },
                },
                result.RouteValues);
        }

        [Fact]
        public async Task NonExistentAreaIsNotFound()
        {
            // Arrange & Act
            var response = await _client.GetAsync("http://area2.localhost/Home/Index");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UrlWithoutSubdomainIsNotFound()
        {
            // Arrange & Act
            var response = await _client.GetAsync("http://localhost/Home/Index");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UrlWithIncorrectHostIsNotFound()
        {

            // Arrange & Act
            var response = await _client.GetAsync("http://area1.contoso/Home/Index");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}