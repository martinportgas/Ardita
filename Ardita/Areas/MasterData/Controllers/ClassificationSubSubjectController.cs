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
    public class ClassificationSubSubjectController : BaseController<TrxSubSubjectClassification>
    {
        public ClassificationSubSubjectController(
            IClassificationSubSubjectService classificationSubSubjectService,
            IClassificationSubjectService classificationSubjectService,
            IClassificationTypeService classificationTypeService,
            IClassificationService classificationService,
            IPositionService positionService)
        {
            _classificationSubSubjectService = classificationSubSubjectService;
            _classificationSubjectService = classificationSubjectService;
            _classificationTypeService = classificationTypeService;
            _classificationService = classificationService;
            _positionService = positionService;
        }
        public override async Task<ActionResult> Index() => await base.Index();

        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                var result = await _classificationSubSubjectService.GetListClassificationSubSubject(model);

                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<JsonResult> GetSubjectClassifictionIdByClassificationId(Guid id)
        {
            try
            {
                var result = await _classificationSubjectService.GetByClassificationId(id);

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
            var classificationSubjectData = await _classificationSubjectService.GetAll();
            var positionData = await _positionService.GetAll();

            ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
            ViewBag.listClassification = new SelectList(classificationData, "ClassificationId", "ClassificationName");
            ViewBag.listClassificationSubject = new SelectList(classificationSubjectData, "SubjectClassificationId", "SubjectClassificationName");
            ViewBag.listPosition = new SelectList(positionData, "PositionId", "Name");
            return View(Const.Form, new TrxSubSubjectClassification());
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _classificationSubSubjectService.GetById(Id);
            if (data.Count() > 0)
            {
                var classificationTypeData = await _classificationTypeService.GetAll();
                var classificationData = await _classificationService.GetAll();
                var classificationSubjectData = await _classificationSubjectService.GetAll();
                var positionData = await _positionService.GetAll();
                var subDetail = await _classificationSubSubjectService.GetDetailByMainId(Id);

                ViewBag.subDetail = (from detail in subDetail
                                     join position in positionData on detail.PositionId equals position.PositionId
                                     select new TrxPermissionClassification
                                     {
                                         PositionId = detail.PositionId,
                                         Position = position,
                                     }).ToList();

                ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
                ViewBag.listClassification = new SelectList(classificationData, "ClassificationId", "ClassificationName");
                ViewBag.listClassificationSubject = new SelectList(classificationSubjectData, "SubjectClassificationId", "SubjectClassificationName");
                ViewBag.listPosition = new SelectList(positionData, "PositionId", "Name");
                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToAction("Index", "ClassificationSubSubject", new { Area = "MasterData" });
            }
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _classificationSubSubjectService.GetById(Id);
            if (data.Count() > 0)
            {
                var classificationTypeData = await _classificationTypeService.GetAll();
                var classificationData = await _classificationService.GetAll();
                var classificationSubjectData = await _classificationSubjectService.GetAll();
                var positionData = await _positionService.GetAll();
                var subDetail = await _classificationSubSubjectService.GetDetailByMainId(Id);

                ViewBag.subDetail = (from detail in subDetail
                                     join position in positionData on detail.PositionId equals position.PositionId
                                     select new TrxPermissionClassification
                                     {
                                         PositionId = detail.PositionId,
                                         Position = position,
                                     }).ToList();

                ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
                ViewBag.listClassification = new SelectList(classificationData, "ClassificationId", "ClassificationName");
                ViewBag.listClassificationSubject = new SelectList(classificationSubjectData, "SubjectClassificationId", "SubjectClassificationName");
                ViewBag.listPosition = new SelectList(positionData, "PositionId", "Name");
                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToAction("Index", "ClassificationSubSubject", new { Area = "MasterData" });
            }

        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _classificationSubSubjectService.GetById(Id);
            if (data.Count() > 0)
            {
                var classificationTypeData = await _classificationTypeService.GetAll();
                var classificationData = await _classificationService.GetAll();
                var classificationSubjectData = await _classificationSubjectService.GetAll();
                var positionData = await _positionService.GetAll();
                var subDetail = await _classificationSubSubjectService.GetDetailByMainId(Id);

                ViewBag.subDetail = (from detail in subDetail
                                     join position in positionData on detail.PositionId equals position.PositionId
                                     select new TrxPermissionClassification
                                     {
                                         PositionId = detail.PositionId,
                                         Position = position,
                                     }).ToList();

                ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
                ViewBag.listClassification = new SelectList(classificationData, "ClassificationId", "ClassificationName");
                ViewBag.listClassificationSubject = new SelectList(classificationSubjectData, "SubjectClassificationId", "SubjectClassificationName");
                ViewBag.listPosition = new SelectList(positionData, "PositionId", "Name");
                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToAction("Index", "ClassificationSubSubject", new { Area = "MasterData" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Save(TrxSubSubjectClassification model)
        {
            int result = 0;
            if (model != null)
            {
                if (model.SubSubjectClassificationId != Guid.Empty)
                {
                    model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdatedDate = DateTime.Now;
                    result = await _classificationSubSubjectService.Update(model);
                }

                else
                {
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;
                    result = await _classificationSubSubjectService.Insert(model);
                }

                var listPosition = Request.Form["posisi[]"].Distinct().ToArray();
                if (listPosition.Length > 0)
                {
                    result = await _classificationSubSubjectService.DeleteDetail(model.SubSubjectClassificationId);

                    TrxPermissionClassification objSubDetail;
                    for (int i = 0; i < listPosition.Length; i++)
                    {
                        var pos = listPosition[i];
                        Guid positionId = Guid.Empty;
                        Guid.TryParse(pos, out positionId);

                        if (!string.IsNullOrEmpty(pos))
                        {
                            objSubDetail = new();
                            objSubDetail.SubSubjectClassificationId = model.SubSubjectClassificationId;
                            objSubDetail.PositionId = positionId;
                            objSubDetail.CreatedBy = AppUsers.CurrentUser(User).UserId;
                            objSubDetail.CreatedDate = DateTime.Now;

                            result = await _classificationSubSubjectService.InsertDetail(objSubDetail);
                        }
                    }
                }
            }
            return RedirectToAction("Index", "ClassificationSubSubject", new { Area = "MasterData" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Delete(TrxSubSubjectClassification model)
        {
            int result = 0;
            if (model != null && model.TypeClassificationId != Guid.Empty)
            {
                result = await _classificationSubSubjectService.Delete(model);
            }
            return RedirectToAction("Index", "ClassificationSubSubject", new { Area = "MasterData" });
        }
    }
}
