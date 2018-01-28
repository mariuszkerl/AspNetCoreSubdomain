using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.WebEncoders.Testing;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestHelpers;
using Xunit;

namespace AspNetCoreSubdomain.Tests
{
    public class AnchorTagHelperTests
    {
        [Theory]
        [MemberData(nameof(MemberDataFactories.AreaInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.AreaInSubdomainTestData))]
        public void CanCreateAreaInSubdomainAnchorTagHelper(
            string host,
            string appRoot,
            string subdomain,
            string controller,
            string action,
            string expectedUrl)
        {
            // Arrange
            var helper = ConfigurationFactories.TagHelperFactory.GetAnchor(routeBuilder =>
            {
                routeBuilder.MapSubdomainRoute(
                    new[] { "example.com" },
                    "default",
                    "{area}",
                    "{controller=Home}/{action=Index}");
            }, host, appRoot, controller, action, subdomain, expectedUrl);
            
            var output = ConfigurationFactories.TagHelperOutputFactory.GetAnchor();

            //Act
            helper.Process(ConfigurationFactories.TagHelperContextFactory.Get(), 
                output);

            //Assert
            Assert.Empty(output.Content.GetContent());
            Assert.Equal(1, output.Attributes.Count);
            Assert.Equal("href", output.Attributes.First().Name);
            Assert.Equal(expectedUrl, output.Attributes.First().Value);
        }

        [Theory]
        [MemberData(nameof(MemberDataFactories.ControllerInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.ControllerInSubdomainTestData))]
        public void CanCreateControllerInSubdomainAnchorTagHelper(
            string host,
            string appRoot,
            string subdomain,
            string action,
            string expectedUrl)
        {
            // Arrange           
            var helper = ConfigurationFactories.TagHelperFactory.GetAnchor(routeBuilder =>
            {
                routeBuilder.MapSubdomainRoute(
                    new[] { "example.com" },
                    "default",
                    "{controller}",
                    "{action=Index}");
            }, host, appRoot, subdomain, action, null, expectedUrl);
            
            var output = ConfigurationFactories.TagHelperOutputFactory.GetAnchor();

            //Act
            helper.Process(ConfigurationFactories.TagHelperContextFactory.Get(), 
                output);

            //Assert
            Assert.Empty(output.Content.GetContent());
            Assert.Equal(1, output.Attributes.Count);
            Assert.Equal("href", output.Attributes.First().Name);
            Assert.Equal(expectedUrl, output.Attributes.First().Value);
        }

        [Theory]
        [MemberData(nameof(MemberDataFactories.ConstantSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.ConstantSubdomainTestData))]
        public void CanCreateConstantSubdomainAnchorTagHelper(
            string host,
            string appRoot,
            string controller,
            string action,
            string expectedUrl)
        {
            // Arrange
            var helper = ConfigurationFactories.TagHelperFactory.GetAnchor(routeBuilder =>
            {
                routeBuilder.MapSubdomainRoute(
                    new[] { "example.com" },
                    "default",
                    "constantsubdomain",
                    "{controller=Home}/{action=Index}");
            }, host, appRoot, controller, action, null, expectedUrl);

            var output = ConfigurationFactories.TagHelperOutputFactory.GetAnchor();

            //Act
            helper.Process(ConfigurationFactories.TagHelperContextFactory.Get(), 
                output);

            //Assert
            Assert.Empty(output.Content.GetContent());
            Assert.Equal(1, output.Attributes.Count);
            Assert.Equal("href", output.Attributes.First().Name);
            Assert.Equal(expectedUrl, output.Attributes.First().Value);
        }
    }
}
