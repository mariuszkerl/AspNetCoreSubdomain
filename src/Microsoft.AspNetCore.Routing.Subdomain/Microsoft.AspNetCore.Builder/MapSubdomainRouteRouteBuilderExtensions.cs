using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder
{
    public static class MapSubdomainRouteRouteBuilderExtensions
    {
        public static IRouteBuilder MapSubdomainRoute(this IRouteBuilder routeBuilder, string[] hostnames, string name, string subdomain, string template)
        {
            routeBuilder.Routes.Add(new SubDomainRoute(hostnames, subdomain, routeBuilder.DefaultHandler, template,
                new DefaultInlineConstraintResolver((IOptions<RouteOptions>)routeBuilder.ServiceProvider.GetService(typeof(IOptions<RouteOptions>)))));

            return routeBuilder;
        }
        public static IRouteBuilder MapSubdomainRoute(this IRouteBuilder routeBuilder, string[] hostnames, string name, string subdomain, string template, object defaults)
        {
            routeBuilder.Routes.Add(new SubDomainRoute(hostnames, subdomain, routeBuilder.DefaultHandler, name, template, new RouteValueDictionary(defaults), null, null,
                new DefaultInlineConstraintResolver((IOptions<RouteOptions>)routeBuilder.ServiceProvider.GetService(typeof(IOptions<RouteOptions>)))));

            return routeBuilder;
        }
    }
}
