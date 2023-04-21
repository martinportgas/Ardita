using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ardita.Areas.MasterData.Controllers;

[CustomAuthorize]
[Area(Const.MasterData)]
public class ArchiveUnitController : Controller
{
    #region MEMBER AND CTR
    private readonly IArchiveUnitService _archiveUnitService;
    private readonly ICompanyService _companyService;

    public ArchiveUnitController(IArchiveUnitService archiveUnitService, ICompanyService companyService)
    {
        _archiveUnitService = archiveUnitService;
        _companyService = companyService;
    }
    #endregion

    #region MAIN ACTION
    public IActionResult Index() => View();

    public async Task<JsonResult> GetData(DataTablePostModel model)
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

    public async Task<IActionResult> Add()
    {
        ViewBag.listCompany = BindCompany();

        await Task.Delay(0);

        return View(Const.Form, new TrxArchiveUnit());
    }

    public async Task<IActionResult> Update(Guid Id)
    {
        var data = await _archiveUnitService.GetById(Id);
        if (data.Any())
        {
            ViewBag.listCompany = BindCompany();

            return View(Const.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public async Task<IActionResult> Remove(Guid Id)
    {
        var data = await _archiveUnitService.GetById(Id);
        if (data.Any())
        {
            ViewBag.listCompany = BindCompany();

            return View(Const.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public async Task<IActionResult> Detail(Guid Id)
    {
        var data = await _archiveUnitService.GetById(Id);
        if (data.Any())
        {
            ViewBag.listCompany = BindCompany();

            return View(Const.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(TrxArchiveUnit model)
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
    public async Task<IActionResult> Delete(TrxArchiveUnit model)
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

    private async Task<SelectList> BindCompany() => new SelectList(await _companyService.GetAll(), nameof(MstCompany.CompanyId), nameof(MstCompany.CompanyName));
    #endregion
}
