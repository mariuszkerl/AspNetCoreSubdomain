using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreSubdomain.Samples.Controllers
{
    public class TestController : Controller
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
        public IActionResult Action1(string id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Action1(string id, string model)
        {
            return View(model: model);
        }
        public IActionResult Action2(string id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Action2(string id, string model)
        {
            return View(model: model);
        }
    }
}