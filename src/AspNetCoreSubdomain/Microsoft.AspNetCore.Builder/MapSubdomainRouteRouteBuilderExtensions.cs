using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Contains subdomain extension methods for IRouteBuilder interface.
    /// </summary>
    public static class MapSubdomainRouteRouteBuilderExtensions
    {
        /// <summary>
        /// Adds a subdomain route to the IRouteBuilder with the specified hostnames, name and template.
        /// </summary>
        /// <param name="routeBuilder">The IRouteBuilder to add the route to.</param>
        /// <param name="hostnames">Hostnames which should be recognized by application.</param>
        /// <param name="name">The name of the route.</param>
        /// <param name="subdomain">The subdomain pattern of the route.</param>
        /// <param name="template">The URL pattern of the route.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IRouteBuilder MapSubdomainRoute(this IRouteBuilder routeBuilder, string[] hostnames, string name, string subdomain, string template)
        {
            return MapSubdomainRoute(routeBuilder, hostnames, name, subdomain, template, defaults: null);
        }

        /// <summary>
        /// Adds a subdomain route to the IRouteBuilder with the specified hostnames, name, template and defaults.
        /// </summary>
        /// <param name="routeBuilder">The IRouteBuilder to add the route to.</param>
        /// <param name="hostnames">Hostnames which should be recognized by application.</param>
        /// <param name="name">The name of the route.</param>
        /// <param name="subdomain">The subdomain pattern of the route.</param>
        /// <param name="template">The URL pattern of the route.</param>
        /// <param name="defaults">An object that contains default values for route parameters. The object's properties represent the names and values of the default values.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IRouteBuilder MapSubdomainRoute(this IRouteBuilder routeBuilder, string[] hostnames, string name, string subdomain, string template, object defaults)
        {
            return MapSubdomainRoute(routeBuilder, hostnames, name, subdomain, template, defaults, constraints: null);
        }

        /// <summary>
        /// Adds a subdomain route to the IRouteBuilder with the specified hostnames, name, template and defaults.
        /// </summary>
        /// <param name="routeBuilder">The IRouteBuilder to add the route to.</param>
        /// <param name="hostnames">Hostnames which should be recognized by application.</param>
        /// <param name="name">The name of the route.</param>
        /// <param name="subdomain">The subdomain pattern of the route.</param>
        /// <param name="template">The URL pattern of the route.</param>
        /// <param name="defaults">An object that contains default values for route parameters. The object's properties represent the names and values of the default values.</param>
        /// <param name="constraints">An object that contains constraints for the route. The object's properties represent the names and values of the constraints.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IRouteBuilder MapSubdomainRoute(this IRouteBuilder routeBuilder, string[] hostnames, string name, string subdomain, string template, object defaults, object constraints)
        {
            return MapSubdomainRoute(routeBuilder, hostnames, name, subdomain, template, defaults, constraints, dataTokens: null);
        }

        /// <summary>
        /// Adds a subdomain route to the IRouteBuilder with the specified hostnames, name, template and defaults.
        /// </summary>
        /// <param name="routeBuilder">The IRouteBuilder to add the route to.</param>
        /// <param name="hostnames">Hostnames which should be recognized by application.</param>
        /// <param name="name">The name of the route.</param>
        /// <param name="subdomain">The subdomain pattern of the route.</param>
        /// <param name="template">The URL pattern of the route.</param>
        /// <param name="defaults">An object that contains default values for route parameters. The object's properties represent the names and values of the default values.</param>
        /// <param name="constraints">An object that contains constraints for the route. The object's properties represent the names and values of the constraints.</param>
        /// <param name="dataTokens">An object that contains data tokens for the route. The object's properties represent the names and values of the data tokens.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IRouteBuilder MapSubdomainRoute(this IRouteBuilder routeBuilder, string[] hostnames, string name, string subdomain, string template, object defaults, object constraints, object dataTokens)
        {
            routeBuilder.Routes.Add(new SubDomainRoute(hostnames, subdomain, routeBuilder.DefaultHandler, name, template, new RouteValueDictionary(defaults), new RouteValueDictionary(constraints), new RouteValueDictionary(dataTokens),
                new DefaultInlineConstraintResolver((IOptions<RouteOptions>)routeBuilder.ServiceProvider.GetService(typeof(IOptions<RouteOptions>)))));

            return routeBuilder;
        }

        /// <summary>
        /// Adds a route to the IRouteBuilder with the specified hostnames, name and template.
        /// </summary>
        /// <param name="routeBuilder">The IRouteBuilder to add the route to.</param>
        /// <param name="hostnames">Hostnames which should be recognized by application.</param>
        /// <param name="name">The name of the route.</param>
        /// <param name="template">The URL pattern of the route.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IRouteBuilder MapRoute(this IRouteBuilder routeBuilder, string[] hostnames, string name, string template)
        {
            return MapRoute(routeBuilder, hostnames, name, template, defaults: null);
        }

        /// <summary>
        /// Adds a route to the IRouteBuilder with the specified hostnames, name and template.
        /// </summary>
        /// <param name="routeBuilder">The IRouteBuilder to add the route to.</param>
        /// <param name="hostnames">Hostnames which should be recognized by application.</param>
        /// <param name="name">The name of the route.</param>
        /// <param name="template">The URL pattern of the route.</param>
        /// <param name="defaults">An object that contains default values for route parameters. The object's properties represent the names and values of the default values.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IRouteBuilder MapRoute(this IRouteBuilder routeBuilder, string[] hostnames, string name, string template, object defaults)
        {
            return MapRoute(routeBuilder, hostnames, name, template, defaults, constraints: null);
        }

        /// <summary>
        /// Adds a route to the IRouteBuilder with the specified hostnames, name and template.
        /// </summary>
        /// <param name="routeBuilder">The IRouteBuilder to add the route to.</param>
        /// <param name="hostnames">Hostnames which should be recognized by application.</param>
        /// <param name="name">The name of the route.</param>
        /// <param name="template">The URL pattern of the route.</param>
        /// <param name="defaults">An object that contains default values for route parameters. The object's properties represent the names and values of the default values.</param>
        /// <param name="constraints">An object that contains constraints for the route. The object's properties represent the names and values of the constraints.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IRouteBuilder MapRoute(this IRouteBuilder routeBuilder, string[] hostnames, string name, string template, object defaults, object constraints)
        {
            return MapRoute(routeBuilder, hostnames, name, template, defaults, constraints, dataTokens: null);
        }

        /// <summary>
        /// Adds a route to the IRouteBuilder with the specified hostnames, name and template.
        /// </summary>
        /// <param name="routeBuilder">The IRouteBuilder to add the route to.</param>
        /// <param name="hostnames">Hostnames which should be recognized by application.</param>
        /// <param name="name">The name of the route.</param>
        /// <param name="template">The URL pattern of the route.</param>
        /// <param name="defaults">An object that contains default values for route parameters. The object's properties represent the names and values of the default values.</param>
        /// <param name="constraints">An object that contains constraints for the route. The object's properties represent the names and values of the constraints.</param>
        /// <param name="dataTokens">An object that contains data tokens for the route. The object's properties represent the names and values of the data tokens.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IRouteBuilder MapRoute(this IRouteBuilder routeBuilder, string[] hostnames, string name, string template, object defaults, object constraints, object dataTokens)
        {
            return MapSubdomainRoute(routeBuilder, hostnames, name, null, template, defaults, constraints, dataTokens);
        }
    }
}
