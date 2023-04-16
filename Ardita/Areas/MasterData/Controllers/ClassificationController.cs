using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area("MasterData")]
    public class ClassificationController : Controller
    {
        private readonly IClassificationService _classificationService;
        private readonly IClassificationTypeService _classificationTypeService;
        public ClassificationController(IClassificationService classificationService, IClassificationTypeService classificationTypeService)
        {
            _classificationService = classificationService;
            _classificationTypeService = classificationTypeService;
        }
        // GET: ClassificationController
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetData(DataTablePostModel model)
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
        public async Task<IActionResult> Add()
        {
            var classificationTypeData = await _classificationTypeService.GetAll();

            ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
            return View();
        }
        public async Task<IActionResult> Update(Guid Id)
        {
            var data = await _classificationService.GetById(Id);
            if (data.Count() > 0)
            {
                var classificationTypeData = await _classificationTypeService.GetAll();

                ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
                return View(data.FirstOrDefault());
            }
            else
            {
                return RedirectToAction("Index", "Classification", new { Area = "MasterData" });
            }
        }
        public async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _classificationService.GetById(Id);
            if (data.Count() > 0)
            {
                var classificationTypeData = await _classificationTypeService.GetAll();

                ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
                return View(data.FirstOrDefault());
            }
            else
            {
                return RedirectToAction("Index", "Classification", new { Area = "MasterData" });
            }

        }
        public async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _classificationService.GetById(Id);
            if (data.Count() > 0)
            {
                var classificationTypeData = await _classificationTypeService.GetAll();

                ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
                return View(data.FirstOrDefault());
            }
            else
            {
                return RedirectToAction("Index", "Classification", new { Area = "MasterData" });
            }
        }

        // POST: ClassificationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(TrxClassification model)
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
        public async Task<IActionResult> Delete(TrxClassification model)
        {
            int result = 0;
            if (model != null && model.ClassificationId != Guid.Empty)
            {
                result = await _classificationService.Delete(model);
            }
            return RedirectToAction("Index", "Classification", new { Area = "MasterData" });
        }
    }
}
