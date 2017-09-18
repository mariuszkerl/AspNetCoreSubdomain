using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Microsoft.AspNetCore.Mvc.Routing
{
    /// <summary>
    /// An implementation of <see cref="IUrlHelper"/> that contains methods to
    /// build URLs for ASP.NET MVC within an application.
    /// </summary>
    public class SubdomainUrlHelper : UrlHelper, IUrlHelper
    {
        // Perf: Share the StringBuilder object across multiple calls of GenerateURL for this UrlHelper
        private StringBuilder _stringBuilder;
        // Perf: Reuse the RouteValueDictionary across multiple calls of Action for this UrlHelper
        private readonly RouteValueDictionary _routeValueDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlHelper"/> class using the specified
        /// <paramref name="actionContext"/>.
        /// </summary>
        /// <param name="actionContext">The <see cref="Mvc.ActionContext"/> for the current request.</param>
        public SubdomainUrlHelper(ActionContext actionContext) : base(actionContext)
        {
            _routeValueDictionary = new RouteValueDictionary();
        }

        /// <inheritdoc />
        public override string Action(UrlActionContext actionContext)
        {
            if (actionContext == null)
            {
                throw new ArgumentNullException(nameof(actionContext));
            }

            var valuesDictionary = GetValuesDictionary(actionContext.Values);

            if (actionContext.Action == null)
            {
                if (!valuesDictionary.ContainsKey("action") &&
                    AmbientValues.TryGetValue("action", out object action))
                {
                    valuesDictionary["action"] = action;
                }
            }
            else
            {
                valuesDictionary["action"] = actionContext.Action;
            }

            if (actionContext.Controller == null)
            {
                if (!valuesDictionary.ContainsKey("controller") &&
                    AmbientValues.TryGetValue("controller", out object controller))
                {
                    valuesDictionary["controller"] = controller;
                }
            }
            else
            {
                valuesDictionary["controller"] = actionContext.Controller;
            }
            
            var pathData = GetVirtualPathData(routeName: null, values: valuesDictionary);
            if (pathData is AbsolutPathData)
            {
                var absolutePathData = pathData as AbsolutPathData;

                if (absolutePathData.Host == HttpContext.Request.Host.Value && absolutePathData.Protocol == HttpContext.Request.Scheme)
                {
                    return GenerateUrl(null, null, pathData, actionContext.Fragment);
                }
                //we don't support changing protocol for subdomain
                return GenerateUrl(absolutePathData.Protocol, absolutePathData.Host, pathData, actionContext.Fragment);
            }
            return GenerateUrl(actionContext.Protocol, actionContext.Host, pathData, actionContext.Fragment);
        }

        /// <inheritdoc />
        public override bool IsLocalUrl(string url)
        {

            //todo: body
            return base.IsLocalUrl(url);
        }

        /// <inheritdoc />
        public override string RouteUrl(UrlRouteContext routeContext)
        {
            if (routeContext == null)
            {
                throw new ArgumentNullException(nameof(routeContext));
            }

            var valuesDictionary = routeContext.Values as RouteValueDictionary ?? GetValuesDictionary(routeContext.Values);
            var pathData = GetVirtualPathData(routeContext.RouteName, valuesDictionary);
            if(pathData is AbsolutPathData)
            {
                //we don't support changing protocol for subdomain
                return GenerateUrl(((AbsolutPathData)pathData).Protocol, ((AbsolutPathData)pathData).Host, pathData, routeContext.Fragment);
            }
            return GenerateUrl(routeContext.Protocol, routeContext.Host, pathData, routeContext.Fragment);
        }

        //// Internal for unit testing.
        //internal void AppendPathAndFragment(StringBuilder builder, VirtualPathData pathData, string fragment)
        //{
        //    var pathBase = HttpContext.Request.PathBase;

        //    if (!pathBase.HasValue)
        //    {
        //        if (pathData.VirtualPath.Length == 0)
        //        {
        //            builder.Append("/");
        //        }
        //        else
        //        {
        //            if (!pathData.VirtualPath.StartsWith("/", StringComparison.Ordinal))
        //            {
        //                builder.Append("/");
        //            }

        //            builder.Append(pathData.VirtualPath);
        //        }
        //    }
        //    else
        //    {
        //        if (pathData.VirtualPath.Length == 0)
        //        {
        //            builder.Append(pathBase.Value);
        //        }
        //        else
        //        {
        //            builder.Append(pathBase.Value);

        //            if (pathBase.Value.EndsWith("/", StringComparison.Ordinal))
        //            {
        //                builder.Length--;
        //            }

        //            if (!pathData.VirtualPath.StartsWith("/", StringComparison.Ordinal))
        //            {
        //                builder.Append("/");
        //            }

        //            builder.Append(pathData.VirtualPath);
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(fragment))
        //    {
        //        builder.Append("#").Append(fragment);
        //    }
        //}

        /// <inheritdoc />
        public override string Content(string contentPath)
        {
            //todo: body

            return base.Content(contentPath);
        }

        /// <inheritdoc />
        public override string Link(string routeName, object values)
        {
            //todo: body
            return base.Link(routeName, values);
        }

        private RouteValueDictionary GetValuesDictionary(object values)
        {
            // Perf: RouteValueDictionary can be cast to IDictionary<string, object>, but it is
            // special cased to avoid allocating boxed Enumerator.
            var routeValuesDictionary = values as RouteValueDictionary;
            if (routeValuesDictionary != null)
            {
                _routeValueDictionary.Clear();
                foreach (var kvp in routeValuesDictionary)
                {
                    _routeValueDictionary.Add(kvp.Key, kvp.Value);
                }

                return _routeValueDictionary;
            }

            var dictionaryValues = values as IDictionary<string, object>;
            if (dictionaryValues != null)
            {
                _routeValueDictionary.Clear();
                foreach (var kvp in dictionaryValues)
                {
                    _routeValueDictionary.Add(kvp.Key, kvp.Value);
                }

                return _routeValueDictionary;
            }

            return new RouteValueDictionary(values);
        }

        private StringBuilder GetStringBuilder()
        {
            if (_stringBuilder == null)
            {
                _stringBuilder = new StringBuilder();
            }

            return _stringBuilder;
        }
    }
}
