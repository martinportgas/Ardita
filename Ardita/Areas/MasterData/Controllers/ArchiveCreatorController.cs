using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ardita.Areas.MasterData.Controllers;

[CustomAuthorize]
[Area(Const.MasterData)]
public class ArchiveCreatorController : Controller
{
    #region MEMBER AND CTR
    private readonly IArchiveUnitService _archiveUnitService;
    private readonly IArchiveCreatorService _archiveCreatorService;

    public ArchiveCreatorController(IArchiveUnitService archiveUnitService, IArchiveCreatorService archiveCreatorService)
    {
        _archiveUnitService = archiveUnitService;
        _archiveCreatorService = archiveCreatorService;
    }
    #endregion

    #region MAIN ACTION
    public IActionResult Index() => View();

    public async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            var result = await _archiveCreatorService.GetList(model);

            return Json(result);

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<IActionResult> Add()
    {
        ViewBag.listArchiveUnit = await BindArchiveUnit();

        await Task.Delay(0);

        return View(Const.Form, new MstCreator());
    }

    public async Task<IActionResult> Update(Guid Id)
    {
        var data = await _archiveCreatorService.GetById(Id);
        if (data.Any())
        {
            ViewBag.listArchiveUnit = await BindArchiveUnit();

            return View(Const.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public async Task<IActionResult> Remove(Guid Id)
    {
        var data = await _archiveCreatorService.GetById(Id);
        if (data.Any())
        {
            ViewBag.listArchiveUnit = await BindArchiveUnit();

            return View(Const.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public async Task<IActionResult> Detail(Guid Id)
    {
        var data = await _archiveCreatorService.GetById(Id);
        if (data.Any())
        {
            ViewBag.listArchiveUnit = await BindArchiveUnit();

            return View(Const.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(MstCreator model)
    {
        if (model != null)
        {
            if (model.CreatorId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _archiveCreatorService.Update(model);
            }

            else
            {
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                await _archiveCreatorService.Insert(model);
            }
        }
        return RedirectToIndex();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(MstCreator model)
    {
        if (model != null && model.ArchiveUnitId != Guid.Empty)
        {
            await _archiveCreatorService.Delete(model);
        }
        return RedirectToIndex();
    }
    #endregion

    #region HELPER
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.ArchiveCreator, new { Area = Const.MasterData });


    private async Task<List<SelectListItem>> BindArchiveUnit()
    {
        var company = await _archiveUnitService.GetAll();
        return company.Select(x => new SelectListItem
        {
            Value = x.ArchiveUnitId.ToString(),
            Text = x.ArchiveUnitName.ToString()
        }).ToList();
    }
    #endregion
}
