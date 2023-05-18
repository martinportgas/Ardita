using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.ArchiveInActive.Controllers;

[CustomAuthorize]
[Area(GlobalConst.ArchiveInActive)]
public class ArchiveReceivedController : BaseController<TrxArchiveMovement>
{
    public ArchiveReceivedController(
        IArchiveReceivedService archiveReceivedService, 
        IArchiveMovementService archiveMovementService,
        ITypeStorageService typeStorageService,
        IArchiveApprovalService archiveApprovalService
        )
    {
        ArchiveReceivedService = archiveReceivedService;
        _archiveMovementService = archiveMovementService;
        _typeStorageService = typeStorageService;
        _archiveApprovalService = archiveApprovalService;
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcceptArchiveAction(TrxArchiveMovement model)
    {
        if (model is not null)
        {
            model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
            model.UpdatedDate = DateTime.Now;
            await ArchiveReceivedService.Update(model);
           
        }
        return RedirectToIndex();
    }
    #region HELPER
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.ArchiveReceived, new { Area = GlobalConst.ArchiveInActive });
    #endregion
}
