using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreSubdomain.WebSites.Core;

namespace AspNetCoreSubdomain.RoutesWithoutSubdomain.Controllers
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