using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.Configuration.Controllers;


public class LabelTemplateController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
