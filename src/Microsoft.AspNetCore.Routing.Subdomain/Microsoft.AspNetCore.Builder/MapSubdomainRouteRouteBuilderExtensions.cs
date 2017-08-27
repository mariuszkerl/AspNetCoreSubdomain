using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder
{
    public static class MapSubdomainRouteRouteBuilderExtensions
    {
        public static IRouteBuilder MapSubdomainRoute(this IRouteBuilder routeBuilder, string[] hostnames, string name, string subdomain, string template)
        {
            return MapSubdomainRoute(routeBuilder, hostnames, name, subdomain, template, defaults: null);
        }
        public static IRouteBuilder MapSubdomainRoute(this IRouteBuilder routeBuilder, string[] hostnames, string name, string subdomain, string template, object defaults)
        {
            return MapSubdomainRoute(routeBuilder, hostnames, name, subdomain, template, defaults, constraints: null);
        }
        public static IRouteBuilder MapSubdomainRoute(this IRouteBuilder routeBuilder, string[] hostnames, string name, string subdomain, string template, object defaults, object constraints)
        {
            return MapSubdomainRoute(routeBuilder, hostnames, name, subdomain, template, defaults, constraints, dataTokens: null);
        }
        public static IRouteBuilder MapRoute(this IRouteBuilder routeBuilder, string[] hostnames, string name, string template, object defaults)
        {
            routeBuilder.Routes.Add(new SubDomainRoute(hostnames, null, routeBuilder.DefaultHandler, name, template, new RouteValueDictionary(defaults), null, null,
                new DefaultInlineConstraintResolver((IOptions<RouteOptions>)routeBuilder.ServiceProvider.GetService(typeof(IOptions<RouteOptions>)))));

            return routeBuilder;
        }

        public static IRouteBuilder MapSubdomainRoute(this IRouteBuilder routeBuilder, string[] hostnames, string name, string subdomain, string template, object defaults, object constraints, object dataTokens)
        {
            routeBuilder.Routes.Add(new SubDomainRoute(hostnames, subdomain, routeBuilder.DefaultHandler, name, template, new RouteValueDictionary(defaults), new RouteValueDictionary(constraints), new RouteValueDictionary(dataTokens),
                new DefaultInlineConstraintResolver((IOptions<RouteOptions>)routeBuilder.ServiceProvider.GetService(typeof(IOptions<RouteOptions>)))));

            return routeBuilder;
        }
    }
}
