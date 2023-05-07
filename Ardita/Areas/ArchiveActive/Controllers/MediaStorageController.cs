using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;


namespace Ardita.Areas.ArchiveActive.Controllers;

[CustomAuthorize]
[Area(Const.ArchiveActive)]
public class MediaStorageController : BaseController<TrxMediaStorage>
{
    #region CTR
    public MediaStorageController(
        IClassificationSubSubjectService classificationSubSubjectService,
        IArchiveService archiveService,
        IArchiveUnitService archiveUnitService,
        ITypeStorageService typeStorageService,
        IFloorService floorService,
        IRoomService roomService,
        IRackService rackService,
        ILevelService levelService,
        IRowService rowService,
        IMediaStorageService mediaStorageService,
        IHostingEnvironment hostingEnvironment)
    {
        _classificationSubSubjectService = classificationSubSubjectService;
        _archiveService = archiveService;
        _archiveUnitService = archiveUnitService;
        _typeStorageService = typeStorageService;
        _floorService = floorService;
        _roomService = roomService;
        _rackService = rackService;
        _levelService = levelService;
        _rowService = rowService;
        _mediaStorageService = mediaStorageService;
        _hostingEnvironment = hostingEnvironment;
    }
    #endregion

    #region MAIN ACTION
    public override async Task<ActionResult> Index() => await base.Index();
    public override async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            QRCodeExtension.Generate("123");

            var result = await _mediaStorageService.GetList(model);

            return Json(result);

        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public override async Task<IActionResult> Add()
    {

        await BindAllDropdown();
        return View(Const.Form, new TrxMediaStorage());
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Save(TrxMediaStorage model)
    {
        if (model is not null)
        {
            string[] archiveId = Request.Form[Const.DetailArray].ToArray();

            if (model.MediaStorageId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _mediaStorageService.Update(model, archiveId);
            }
            else
            {
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                await _mediaStorageService.Insert(model, archiveId);
            }
        }
        return RedirectToIndex();
    }
    public override async Task<IActionResult> Detail(Guid Id)
    {
        var data = await _mediaStorageService.GetById(Id);
        if (data is not null)
        {
            await BindAllDropdown();

            return View(Const.Form, data);
        }
        else
        {
            return RedirectToIndex();
        }
    }
    public override async Task<IActionResult> Update(Guid Id)
    {
        var data = await _mediaStorageService.GetById(Id);
        if (data is not null)
        {
            await BindAllDropdown();

            return View(Const.Form, data);
        }
        else
        {
            return RedirectToIndex();
        }
    }
    public override async Task<IActionResult> Remove(Guid Id)
    {
        var data = await _mediaStorageService.GetById(Id);
        if (data is not null)
        {
            await BindAllDropdown();

            return View(Const.Form, data);
        }
        else
        {
            return RedirectToIndex();
        }
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Delete(TrxMediaStorage model)
    {
        if (model != null && model.MediaStorageId != Guid.Empty)
        {
            model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
            model.UpdatedDate = DateTime.Now;
            await _mediaStorageService.Delete(model);
        }
        return RedirectToIndex();
    }
    #endregion

    #region HELPER
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.MediaStorage, new { Area = Const.ArchiveActive });
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
    public async Task<FileResult> BindQrCode(string text)
    {
        await Task.Delay(0);

        var file = QRCodeExtension.Generate(text);

        return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, "QRCode.svg");
    }
    public async Task<FileResult> BindLabel(string MediaStorageId)
    {
        Guid Id = new(MediaStorageId);
        TrxMediaStorage data = await _mediaStorageService.GetById(Id);
        string FilePath = Path.Combine(_hostingEnvironment.WebRootPath, "LabelArchive.docx");
        var file = Label.GenerateLabelArchive(FilePath, data);

        return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, "Label.pdf");
    }
    #endregion
}