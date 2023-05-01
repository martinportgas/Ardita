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
    #region CTR
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
    #endregion

    public override async Task<ActionResult> Index() => await base.Index();
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
    public override async Task<IActionResult> Add()
    {
        await BindAllDropdown();

        return View(Const.Form, new TrxArchive());
    }
    public override async Task<IActionResult> Update(Guid Id)
    {
        var data = await _archiveService.GetById(Id);
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
    public override async Task<IActionResult> Save(TrxArchive model)
    {
        if (model is not null)
        {
            var files = Request.Form[Const.Files];

            if (model.ArchiveId != Guid.Empty)
            {
                string[] filesDeleted = Request.Form[Const.IdFileDeletedArray].ToArray();

                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _archiveService.Update(model, files, filesDeleted);
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
    public override async Task<IActionResult> Remove(Guid Id)
    {
        var data = await _archiveService.GetById(Id);
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
    public override async Task<IActionResult> Delete(TrxArchive model)
    {
        if (model != null && model.ArchiveId != Guid.Empty)
        {
            await _archiveService.Delete(model);
        }
        return RedirectToIndex();
    }
    public override async Task<IActionResult> Detail(Guid Id)
    {
        var data = await _archiveService.GetById(Id);
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
    #region HELPER
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.Archive, new { Area = Const.ArchiveActive });

    protected async Task BindAllDropdown()
    {
        ViewBag.listGmd = await BindGmds();
        ViewBag.listSubSubjectClasscification = await BindSubSubjectClasscifications();
        ViewBag.listSecurityClassification = await BindSecurityClassifications();
    }

    [HttpGet]
    public IActionResult BindDownload(string path) => File(System.IO.File.OpenRead(path), "application/octet-stream", Path.GetFileName(path));
    #endregion 
}
