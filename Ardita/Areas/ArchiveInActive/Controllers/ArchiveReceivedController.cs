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
public class ArchiveReceivedController : BaseController<TrxArchiveMovement>
{
    public ArchiveReceivedController(
        IArchiveReceivedService archiveReceivedService, 
        IArchiveMovementService archiveMovementService,
        ITypeStorageService typeStorageService,
        IArchiveApprovalService archiveApprovalService,
        IUserService userService,
        IEmployeeService employeeService,
        IMediaStorageService mediaStorageService,
        IHostingEnvironment hostingEnvironment,
        IArchiveService archiveService,
            IArchiveUnitService archiveUnitService,
        IClassificationService classificationService,
        IClassificationSubjectService classificationSubjectService,
        IFloorService floorService,
        IRoomService roomService,
        IRackService rackService,
        ILevelService levelService,
        IRowService rowService,
        IArchiveCreatorService archiveCreatorService,
            IClassificationSubSubjectService classificationSubSubjectService
        )
    {
        ArchiveReceivedService = archiveReceivedService;
        _archiveMovementService = archiveMovementService;
        _typeStorageService = typeStorageService;
        _archiveApprovalService = archiveApprovalService;
        _employeeService = employeeService;
        _userService = userService;
        _hostingEnvironment = hostingEnvironment;
        _mediaStorageService = mediaStorageService;
        _archiveService = archiveService;
        _archiveUnitService = archiveUnitService;
        _classificationSubSubjectService = classificationSubSubjectService;
        _classificationSubjectService = classificationSubjectService;
        _levelService = levelService;
        _rowService = rowService;
        _archiveCreatorService = archiveCreatorService;
        _classificationService = classificationService;
        _rackService = rackService;
        _roomService = roomService;
        _floorService = floorService;
    }
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
            var result = await ArchiveReceivedService.GetList(model);

            return Json(result);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IActionResult> AcceptArchiveForm(Guid Id)
    {
        var model = await _archiveMovementService.GetById(Id);
        if (model != null)
        {
            ViewBag.listTypeStorage = await BindTypeStorageByCompanyId(AppUsers.CurrentUser(User).CompanyId);
            ViewBag.subDetail = await _archiveMovementService.GetDetailByMainId(Id);
            ViewBag.approval = await _archiveApprovalService.GetByTransIdandApprovalCode(Id, GlobalConst.ArchiveMovement);
            return View(GlobalConst.Form, model);
        }
        else
        {
            return RedirectToIndex();
        }
    }
    public override async Task<IActionResult> Detail(Guid Id)
    {
        var model = await _archiveMovementService.GetById(Id);
        if (model != null)
        {
            ViewBag.listTypeStorage = await BindTypeStorageByCompanyId(AppUsers.CurrentUser(User).CompanyId);
            ViewBag.subDetail = await _archiveMovementService.GetDetailByMainId(Id);
            ViewBag.approval = await _archiveApprovalService.GetByTransIdandApprovalCode(Id, GlobalConst.ArchiveMovement);
            return View(GlobalConst.Form, model);
        }
        else
        {
            return RedirectToIndex();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcceptArchiveAction(TrxArchiveMovement model)
    {
        if (model is not null)
        {
            var ApprovalAction = Request.Form[GlobalConst.Approval];

            model.StatusId = ApprovalAction == GlobalConst.Approve ? (int)GlobalConst.STATUS.ArchiveReceived : (int)GlobalConst.STATUS.Rejected;
            model.StatusReceived = ApprovalAction == GlobalConst.Approve ? (int)GlobalConst.STATUS.ArchiveReceived : (int)GlobalConst.STATUS.Rejected;
            model.ReceivedBy = AppUsers.CurrentUser(User).UserId;
            model.DateReceived = DateTime.Now;
            await ArchiveReceivedService.Update(model);

            if (ApprovalAction == GlobalConst.Approve)
            {
                var modelDetail = await _archiveMovementService.GetDetailByMainId(model.ArchiveMovementId);
                if (modelDetail.Any())
                {
                    foreach (var item in modelDetail)
                    {
                        var mediaStorage = await _mediaStorageService.GetDetailByArchiveId(item.ArchiveId);
                        if (mediaStorage != null)
                        {
                            mediaStorage.IsActive = false;
                            mediaStorage.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                            mediaStorage.UpdatedDate = DateTime.Now;

                            await _mediaStorageService.UpdateDetail(mediaStorage);
                        }

                        var archive = await _archiveService.GetById(item.ArchiveId);
                        if (archive != null)
                        {
                            archive.IsArchiveActive = false;
                            archive.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                            archive.UpdatedDate = DateTime.Now;
                            archive.InactiveBy = AppUsers.CurrentUser(User).UserId;
                            archive.InactiveDate = DateTime.Now;

                            await _archiveService.Update(archive, "", new string[] { });
                        }
                    }
                }
            }

        }
        return RedirectToIndex();
    }
    [HttpGet]
    public async Task<IActionResult> DownloadFile(Guid Id)
    {
        //TrxArchiveMovement data = await _archiveMovementService.GetById(Id);

        //var user = await _userService.GetById(data.CreatedBy);
        //var employee = await _employeeService.GetById(user.EmployeeId);

        //var userReceived = await _userService.GetById((Guid)data.ReceivedBy);
        //var employeeReceived = await _employeeService.GetById(userReceived.EmployeeId);

        //var detail = await _archiveMovementService.GetDetailByMainId(Id);

        //string FilePath = Path.Combine(_hostingEnvironment.WebRootPath, "BA_Pemindahan_Arsip.docx");
        //var file = Label.GenerateBAMovement(FilePath, data, detail, employee, employeeReceived);

        //return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, $"{data.DocumentCode}.pdf");


        var settings = await _templateSettingService.GetAll();
        var setting = settings.Where(x => x.TemplateName == GlobalConst.TemplatePenerimaanArsip).FirstOrDefault();

        var data = await _templateSettingService.GetDataView(setting.SourceData, Id);

        string FilePath = Path.Combine(_hostingEnvironment.WebRootPath, setting.Path);
        var file = Label.GenerateFromTemplate(setting.MstTemplateSettingDetails.ToList(), data, FilePath);

        TrxArchiveMovement dataMain = await _archiveMovementService.GetById(Id);
        return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, $"{GlobalConst.TemplatePenerimaanArsip.Replace(" ", "")}-{dataMain.DocumentCode}.pdf");
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
    #region HELPER
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.ArchiveReceived, new { Area = GlobalConst.ArchiveInActive });
    #endregion
}
