using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreSubdomain.Samples.Areas.Area1.Controllers
{
    [Area("Area1")]
    public class TestController : Controller
    {
        public IActionResult Action1(string id)
        {
            return View();
        }
    }
}
