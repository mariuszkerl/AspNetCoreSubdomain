using Microsoft.AspNetCore.Mvc;
using AspNetCoreSubdomain.WebSites.Core;
using System;

namespace AspNetCoreSubdomain.ConstraintsWebSite.Controllers
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
        public IActionResult Guid(Guid parameter)
        {
            return _generator.Generate("/Home/Guid");
        }
        public IActionResult Long(long parameter)
        {
            return _generator.Generate("/Home/Long");
        }
    }
}