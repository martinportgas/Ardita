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
public class TypeStorageController : BaseController<TrxTypeStorage>
{
    public TypeStorageController(IArchiveUnitService archiveUnitService, IRoomService roomService, ITypeStorageService typeStorage)
    {
        _archiveUnitService = archiveUnitService;
        _roomService = roomService;
        _typeStorageService = typeStorage;
    }

    #region MAIN ACTION
    public override async Task<ActionResult> Index() => await base.Index();

    public override async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            var result = await _typeStorageService.GetList(model);

            return Json(result);

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public override async Task<IActionResult> Add()
    {
        ViewBag.listArchiveUnit = await BindArchiveUnits();
        ViewBag.listTypeStorage = await BindTypeStorage();

        return View(Const.Form, new TrxTypeStorage());
    }

    public override async Task<IActionResult> Update(Guid Id)
    {
        var data = await _typeStorageService.GetById(Id);
        if (data != null)
        {
            ViewBag.listArchiveUnit = await BindArchiveUnits();
            ViewBag.listTypeStorage = await BindTypeStorage();

            return View(Const.Form, data != null);
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public override async Task<IActionResult> Remove(Guid Id)
    {
        var data = await _typeStorageService.GetById(Id);
        if (data != null)
        {
            ViewBag.listArchiveUnit = await BindArchiveUnits();
            ViewBag.listTypeStorage = await BindTypeStorage();

            return View(Const.Form, data != null);
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public override async Task<IActionResult> Detail(Guid Id)
    {
        var data = await _typeStorageService.GetById(Id);
        if (data != null)
        {
            ViewBag.listArchiveUnit = await BindArchiveUnits();
            ViewBag.listTypeStorage = await BindTypeStorage();

            return View(Const.Form, data != null);
        }
        else
        {
            return RedirectToIndex();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Save(TrxTypeStorage model)
    {
        if (model != null)
        {
            if (model.TypeStorageId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _typeStorageService.Update(model);
            }

            else
            {
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                await _typeStorageService.Insert(model);
            }
        }
        return RedirectToIndex();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Delete(TrxTypeStorage model)
    {
        if (model != null && model.TypeStorageId != Guid.Empty)
        {
            await _typeStorageService.Delete(model);
        }
        return RedirectToIndex();
    }
    #endregion

    #region HELPER
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.TypeStorage, new { Area = Const.MasterData });

    #endregion
}
