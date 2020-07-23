using Microsoft.AspNetCore.Http;
using System;

namespace Microsoft.AspNetCore.Mvc.Routing
{
    /// <summary>
    /// A factory for creating <see cref="IUrlHelper"/> instances.
    /// </summary>
    public class SubdomainUrlHelperFactory : IUrlHelperFactory
    {
        /// <summary>
        /// Gets an <see cref="IUrlHelper"/> for the request associated with context.
        /// </summary>
        /// <param name="context">The <see cref="ActionContext"/> associated with the current request.</param>
        /// <returns>An <see cref="IUrlHelper"/> for the request associated with context</returns>
        public IUrlHelper GetUrlHelper(ActionContext context)
        {
            var httpContext = context.HttpContext;

            if (httpContext == null)
            {
                throw new ArgumentException(nameof(ActionContext.HttpContext));
            }

            if (httpContext.Items == null)
            {
                throw new ArgumentException(nameof(HttpContext.Items));
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
