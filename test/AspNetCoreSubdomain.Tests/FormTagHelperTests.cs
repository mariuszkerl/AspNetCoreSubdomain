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
    async public void CanCreateAreaInSubdomainFormTagHelper(
            string host,
            string appRoot,
            string subdomain,
            string controller,
            string action,
            string expectedUrl)
    {
        // Arrange
        var services = ConfigurationFactories.ServiceProviderFacotry.Get();
        var routeBuilder = ConfigurationFactories.RouteBuilderFactory.Get(services);
        var httpContext = ConfigurationFactories.HttpContextFactory.Get(services, host, appRoot);
        var mvcViewOptions = ConfigurationFactories.OptionsFactory.GetMvcViewOptions();

        routeBuilder.MapSubdomainRoute(
            new[] { "localhost" },
            "default",
            "{area}",
            "{controller=Home}/{action=Index}");

        var actionContext = ConfigurationFactories.ActionContextFactory.Get(httpContext, new ActionDescriptor());

        actionContext.RouteData = new RouteData();
        actionContext.RouteData.Values.Add("action", action);
        actionContext.RouteData.Values.Add("controller", controller);
        actionContext.RouteData.Values.Add("area", subdomain);
        actionContext.RouteData.Routers.Add(routeBuilder.Build());

        var metadataProvider = new EmptyModelMetadataProvider();
        var htmlGenerator = new TestHtmlGenerator(metadataProvider, mvcViewOptions, new SubdomainUrlHelperFactory());
        var helper = new Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper(htmlGenerator)
        {
            ViewContext = ConfigurationFactories.ViewContextFactory.Get(actionContext, null, htmlGenerator, metadataProvider, new ModelStateDictionary()),
            Action = action,
            Controller = controller,
            Area = subdomain,
            Antiforgery = false
        };
        var context = new TagHelperContext(
           allAttributes: new TagHelperAttributeList(
               new[] {
               new TagHelperAttribute("asp-action", action),
               new TagHelperAttribute("asp-controller", controller),
               new TagHelperAttribute("asp-area", subdomain) }),
           items: new Dictionary<object, object>(),
           uniqueId: "test-id");
        var output = new TagHelperOutput(
            "form",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        //Act
        helper.Process(context, output);

        //Assert
        Assert.Empty(output.Content.GetContent());
        Assert.Equal(expectedUrl, output.Attributes["action"].Value);
    }

    [Theory]
    [MemberData(nameof(MemberDataFactories.ControllerInSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.ControllerInSubdomainTestData))]
    async public void CanCreateControllerInSubdomainFormTagHelper(
        string host,
        string appRoot,
        string subdomain,
        string action,
        string expectedUrl)
    {
        // Arrange
        var services = ConfigurationFactories.ServiceProviderFacotry.Get();
        var routeBuilder = ConfigurationFactories.RouteBuilderFactory.Get(services);
        var httpContext = ConfigurationFactories.HttpContextFactory.Get(services, host, appRoot);
        var mvcViewOptions = ConfigurationFactories.OptionsFactory.GetMvcViewOptions();

        routeBuilder.MapSubdomainRoute(
            new[] { "localhost" },
            "default",
            "{controller}",
            "{action=Index}");

        var actionContext = ConfigurationFactories.ActionContextFactory.Get(httpContext, new ActionDescriptor());

        actionContext.RouteData = new RouteData();
        actionContext.RouteData.Values.Add("action", action);
        actionContext.RouteData.Values.Add("controller", subdomain);
        actionContext.RouteData.Routers.Add(routeBuilder.Build());

        var metadataProvider = new EmptyModelMetadataProvider();
        var htmlGenerator = new TestHtmlGenerator(metadataProvider, mvcViewOptions, new SubdomainUrlHelperFactory());
        var helper = new Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper(htmlGenerator)
        {
            ViewContext = ConfigurationFactories.ViewContextFactory.Get(actionContext, null, htmlGenerator, metadataProvider, new ModelStateDictionary()),
            Action = action,
            Controller = subdomain,
            Antiforgery = false
        };
        var context = new TagHelperContext(
           allAttributes: new TagHelperAttributeList(
               new[] {
               new TagHelperAttribute("asp-action", action),
               new TagHelperAttribute("asp-controller", subdomain) }),
           items: new Dictionary<object, object>(),
           uniqueId: "test-id");
        var output = new TagHelperOutput(
            "form",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        //Act
        helper.Process(context, output);

        //Assert
        Assert.Empty(output.Content.GetContent());
        Assert.Equal(expectedUrl, output.Attributes["action"].Value);
    }

    [Theory]
    [MemberData(nameof(MemberDataFactories.ConstantSubdomainTestData.Generate), MemberType = typeof(MemberDataFactories.ConstantSubdomainTestData))]
    async public void CanCreateConstantSubdomainFormTagHelper(
        string host,
        string appRoot,
        string controller,
        string action,
        string expectedUrl)
    {
        // Arrange
        var services = ConfigurationFactories.ServiceProviderFacotry.Get();
        var routeBuilder = ConfigurationFactories.RouteBuilderFactory.Get(services);
        var httpContext = ConfigurationFactories.HttpContextFactory.Get(services, host, appRoot);
        var mvcViewOptions = ConfigurationFactories.OptionsFactory.GetMvcViewOptions();

        routeBuilder.MapSubdomainRoute(
            new[] { "localhost" },
            "default",
            "constantsubdomain",
            "{controller=Home}/{action=Index}");

        var actionContext = ConfigurationFactories.ActionContextFactory.Get(httpContext, new ActionDescriptor());

        actionContext.RouteData = new RouteData();
        actionContext.RouteData.Values.Add("action", action);
        actionContext.RouteData.Values.Add("controller", controller);
        actionContext.RouteData.Routers.Add(routeBuilder.Build());

        var metadataProvider = new EmptyModelMetadataProvider();
        var htmlGenerator = new TestHtmlGenerator(metadataProvider, mvcViewOptions, new SubdomainUrlHelperFactory());
        var helper = new Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper(htmlGenerator)
        {
            ViewContext = ConfigurationFactories.ViewContextFactory.Get(actionContext, null, htmlGenerator, metadataProvider, new ModelStateDictionary()),
            Action = action,
            Controller = controller,
            Antiforgery = false
        };
        var context = new TagHelperContext(
           allAttributes: new TagHelperAttributeList(
               new[] {
               new TagHelperAttribute("asp-action", action),
               new TagHelperAttribute("asp-controller", controller) }),
           items: new Dictionary<object, object>(),
           uniqueId: "test-id");
        var output = new TagHelperOutput(
            "form",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var tagHelperContent = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(tagHelperContent);
            });

        //Act
        helper.Process(context, output);

        //Assert

        Assert.Empty(output.Content.GetContent());
        Assert.Equal(expectedUrl, output.Attributes["action"].Value);
    }
}