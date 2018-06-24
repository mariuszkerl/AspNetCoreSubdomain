using Microsoft.AspNetCore.Mvc;
using AspNetCoreSubdomain.WebSites.Core;

namespace AspNetCoreSubdomain.IntConstraintWebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly SubdomainRoutingResponseGenerator _generator;

        public HomeController(SubdomainRoutingResponseGenerator generator)
        {
            _generator = generator;
        }

        public IActionResult Index(int? parameter = 0)
        {
            return _generator.Generate("/", "/Home", "/Home/Index");
        }
    }
}