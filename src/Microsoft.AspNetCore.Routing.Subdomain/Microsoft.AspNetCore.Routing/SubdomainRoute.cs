using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Template;
using System.Text.Encodings.Web;
using Microsoft.Extensions.ObjectPool;
using Microsoft.AspNetCore.Routing.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing.Subdomain.Microsoft.AspNetCore.Routing;
using System;

namespace Microsoft.AspNetCore.Routing
{
    public class SubDomainRoute : Route
    {
        public string[] Hostnames { get; private set; }

        public string Subdomain { get; private set; }

        public SubDomainRoute(string[] hostnames, string subdomain, IRouter target, string routeName, string routeTemplate, RouteValueDictionary defaults, IDictionary<string, object> constraints,
           RouteValueDictionary dataTokens, IInlineConstraintResolver inlineConstraintResolver)
           : base(target, routeName, routeTemplate, defaults, constraints, dataTokens, inlineConstraintResolver)
        {
            Hostnames = hostnames;
            Subdomain = subdomain;
        }
        public override Task RouteAsync(RouteContext context)
        {
            var host = context.HttpContext.Request.Host.Value;

            string foundHostname = GetHostname(host);

            if (foundHostname == null && Subdomain != null)
                return Task.CompletedTask;

            if (Subdomain == null)
            {
                return base.RouteAsync(context);
            }

            var subdomain = host.Substring(0, host.IndexOf(GetHostname(host)) - 1);

            if (!IsParameterName(Subdomain) && subdomain.ToLower() != Subdomain.ToLower())
            {
                return Task.CompletedTask;
            }

            //that's for overriding default for subdomain
            if (IsParameterName(Subdomain))
            {
                context.RouteData.Values.Add(ParameterNameFrom(Subdomain), subdomain);
            }
            return base.RouteAsync(context);
        }

        protected override Task OnRouteMatched(RouteContext context)
        {
            if (Subdomain == null)
            {
                return base.OnRouteMatched(context);
            }
            var host = context.HttpContext.Request.Host.Value;
            var subdomain = host.Substring(0, host.IndexOf(GetHostname(host)) - 1);
            var routeData = new RouteData(context.RouteData);

            // this will allow to get value from example view via RouteData
            if (IsParameterName(Subdomain) && !routeData.Values.ContainsKey(ParameterNameFrom(Subdomain)))
            {
                routeData.Values.Add(ParameterNameFrom(Subdomain), subdomain);
            }

            context.RouteData = routeData;

            return base.OnRouteMatched(context);
        }

        public override VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            if (!(context is SubdomainVirtualPathContext))
            {
                return null;
            }

            if (Subdomain == null)
            {
                return GetVirtualPath(context, context.Values, BuildUrl(context));
            }

            var subdomainParameter = IsParameterName(Subdomain) ? ParameterNameFrom(Subdomain) : Subdomain;

            var containsSubdomainParameter = context.Values.ContainsKey(subdomainParameter);

            var defaultsContainsSubdomain = this.Defaults.ContainsKey(subdomainParameter);

            if (IsParameterName(Subdomain))
            {
                var sParameter = ParameterNameFrom(Subdomain);
                if (context.Values.ContainsKey(sParameter))
                {
                    return ParameterSubdomain(context, context.Values[subdomainParameter].ToString());
                }
                else if (this.Defaults.ContainsKey(sParameter))
                {
                    return ParameterSubdomain(context, this.Defaults[sParameter].ToString());
                }
            }
            else
            {
                if (!IsParameterName(Subdomain))
                {
                    return StaticSubdomain(context, subdomainParameter);
                }
            }

            return null;
        }

        private string GetHostname(string host)
        {
            foreach (var hostname in Hostnames)
            {
                if (!host.EndsWith(hostname) || host == hostname)
                {
                    continue;
                }

                return hostname;
            }

            return null;
        }

        private string ParameterNameFrom(string value)
        {
            return value.Substring(1, value.LastIndexOf("}") - 1);
        }

        private bool IsParameterName(string value)
        {
            if (value.StartsWith("{") && value.EndsWith("}"))
                return true;

            return false;
        }

        private bool EqualsToUrlParameter(string value, string urlParameter)
        {
            var param = ParameterNameFrom(urlParameter);

            return value.Equals(param);
        }

        private string CreateVirtualPathString(VirtualPathData vpd, RouteValueDictionary values)
        {
            var vp = vpd.VirtualPath;

            if (vp.Contains('?'))
            {
                return string.Format("{0}&{1}={2}", vp, Subdomain, values[Subdomain]);
            }
            else
            {
                return string.Format("{0}?{1}={2}", vp, Subdomain, values[Subdomain]);
            }
        }

        private AbsolutPathData StaticSubdomain(VirtualPathContext context, string subdomainParameter)
        {
            var hostBuilder = BuilSubdomaindUrl(context, subdomainParameter);

            return GetVirtualPath(context, context.Values, hostBuilder);
        }

        private AbsolutPathData ParameterSubdomain(VirtualPathContext context, string subdomainValue)
        {
            var hostBuilder = BuilSubdomaindUrl(context, subdomainValue);

            //we have to remove our subdomain so it will not be added as query string while using GetVirtualPath method
            var values = new RouteValueDictionary(context.Values);
            values.Remove(ParameterNameFrom(Subdomain));

            return GetVirtualPath(context, values, hostBuilder);
        }

        private AbsolutPathData GetVirtualPath(VirtualPathContext context, RouteValueDictionary routeValues, StringBuilder hostBuilder)
        {
            var path = base.GetVirtualPath(new VirtualPathContext(context.HttpContext, context.AmbientValues, routeValues));

            if (path == null) { return null; }

            return new AbsolutPathData(this, path.VirtualPath, hostBuilder.ToString());
        }

        private StringBuilder BuildUrl(VirtualPathContext context)
        {
            return BuildAbsoluteUrl(context, (hostBuilder, host) =>
            {
                hostBuilder
                    .Append(host);
            });
        }

        private StringBuilder BuilSubdomaindUrl(VirtualPathContext context, string subdomainValue)
        {
            return BuildAbsoluteUrl(context, (hostBuilder, host) =>
            {
                hostBuilder
                .Append(subdomainValue)
                .Append(".")
                .Append(host);
            });
        }

        private StringBuilder BuildAbsoluteUrl(VirtualPathContext context, Action<StringBuilder, string> buildAction)
        {
            string foundHostname = GetHostname(context.HttpContext.Request.Host.Value);

            string host = context.HttpContext.Request.Host.Value;
            if (!string.IsNullOrEmpty(foundHostname))
            {
                var subdomain = context.HttpContext.Request.Host.Value.Substring(0, context.HttpContext.Request.Host.Value.IndexOf(foundHostname) - 1);

                if (!string.IsNullOrEmpty(subdomain))
                {
                    host = foundHostname;
                }
            }

            var hostBuilder = new StringBuilder();
            hostBuilder
            .Append(context.HttpContext.Request.Scheme)
            .Append("://");
            buildAction(hostBuilder, host);
            return hostBuilder;
        }

        private TemplateBinder Binder(HttpContext context)
        {
            //that's from RouteBase.cs
            var urlEncoder = context.RequestServices.GetRequiredService<UrlEncoder>();
            var pool = context.RequestServices.GetRequiredService<ObjectPool<UriBuildingContext>>();
            return new TemplateBinder(urlEncoder, pool, ParsedTemplate, Defaults);
        }
    }
}
