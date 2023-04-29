using Ardita.Controllers;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.ArchiveActive.Controllers;

[CustomAuthorize]
[Area(Const.ArchiveActive)]
public class MediaStorageController : BaseController<TrxMediaStorage>
{
    public override async Task<ActionResult> Index() => await base.Index();
}
