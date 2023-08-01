using Ardita.Controllers;
using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using NPOI.HPSF;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(GlobalConst.MasterData)]
    public class ClassificationSubjectController : BaseController<TrxSubjectClassification>
    {
        #region MEMBER AND CTR
        public ClassificationSubjectController(
            IClassificationSubjectService classificationSubjectService,
            IClassificationTypeService classificationTypeService,
            IClassificationService classificationService)
        {
            _classificationSubjectService = classificationSubjectService;
            _classificationTypeService = classificationTypeService;
            _classificationService = classificationService;
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
            return View(GlobalConst.Form, new TrxSubjectClassification());
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var model = await _classificationSubjectService.GetById(Id);
            if (model != null)
            {
                ViewBag.listClassificationType = await BindClassificationTypes();
                ViewBag.listClassification = await BindClasscifications();
                return View(GlobalConst.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var model = await _classificationSubjectService.GetById(Id);
            if (model != null)
            {
                ViewBag.listClassificationType = await BindClassificationTypes();
                ViewBag.listClassification = await BindClasscifications();
                return View(GlobalConst.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var model = await _classificationSubjectService.GetById(Id);
            if (model != null)
            {
                ViewBag.listClassificationType = await BindClassificationTypes();
                ViewBag.listClassification = await BindClasscifications();
                return View(GlobalConst.Form, model);
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
        public async Task<IActionResult> DownloadTemplate()
        {
            try
            {
                string fileName = $"{GlobalConst.Template}-{nameof(TrxSubjectClassification).ToCleanNameOf()}";
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxSubjectClassification).ToCleanNameOf());
                ISheet excelSheetParent = workbook.CreateSheet(nameof(TrxClassification).ToCleanNameOf());

                IRow row = excelSheet.CreateRow(0);
                IRow rowParent = excelSheetParent.CreateRow(0);

                row.CreateCell(0).SetCellValue(nameof(GlobalConst.No));
                row.CreateCell(1).SetCellValue(nameof(TrxSubjectClassification.SubjectClassificationCode));
                row.CreateCell(2).SetCellValue(nameof(TrxSubjectClassification.SubjectClassificationName));
                row.CreateCell(3).SetCellValue(nameof(TrxClassification.ClassificationCode));

                rowParent.CreateCell(0).SetCellValue(nameof(GlobalConst.No));
                rowParent.CreateCell(1).SetCellValue(nameof(TrxClassification.ClassificationCode));
                rowParent.CreateCell(2).SetCellValue(nameof(TrxClassification.ClassificationName));

                var dataClassification = await _classificationService.GetAll();

                int no = 1;
                foreach (var item in dataClassification)
                {
                    rowParent = excelSheetParent.CreateRow(no);

                    rowParent.CreateCell(0).SetCellValue(no);
                    rowParent.CreateCell(1).SetCellValue(item.ClassificationCode);
                    rowParent.CreateCell(2).SetCellValue(item.ClassificationName);
                    no += 1;
                }
                using (var exportData = new MemoryStream())
                {
                    workbook.Write(exportData);
                    byte[] bytes = exportData.ToArray();
                    return File(bytes, GlobalConst.EXCEL_FORMAT_TYPE, $"{fileName}.xlsx");
                }
            }
            catch (Exception ex) 
            {
                throw new Exception();
            }
        }
        public async Task<IActionResult> Export()
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

                row.CreateCell(0).SetCellValue(nameof(GlobalConst.No));
                row.CreateCell(1).SetCellValue(nameof(TrxSubjectClassification.SubjectClassificationCode));
                row.CreateCell(2).SetCellValue(nameof(TrxSubjectClassification.SubjectClassificationName));
                row.CreateCell(3).SetCellValue(nameof(TrxClassification.ClassificationCode));
                row.CreateCell(4).SetCellValue(nameof(TrxClassification.ClassificationName));

                int no = 1;
                foreach (var item in data)
                {
                    var dataClassification = dataClassifications.Where(x => x.ClassificationId == item.ClassificationId).FirstOrDefault();
                    if(dataClassification != null)
                    {
                        row = excelSheet.CreateRow(no);
                        row.CreateCell(0).SetCellValue(no);
                        row.CreateCell(1).SetCellValue(item.SubjectClassificationCode);
                        row.CreateCell(2).SetCellValue(item.SubjectClassificationName);
                        row.CreateCell(3).SetCellValue(dataClassification.ClassificationCode);
                        row.CreateCell(4).SetCellValue(dataClassification.ClassificationName);
                        no += 1;
                    }
                }
                using (var exportData = new MemoryStream())
                {
                    workbook.Write(exportData);
                    byte[] bytes = exportData.ToArray();
                    return File(bytes, GlobalConst.EXCEL_FORMAT_TYPE, $"{fileName}.xlsx");
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public async Task<IActionResult> UploadForm()
        {
            await Task.Delay(0);
            ViewBag.errorCount = TempData["errorCount"] == null ? -1 : TempData["errorCount"];
            return View();
        }
        public async Task<IActionResult> Upload()
        {
            try
            {
                IFormFile file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    var result = Extensions.Global.ImportExcel(file, GlobalConst.Upload, string.Empty);

                    if (result.Rows.Count > 0)
                    {
                        var dataSubjectClassifications = await _classificationSubjectService.GetAll();
                        var dataClassifications = await _classificationService.GetAll();

                        List<TrxSubjectClassification> models = new();
                        TrxSubjectClassification model;

                        bool valid = true;
                        int errorCount = 0;

                        result.Columns.Add("Keterangan");
                        var classificationDetail = await _classificationService.GetAll();

                        foreach (DataRow row in result.Rows)
                        {
                            string error = string.Empty;
                            var dataClassification = dataClassifications.Where(x => x.ClassificationCode == row[3].ToString()).FirstOrDefault();

                            if (dataClassification == null)
                            {
                                valid = false;
                                error = "_Kode Klasifikasi tidak ditemukan";
                            }
                            if (dataSubjectClassifications.Where(x => x.SubjectClassificationCode == row[1].ToString()).Count() > 0)
                            {
                                valid = false;
                                error = "_Kode Subyek Klasifikasi sudah ada!";
                            }
                            else
                            {
                                if (models.Where(x => x.SubjectClassificationCode == row[1].ToString()).Count() > 0)
                                {
                                    valid = false;
                                    error = "_Kode Subyek Klasifikasi sudah ada!";
                                }
                            }


                            if (valid)
                            {
                                model = new();
                                model.ClassificationId = Guid.NewGuid();
                                model.SubjectClassificationCode = row[1].ToString();
                                model.SubjectClassificationName = row[2].ToString();
                                model.ClassificationId = dataClassification.ClassificationId;
                                model.IsActive = true;
                                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                                model.CreatedDate = DateTime.Now;

                                models.Add(model);
                            }
                            else
                            {
                                errorCount++;
                            }
                            row["Keterangan"] = error;
                        }
                        ViewBag.result = JsonConvert.SerializeObject(result);
                        ViewBag.errorCount = errorCount;

                        if (valid)
                            await _classificationSubjectService.InsertBulk(models);
                    }
                    return View(GlobalConst.UploadForm);
                }
                else
                {
                    TempData["errorCount"] = 100000001;
                    return RedirectToAction(GlobalConst.UploadForm);

                }
            }
            catch (Exception ex)
            {
                TempData["errorCount"] = 100000001;
                return RedirectToAction(GlobalConst.UploadForm);
            }
        }
        #endregion

        #region HELPER
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.ClassificationSubject, new { Area = GlobalConst.MasterData });
        #endregion
    }
}
