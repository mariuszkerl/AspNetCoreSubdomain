using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.AspNetCore.Routing.Subdomain.Samples.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index(string language)
        {
            return View();
        }
        public IActionResult Action1(string id)
        {
            return View();
        }
        public IActionResult Action2(string id)
        {
            return View();
        }
    }
}