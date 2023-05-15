using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.ArchiveInActive.Controllers;

[CustomAuthorize]
[Area(GlobalConst.ArchiveInActive)]
public class ArchiveReceivedController : BaseController<TrxArchiveReceived>
{
    public ArchiveReceivedController(IArchiveReceivedService archiveReceivedService)
    {
        ArchiveReceivedService = archiveReceivedService;
    }
    public override async Task<ActionResult> Index() => await base.Index();

    public override async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            var result = await ArchiveReceivedService.GetList(model);

            return Json(result);

        }
        catch (Exception)
        {
            throw;
        }
    }
}
