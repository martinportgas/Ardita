using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area("MasterData")]
    public class FloorController : Controller
    {
        private readonly IFloorService _floorService;

        public FloorController(IFloorService floorService)
        {
            _floorService = floorService;
        }

        // GET: FloorController
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                var result = await _floorService.GetListClassification(model);

                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IActionResult> Form(Guid Id, string FormType)
        {
            var model = new TrxFloor();
            var data = await _floorService.GetById(Id);

            if (data.Count() > 0)
                model = data.FirstOrDefault();

            ViewBag.FormType = FormType;

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExecuteForm(TrxFloor model, string FormType)
        {
            if (model != null)
            {
                if (model.FloorId != Guid.Empty)
                {
                    
                    model.ArchiveUnitId = new Guid("B0ADFB41-1516-4145-8623-D0118FBE646D");
                    model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdatedDate = DateTime.Now;

                    if(FormType == "Update")
                        await _floorService.Update(model);
                    else if(FormType == "Delete")
                        await _floorService.Delete(model);
                    else
                        return RedirectToAction("Index", "Floor", new { Area = "MasterData" });
                }
                else
                {
                    model.ArchiveUnitId = new Guid("B0ADFB41-1516-4145-8623-D0118FBE646D");
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;
                    await _floorService.Insert(model);
                }
            }
            return RedirectToAction("Index", "Floor", new { Area = "MasterData" });
        }
        //public async Task<IActionResult> Add()
        //{
        //    //var classificationTypeData = await _classificationTypeService.GetAll();

        //    //ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
        //    return View();
        //}
        //public async Task<IActionResult> Update(Guid Id)
        //{
        //    var data = await _floorService.GetById(Id);
        //    if (data.Count() > 0)
        //    {
        //        //var classificationTypeData = await _classificationTypeService.GetAll();

        //        //ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
        //        return View(data.FirstOrDefault());
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Floor", new { Area = "MasterData" });
        //    }
        //}
        //public async Task<IActionResult> Remove(Guid Id)
        //{
        //    var data = await _floorService.GetById(Id);
        //    if (data.Count() > 0)
        //    {
        //        //var classificationTypeData = await _classificationTypeService.GetAll();

        //        //ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
        //        return View(data.FirstOrDefault());
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Floor", new { Area = "MasterData" });
        //    }
        //}
        //public async Task<IActionResult> Detail(Guid Id)
        //{
        //    var data = await _floorService.GetById(Id);
        //    if (data.Count() > 0)
        //    {
        //        //var classificationTypeData = await _classificationTypeService.GetAll();

        //        //ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
        //        return View(data.FirstOrDefault());
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Floor", new { Area = "MasterData" });
        //    }
        //}

        //// POST: FloorController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Save(TrxFloor model)
        //{
        //    int result = 0;
        //    if (model != null)
        //    {
        //        if (model.FloorId != Guid.Empty)
        //        {
        //            model.ArchiveUnitId = new Guid("B0ADFB41-1516-4145-8623-D0118FBE646D");
        //            model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
        //            model.UpdatedDate = DateTime.Now;
        //            result = await _floorService.Update(model);
        //        }

        //        else
        //        {
        //            model.ArchiveUnitId = new Guid("B0ADFB41-1516-4145-8623-D0118FBE646D");
        //            model.CreatedBy = AppUsers.CurrentUser(User).UserId;
        //            model.CreatedDate = DateTime.Now;
        //            result = await _floorService.Insert(model);
        //        }
        //    }
        //    return RedirectToAction("Index", "Floor", new { Area = "MasterData" });
        //}

        //// POST: FloorController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Delete(TrxFloor model)
        //{
        //    int result = 0;
        //    if (model != null && model.FloorId != Guid.Empty)
        //    {
        //        result = await _floorService.Delete(model);
        //    }
        //    return RedirectToAction("Index", "Floor", new { Area = "MasterData" });
        //}
    }
}
