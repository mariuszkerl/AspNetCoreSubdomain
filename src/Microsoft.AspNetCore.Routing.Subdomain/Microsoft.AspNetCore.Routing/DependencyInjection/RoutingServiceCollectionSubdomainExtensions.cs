using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RoutingServiceCollectionSubdomainExtensions
    {
        public static IServiceCollection AddSubdomains(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddSingleton(typeof(IUrlHelperFactory), typeof(SubdomainUrlHelperFactory));

            return services;
        }
    }
}
