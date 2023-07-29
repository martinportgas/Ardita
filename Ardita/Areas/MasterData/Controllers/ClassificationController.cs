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

namespace Ardita.Areas.MasterData.Controllers;

[CustomAuthorizeAttribute]
[Area(GlobalConst.MasterData)]
public class ClassificationController : BaseController<TrxClassification>
{
    #region MEMBER AND CTR
    public ClassificationController(IClassificationService classificationService, IClassificationTypeService classificationTypeService, IArchiveCreatorService archiveCreatorService)
    {
        _classificationService = classificationService;
        _classificationTypeService = classificationTypeService;
        _archiveCreatorService = archiveCreatorService;
    }
    #endregion
    #region MAIN ACTION
    public override async Task<ActionResult> Index() => await base.Index();

    public override async Task<JsonResult> GetData(DataTablePostModel model)
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
    public override async Task<IActionResult> Add()
    {
        ViewBag.listClassificationType = await BindClassificationTypes();
        ViewBag.ListArchiveCreator = await BindArchiveCreators();
        return View(GlobalConst.Form, new TrxClassification());
    }
    public override async Task<IActionResult> Update(Guid Id)
    {
        var model = await _classificationService.GetById(Id);
        if (model != null)
        {
            ViewBag.listClassificationType = await BindClassificationTypes();
            ViewBag.ListArchiveCreator = await BindArchiveCreators();
            return View(GlobalConst.Form, model);
        }
        else
        {
            return RedirectToIndex();
        }
    }
    public override async Task<IActionResult> Remove(Guid Id)
    {
        var model = await _classificationService.GetById(Id);
        if (model != null)
        {
            ViewBag.listClassificationType = await BindClassificationTypes();
            ViewBag.ListArchiveCreator = await BindArchiveCreators();
            return View(GlobalConst.Form, model);
        }
        else
        {
            return RedirectToIndex();
        }
    }
    public override async Task<IActionResult> Detail(Guid Id)
    {
        var model = await _classificationService.GetById(Id);
        if (model != null)
        {
            ViewBag.listClassificationType = await BindClassificationTypes();
            ViewBag.ListArchiveCreator = await BindArchiveCreators();
            return View(GlobalConst.Form, model);
        }
        else
        {
            return RedirectToIndex();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Save(TrxClassification model)
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
        return RedirectToIndex();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Delete(TrxClassification model)
    {
        int result = 0;
        if (model != null && model.ClassificationId != Guid.Empty)
        {
            model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
            result = await _classificationService.Delete(model);
        }
        return RedirectToIndex();
    }
    public async Task<IActionResult> DownloadTemplate()
    {
        try
        {
            string fileName = $"{GlobalConst.Template}-{nameof(TrxClassification).ToCleanNameOf()}";
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet(nameof(TrxClassification).ToCleanNameOf());
            ISheet excelSheetParent = workbook.CreateSheet(nameof(MstTypeClassification).ToCleanNameOf());
            ISheet excelSheetCreator = workbook.CreateSheet(nameof(MstCreator).ToCleanNameOf());

            IRow row = excelSheet.CreateRow(0);
            IRow rowParent = excelSheetParent.CreateRow(0);
            IRow rowCreator = excelSheetCreator.CreateRow(0);

            row.CreateCell(0).SetCellValue(nameof(GlobalConst.No));
            row.CreateCell(1).SetCellValue(nameof(TrxClassification.ClassificationCode));
            row.CreateCell(2).SetCellValue(nameof(TrxClassification.ClassificationName));
            row.CreateCell(3).SetCellValue(nameof(MstTypeClassification.TypeClassificationCode));
            row.CreateCell(4).SetCellValue(nameof(MstCreator.CreatorCode));

            rowParent.CreateCell(0).SetCellValue(nameof(GlobalConst.No));
            rowParent.CreateCell(1).SetCellValue(nameof(MstTypeClassification.TypeClassificationCode));
            rowParent.CreateCell(2).SetCellValue(nameof(MstTypeClassification.TypeClassificationName));

            rowCreator.CreateCell(0).SetCellValue(nameof(GlobalConst.No));
            rowCreator.CreateCell(1).SetCellValue(nameof(MstCreator.CreatorCode));
            rowCreator.CreateCell(2).SetCellValue(nameof(MstCreator.CreatorName));

            var dataclassificationType = await _classificationTypeService.GetAll();

            int no = 1;
            foreach (var item in dataclassificationType)
            {
                rowParent = excelSheetParent.CreateRow(no);

                rowParent.CreateCell(0).SetCellValue(item.TypeClassificationCode);
                rowParent.CreateCell(1).SetCellValue(item.TypeClassificationName);
                no += 1;
            }

            var creators = await _archiveCreatorService.GetAll();
            no = 1;
            foreach (var item in creators)
            {
                rowCreator = excelSheetCreator.CreateRow(no);

                rowCreator.CreateCell(0).SetCellValue(no);
                rowCreator.CreateCell(1).SetCellValue(item.CreatorCode);
                rowCreator.CreateCell(2).SetCellValue(item.CreatorName);
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
            string fileName = nameof(TrxClassification).ToCleanNameOf();
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);

            var data = await _classificationService.GetAll();
            var type = await _classificationTypeService.GetAll();
            var creators = await _archiveCreatorService.GetAll();

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet(nameof(TrxClassification).ToCleanNameOf());

            IRow row = excelSheet.CreateRow(0);

            row.CreateCell(0).SetCellValue(nameof(GlobalConst.No));
            row.CreateCell(1).SetCellValue(nameof(TrxClassification.ClassificationCode));
            row.CreateCell(2).SetCellValue(nameof(TrxClassification.ClassificationName));
            row.CreateCell(3).SetCellValue(nameof(MstTypeClassification.TypeClassificationCode));
            row.CreateCell(4).SetCellValue(nameof(MstTypeClassification.TypeClassificationName));
            row.CreateCell(5).SetCellValue(nameof(MstCreator.CreatorName));

            int no = 1;
            foreach (var item in data)
            {
                var typeData = type.Where(x => x.TypeClassificationId == item.TypeClassificationId).FirstOrDefault();
                var creatorData = creators.Where(x => x.CreatorId == item.CreatorId).FirstOrDefault();
                if (typeData != null && creatorData != null)
                {
                    row = excelSheet.CreateRow(no);
                    row.CreateCell(0).SetCellValue(no);
                    row.CreateCell(1).SetCellValue(item.ClassificationCode);
                    row.CreateCell(2).SetCellValue(item.ClassificationName);
                    row.CreateCell(3).SetCellValue(typeData.TypeClassificationCode);
                    row.CreateCell(4).SetCellValue(typeData.TypeClassificationName);
                    row.CreateCell(5).SetCellValue(creatorData.CreatorName);
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
                    var type = await _classificationTypeService.GetAll();
                    var creator = await _archiveCreatorService.GetAll();

                    List<TrxClassification> models = new();
                    TrxClassification model;

                    bool valid = true;
                    int errorCount = 0;

                    result.Columns.Add("Keterangan");
                    var classificationDetail = await _classificationService.GetAll();

                    foreach (DataRow row in result.Rows)
                    {
                        string error = string.Empty;
                        var dataType = type.Where(x => x.TypeClassificationCode == row[3].ToString()).FirstOrDefault();
                        var dataCreator = creator.Where(x => x.CreatorCode == row[4].ToString()).FirstOrDefault();

                        if (classificationDetail.Where(x => x.ClassificationCode == row[1].ToString()).Count() > 0)
                        {
                            valid = false;
                            error = "_Kode Klasifikasi sudah ada";
                        }
                        else if (dataType == null)
                        {
                            valid = false;
                            error = "_Tipe Klasifikasi tidak ditemukan!";
                        }
                        else if (dataCreator == null)
                        {
                            valid = false;
                            error = "_Pencipta tidak ditemukan!";
                        }


                        if (valid)
                        {
                            model = new();
                            model.ClassificationId = Guid.NewGuid();
                            model.ClassificationCode = row[1].ToString();
                            model.ClassificationName = row[2].ToString();
                            model.TypeClassificationId = dataType.TypeClassificationId;
                            model.CreatorId = dataCreator.CreatorId;
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
                        await _classificationService.InsertBulk(models);
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
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.Classification, new { Area = GlobalConst.MasterData });
    #endregion
}
