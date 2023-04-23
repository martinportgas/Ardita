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
public class TypeStorageController : Controller
{
    private readonly IArchiveUnitService _archiveUnitService;
    private readonly IRoomService _roomService;
    private readonly ITypeStorageService _typeStorage;

    public TypeStorageController(IArchiveUnitService archiveUnitService, IRoomService roomService, ITypeStorageService typeStorage)
    {
        _archiveUnitService = archiveUnitService;
        _roomService = roomService;
        _typeStorage = typeStorage;
    }

    #region MAIN ACTION
    public IActionResult Index() => View();

    public async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            var result = await _typeStorage.GetList(model);

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
        ViewBag.listTypeStorage = await BindTypeStorage();

        return View(Const.Form, new TrxTypeStorage());
    }

    public async Task<IActionResult> Update(Guid Id)
    {
        var data = await _typeStorage.GetById(Id);
        if (data.Any())
        {
            ViewBag.listArchiveUnit = await BindArchiveUnit();
            ViewBag.listTypeStorage = await BindTypeStorage();

            return View(Const.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public async Task<IActionResult> Remove(Guid Id)
    {
        var data = await _typeStorage.GetById(Id);
        if (data.Any())
        {
            ViewBag.listArchiveUnit = await BindArchiveUnit();
            ViewBag.listTypeStorage = await BindTypeStorage();

            return View(Const.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public async Task<IActionResult> Detail(Guid Id)
    {
        var data = await _typeStorage.GetById(Id);
        if (data.Any())
        {
            ViewBag.listArchiveUnit = await BindArchiveUnit();
            ViewBag.listTypeStorage = await BindTypeStorage();

            return View(Const.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(TrxTypeStorage model)
    {
        if (model != null)
        {
            if (model.TypeStorageId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _typeStorage.Update(model);
            }

            else
            {
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                await _typeStorage.Insert(model);
            }
        }
        return RedirectToIndex();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(TrxTypeStorage model)
    {
        if (model != null && model.TypeStorageId != Guid.Empty)
        {
            await _typeStorage.Delete(model);
        }
        return RedirectToIndex();
    }
    #endregion

    #region HELPER
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.TypeStorage, new { Area = Const.MasterData });

    private async Task<List<SelectListItem>> BindArchiveUnit()
    {
        var data = await _archiveUnitService.GetAll();
        return data.Select(x => new SelectListItem
        {
            Value = x.ArchiveUnitId.ToString(),
            Text = x.ArchiveUnitName.ToString()
        }).ToList();
    }

    private async Task<List<SelectListItem>> BindTypeStorage()
    {
        var data = await _typeStorage.GetAll();
        return data.Select(x => new SelectListItem
        {
            Value = x.TypeStorageId.ToString(),
            Text = x.TypeStorageName.ToString()
        }).ToList();
    }
    #endregion
}
