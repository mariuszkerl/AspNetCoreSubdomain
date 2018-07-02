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
                .UseStartup<AspNetCoreSubdomain.ConstraintsWebSite.Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task IntConstraintIsWorkingOnSubdomain()
        {
            // Arrange & Act
            var response = await _client.GetAsync("http://1.localhost/");

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

        [Fact]
        public async Task ConstraintIsNotFoundOnStringSubdomain()
        {
            // Arrange & Act
            var response = await _client.GetAsync("http://test.localhost/");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task BoolConstraintIsWorkingOnSubdomain()
        {
            // Arrange & Act
            var response = await _client.GetAsync("http://true.localhost/");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SubdomainRoutingResult>(body);

            Assert.Contains("/Home/Boolean", result.ExpectedUrls);
            Assert.Equal("Home", result.Controller);
            Assert.Equal("Boolean", result.Action);
            Assert.Equal(
                new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                {
                    { "parameter", "true" },
                    { "controller", "Home" },
                    { "action", "Boolean" },
                },
                result.RouteValues);
        }

        [Fact]
        public async Task GuidConstraintIsWorkingOnSubdomain()
        {
            // Arrange & Act
            var response = await _client.GetAsync("http://75508A0E-0BFA-42C0-9D65-055331F4D40B.localhost/");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SubdomainRoutingResult>(body);

            Assert.Contains("/Home/Guid", result.ExpectedUrls);
            Assert.Equal("Home", result.Controller);
            Assert.Equal("Guid", result.Action);
            Assert.Equal(
                new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                {
                    { "parameter", "75508A0E-0BFA-42C0-9D65-055331F4D40B".ToLower() },
                    { "controller", "Home" },
                    { "action", "Guid" },
                },
                result.RouteValues);
        }

        [Fact]
        public async Task LongConstraintIsWorkingOnSubdomain()
        {
            // Arrange & Act
            //add full route since app cannot distinguish between int and long
            var response = await _client.GetAsync("http://9223372036854775807.localhost/");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var body = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SubdomainRoutingResult>(body);

            Assert.Contains("/Home/Long", result.ExpectedUrls);
            Assert.Equal("Home", result.Controller);
            Assert.Equal("Long", result.Action);
            Assert.Equal(
                new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase)
                {
                    { "parameter", "9223372036854775807" },
                    { "controller", "Home" },
                    { "action", "Long" },
                },
                result.RouteValues);
        }
    }
}
