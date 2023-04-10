using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.General.Controllers
{
    [Area("General")]
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ErrorAuthorized()
        {
            return View();
        }
    }
}
