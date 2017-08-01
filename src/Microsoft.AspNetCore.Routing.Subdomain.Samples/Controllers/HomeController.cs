using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.AspNetCore.Routing.Subdomain.Samples.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult StaticSubdomain()
        {
            return View();
        }

        public IActionResult ParameterFromSubdomain(string yourParameterName)
        {
            return View();
        }
        public IActionResult Parameters(string yourParameterName2, string id)
        {
            return View();
        }
    }
}
