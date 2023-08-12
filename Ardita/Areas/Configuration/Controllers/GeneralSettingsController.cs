using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.DbModels;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.Configuration.Controllers;

[CustomAuthorize]
[Area(GlobalConst.Configuration)]
public class GeneralSettingsController : BaseController<MstGeneralSetting>
{
    public override async Task<ActionResult> Index() => await base.Index();

    public override async Task<IActionResult> Add()
    {
        await Task.Delay(0);
        return View(GlobalConst.Form, new MstGeneralSetting());
    }

}
