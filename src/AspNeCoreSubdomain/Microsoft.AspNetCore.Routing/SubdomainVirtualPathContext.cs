using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Routing.Subdomain.Microsoft.AspNetCore.Routing
{
    public class SubdomainVirtualPathContext : VirtualPathContext
    {
        public SubdomainVirtualPathContext(HttpContext httpContext, RouteValueDictionary ambientValues, RouteValueDictionary values)
            : base(httpContext, ambientValues, values) { }
        public SubdomainVirtualPathContext(HttpContext httpContext, RouteValueDictionary ambientValues, RouteValueDictionary values, string routeName)
            : base(httpContext, ambientValues, values, routeName) { }
    }
}
