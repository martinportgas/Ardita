using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Ardita.Areas.ArchiveInActive.Controllers;

[CustomAuthorize]
[Area(GlobalConst.ArchiveInActive)]
public class MediaStorageInActiveController : BaseController<TrxMediaStorageInActive>
{
    public MediaStorageInActiveController(
        IArchiveCreatorService archiveCreatorService,
        IMediaStorageInActiveService mediaStorageInActiveService,
        IClassificationSubjectService classificationSubjectService,
        IClassificationSubSubjectService classificationSubSubjectService,
        IArchiveService archiveService,
        IArchiveUnitService archiveUnitService,
        ITypeStorageService typeStorageService,
        IFloorService floorService,
        IRoomService roomService,
        IRackService rackService,
        ILevelService levelService,
        IRowService rowService,
        IGmdService gmdService,
        ISubTypeStorageService subTypeStorageService,
        IClassificationService classificationService,
        IHostingEnvironment hostingEnvironment
        )
    {
        _archiveCreatorService = archiveCreatorService;
        _MediaStorageInActiveService = mediaStorageInActiveService;
        _classificationSubjectService = classificationSubjectService;
        _classificationSubSubjectService = classificationSubSubjectService;
        _archiveService = archiveService;
        _archiveUnitService = archiveUnitService;
        _typeStorageService = typeStorageService;
        _floorService = floorService;
        _roomService = roomService;
        _rackService = rackService;
        _levelService = levelService;
        _rowService = rowService;
        _gmdService = gmdService;
        _subTypeStorageService = subTypeStorageService;
        _classificationService = classificationService;
        _hostingEnvironment = hostingEnvironment;
    }
    public override async Task<ActionResult> Index()
    {
        await AllViewBagIndex();
        return View();
    }

    public override async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            model.SessionUser = User;
            var result = await _MediaStorageInActiveService.GetList(model);

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
                await _MediaStorageInActiveService.Update(model, listSts!, listArchive!);
            }
            else
            {
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                await _MediaStorageInActiveService.Insert(model, listSts!, listArchive!);
            }
        }
        return RedirectToIndex();
    }

    public override async Task<IActionResult> Detail(Guid Id)
    {
        return await InitFormView(Id);
    }

    public override async Task<IActionResult> Update(Guid Id)
    {
        return await InitFormView(Id);
    }

    public override async Task<IActionResult> Remove(Guid Id)
    {
        return await InitFormView(Id);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Delete(TrxMediaStorageInActive model)
    {
        if (model != null && model.MediaStorageInActiveId != Guid.Empty)
        {
            model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
            model.UpdatedDate = DateTime.Now;
            await _MediaStorageInActiveService.Delete(model.MediaStorageInActiveId);
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
        ViewBag.listGMDDetail = await BindGmdDetail();
        ViewBag.listSubject = await BindAllSubjectClasscifications();
    }
    public async Task AllViewBagIndex()
    {
        ViewBag.ListArchiveUnit = await BindAllArchiveUnits();
        ViewBag.ListFloor = await BindFloors();
        ViewBag.ListRoom = await BindRooms();
        ViewBag.ListRack = await BindRacks();
        ViewBag.ListLevel = await BindLevels();
        ViewBag.ListRow = await BindRows();
        ViewBag.ListArchiveCreator = await BindArchiveCreators();
        ViewBag.ListClassification = await BindClasscifications();
        ViewBag.ListSubjectClassification = await BindSubjectClasscifications();
    }

    private async Task<IActionResult> InitFormView(Guid Id)
    {
        var data = await _MediaStorageInActiveService.GetById(Id);
        if (data is not null)
        {
            await BindAllDropdown();

            return View(GlobalConst.Form, data);
        }
        else
        {
            return RedirectToIndex();
        }
    }
    [HttpGet]
    public async Task<IActionResult> DownloadFile(Guid Id)
    {
        TrxMediaStorageInActive data = await _MediaStorageInActiveService.GetById(Id);

        string FilePath = Path.Combine(_hostingEnvironment.WebRootPath, "LabelArchiveInActive.docx");
        var file = Label.GenerateLabelInActive(FilePath, data, data.TrxMediaStorageInActiveDetails);

        return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, $"{data.MediaStorageInActiveCode}.pdf");
    }
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.MediaStorageInActive, new { Area = GlobalConst.ArchiveInActive });
}
