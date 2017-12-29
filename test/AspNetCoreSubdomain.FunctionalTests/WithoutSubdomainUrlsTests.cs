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
    public class WithoutSubdomainUrlsTests
    {
        private HttpClient _client { get; }

        private TestServer _server { get; set; }
        public WithoutSubdomainUrlsTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<AspNetCoreSubdomain.RoutesWithoutSubdomain.Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task UrlWithSubdomainIsNotFound()
        {
            // Arrange & Act
            var response = await _client.GetAsync("http://test.localhost/Home/Index");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UrlWithoutSubdomainAndVirtualPathIsFound()
        {
            // Arrange & Act
            var response = await _client.GetAsync("http://localhost/Home");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SubdomainRoutingResult>(body);

            Assert.Contains("/Home", result.ExpectedUrls);
            Assert.Equal("Home", result.Controller);
            Assert.Equal("Index", result.Action);
            Assert.Equal(
                new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                {
                    { "controller", "Home" },
                    { "action", "Index" },
                },
                result.RouteValues);
        }
        [Fact]
        public async Task UrlWithoutSubdomainWithControllerAndActionIsFound()
        {
            // Arrange & Act
            var response = await _client.GetAsync("http://localhost/Home/Index");

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
                    { "controller", "Home" },
                    { "action", "Index" },
                },
                result.RouteValues);
        }
        [Fact]
        public async Task UrlWithoutSubdomainWithControllerIsFound()
        {
            // Arrange & Act
            var response = await _client.GetAsync("http://localhost/");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SubdomainRoutingResult>(body);

            Assert.Contains("/", result.ExpectedUrls);
            Assert.Equal("Home", result.Controller);
            Assert.Equal("Index", result.Action);
            Assert.Equal(
                new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                {
                    { "controller", "Home" },
                    { "action", "Index" },
                },
                result.RouteValues);
        }
    }
}