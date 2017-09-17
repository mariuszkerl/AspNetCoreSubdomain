using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreSubdomain.SubdomainsAreaWebSite.Areas.Controllers
{
    [Area("Area1")]
    public class HomeController : Controller
    {
        private readonly SubdomainRoutingResponseGenerator _generator;

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
