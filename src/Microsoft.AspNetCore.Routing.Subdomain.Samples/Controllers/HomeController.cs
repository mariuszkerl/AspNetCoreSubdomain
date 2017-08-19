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

        public IActionResult Action1()
        {
            return View();
        }

        public IActionResult Action2(string parameter1)
        {
            return View();
        }
        public IActionResult Action3(string parameter2, string id)
        {
            return View();
        }
        public IActionResult Action4(string id)
        {
            return View();
        }
        public IActionResult SubdomainsPage()
        {
            return View();
        }
    }
}
