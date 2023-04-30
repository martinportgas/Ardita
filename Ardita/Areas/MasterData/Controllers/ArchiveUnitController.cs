using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.MasterData.Controllers;

[CustomAuthorize]
[Area(Const.MasterData)]
public class ArchiveUnitController : BaseController<TrxArchiveUnit>
{
    #region MEMBER AND CTR
    public ArchiveUnitController(IArchiveUnitService archiveUnitService, ICompanyService companyService)
    {
        _archiveUnitService = archiveUnitService;
        _companyService = companyService;
    }
    #endregion

    #region MAIN ACTION
    public override async Task<ActionResult> Index() => await base.Index();

    public override async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            var result = await _archiveUnitService.GetList(model);

            return Json(result);

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public override async Task<IActionResult> Add()
    {
        ViewBag.listCompany = await BindCompanies();

        return View(Const.Form, new TrxArchiveUnit());
    }

    public override async Task<IActionResult> Update(Guid Id)
    {
        var data = await _archiveUnitService.GetById(Id);
        if (data != null)
        {
            ViewBag.listCompany = await BindCompanies();

            return View(Const.Form, data);
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public override async Task<IActionResult> Remove(Guid Id)
    {
        var data = await _archiveUnitService.GetById(Id);
        if (data != null)
        {
            ViewBag.listCompany = await BindCompanies();

            return View(Const.Form, data);
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public override async Task<IActionResult> Detail(Guid Id)
    {
        var data = await _archiveUnitService.GetById(Id);
        if (data != null)
        {
            ViewBag.listCompany = await BindCompanies();

            return View(Const.Form, data);
        }
        else
        {
            return RedirectToIndex();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Save(TrxArchiveUnit model)
    {
        if (model != null)
        {
            if (model.ArchiveUnitId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _archiveUnitService.Update(model);
            }

            else
            {
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                await _archiveUnitService.Insert(model);
            }
        }
        return RedirectToIndex();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Delete(TrxArchiveUnit model)
    {
        if (model != null && model.ArchiveUnitId != Guid.Empty)
        {
            await _archiveUnitService.Delete(model);
        }
        return RedirectToIndex();
    }
    #endregion

    #region HELPER
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.ArchiveUnit, new { Area = Const.MasterData });
    #endregion
}
