using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.ArchiveInActive.Controllers;

[CustomAuthorize]
[Area(GlobalConst.ArchiveInActive)]
public class MediaStorageInActiveController : BaseController<TrxMediaStorageInActive>
{
    public MediaStorageInActiveController(
        IMediaStorageInActiveService mediaStorageInActiveService
        )
    {
        MediaStorageInActiveService = mediaStorageInActiveService;
    }
    public override async Task<ActionResult> Index() => await base.Index();

    public override async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            var result = await MediaStorageInActiveService.GetList(model);

            return Json(result);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public override async Task<IActionResult> Add()
    {
        await BindAllDropdown();
        return View(GlobalConst.Form, new TrxMediaStorage());
    }

    protected async Task BindAllDropdown()
    {
        ViewBag.listSubSubject = await BindSubSubjectClasscifications();
        ViewBag.listArchive = await BindArchives();
        ViewBag.listArchiveUnit = await BindArchiveUnits();
        ViewBag.listTypeStorage = await BindTypeStorage();
        ViewBag.listFloor = await BindFloors();
        ViewBag.listRoom = await BindRooms();
        ViewBag.listRack = await BindRacks();
        ViewBag.listLevel = await BindLevels();
        ViewBag.listRow = await BindRows();
    }
}
