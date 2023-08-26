using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;


namespace Ardita.Areas.ArchiveActive.Controllers;

[CustomAuthorize]
[Area(GlobalConst.ArchiveActive)]
public class MediaStorageController : BaseController<TrxMediaStorage>
{
    #region CTR
    public MediaStorageController(
        IClassificationSubSubjectService classificationSubSubjectService,
        IClassificationSubjectService classificationSubjectService,
        IArchiveService archiveService,
        IArchiveUnitService archiveUnitService,
        ITypeStorageService typeStorageService,
        IFloorService floorService,
        IRoomService roomService,
        IRackService rackService,
        ILevelService levelService,
        IRowService rowService,
        IMediaStorageService mediaStorageService,
        ISubTypeStorageService subTypeStorageService,
        IHostingEnvironment hostingEnvironment,
        IGmdService gmdService,
        ISecurityClassificationService securityClassificationService,
        IArchiveOwnerService archiveOwnerService,
        IArchiveTypeService archiveTypeService,
        IArchiveCreatorService archiveCreatorService,
        IClassificationService classificationService,
        IArchiveOutIndicatorService archiveOutIndicatorService,
        IFileArchiveDetailService fileArchiveDetailService,
        ITemplateSettingService templateSettingService)
    {
        _classificationSubSubjectService = classificationSubSubjectService;
        _classificationSubjectService = classificationSubjectService;
        _archiveService = archiveService;
        _archiveUnitService = archiveUnitService;
        _typeStorageService = typeStorageService;
        _floorService = floorService;
        _roomService = roomService;
        _rackService = rackService;
        _levelService = levelService;
        _rowService = rowService;
        _mediaStorageService = mediaStorageService;
        SubTypeStorageService = subTypeStorageService;
        _hostingEnvironment = hostingEnvironment;
        _gmdService = gmdService;
        _securityClassificationService = securityClassificationService;
        _archiveOwnerService = archiveOwnerService;
        _archiveTypeService = archiveTypeService;
        _archiveCreatorService = archiveCreatorService;
        _classificationService = classificationService;
        _archiveOutIndicatorService = archiveOutIndicatorService;
        _fileArchiveDetailService = fileArchiveDetailService;
        _templateSettingService = templateSettingService;
    }
    #endregion

    #region MAIN ACTION
    public override async Task<ActionResult> Index()
    {
        await AllViewBagIndex();
        return await base.Index();
    }
    public override async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            model.SessionUser = User;
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
        var model = new TrxMediaStorage();
        model.MediaStorageCode = GlobalConst.InitialCode;
        return View(GlobalConst.Form, model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Save(TrxMediaStorage model)
    {
        if (model is not null)
        {
            string[] archiveId = Request.Form[GlobalConst.DetailArray].ToArray();
            model.StatusId = Request.Form[GlobalConst.Submit].ToString() == GlobalConst.Submit ? (int)GlobalConst.STATUS.Submit : (int)GlobalConst.STATUS.Draft;

            if (Request.Form[GlobalConst.Submit] == GlobalConst.IsUsed)
            {
                var detailIsUsed = Request.Form[GlobalConst.DetailIsUsedArray].ToArray();
                var usedBy = Request.Form[GlobalConst.UsedByArray].ToArray();
                var usedDate = Request.Form[GlobalConst.UsedDateArray].ToArray();
                var returnDate = Request.Form[GlobalConst.ReturnDateArray].ToArray();

                for (int i = 0; i < detailIsUsed.ToArray().Length; i++)
                {
                    if (!string.IsNullOrEmpty(usedBy[i]) && !string.IsNullOrEmpty(usedDate[i]))
                    {
                        var result = await _archiveOutIndicatorService.Process(model.MediaStorageId, detailIsUsed[i], usedBy[i], usedDate[i], returnDate[i], AppUsers.CurrentUser(User).UserId);

                        await _mediaStorageService.UpdateDetailIsUsed(new Guid(detailIsUsed[i]), usedBy[i], result);
                    }
                }        
            }
            else
            {
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

           
        }
        return RedirectToIndex();
    }
    public override async Task<IActionResult> Detail(Guid Id)
    {
        var data = await _mediaStorageService.GetById(Id);
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
    public async Task<IActionResult> DetailArchive(Guid Id)
    {
        var data = await _archiveService.GetById(Id);
        if (data is not null)
        {
            await BindAllDropdownArchive();
            ViewBag.isModal = true;
            return View("../Archive/Form", data);
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

            return View(GlobalConst.Form, data);
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

            return View(GlobalConst.Form, data);
        }
        else
        {
            return RedirectToIndex();
        }
    }
    public async Task<IActionResult> IsUsed(Guid Id)
    {
        var data = await _mediaStorageService.GetById(Id);
        if (data is not null)
        {
            await BindAllDropdown();

            ViewBag.DetailOutIndicatior = await _archiveOutIndicatorService.GetByMediaStorageId(Id);
            return View(GlobalConst.Form, data);
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
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.MediaStorage, new { Area = GlobalConst.ArchiveActive });
    protected async Task BindAllDropdown()
    {
        ViewBag.listSubSubject = await BindClasscificationSubjects();
        ViewBag.listArchive = await BindArchives();
        ViewBag.listArchiveUnit = await BindArchiveUnits();
        ViewBag.listTypeStorage = await BindTypeStorage();
        ViewBag.listFloor = await BindFloors();
        ViewBag.listRoom = await BindRooms();
        ViewBag.listRack = await BindRacks();
        ViewBag.listLevel = await BindLevels();
        ViewBag.listRow = await BindRows();
        ViewBag.listGMDDetail = await BindGmdDetail();
    }
    protected async Task BindAllDropdownArchive()
    {
        ViewBag.listGmd = await BindGmds();
        ViewBag.listSubSubjectClasscification = await BindClasscificationSubjects();
        ViewBag.listSecurityClassification = await BindSecurityClassifications();
        ViewBag.listArchiveOwner = await BindArchiveOwners();
        ViewBag.listArchiveType = await BindArchiveTypes();
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
    public async Task<FileResult> BindQrCode(string text)
    {
        await Task.Delay(0);

        var file = QRCodeExtension.Generate(text);

        return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, "QRCode.svg");
    }
    //public async Task<FileResult> BindLabel(string MediaStorageId)
    //{
    //    Guid Id = new(MediaStorageId);
    //    TrxMediaStorage data = await _mediaStorageService.GetById(Id);
    //    string FilePath = Path.Combine(_hostingEnvironment.WebRootPath, "LabelArchive.docx");
    //    var file = Label.GenerateLabelArchive(FilePath, data);

    //    return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, $"{data.Row.Level!.Rack!.RackName + "-" + data.Row.Level.LevelName + "-" + data.Row.RowName}.pdf");
    //}
    public async Task<FileResult> BindLabel(Guid MediaStorageId)
    {
        var settings = await _templateSettingService.GetAll();
        var setting = settings.Where(x => x.TemplateName == GlobalConst.TemplateMediaPenyimpananArsipAktif).FirstOrDefault();

        var data = await _templateSettingService.GetDataView(setting.SourceData, MediaStorageId);

        string FilePath = Path.Combine(_hostingEnvironment.WebRootPath, setting.Path);
        var file = Label.GenerateFromTemplate(setting.MstTemplateSettingDetails.ToList(), data, FilePath);

        TrxMediaStorage dataStorage = await _mediaStorageService.GetById(MediaStorageId);
        return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, $"{dataStorage.Row.Level!.Rack!.RackName + "-" + dataStorage.Row.Level.LevelName + "-" + dataStorage.Row.RowName}.pdf");
    }
    #endregion
}