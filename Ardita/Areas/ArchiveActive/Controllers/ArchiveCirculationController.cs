using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.ArchiveActive.Controllers;

[CustomAuthorize]
[Area(GlobalConst.ArchiveActive)]
public class ArchiveCirculationController : BaseController<TrxArchiveMovement>
{
    #region MEMBER AND CTR
    public ArchiveCirculationController(
        IArchiveDestroyService archiveDestroyService,
        IArchiveExtendService archiveExtendService,
        IArchiveMovementService archiveMovementService,
        IArchiveService archiveService,
        IEmployeeService employeeService,
        IArchiveRetentionService archiveRetentionService,
        IArchiveApprovalService archiveApprovalService,
        ITypeStorageService typeStorageService,
        IMediaStorageService mediaStorageService)
    {
        _archiveExtendService = archiveExtendService;
        _employeeService = employeeService;
        _archiveRetentionService = archiveRetentionService;
        _archiveApprovalService = archiveApprovalService;
        _archiveService = archiveService;
        _archiveDestroyService = archiveDestroyService;
        _archiveMovementService = archiveMovementService;
        _typeStorageService = typeStorageService;
        _mediaStorageService = mediaStorageService;
    }
    #endregion
    public override async Task<ActionResult> Index() => await base.Index();
    public override async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            var result = await _archiveMovementService.GetList(model);

            return Json(result);

        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public async Task<IActionResult> Form()
    {
        await Task.Delay(0);
        return View(GlobalConst.Form);
    }
}
