using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.Configuration.Controllers;

public class RentSettingsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
