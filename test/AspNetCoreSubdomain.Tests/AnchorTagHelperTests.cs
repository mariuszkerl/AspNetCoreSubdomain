using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing.Constraints;
using System.Linq;
using TestHelpers;
using Xunit;

namespace AspNetCoreSubdomain.Tests
{
    public class AnchorTagHelperTests
    {
        [Theory]
        [MemberData(nameof(MemberDataFactories.ConstraintInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.ConstraintInSubdomainTestData))]
        public void CanCreateInlineConstraintInSubdomainAnchorTagHelper(
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
                    "{area:bool}",
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
        [MemberData(nameof(MemberDataFactories.ConstraintInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.ConstraintInSubdomainTestData))]
        public void CanCreateParameterConstraintInSubdomainAnchorTagHelper(
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
                    "{controller=Home}/{action=Index}",
                    null,
                    new { area = new BoolRouteConstraint() });
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
        [MemberData(nameof(MemberDataFactories.W3ConstraintInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.W3ConstraintInSubdomainTestData))]
        public void CanCreateW3InlineConstraintInSubdomainAnchorTagHelper(
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
                    "{area:bool}",
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
        [MemberData(nameof(MemberDataFactories.W3ConstraintInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.W3ConstraintInSubdomainTestData))]
        public void CanCreateW3ParameterConstraintInSubdomainAnchorTagHelper(
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
                    "{controller=Home}/{action=Index}",
                    null,
                    new { area = new BoolRouteConstraint() });
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
        [Theory]
        [MemberData(nameof(MemberDataFactories.W3AreaInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.W3AreaInSubdomainTestData))]
        public void CanCreateW3AreaInSubdomainAnchorTagHelper(
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
        [MemberData(nameof(MemberDataFactories.W3ControllerInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.W3ControllerInSubdomainTestData))]
        public void CanCreateW3ControllerInSubdomainAnchorTagHelper(
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
        [MemberData(nameof(MemberDataFactories.W3ConstantSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.W3ConstantSubdomainTestData))]
        public void CanCreateW3ConstantSubdomainAnchorTagHelper(
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
