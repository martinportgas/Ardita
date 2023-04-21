using Ardita.Globals;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.MasterData.Controllers;

public class TypeStorageController : Controller
{
    [CustomAuthorize]
    [Area(Const.MasterData)]
    public IActionResult Index()
    {
        return View();
    }
}
