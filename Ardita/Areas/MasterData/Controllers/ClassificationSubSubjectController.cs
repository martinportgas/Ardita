using Ardita.Controllers;
using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(GlobalConst.MasterData)]
    public class ClassificationSubSubjectController : BaseController<TrxSubSubjectClassification>
    {
        #region MEMBER AND CTR
        public ClassificationSubSubjectController(
            IClassificationSubSubjectService classificationSubSubjectService,
            IClassificationSubjectService classificationSubjectService,
            IClassificationTypeService classificationTypeService,
            IClassificationService classificationService,
            IPositionService positionService,
            IArchiveCreatorService archiveCreatorService,
            ISecurityClassificationService securityClassificationService)
        {
            _classificationSubSubjectService = classificationSubSubjectService;
            _classificationSubjectService = classificationSubjectService;
            _classificationTypeService = classificationTypeService;
            _classificationService = classificationService;
            _positionService = positionService;
            _archiveCreatorService = archiveCreatorService;
            _securityClassificationService = securityClassificationService;
        }
        #endregion
        #region MAIN ACTION
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
        public override async Task<IActionResult> Add()
        {
            ViewBag.listClassificationType = await BindClassificationTypes();
            ViewBag.listClassification = await BindClasscifications();
            ViewBag.listClassificationSubject = await BindClasscificationSubjects();
            ViewBag.listPosition = await BindPositions();
            ViewBag.ListArchiveCreator = await BindArchiveCreators();
            ViewBag.ListSecurityClassification = await BindSecurityClassifications();
            return View(GlobalConst.Form, new TrxSubSubjectClassification());
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var model = await _classificationSubSubjectService.GetById(Id);
            if (model != null)
            {
                ViewBag.listClassificationType = await BindClassificationTypes();
                ViewBag.listClassification = await BindClasscifications();
                ViewBag.listClassificationSubject = await BindClasscificationSubjects();
                ViewBag.listPosition = await BindPositions();
                ViewBag.ListArchiveCreator = await BindArchiveCreators();
                ViewBag.ListSecurityClassification = await BindSecurityClassifications();
                ViewBag.subDetail = await _classificationSubSubjectService.GetListDetailPermissionClassifications(Id);

                return View(GlobalConst.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var model = await _classificationSubSubjectService.GetById(Id);
            if (model != null)
            {
                ViewBag.listClassificationType = await BindClassificationTypes();
                ViewBag.listClassification = await BindClasscifications();
                ViewBag.listClassificationSubject = await BindClasscificationSubjects();
                ViewBag.listPosition = await BindPositions();
                ViewBag.ListArchiveCreator = await BindArchiveCreators();
                ViewBag.ListSecurityClassification = await BindSecurityClassifications();
                ViewBag.subDetail = await _classificationSubSubjectService.GetListDetailPermissionClassifications(Id);

                return View(GlobalConst.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var model = await _classificationSubSubjectService.GetById(Id);
            if (model != null)
            {
                ViewBag.listClassificationType = await BindClassificationTypes();
                ViewBag.listClassification = await BindClasscifications();
                ViewBag.listClassificationSubject = await BindClasscificationSubjects();
                ViewBag.listPosition = await BindPositions();
                ViewBag.ListArchiveCreator = await BindArchiveCreators();
                ViewBag.ListSecurityClassification = await BindSecurityClassifications();
                ViewBag.subDetail = await _classificationSubSubjectService.GetListDetailPermissionClassifications(Id);

                return View(GlobalConst.Form, model);
            }
            else
            {
                return RedirectToIndex();
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
            return RedirectToIndex();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Delete(TrxSubSubjectClassification model)
        {
            int result = 0;
            if (model != null && model.SubSubjectClassificationId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                result = await _classificationSubSubjectService.Delete(model);
            }
            return RedirectToIndex();
        }
        public async Task<IActionResult> UploadForm()
        {
            await Task.Delay(0);
            ViewBag.errorCount = TempData["errorCount"] == null ? -1 : TempData["errorCount"];
            return View();
        }
        public async Task<IActionResult> DownloadTemplate()
        {
            try
            {
                string fileName = $"{GlobalConst.Template}-{nameof(TrxSubSubjectClassification).ToCleanNameOf()}";
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxSubSubjectClassification).ToCleanNameOf());
                ISheet excelSheetSubject = workbook.CreateSheet(nameof(TrxSubjectClassification).ToCleanNameOf());
                ISheet excelSheetSecurity = workbook.CreateSheet(nameof(MstSecurityClassification).ToCleanNameOf());

                IRow row = excelSheet.CreateRow(0);
                IRow rowSubject = excelSheetSubject.CreateRow(0);
                IRow rowSecurity = excelSheetSecurity.CreateRow(0);

                row.CreateCell(0).SetCellValue(nameof(GlobalConst.No));
                row.CreateCell(1).SetCellValue(nameof(TrxSubSubjectClassification.SubSubjectClassificationCode));
                row.CreateCell(2).SetCellValue(nameof(TrxSubSubjectClassification.SubSubjectClassificationName));
                row.CreateCell(3).SetCellValue(nameof(TrxSubjectClassification.SubjectClassificationCode));
                row.CreateCell(4).SetCellValue(nameof(MstSecurityClassification.SecurityClassificationCode));
                row.CreateCell(5).SetCellValue(nameof(TrxSubSubjectClassification.RetentionActive));
                row.CreateCell(6).SetCellValue(nameof(TrxSubSubjectClassification.RetentionInactive));
                row.CreateCell(7).SetCellValue(nameof(TrxSubSubjectClassification.BasicInformation));

                rowSubject.CreateCell(0).SetCellValue(nameof(GlobalConst.No));
                rowSubject.CreateCell(1).SetCellValue(nameof(TrxSubjectClassification.SubjectClassificationCode));
                rowSubject.CreateCell(2).SetCellValue(nameof(TrxSubjectClassification.SubjectClassificationName));

                var dataclassificationSubject = await _classificationSubjectService.GetAll();

                int no = 1;
                foreach (var item in dataclassificationSubject)
                {
                    rowSubject = excelSheetSubject.CreateRow(no);

                    rowSubject.CreateCell(0).SetCellValue(no);
                    rowSubject.CreateCell(1).SetCellValue(item.SubjectClassificationCode);
                    rowSubject.CreateCell(2).SetCellValue(item.SubjectClassificationName);
                    no += 1;
                }

                rowSecurity.CreateCell(0).SetCellValue(nameof(GlobalConst.No));
                rowSecurity.CreateCell(1).SetCellValue(nameof(MstSecurityClassification.SecurityClassificationCode));
                rowSecurity.CreateCell(2).SetCellValue(nameof(MstSecurityClassification.SecurityClassificationName));

                var dataSecurity = await _securityClassificationService.GetAll();

                no = 1;
                foreach (var item in dataSecurity)
                {
                    rowSecurity = excelSheetSecurity.CreateRow(no);

                    rowSecurity.CreateCell(0).SetCellValue(no);
                    rowSecurity.CreateCell(1).SetCellValue(item.SecurityClassificationCode);
                    rowSecurity.CreateCell(2).SetCellValue(item.SecurityClassificationName);
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
                string fileName = nameof(TrxSubSubjectClassification).ToCleanNameOf();
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                var data = await _classificationSubSubjectService.GetAll();
                var dataSubjects = await _classificationSubjectService.GetAll();
                var dataCreators = await _archiveCreatorService.GetAll();
                var dataSecuritys = await _securityClassificationService.GetAll();

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxSubSubjectClassification).ToCleanNameOf());

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue(nameof(GlobalConst.No));
                row.CreateCell(1).SetCellValue(nameof(TrxSubSubjectClassification.SubSubjectClassificationCode));
                row.CreateCell(2).SetCellValue(nameof(TrxSubSubjectClassification.SubSubjectClassificationName));
                row.CreateCell(3).SetCellValue(nameof(MstCreator.CreatorCode));
                row.CreateCell(4).SetCellValue(nameof(MstCreator.CreatorName));
                row.CreateCell(5).SetCellValue(nameof(TrxSubjectClassification.SubjectClassificationCode));
                row.CreateCell(6).SetCellValue(nameof(TrxSubjectClassification.SubjectClassificationName));
                row.CreateCell(7).SetCellValue(nameof(MstSecurityClassification.SecurityClassificationCode));
                row.CreateCell(8).SetCellValue(nameof(MstSecurityClassification.SecurityClassificationName));
                row.CreateCell(9).SetCellValue(nameof(TrxSubSubjectClassification.RetentionActive));
                row.CreateCell(10).SetCellValue(nameof(TrxSubSubjectClassification.RetentionInactive));
                row.CreateCell(11).SetCellValue(nameof(TrxSubSubjectClassification.BasicInformation));

                int no = 1;
                if (data.Any())
                {
                    foreach (var item in data)
                    {
                        var dataCreator = dataCreators.Where(x => x.CreatorId == item.CreatorId).FirstOrDefault();
                        var dataSubject = dataSubjects.Where(x => x.SubjectClassificationId == item.SubjectClassificationId).FirstOrDefault();
                        var dataSecurity = dataSecuritys.Where(x => x.SecurityClassificationId == item.SecurityClassificationId).FirstOrDefault();

                        if(dataCreator != null && dataSubject != null && dataSecurity != null)
                        {
                            row = excelSheet.CreateRow(no);
                            row.CreateCell(0).SetCellValue(no);
                            row.CreateCell(1).SetCellValue(item.SubSubjectClassificationCode);
                            row.CreateCell(2).SetCellValue(item.SubSubjectClassificationName);
                            row.CreateCell(3).SetCellValue(dataCreator.CreatorCode);
                            row.CreateCell(4).SetCellValue(dataCreator.CreatorName);
                            row.CreateCell(5).SetCellValue(dataSubject.SubjectClassificationCode);
                            row.CreateCell(6).SetCellValue(dataSubject.SubjectClassificationName);
                            row.CreateCell(7).SetCellValue(dataSecurity.SecurityClassificationCode);
                            row.CreateCell(8).SetCellValue(dataSecurity.SecurityClassificationName);
                            row.CreateCell(9).SetCellValue(item.RetentionActive.ToString());
                            row.CreateCell(10).SetCellValue(item.RetentionInactive.ToString());
                            row.CreateCell(11).SetCellValue(item.BasicInformation);
                            no += 1;
                        }
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
                        var dataSubjects = await _classificationSubjectService.GetAll();
                        var dataCreators = await _archiveCreatorService.GetAll();
                        var dataSecuritys = await _securityClassificationService.GetAll();
                        var dataSubSubjects = await _classificationSubSubjectService.GetAll();

                        List<TrxSubSubjectClassification> models = new();
                        TrxSubSubjectClassification model;

                        bool valid = true;
                        int errorCount = 0;

                        result.Columns.Add("Keterangan");

                        foreach (DataRow row in result.Rows)
                        {
                            string error = string.Empty;

                            if (dataSubjects.Where(x => x.SubjectClassificationCode == row[3].ToString()).ToList().Count == 0)
                            {
                                valid = false;
                                error = "_Kode Subjek Klasifikasi tidak ditemukan";
                            }
                            if (dataSubSubjects.Where(x => x.SubSubjectClassificationCode == row[1].ToString()).Count() > 0)
                            {
                                valid = false;
                                error = "_Kode Sub Subyek Klasifikasi sudah ada!";
                            }
                            else
                            {
                                if (models.Where(x => x.SubSubjectClassificationCode == row[1].ToString()).Count() > 0)
                                {
                                    valid = false;
                                    error = "_Kode Sub Subyek Klasifikasi sudah ada!";
                                }
                            }
                            if (dataSecuritys.Where(x => x.SecurityClassificationCode == row[4].ToString()).Count() == 0)
                            {
                                valid = false;
                                error = "_Kode Keamanan Klasifikasi tidak ditemukan!";
                            }
                            if (!int.TryParse(row[5].ToString(), out int n))
                            {
                                valid = false;
                                error = "_Retensi Aktif harus angka!";
                            }
                            if (!int.TryParse(row[6].ToString(), out int m))
                            {
                                valid = false;
                                error = "_Retensi In Aktif harus angka!";
                            }

                            if (valid)
                            {
                                model = new();
                                model.SubSubjectClassificationId = Guid.NewGuid();
                                model.SubSubjectClassificationCode = row[1].ToString();
                                model.SubSubjectClassificationName = row[2].ToString();
                                model.CreatorId = dataSubjects.Where(x => x.SubjectClassificationCode == row[3].ToString()).FirstOrDefault().Classification.CreatorId;
                                model.SubjectClassificationId = dataSubjects.Where(x => x.SubjectClassificationCode == row[3].ToString()).FirstOrDefault().SubjectClassificationId;
                                model.SecurityClassificationId = dataSecuritys.Where(x => x.SecurityClassificationCode == row[4].ToString()).FirstOrDefault().SecurityClassificationId;
                                model.RetentionActive = int.Parse(row[5].ToString());
                                model.RetentionInactive = int.Parse(row[6].ToString());
                                model.BasicInformation = row[7].ToString();
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
                            await _classificationSubSubjectService.InsertBulk(models);
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
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.ClassificationSubSubject, new { Area = GlobalConst.MasterData });
        #endregion
    }
}
