using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Template;

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Routing.Constraints;

namespace Microsoft.AspNetCore.Routing
{
    public class SubDomainRoute : Route
    {

        private static readonly string w3 = "www.";
        private static readonly string w3Regex = "^www.";
        private static readonly IDictionary<string, Type> _unavailableConstraints = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
        {
            { "datetime", typeof(DateTimeRouteConstraint) },
            { "decimal", typeof(DecimalRouteConstraint) },
            { "double", typeof(DoubleRouteConstraint) },
            { "float", typeof(FloatRouteConstraint) },
        };

        private readonly IDictionary<string, IRouteConstraint> constraintsWithSubdomainConstraint;

        public string[] Hostnames { get; private set; }

        public string Subdomain { get; private set; }

        public RouteTemplate SubdomainParsed { get; private set; }

        public SubDomainRoute(string[] hostnames, string subdomain, IRouter target, string routeName, string routeTemplate, RouteValueDictionary defaults, IDictionary<string, object> constraints,
           RouteValueDictionary dataTokens, IInlineConstraintResolver inlineConstraintResolver, IOptions<RouteOptions> routeOptions)
           : base(target, routeName, routeTemplate, defaults, constraints, dataTokens, inlineConstraintResolver)
        {
            Hostnames = hostnames;
            
            if (string.IsNullOrEmpty(subdomain))
            {
                return;
            }

            SubdomainParsed = TemplateParser.Parse(subdomain);
            Constraints = GetConstraints(inlineConstraintResolver, TemplateParser.Parse(routeTemplate), constraints);

            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing
            constraintsWithSubdomainConstraint = GetConstraints(ConstraintResolver, SubdomainParsed, null);

            Defaults = GetDefaults(SubdomainParsed, Defaults);
            //Defaults = GetDefaults(TemplateParser.Parse(routeTemplate), defaults);


            if (constraintsWithSubdomainConstraint.Count == 1)
            {
                Subdomain = RemoveConstraint(subdomain);
            }
            else
            {
                Subdomain = subdomain;
            }

            if (IsParameterName(Subdomain))
            {
                if (constraintsWithSubdomainConstraint.Any(x => _unavailableConstraints.Values.Contains(x.Value.GetType()) && x.Key == ParameterNameFrom(Subdomain)))
                {
                    throw new ArgumentException($"Constraint invalid on subdomain! " +
                        $"Constraints: {string.Join(Environment.NewLine, _unavailableConstraints.Select(x => x.Key))}{Environment.NewLine}are unavailable for subdomain.");
                }

                foreach (var c in Constraints)
                {
                    constraintsWithSubdomainConstraint.Add(c);
                }

                if (Constraints.Keys.Contains(ParameterNameFrom(subdomain)))
                {
                    Constraints.Remove(ParameterNameFrom(subdomain));
                }
            }
        }

        public override Task RouteAsync(RouteContext context)
        {
            var host = context.HttpContext.Request.Host.Value;

            string foundHostname = GetHostname(host);

            if (foundHostname == null && Subdomain != null)
            {
                return Task.CompletedTask;
            }

            if (Subdomain == null)
            {
                if (foundHostname != null)
                {
                    return Task.CompletedTask;
                }

                return base.RouteAsync(context);
            }

            var subdomain = host.Substring(0, host.IndexOf(GetHostname(host)) - 1);

            if (!IsParameterName(Subdomain) && subdomain.ToLower() != Subdomain.ToLower())
            {
                return Task.CompletedTask;
            }

            var parsedTemplate = TemplateParser.Parse(Subdomain);
            //that's for overriding default for subdomain
            if (IsParameterName(Subdomain) &&
                Defaults.ContainsKey(ParameterNameFrom(Subdomain)) &&
                !context.RouteData.Values.ContainsKey(ParameterNameFrom(Subdomain)))
            {
                context.RouteData.Values.Add(ParameterNameFrom(Subdomain), subdomain);
            }

            if (IsParameterName(Subdomain) &&
                constraintsWithSubdomainConstraint.ContainsKey(ParameterNameFrom(Subdomain)))
            {
                if (!RouteConstraintMatcher.Match(
                        constraintsWithSubdomainConstraint,
                        new RouteValueDictionary
                        {
                            {  ParameterNameFrom(Subdomain), subdomain }
                        },
                        context.HttpContext,
                        this,
                        RouteDirection.IncomingRequest,
                        context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger(typeof(RouteConstraintMatcher).FullName)))
                {
                    return Task.CompletedTask;
                }
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

            if (IsParameterName(Subdomain))
            {
                //override default
                if (Defaults.ContainsKey(ParameterNameFrom(Subdomain)) && routeData.Values.ContainsKey(ParameterNameFrom(Subdomain)))
                {
                    routeData.Values[ParameterNameFrom(Subdomain)] = subdomain;
                }
                //or add this which will allow to get value from example view via RouteData
                else if (!routeData.Values.ContainsKey(ParameterNameFrom(Subdomain)))
                {
                    routeData.Values.Add(ParameterNameFrom(Subdomain), subdomain);
                }
            }

            context.RouteData = routeData;

            return base.OnRouteMatched(context);
        }

        public override VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            if (Subdomain == null)
            {
                //if route is without subdomain and we are on host without subdomain use base method
                //we don't need whole URL for such case
                if (GetHostname(context.HttpContext.Request.Host.Value) == null)
                {
                    return base.GetVirtualPath(context);
                }

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
            var nonW3Host = System.Text.RegularExpressions.Regex.Replace(host, w3Regex, "");
            foreach (var hostname in Hostnames)
            {
                if (!nonW3Host.EndsWith(hostname) || nonW3Host == hostname)
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

            return new AbsolutPathData(this, path.VirtualPath, hostBuilder.ToString(), context.HttpContext.Request.Scheme);
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

            string host = System.Text.RegularExpressions.Regex.Replace(context.HttpContext.Request.Host.Value, w3Regex, "");
            if (!string.IsNullOrEmpty(foundHostname))
            {
                var subdomain = host.Substring(0, host.IndexOf(foundHostname) - 1);

                if (!string.IsNullOrEmpty(subdomain))
                {
                    host = foundHostname;
                }
            }

            var hostBuilder = new StringBuilder();

            if (context.HttpContext.Request.Host.Value.StartsWith(w3))
            {
                hostBuilder.Append(w3);
            }

            buildAction(hostBuilder, host);

            return hostBuilder;
        }

        private string RemoveConstraint(string segment)
        {
            return $"{segment.Substring(0, segment.IndexOf(':'))}}}";
        }
    }
}
