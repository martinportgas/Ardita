using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.ArchiveActive.Controllers;

[CustomAuthorize]
[Area(Const.ArchiveActive)]
public class ArchiveController : BaseController<TrxArchive>
{
    public ArchiveController(
        IArchiveService archiveService, 
        IGmdService gmdService,
        IClassificationSubSubjectService classificationSubSubjectService,
        ISecurityClassificationService securityClassificationService,
        IArchiveCreatorService archiveCreatorService)
    {
        _archiveService = archiveService;
        _gmdService = gmdService;
        _classificationSubSubjectService = classificationSubSubjectService;
        _securityClassificationService = securityClassificationService;
        _archiveCreatorService = archiveCreatorService;
    }

    public override async Task<ActionResult> Index() => await base.Index();

    public override async Task<IActionResult> Add()
    {
        ViewBag.listGmd = await BindGmds();
        ViewBag.listSubSubjectClasscification = await BindSubSubjectClasscifications();
        ViewBag.listSecurityClassification = await BindSecurityClassifications();

        return View(Const.Form, new TrxArchive());
    }
    public override async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            var result = await _archiveService.GetList(model);

            return Json(result);

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Save(TrxArchive model)
    {
        if (model is not null)
        {
            var files = Request.Form[Const.Files];

            if (model.ArchiveId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _archiveService.Update(model);
            }
            else
            {
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                await _archiveService.Insert(model, files);
            }
        }
        return RedirectToIndex();
    }

    private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.Archive, new { Area = Const.ArchiveActive });

}
