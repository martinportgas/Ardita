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
public class ClassificationTypeController : BaseController<MstTypeClassification>
{
    public ClassificationTypeController(IClassificationTypeService classificationTypeService)
    {
        _classificationTypeService = classificationTypeService;
    }
    public override async Task<ActionResult> Index() => await base.Index();
    public override async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            var result = await _classificationTypeService.GetListClassificationType(model);

            return Json(result);

        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public override async Task<IActionResult> Add()
    {
        return View(Const.Form, new MstTypeClassification());
    }
    public override async Task<IActionResult> Update(Guid Id)
    {
        var data = await _classificationTypeService.GetById(Id);
        if (data.Count() > 0)
        {
            return View(Const.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToAction("Index", "ClassificationType", new { Area = "MasterData" });
        }
    }
    public override async Task<IActionResult> Remove(Guid Id)
    {
        var data = await _classificationTypeService.GetById(Id);
        if (data.Count() > 0)
        {
            return View(Const.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToAction("Index", "ClassificationType", new { Area = "MasterData" });
        }

    }
    public override async Task<IActionResult> Detail(Guid Id)
    {
        var data = await _classificationTypeService.GetById(Id);
        if (data.Count() > 0)
        {
            return View(Const.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToAction("Index", "ClassificationType", new { Area = "MasterData" });
        }
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Save(MstTypeClassification model)
    {
        int result = 0;
        if (model != null)
        {
            if (model.TypeClassificationId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                result = await _classificationTypeService.Update(model);
            }

            else
            {
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                result = await _classificationTypeService.Insert(model);
            }
        }
        return RedirectToAction("Index", "ClassificationType", new { Area = "MasterData" });
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Delete(MstTypeClassification model)
    {
        int result = 0;
        if (model != null && model.TypeClassificationId != Guid.Empty)
        {
            result = await _classificationTypeService.Delete(model);
        }
        return RedirectToAction("Index", "ClassificationType", new { Area = "MasterData" });
    }
}
