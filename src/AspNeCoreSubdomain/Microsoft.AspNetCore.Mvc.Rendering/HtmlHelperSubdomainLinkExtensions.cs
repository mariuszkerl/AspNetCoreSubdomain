using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Subdomain.Microsoft.AspNetCore.Routing;
using System.Linq;

namespace Microsoft.AspNetCore.Mvc.Rendering
{
    [System.Obsolete("Use standrad MVC ActionLink instead. This will be removed in v1.0.0")]
    public static class HtmlHelperSubdomainLinkExtensions
    {
        public static IHtmlContent SubdomainLink(this IHtmlHelper helper, string linkText, string actionName)
        {
            return helper.ActionLink(linkText, actionName);
        }

        public static IHtmlContent SubdomainLink(this IHtmlHelper helper, string linkText, string actionName, object routeValues)
        {
            return helper.ActionLink(linkText, actionName, routeValues);
        }

        public static IHtmlContent SubdomainLink(this IHtmlHelper helper, string linkText, string actionName, string controllerName)
        {
            return helper.ActionLink(linkText, actionName, controllerName);
        }

        public static IHtmlContent SubdomainLink(this IHtmlHelper helper, string linkText, string actionName, string controllerName, object routeValues)
        {
            return helper.ActionLink(linkText, actionName, controllerName, routeValues);
        }

        public static IHtmlContent SubdomainLink(this IHtmlHelper helper, string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            return helper.ActionLink(linkText, actionName, routeValues, htmlAttributes);
        }

        public static IHtmlContent SubdomainLink(this IHtmlHelper helper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            return helper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
        }
    }
}
