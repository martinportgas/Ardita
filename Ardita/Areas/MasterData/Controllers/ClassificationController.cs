using Ardita.Controllers;
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
public class ClassificationController : BaseController<TrxClassification>
{
    public ClassificationController(IClassificationService classificationService, IClassificationTypeService classificationTypeService)
    {
        _classificationService = classificationService;
        _classificationTypeService = classificationTypeService;
    }
    // GET: ClassificationController
    public override async Task<ActionResult> Index() => await base.Index();
    public override async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            var result = await _classificationService.GetListClassification(model);

            return Json(result);

        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public override async Task<IActionResult> Add()
    {
        var classificationTypeData = await _classificationTypeService.GetAll();

        ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
        return View(Const.Form, new TrxClassification());
    }
    public override async Task<IActionResult> Update(Guid Id)
    {
        var data = await _classificationService.GetById(Id);
        if (data.Count() > 0)
        {
            var classificationTypeData = await _classificationTypeService.GetAll();

            ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
            return View(Const.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToAction("Index", "Classification", new { Area = "MasterData" });
        }
    }
    public override async Task<IActionResult> Remove(Guid Id)
    {
        var data = await _classificationService.GetById(Id);
        if (data.Count() > 0)
        {
            var classificationTypeData = await _classificationTypeService.GetAll();

            ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
            return View(Const.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToAction("Index", "Classification", new { Area = "MasterData" });
        }

    }
    public override async Task<IActionResult> Detail(Guid Id)
    {
        var data = await _classificationService.GetById(Id);
        if (data.Count() > 0)
        {
            var classificationTypeData = await _classificationTypeService.GetAll();

            ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
            return View(Const.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToAction("Index", "Classification", new { Area = "MasterData" });
        }
    }

    // POST: ClassificationController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Save(TrxClassification model)
    {
        int result = 0;
        if (model != null)
        {
            if (model.ClassificationId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                result = await _classificationService.Update(model);
            }

            else
            {
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                result = await _classificationService.Insert(model);
            }
        }
        return RedirectToAction("Index", "Classification", new { Area = "MasterData" });
    }

    // POST: ClassificationController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Delete(TrxClassification model)
    {
        int result = 0;
        if (model != null && model.ClassificationId != Guid.Empty)
        {
            result = await _classificationService.Delete(model);
        }
        return RedirectToAction("Index", "Classification", new { Area = "MasterData" });
    }
}
