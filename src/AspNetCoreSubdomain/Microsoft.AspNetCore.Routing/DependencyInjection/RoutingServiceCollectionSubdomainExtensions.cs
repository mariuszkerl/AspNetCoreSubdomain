using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains methods injecting subdomain dependencies into <see cref="IServiceCollection"/>.
    /// </summary>
    public static class RoutingServiceCollectionSubdomainExtensions
    {
        /// <summary>
        /// Injects subdomain dependencies into <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">Services to inject to.</param>
        /// <returns>Returns updated instance of <see cref="IServiceCollection"/>.</returns>
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
