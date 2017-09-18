using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace AspNetCoreSubdomain.SubdomainsAreaWebSite
{
    public class SubdomainRoutingResponseGenerator
    {
        private readonly ActionContext _actionContext;
        private readonly IUrlHelperFactory _urlHelperFactory;

        public SubdomainRoutingResponseGenerator(IActionContextAccessor contextAccessor, IUrlHelperFactory urlHelperFactory)
        {
            if(!(urlHelperFactory is SubdomainUrlHelperFactory))
            {
                throw new ArgumentException("IUrlHelperFactory is of wrong type. Please use AddSubdomains method in your startup class", "urlHelperFactory");
            }

            _urlHelperFactory = urlHelperFactory;

            _actionContext = contextAccessor.ActionContext;
            if (_actionContext == null)
            {
                throw new InvalidOperationException("ActionContext should not be null here.");
            }
        }

        public JsonResult Generate(params string[] expectedUrls)
        {
            var query = _actionContext.HttpContext.Request.Query;

            var attributeRoutingInfo = _actionContext.ActionDescriptor.AttributeRouteInfo;

            return new JsonResult(new
            {
                expectedUrls = expectedUrls,
                routeName = attributeRoutingInfo == null ? null : attributeRoutingInfo.Name,
                routeValues = new Dictionary<string, object>(_actionContext.RouteData.Values),

                action = ((ControllerActionDescriptor)_actionContext.ActionDescriptor).ActionName,
                controller = ((ControllerActionDescriptor)_actionContext.ActionDescriptor).ControllerName
            });
        }
    }
}