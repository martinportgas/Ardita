using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area("MasterData")]
    public class ClassificationSubjectController : BaseController<TrxSubjectClassification>
    {
        public ClassificationSubjectController(
            IClassificationSubjectService classificationSubjectService,
            IClassificationTypeService classificationTypeService,
            IClassificationService classificationService)
        {
            _classificationSubjectService = classificationSubjectService;
            _classificationTypeService = classificationTypeService;
            _classificationService = classificationService;
        }
        public override async Task<ActionResult> Index() => await base.Index();

        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                var result = await _classificationSubjectService.GetListClassificationSubject(model);

                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<JsonResult> GetClassifictionIdByTypeId(Guid id)
        {
            try
            {
                var result = await _classificationService.GetByTypeId(id);

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
            var classificationData = await _classificationService.GetAll();

            ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
            ViewBag.listClassification = new SelectList(classificationData, "ClassificationId", "ClassificationName");
            return View(Const.Form, new TrxSubjectClassification());
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _classificationSubjectService.GetById(Id);
            if (data.Count() > 0)
            {
                var classificationTypeData = await _classificationTypeService.GetAll();
                var classificationData = await _classificationService.GetAll();

                ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
                ViewBag.listClassification = new SelectList(classificationData, "ClassificationId", "ClassificationName");
                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToAction("Index", "ClassificationSubject", new { Area = "MasterData" });
            }
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _classificationSubjectService.GetById(Id);
            if (data.Count() > 0)
            {
                var classificationTypeData = await _classificationTypeService.GetAll();
                var classificationData = await _classificationService.GetAll();

                ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
                ViewBag.listClassification = new SelectList(classificationData, "ClassificationId", "ClassificationName");
                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToAction("Index", "ClassificationSubject", new { Area = "MasterData" });
            }

        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _classificationSubjectService.GetById(Id);
            if (data.Count() > 0)
            {
                var classificationTypeData = await _classificationTypeService.GetAll();
                var classificationData = await _classificationService.GetAll();

                ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
                ViewBag.listClassification = new SelectList(classificationData, "ClassificationId", "ClassificationName");
                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToAction("Index", "ClassificationSubject", new { Area = "MasterData" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async override Task<IActionResult> Save(TrxSubjectClassification model)
        {
            int result = 0;
            if (model != null)
            {
                if (model.SubjectClassificationId != Guid.Empty)
                {
                    model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdatedDate = DateTime.Now;
                    result = await _classificationSubjectService.Update(model);
                }

                else
                {
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;
                    result = await _classificationSubjectService.Insert(model);
                }
            }
            return RedirectToAction("Index", "ClassificationSubject", new { Area = "MasterData" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async override Task<IActionResult> Delete(TrxSubjectClassification model)
        {
            int result = 0;
            if (model != null && model.SubjectClassificationId != Guid.Empty)
            {
                result = await _classificationSubjectService.Delete(model);
            }
            return RedirectToAction("Index", "ClassificationSubject", new { Area = "MasterData" });
        }
    }
}
