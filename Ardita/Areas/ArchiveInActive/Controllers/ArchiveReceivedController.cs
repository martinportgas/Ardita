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
        IHostingEnvironment hostingEnvironment
        )
    {
        ArchiveReceivedService = archiveReceivedService;
        _archiveMovementService = archiveMovementService;
        _typeStorageService = typeStorageService;
        _archiveApprovalService = archiveApprovalService;
        _employeeService = employeeService;
        _userService = userService;
        _hostingEnvironment = hostingEnvironment;
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

            model.StatusReceived = ApprovalAction == GlobalConst.Approve ? (int)GlobalConst.STATUS.ArchiveReceived : (int)GlobalConst.STATUS.Rejected;
            model.ReceivedBy = AppUsers.CurrentUser(User).UserId;
            model.DateReceived = DateTime.Now;
            await ArchiveReceivedService.Update(model);
           
        }
        return RedirectToIndex();
    }
    [HttpGet]
    public async Task<IActionResult> DownloadFile(Guid Id)
    {
        TrxArchiveMovement data = await _archiveMovementService.GetById(Id);

        var user = await _userService.GetById(data.CreatedBy);
        var employee = await _employeeService.GetById(user.EmployeeId);

        var userReceived = await _userService.GetById((Guid)data.ReceivedBy);
        var employeeReceived = await _employeeService.GetById(userReceived.EmployeeId);

        var detail = await _archiveMovementService.GetDetailByMainId(Id);

        string FilePath = Path.Combine(_hostingEnvironment.WebRootPath, "BA_Pemindahan_Arsip.docx");
        var file = Label.GenerateBAMovement(FilePath, data, detail, employee, employeeReceived);

        return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, $"{data.DocumentCode}.pdf");
    }
    #region HELPER
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.ArchiveReceived, new { Area = GlobalConst.ArchiveInActive });
    #endregion
}
