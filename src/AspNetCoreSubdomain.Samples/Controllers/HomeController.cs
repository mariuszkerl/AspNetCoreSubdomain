using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreSubdomain.Samples.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string model)
        {
            return View(model: model);
        }

        public IActionResult Action1()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Action1(string model)
        {
            return View(model: model);
        }

        public IActionResult Action2(string parameter1)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Action2(string parameter1, string model)
        {
            return View(model: model);
        }
        public IActionResult Action3(string parameter2, string id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Action3(string parameter2, string id, string model)
        {
            return View(model: model);
        }
        public IActionResult Action4(string id)
        {
            return View();
        }
        [HttpPost]
        public IActionResult Action4(string id, string model)
        {
            return View(model: model);
        }
        public IActionResult Action5(string id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Action5(string id, string model)
        {
            return View(model: model);
        }
        public IActionResult SubdomainsPage()
        {
            return View();
        }
        public IActionResult Forms()
        {
            return View();
        }
        public IActionResult SubdomainFormsPage()
        {
            return View();
        }
    }
}
