using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area("MasterData")]
    public class ClassificationTypeController : Controller
    {
        private readonly IClassificationTypeService _classificationTypeService;
        public ClassificationTypeController(IClassificationTypeService classificationTypeService)
        {
            _classificationTypeService = classificationTypeService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetData(DataTablePostModel model)
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
        public async Task<IActionResult> Add()
        {
            return View();
        }
        public async Task<IActionResult> Update(Guid Id)
        {
            var data = await _classificationTypeService.GetById(Id);
            if (data.Count() > 0)
            {
                return View(data.FirstOrDefault());
            }
            else
            {
                return RedirectToAction("Index", "ClassificationType", new { Area = "MasterData" });
            }
        }
        public async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _classificationTypeService.GetById(Id);
            if (data.Count() > 0)
            {
                return View(data.FirstOrDefault());
            }
            else
            {
                return RedirectToAction("Index", "ClassificationType", new { Area = "MasterData" });
            }

        }
        public async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _classificationTypeService.GetById(Id);
            if (data.Count() > 0)
            {
                return View(data.FirstOrDefault());
            }
            else
            {
                return RedirectToAction("Index", "ClassificationType", new { Area = "MasterData" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(MstTypeClassification model)
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
        public async Task<IActionResult> Delete(MstTypeClassification model)
        {
            int result = 0;
            if (model != null && model.TypeClassificationId != Guid.Empty)
            {
                result = await _classificationTypeService.Delete(model);
            }
            return RedirectToAction("Index", "ClassificationType", new { Area = "MasterData" });
        }
    }
}
