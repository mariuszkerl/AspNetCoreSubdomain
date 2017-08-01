using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Subdomain.Microsoft.AspNetCore.Routing;
using System.Linq;

namespace Microsoft.AspNetCore.Mvc.Rendering
{
    public static class HtmlHelperSubdomainLinkExtensions
    {
        public static IHtmlContent SubdomainLink(this IHtmlHelper helper, string linkText, string actionName)
        {
            var values = MergeRouteValues(new RouteValueDictionary(), actionName, null);

            return BuildAnchor(helper, linkText, values);
        }

        public static IHtmlContent SubdomainLink(this IHtmlHelper helper, string linkText, string actionName, object routeValues)
        {
            var values = MergeRouteValues(new RouteValueDictionary(routeValues), actionName, null);

            return BuildAnchor(helper, linkText, values);
        }

        public static IHtmlContent SubdomainLink(this IHtmlHelper helper, string linkText, string actionName, string controllerName)
        {
            var values = MergeRouteValues(new RouteValueDictionary(), actionName, controllerName);

            return BuildAnchor(helper, linkText, values);
        }

        public static IHtmlContent SubdomainLink(this IHtmlHelper helper, string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            var values = MergeRouteValues(new RouteValueDictionary(routeValues), actionName, null);

            return BuildAnchor(helper, linkText, values, htmlAttributes);
        }

        public static IHtmlContent SubdomainLink(this IHtmlHelper helper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            var values = MergeRouteValues(new RouteValueDictionary(routeValues), actionName, controllerName);

            return BuildAnchor(helper, linkText, values, htmlAttributes);
        }

        public static IHtmlContent SubdomainLink(this IHtmlHelper helper, string linkText, string actionName, string controllerName, object routeValues)
        {
            var values = MergeRouteValues(new RouteValueDictionary(routeValues), actionName, controllerName);

            return BuildAnchor(helper, linkText, values);
        }

        private static TagBuilder BuildAnchor(IHtmlHelper helper, string linkText, RouteValueDictionary values)
        {
            var routeCollection = helper.ViewContext.RouteData.Routers.First(x => x is RouteCollection) as RouteCollection;
            var virtualPath = routeCollection.GetVirtualPath(new SubdomainVirtualPathContext(helper.ViewContext.HttpContext,
                helper.ViewContext.RouteData.Values, values));

            var tagBuilder = new TagBuilder("a");
            tagBuilder.InnerHtml.Append(linkText);
            string url;

            if (virtualPath is AbsolutPathData)
            {
                var absolutePath = virtualPath as AbsolutPathData;
                url = string.Concat(absolutePath.Host, absolutePath.VirtualPath);
            }
            else
            {
                url = virtualPath.VirtualPath;
            }

            tagBuilder.Attributes.Add("href", url);

            return tagBuilder;
        }

        private static TagBuilder BuildAnchor(IHtmlHelper helper, string linkText, RouteValueDictionary values, object htmlAttributes)
        {
            var tagBuilder = BuildAnchor(helper, linkText, values);
            var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            tagBuilder.MergeAttributes(attributes);

            return tagBuilder;
        }

        private static RouteValueDictionary MergeRouteValues(RouteValueDictionary dictionary, string actionName, string controllerName)
        {
            if (!string.IsNullOrEmpty(actionName))
            {
                dictionary.Add("action", actionName);
            }
            if (!string.IsNullOrEmpty(controllerName))
            {
                dictionary.Add("controller", controllerName);
            }

            return dictionary;
        }
    }
}
