﻿using Ardita.Controllers;
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
        IMediaStorageInActiveService mediaStorageInActiveService,
        IClassificationSubSubjectService classificationSubSubjectService,
        IArchiveService archiveService,
        IArchiveUnitService archiveUnitService,
        ITypeStorageService typeStorageService,
        IFloorService floorService,
        IRoomService roomService,
        IRackService rackService,
        ILevelService levelService,
        IRowService rowService,
        ISubTypeStorageService subTypeStorageService
        )
    {
        MediaStorageInActiveService = mediaStorageInActiveService;
        _classificationSubSubjectService = classificationSubSubjectService;
        _archiveService = archiveService;
        _archiveUnitService = archiveUnitService;
        _typeStorageService = typeStorageService;
        _floorService = floorService;
        _roomService = roomService;
        _rackService = rackService;
        _levelService = levelService;
        _rowService = rowService;
        _subTypeStorageService = subTypeStorageService;
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
        return View(GlobalConst.Form, new TrxMediaStorageInActive());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Save(TrxMediaStorageInActive model)
    {
        if (model is not null)
        {

            var listSts = Request.Form[GlobalConst.listSts].ToArray();
            var listArchive = Request.Form[GlobalConst.listArchive].ToArray();

            model.StatusId = Request.Form[GlobalConst.Submit].ToString() == GlobalConst.Submit ? (int)GlobalConst.STATUS.Submit : (int)GlobalConst.STATUS.Draft;


            if (model.MediaStorageInActiveId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                //await MediaStorageInActiveService.Update(model, archiveId);
            }
            else
            {
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                await MediaStorageInActiveService.Insert(model, listSts!, listArchive!);
            }
        }
        return RedirectToIndex();
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
        ViewBag.listSubTypeStorage = await BindSubTypeStorage();
    }

    private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.MediaStorageInActive, new { Area = GlobalConst.ArchiveInActive });
}
