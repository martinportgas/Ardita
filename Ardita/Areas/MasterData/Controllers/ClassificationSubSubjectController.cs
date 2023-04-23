using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting.Internal;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area("MasterData")]
    public class ClassificationSubSubjectController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IClassificationSubSubjectService _classificationSubSubjectService;
        private readonly IClassificationSubjectService _classificationSubjectService;
        private readonly IClassificationService _classificationService;
        private readonly IClassificationTypeService _classificationTypeService;
        private readonly IPositionService _positionService;
        public ClassificationSubSubjectController(
            IHostingEnvironment hostingEnvironment,
            IClassificationSubSubjectService classificationSubSubjectService,
            IClassificationSubjectService classificationSubjectService,
            IClassificationTypeService classificationTypeService,
            IClassificationService classificationService,
            IPositionService positionService)
        {
            _hostingEnvironment = hostingEnvironment;
            _classificationSubSubjectService = classificationSubSubjectService;
            _classificationSubjectService = classificationSubjectService;
            _classificationTypeService = classificationTypeService;
            _classificationService = classificationService;
            _positionService = positionService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetData(DataTablePostModel model)
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
        public async Task<IActionResult> Add()
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
        public async Task<IActionResult> Update(Guid Id)
        {
            var data = await _classificationSubSubjectService.GetAll();
            var model = data.Where(x => x.SubSubjectClassificationId == Id).FirstOrDefault();
            if (model != null)
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
                return View(Const.Form, model);
            }
            else
            {
                return RedirectToAction("Index", "ClassificationSubSubject", new { Area = "MasterData" });
            }
        }
        public async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _classificationSubSubjectService.GetAll();
            var model = data.Where(x => x.SubSubjectClassificationId == Id).FirstOrDefault();
            if (model != null)
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
                return View(Const.Form, model);
            }
            else
            {
                return RedirectToAction("Index", "ClassificationSubSubject", new { Area = "MasterData" });
            }

        }
        public async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _classificationSubSubjectService.GetAll();
            var model = data.Where(x => x.SubSubjectClassificationId == Id).FirstOrDefault();
            if (model != null)
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
                return View(Const.Form, model);
            }
            else
            {
                return RedirectToAction("Index", "ClassificationSubSubject", new { Area = "MasterData" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(TrxSubSubjectClassification model)
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
        public async Task<IActionResult> Delete(TrxSubSubjectClassification model)
        {
            int result = 0;
            if (model != null && model.SubSubjectClassificationId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                result = await _classificationSubSubjectService.Delete(model);
            }
            return RedirectToAction("Index", "ClassificationSubSubject", new { Area = "MasterData" });
        }
        public async Task<IActionResult> DownloadTemplate()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"ClassificationSubSubjectTemplate.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("ClassificationSubSubject");
                ISheet excelSheetSubject = workbook.CreateSheet("ClassificationSubject");

                IRow row = excelSheet.CreateRow(0);
                IRow rowSubject = excelSheetSubject.CreateRow(0);

                row.CreateCell(0).SetCellValue("ClassificationSubSubjectCode");
                row.CreateCell(1).SetCellValue("ClassificationSubSubjectName");
                row.CreateCell(2).SetCellValue("CreatorCode");
                row.CreateCell(3).SetCellValue("ClassificationSubjectCode");
                row.CreateCell(4).SetCellValue("ClassificationSecurityCode");
                row.CreateCell(5).SetCellValue("RetentionActive");
                row.CreateCell(6).SetCellValue("RetentionInActive");
                row.CreateCell(7).SetCellValue("BasicInfo");

                row = excelSheet.CreateRow(1);
                row.CreateCell(0).SetCellValue("Sample Code");
                row.CreateCell(1).SetCellValue("Sample Name");
                row.CreateCell(2).SetCellValue("Sample Creator Code");
                row.CreateCell(3).SetCellValue("Sample Classification Subject Code");
                row.CreateCell(4).SetCellValue("Sample Classification Security Code");
                row.CreateCell(5).SetCellValue("10");
                row.CreateCell(6).SetCellValue("20");
                row.CreateCell(7).SetCellValue("Deskripsi");

                rowSubject.CreateCell(0).SetCellValue("Code");
                rowSubject.CreateCell(1).SetCellValue("Name");

                var dataclassificationSubject = await _classificationSubjectService.GetAll();

                int no = 1;
                foreach (var item in dataclassificationSubject)
                {
                    rowSubject = excelSheetSubject.CreateRow(no);

                    rowSubject.CreateCell(0).SetCellValue(item.SubjectClassificationCode);
                    rowSubject.CreateCell(1).SetCellValue(item.SubjectClassificationName);
                    no += 1;
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
        public async Task<IActionResult> Export()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"ClassificationSubSubjectData.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            var data = await _classificationSubSubjectService.GetAll();
            var subject = await _classificationSubjectService.GetAll();

            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Classification");

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("ClassificationSubSubjectCode");
                row.CreateCell(1).SetCellValue("ClassificationSubSubjectName");
                row.CreateCell(2).SetCellValue("CreatorCode");
                row.CreateCell(3).SetCellValue("CreatorName");
                row.CreateCell(4).SetCellValue("ClassificationSubjectCode");
                row.CreateCell(5).SetCellValue("ClassificationSubjectName");
                row.CreateCell(6).SetCellValue("ClassificationSecurityCode");
                row.CreateCell(7).SetCellValue("ClassificationSecurityName");
                row.CreateCell(8).SetCellValue("RetentionActive");
                row.CreateCell(9).SetCellValue("RetentionInActive");
                row.CreateCell(10).SetCellValue("BasicInfo");

                int no = 1;
                foreach (var item in data)
                {
                    row = excelSheet.CreateRow(no);
                    row.CreateCell(0).SetCellValue(item.SubSubjectClassificationCode);
                    row.CreateCell(1).SetCellValue(item.SubSubjectClassificationName);
                    row.CreateCell(2).SetCellValue(item.SubSubjectClassificationName);
                    row.CreateCell(3).SetCellValue(item.SubSubjectClassificationName);
                    var subjectData = subject.Where(x => x.SubjectClassificationId == item.SubjectClassificationId).FirstOrDefault();
                    row.CreateCell(4).SetCellValue(subjectData.SubjectClassificationCode);
                    row.CreateCell(5).SetCellValue(subjectData.SubjectClassificationName);
                    row.CreateCell(6).SetCellValue(subjectData.SubjectClassificationCode);
                    row.CreateCell(7).SetCellValue(subjectData.SubjectClassificationName);
                    row.CreateCell(8).SetCellValue(item.RetentionActive.ToString());
                    row.CreateCell(9).SetCellValue(item.RetentionInactive.ToString());
                    row.CreateCell(10).SetCellValue(item.BasicInformation);
                    no += 1;
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
        public async Task<ActionResult> Upload()
        {
            IFormFile file = Request.Form.Files[0];
            var result = Extensions.Global.ImportExcel(file, "Upload", _hostingEnvironment.WebRootPath);

            var subjectClassifications = await _classificationSubjectService.GetAll();

            List<TrxSubSubjectClassification> models = new();
            TrxSubSubjectClassification model;

            foreach (DataRow row in result.Rows)
            {
                model = new();
                model.SubSubjectClassificationId = Guid.NewGuid();
                model.SubSubjectClassificationCode = row[0].ToString();
                model.SubSubjectClassificationName = row[1].ToString();
                model.CreatorId = subjectClassifications.Where(x => x.SubjectClassificationCode == row[3].ToString()).FirstOrDefault().SubjectClassificationId;
                model.SubjectClassificationId = subjectClassifications.Where(x => x.SubjectClassificationCode == row[3].ToString()).FirstOrDefault().SubjectClassificationId;
                model.SecurityClassificationId = subjectClassifications.Where(x => x.SubjectClassificationCode == row[3].ToString()).FirstOrDefault().SubjectClassificationId;
                model.RetentionActive = (int)row[5];
                model.RetentionInactive = (int)row[6];
                model.BasicInformation = row[7].ToString();
                model.IsActive = true;
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;

                models.Add(model);
            }
            await _classificationSubSubjectService.InsertBulk(models);

            return RedirectToAction("Index", "ClassificationSubSubject", new { Area = "MasterData" });
        }
    }
}
