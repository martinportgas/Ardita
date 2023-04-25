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
using Microsoft.Extensions.Hosting.Internal;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area("MasterData")]
    public class ClassificationSubjectController : BaseController<TrxSubjectClassification>
    {
        private IHostingEnvironment _hostingEnvironment;
        public ClassificationSubjectController(
            IHostingEnvironment hostingEnvironment,
            IClassificationSubjectService classificationSubjectService,
            IClassificationTypeService classificationTypeService,
            IClassificationService classificationService)
        {
            _classificationSubjectService = classificationSubjectService;
            _classificationTypeService = classificationTypeService;
            _classificationService = classificationService;
            _hostingEnvironment = hostingEnvironment;
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
            var data = await _classificationSubjectService.GetAll();
            var model = data.Where(x => x.SubjectClassificationId == Id).FirstOrDefault();

            if (model != null)
            {
                var classificationTypeData = await _classificationTypeService.GetAll();
                var classificationData = await _classificationService.GetAll();

                ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
                ViewBag.listClassification = new SelectList(classificationData, "ClassificationId", "ClassificationName");

                return View(Const.Form, model);
            }
            else
            {
                return RedirectToAction("Index", "ClassificationSubject", new { Area = "MasterData" });
            }
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _classificationSubjectService.GetAll();
            var model = data.Where(x => x.SubjectClassificationId == Id).FirstOrDefault();

            if (model != null)
            {
                var classificationTypeData = await _classificationTypeService.GetAll();
                var classificationData = await _classificationService.GetAll();

                ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
                ViewBag.listClassification = new SelectList(classificationData, "ClassificationId", "ClassificationName");

                return View(Const.Form, model);
            }
            else
            {
                return RedirectToAction("Index", "ClassificationSubject", new { Area = "MasterData" });
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _classificationSubjectService.GetAll();
            var model = data.Where(x => x.SubjectClassificationId == Id).FirstOrDefault();

            if (model != null)
            {
                var classificationTypeData = await _classificationTypeService.GetAll();
                var classificationData = await _classificationService.GetAll();

                ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
                ViewBag.listClassification = new SelectList(classificationData, "ClassificationId", "ClassificationName");

                return View(Const.Form, model);
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
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                result = await _classificationSubjectService.Delete(model);
            }
            return RedirectToAction("Index", "ClassificationSubject", new { Area = "MasterData" });
        }
        public async Task<IActionResult> DownloadTemplate()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"ClassificationSubjectTemplate.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("ClassificationSubject");
                ISheet excelSheetParent = workbook.CreateSheet("Classification");

                IRow row = excelSheet.CreateRow(0);
                IRow rowParent = excelSheetParent.CreateRow(0);

                row.CreateCell(0).SetCellValue("ClassificationSubjectCode");
                row.CreateCell(1).SetCellValue("ClassificationSubjectName");
                row.CreateCell(2).SetCellValue("ClassificationCode");
                row.CreateCell(3).SetCellValue("Active");

                row = excelSheet.CreateRow(1);
                row.CreateCell(0).SetCellValue("Sample Code");
                row.CreateCell(1).SetCellValue("Sample Name");
                row.CreateCell(2).SetCellValue("Sample Classification Code");
                row.CreateCell(3).SetCellValue("1");

                rowParent.CreateCell(0).SetCellValue("Code");
                rowParent.CreateCell(1).SetCellValue("Name");

                var dataClassification = await _classificationService.GetAll();

                int no = 1;
                foreach (var item in dataClassification)
                {
                    rowParent = excelSheetParent.CreateRow(no);

                    rowParent.CreateCell(0).SetCellValue(item.ClassificationCode);
                    rowParent.CreateCell(1).SetCellValue(item.ClassificationName);
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
            string sFileName = @"ClassificationSubjectData.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            var data = await _classificationSubjectService.GetAll();
            var type = await _classificationService.GetAll();

            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("ClassificationSubject");

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("ClassificationSubjectCode");
                row.CreateCell(1).SetCellValue("ClassificationSubjectName");
                row.CreateCell(2).SetCellValue("ClassificationCode");
                row.CreateCell(3).SetCellValue("ClassificationName");
                row.CreateCell(4).SetCellValue("Active");

                int no = 1;
                foreach (var item in data)
                {
                    row = excelSheet.CreateRow(no);
                    row.CreateCell(0).SetCellValue(item.SubjectClassificationCode);
                    row.CreateCell(1).SetCellValue(item.SubjectClassificationName);
                    var typeData = type.Where(x => x.ClassificationId == item.ClassificationId).FirstOrDefault();
                    row.CreateCell(2).SetCellValue(typeData.ClassificationCode);
                    row.CreateCell(3).SetCellValue(typeData.ClassificationName);
                    row.CreateCell(4).SetCellValue((bool)item.IsActive ? "1" : "0");
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

            var type = await _classificationService.GetAll();

            List<TrxSubjectClassification> models = new();
            TrxSubjectClassification model;

            foreach (DataRow row in result.Rows)
            {
                model = new();
                model.ClassificationId = Guid.NewGuid();
                model.SubjectClassificationCode = row[0].ToString();
                model.SubjectClassificationName = row[1].ToString();
                model.ClassificationId = type.Where(x => x.ClassificationCode == row[2].ToString()).FirstOrDefault().ClassificationId;
                model.IsActive = row[3].ToString() == "1";
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;

                models.Add(model);
            }
            await _classificationSubjectService.InsertBulk(models);

            return RedirectToAction("Index", "ClassificationSubject", new { Area = "MasterData" });
        }
    }
}
