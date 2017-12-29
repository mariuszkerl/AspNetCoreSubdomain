using AspNetCoreSubdomain.SubdomainsAreaWebSite;
using AspNetCoreSubdomain.WebSites.Core;
using Microsoft.AspNetCore.Mvc;

namespace RoutingWebSite
{
    public class HomeController : Controller
    {
        private readonly SubdomainRoutingResponseGenerator _generator;

        // We should not reach normal controller even if it exists
        // without defined normal route.

        public HomeController(SubdomainRoutingResponseGenerator generator)
        {
            _generator = generator;
        }

        public IActionResult Index()
        {
            return _generator.Generate("/", "/Home", "/Home/Index");
        }
    }
}