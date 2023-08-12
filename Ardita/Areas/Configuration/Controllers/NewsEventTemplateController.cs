using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.Configuration.Controllers;

public class NewsEventTemplateController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
