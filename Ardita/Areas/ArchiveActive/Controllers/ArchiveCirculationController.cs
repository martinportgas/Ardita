using Ardita.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.ArchiveActive.Controllers;

[CustomAuthorize]
[Area(GlobalConst.ArchiveActive)]
public class ArchiveCirculationController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Form()
    {
        await Task.Delay(0);
        return View(GlobalConst.Form);
    }
}
