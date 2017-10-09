using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.WebEncoders.Testing;
using Moq;
using TestHelpers;
using Xunit;

public class FormTagHelperTests
{
    [Theory]
    [MemberData(nameof(MemberDataFactories.AreaInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.AreaInSubdomainTestData))]
    public void CanCreateAreaInSubdomainFormTagHelper(
            string host,
            string appRoot,
            string subdomain,
            string controller,
            string action,
            string expectedUrl)
    {
        // Arrange
        var helper = ConfigurationFactories.TagHelperFactory.GetForm(routeBuilder =>
        {
            routeBuilder.MapSubdomainRoute(
                new[] { "localhost" },
                "default",
                "{area}",
                "{controller=Home}/{action=Index}");
        }, host, appRoot, controller, action, subdomain, expectedUrl);

        var output = ConfigurationFactories.TagHelperOutputFactory.GetForm();

        //Act
        helper.Process(ConfigurationFactories.TagHelperContextFactory.Get(),
            output);

        //Assert
        Assert.Empty(output.Content.GetContent());
        Assert.Equal(expectedUrl, output.Attributes["action"].Value);
    }

    [Theory]
    [MemberData(nameof(MemberDataFactories.ControllerInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.ControllerInSubdomainTestData))]
    public void CanCreateControllerInSubdomainFormTagHelper(
        string host,
        string appRoot,
        string subdomain,
        string action,
        string expectedUrl)
    {
        // Arrange
        var helper = ConfigurationFactories.TagHelperFactory.GetForm(routeBuilder =>
        {
            routeBuilder.MapSubdomainRoute(
                new[] { "localhost" },
                "default",
                "{controller}",
                "{action=Index}");
        }, host, appRoot, subdomain, action, null, expectedUrl);
        var output = ConfigurationFactories.TagHelperOutputFactory.GetForm();

        //Act
        helper.Process(ConfigurationFactories.TagHelperContextFactory.Get(),
            output);

        //Assert
        Assert.Empty(output.Content.GetContent());
        Assert.Equal(expectedUrl, output.Attributes["action"].Value);
    }

    [Theory]
    [MemberData(nameof(MemberDataFactories.ConstantSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.ConstantSubdomainTestData))]
    public void CanCreateConstantSubdomainFormTagHelper(
        string host,
        string appRoot,
        string controller,
        string action,
        string expectedUrl)
    {
        // Arrange
        var helper = ConfigurationFactories.TagHelperFactory.GetForm(routeBuilder =>
        {
            routeBuilder.MapSubdomainRoute(
                new[] { "localhost" },
                "default",
                "constantsubdomain",
                "{controller=Home}/{action=Index}");
        }, host, appRoot, controller, action, null, expectedUrl);

        var output = ConfigurationFactories.TagHelperOutputFactory.GetForm();

        //Act
        helper.Process(ConfigurationFactories.TagHelperContextFactory.Get(),
            output);

        //Assert
        Assert.Empty(output.Content.GetContent());
        Assert.Equal(expectedUrl, output.Attributes["action"].Value);
    }
}