using Microsoft.AspNetCore.Mvc;
using AspNetCoreSubdomain.WebSites.Core;
using System;

namespace AspNetCoreSubdomain.IntConstraintWebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly SubdomainRoutingResponseGenerator _generator;

        public HomeController(SubdomainRoutingResponseGenerator generator)
        {
            _generator = generator;
        }

        public IActionResult Index(int parameter)
        {
            return _generator.Generate("/", "/Home", "/Home/Index");
        }

        public IActionResult Boolean(bool parameter)
        {
            return _generator.Generate("/Home/Boolean");
        }
        public IActionResult Guid(Guid id)
        {
            return _generator.Generate("/Home/Guid");
        }
    }
}