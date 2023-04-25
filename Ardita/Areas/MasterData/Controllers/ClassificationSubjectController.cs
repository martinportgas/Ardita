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
using NPOI.HPSF;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(Const.MasterData)]
    public class ClassificationSubjectController : BaseController<TrxSubjectClassification>
    {
        #region MEMBER AND CTR
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
        #endregion
        #region MAIN ACTION
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
        public override async Task<IActionResult> Add()
        {
            ViewBag.listClassificationType = await BindClassificationTypes();
            ViewBag.listClassification = await BindClasscifications();
            return View(Const.Form, new TrxSubjectClassification());
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _classificationSubjectService.GetAll();
            var model = data.Where(x => x.SubjectClassificationId == Id).FirstOrDefault();

            if (model != null)
            {
                ViewBag.listClassificationType = await BindClassificationTypes();
                ViewBag.listClassification = await BindClasscifications();
                return View(Const.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _classificationSubjectService.GetAll();
            var model = data.Where(x => x.SubjectClassificationId == Id).FirstOrDefault();

            if (model != null)
            {
                ViewBag.listClassificationType = await BindClassificationTypes();
                ViewBag.listClassification = await BindClasscifications();
                return View(Const.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _classificationSubjectService.GetAll();
            var model = data.Where(x => x.SubjectClassificationId == Id).FirstOrDefault();

            if (model != null)
            {
                ViewBag.listClassificationType = await BindClassificationTypes();
                ViewBag.listClassification = await BindClasscifications();
                return View(Const.Form, model);
            }
            else
            {
                return RedirectToIndex();
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
            return RedirectToIndex();
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
            return RedirectToIndex();
        }
        public async Task DownloadTemplate()
        {
            try
            {
                string fileName = $"{Const.Template}-{nameof(TrxSubjectClassification).ToCleanNameOf()}";
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxSubjectClassification).ToCleanNameOf());
                ISheet excelSheetParent = workbook.CreateSheet(nameof(TrxClassification).ToCleanNameOf());

                IRow row = excelSheet.CreateRow(0);
                IRow rowParent = excelSheetParent.CreateRow(0);

                row.CreateCell(0).SetCellValue(nameof(TrxSubjectClassification.SubjectClassificationCode));
                row.CreateCell(1).SetCellValue(nameof(TrxSubjectClassification.SubjectClassificationName));
                row.CreateCell(2).SetCellValue(nameof(TrxClassification.ClassificationCode));

                rowParent.CreateCell(0).SetCellValue(nameof(TrxClassification.ClassificationCode));
                rowParent.CreateCell(1).SetCellValue(nameof(TrxClassification.ClassificationName));

                var dataClassification = await _classificationService.GetAll();

                int no = 1;
                foreach (var item in dataClassification)
                {
                    rowParent = excelSheetParent.CreateRow(no);

                    rowParent.CreateCell(0).SetCellValue(item.ClassificationCode);
                    rowParent.CreateCell(1).SetCellValue(item.ClassificationName);
                    no += 1;
                }
                workbook.WriteExcelToResponse(HttpContext, fileName);
            }
            catch (Exception ex) 
            {
                throw new Exception();
            }
        }
        public async Task Export()
        {
            try
            {
                string fileName = nameof(TrxSubjectClassification).ToCleanNameOf();
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                var data = await _classificationSubjectService.GetAll();
                var dataClassifications = await _classificationService.GetAll();

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxSubjectClassification).ToCleanNameOf());

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue(nameof(TrxSubjectClassification.SubjectClassificationCode));
                row.CreateCell(1).SetCellValue(nameof(TrxSubjectClassification.SubjectClassificationName));
                row.CreateCell(2).SetCellValue(nameof(TrxClassification.ClassificationCode));
                row.CreateCell(3).SetCellValue(nameof(TrxClassification.ClassificationName));

                int no = 1;
                foreach (var item in data)
                {
                    var dataClassification = dataClassifications.Where(x => x.ClassificationId == item.ClassificationId).FirstOrDefault();
                    if(dataClassification != null)
                    {
                        row = excelSheet.CreateRow(no);
                        row.CreateCell(0).SetCellValue(item.SubjectClassificationCode);
                        row.CreateCell(1).SetCellValue(item.SubjectClassificationName);
                        row.CreateCell(2).SetCellValue(dataClassification.ClassificationCode);
                        row.CreateCell(3).SetCellValue(dataClassification.ClassificationName);
                        no += 1;
                    }
                }
                workbook.WriteExcelToResponse(HttpContext, fileName);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public async Task<IActionResult> Upload()
        {
            try
            {
                IFormFile file = Request.Form.Files[0];
                var result = Extensions.Global.ImportExcel(file, Const.Upload, _hostingEnvironment.WebRootPath);

                var dataClassifications = await _classificationService.GetAll();

                List<TrxSubjectClassification> models = new();
                TrxSubjectClassification model;

                foreach (DataRow row in result.Rows)
                {
                    var dataClassification = dataClassifications.Where(x => x.ClassificationCode == row[2].ToString()).FirstOrDefault();
                    if (dataClassification != null)
                    {
                        model = new();
                        model.ClassificationId = Guid.NewGuid();
                        model.SubjectClassificationCode = row[0].ToString();
                        model.SubjectClassificationName = row[1].ToString();
                        model.ClassificationId = dataClassification.ClassificationId;
                        model.IsActive = true;
                        model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                        model.CreatedDate = DateTime.Now;

                        models.Add(model);
                    }
                }
                await _classificationSubjectService.InsertBulk(models);

                return RedirectToIndex();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        #endregion

        #region HELPER
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.ClassificationSubject, new { Area = Const.MasterData });
        #endregion
    }
}
