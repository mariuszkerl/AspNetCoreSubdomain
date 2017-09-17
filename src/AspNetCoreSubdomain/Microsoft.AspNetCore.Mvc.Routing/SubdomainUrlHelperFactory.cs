using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Mvc.Routing
{
    public class SubdomainUrlHelperFactory : /*UrlHelperFactory, */IUrlHelperFactory
    {
        public IUrlHelper GetUrlHelper(ActionContext context)
        {
            var httpContext = context.HttpContext;

            if (httpContext == null)
            {
                throw new ArgumentException(nameof(ActionContext.HttpContext));
                //throw new ArgumentException(Resources.FormatPropertyOfTypeCannotBeNull(
                //    nameof(ActionContext.HttpContext),
                //    nameof(ActionContext)));
            }

            if (httpContext.Items == null)
            {
                throw new ArgumentException(nameof(HttpContext.Items));
                //throw new ArgumentException(Resources.FormatPropertyOfTypeCannotBeNull(
                //    nameof(HttpContext.Items),
                //    nameof(HttpContext)));
            }

            // Perf: Create only one UrlHelper per context
            if (httpContext.Items.TryGetValue(typeof(IUrlHelper), out object value) && value is IUrlHelper)
            {
                return (IUrlHelper)value;
            }

            var urlHelper = new SubdomainUrlHelper(context);
            httpContext.Items[typeof(IUrlHelper)] = urlHelper;

            return urlHelper;
        }
    }
}
