using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(Const.MasterData)]
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
            return View(Const.Form, new TrxSubSubjectClassification());
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

                return View(Const.Form, model);
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

                return View(Const.Form, model);
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

                return View(Const.Form, model);
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
        public async Task<IActionResult> DownloadTemplate()
        {
            try
            {
                string fileName = $"{Const.Template}-{nameof(TrxSubSubjectClassification).ToCleanNameOf()}";
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxSubSubjectClassification).ToCleanNameOf());
                ISheet excelSheetCreator = workbook.CreateSheet(nameof(MstCreator).ToCleanNameOf());
                ISheet excelSheetSubject = workbook.CreateSheet(nameof(TrxSubjectClassification).ToCleanNameOf());
                ISheet excelSheetSecurity = workbook.CreateSheet(nameof(MstSecurityClassification).ToCleanNameOf());

                IRow row = excelSheet.CreateRow(0);
                IRow rowCreator = excelSheetCreator.CreateRow(0);
                IRow rowSubject = excelSheetSubject.CreateRow(0);
                IRow rowSecurity = excelSheetSecurity.CreateRow(0);

                row.CreateCell(0).SetCellValue(nameof(TrxSubSubjectClassification.SubSubjectClassificationCode));
                row.CreateCell(1).SetCellValue(nameof(TrxSubSubjectClassification.SubSubjectClassificationName));
                row.CreateCell(2).SetCellValue(nameof(MstCreator.CreatorCode));
                row.CreateCell(3).SetCellValue(nameof(TrxSubjectClassification.SubjectClassificationCode));
                row.CreateCell(4).SetCellValue(nameof(MstSecurityClassification.SecurityClassificationCode));
                row.CreateCell(5).SetCellValue(nameof(TrxSubSubjectClassification.RetentionActive));
                row.CreateCell(6).SetCellValue(nameof(TrxSubSubjectClassification.RetentionInactive));
                row.CreateCell(7).SetCellValue(nameof(TrxSubSubjectClassification.BasicInformation));

                rowCreator.CreateCell(0).SetCellValue(nameof(MstCreator.CreatorCode));
                rowCreator.CreateCell(1).SetCellValue(nameof(MstCreator.CreatorName));

                var dataCreator = await _archiveCreatorService.GetAll();

                int no = 1;
                foreach (var item in dataCreator)
                {
                    rowCreator = excelSheetCreator.CreateRow(no);

                    rowCreator.CreateCell(0).SetCellValue(item.CreatorCode);
                    rowCreator.CreateCell(1).SetCellValue(item.CreatorName);
                    no += 1;
                }

                rowSubject.CreateCell(0).SetCellValue(nameof(TrxSubjectClassification.SubjectClassificationCode));
                rowSubject.CreateCell(1).SetCellValue(nameof(TrxSubjectClassification.SubjectClassificationName));

                var dataclassificationSubject = await _classificationSubjectService.GetAll();

                no = 1;
                foreach (var item in dataclassificationSubject)
                {
                    rowSubject = excelSheetSubject.CreateRow(no);

                    rowSubject.CreateCell(0).SetCellValue(item.SubjectClassificationCode);
                    rowSubject.CreateCell(1).SetCellValue(item.SubjectClassificationName);
                    no += 1;
                }

                rowSecurity.CreateCell(0).SetCellValue(nameof(MstSecurityClassification.SecurityClassificationCode));
                rowSecurity.CreateCell(1).SetCellValue(nameof(MstSecurityClassification.SecurityClassificationName));

                var dataSecurity = await _securityClassificationService.GetAll();

                no = 1;
                foreach (var item in dataSecurity)
                {
                    rowSecurity = excelSheetSecurity.CreateRow(no);

                    rowSecurity.CreateCell(0).SetCellValue(item.SecurityClassificationCode);
                    rowSecurity.CreateCell(1).SetCellValue(item.SecurityClassificationName);
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

                row.CreateCell(0).SetCellValue(nameof(TrxSubSubjectClassification.SubSubjectClassificationCode));
                row.CreateCell(1).SetCellValue(nameof(TrxSubSubjectClassification.SubSubjectClassificationName));
                row.CreateCell(2).SetCellValue(nameof(MstCreator.CreatorCode));
                row.CreateCell(3).SetCellValue(nameof(MstCreator.CreatorName));
                row.CreateCell(4).SetCellValue(nameof(TrxSubjectClassification.SubjectClassificationCode));
                row.CreateCell(5).SetCellValue(nameof(TrxSubjectClassification.SubjectClassificationName));
                row.CreateCell(6).SetCellValue(nameof(MstSecurityClassification.SecurityClassificationCode));
                row.CreateCell(7).SetCellValue(nameof(MstSecurityClassification.SecurityClassificationName));
                row.CreateCell(8).SetCellValue(nameof(TrxSubSubjectClassification.RetentionActive));
                row.CreateCell(9).SetCellValue(nameof(TrxSubSubjectClassification.RetentionInactive));
                row.CreateCell(10).SetCellValue(nameof(TrxSubSubjectClassification.BasicInformation));

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
                            row.CreateCell(0).SetCellValue(item.SubSubjectClassificationCode);
                            row.CreateCell(1).SetCellValue(item.SubSubjectClassificationName);
                            row.CreateCell(2).SetCellValue(dataCreator.CreatorCode);
                            row.CreateCell(3).SetCellValue(dataCreator.CreatorName);
                            row.CreateCell(4).SetCellValue(dataSubject.SubjectClassificationCode);
                            row.CreateCell(5).SetCellValue(dataSubject.SubjectClassificationName);
                            row.CreateCell(6).SetCellValue(dataSecurity.SecurityClassificationCode);
                            row.CreateCell(7).SetCellValue(dataSecurity.SecurityClassificationName);
                            row.CreateCell(8).SetCellValue(item.RetentionActive.ToString());
                            row.CreateCell(9).SetCellValue(item.RetentionInactive.ToString());
                            row.CreateCell(10).SetCellValue(item.BasicInformation);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload()
        {
            IFormFile file = Request.Form.Files[0];

            var result = Extensions.Global.ImportExcel(file, Const.Upload, string.Empty);

            var dataSubjects = await _classificationSubjectService.GetAll();
            var dataCreators = await _archiveCreatorService.GetAll();
            var dataSecuritys = await _securityClassificationService.GetAll();

            List<TrxSubSubjectClassification> models = new();
            TrxSubSubjectClassification model;

            foreach (DataRow row in result.Rows)
            {
                var dataCreator = dataCreators.Where(x => x.CreatorCode == row[3].ToString()).FirstOrDefault();
                var dataSubject = dataSubjects.Where(x => x.SubjectClassificationCode == row[3].ToString()).FirstOrDefault();
                var dataSecurity = dataSecuritys.Where(x => x.SecurityClassificationCode == row[4].ToString()).FirstOrDefault();

                if (dataCreator != null && dataSubject != null && dataSecurity != null)
                {
                    model = new();
                    model.SubSubjectClassificationId = Guid.NewGuid();
                    model.SubSubjectClassificationCode = row[0].ToString();
                    model.SubSubjectClassificationName = row[1].ToString();
                    model.CreatorId = dataCreator.CreatorId;
                    model.SubjectClassificationId = dataSubject.SubjectClassificationId;
                    model.SecurityClassificationId = dataSecurity.SecurityClassificationId;
                    model.RetentionActive = (int)row[5];
                    model.RetentionInactive = (int)row[6];
                    model.BasicInformation = row[7].ToString();
                    model.IsActive = true;  
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;

                    models.Add(model);
                }
            }
            await _classificationSubSubjectService.InsertBulk(models);

            return RedirectToIndex();
        }
        #endregion
        #region HELPER
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.ClassificationSubSubject, new { Area = Const.MasterData });
        #endregion
    }
}
