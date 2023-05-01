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

namespace Ardita.Areas.MasterData.Controllers;

[CustomAuthorizeAttribute]
[Area(Const.MasterData)]
public class ClassificationController : BaseController<TrxClassification>
{
    #region MEMBER AND CTR
    public ClassificationController(IClassificationService classificationService, IClassificationTypeService classificationTypeService)
    {
        _classificationService = classificationService;
        _classificationTypeService = classificationTypeService;
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
        return View(Const.Form, new TrxClassification());
    }
    public override async Task<IActionResult> Update(Guid Id)
    {
        var model = await _classificationService.GetById(Id);
        if (model.Any())
        {
            ViewBag.listClassificationType = await BindClassificationTypes(); 
            return View(Const.Form, model.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }
    public override async Task<IActionResult> Remove(Guid Id)
    {
        var model = await _classificationService.GetById(Id);
        if (model.Any())
        {
            ViewBag.listClassificationType = await BindClassificationTypes();
            return View(Const.Form, model.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }
    public override async Task<IActionResult> Detail(Guid Id)
    {
        var model = await _classificationService.GetById(Id);
        if (model.Any())
        {
            ViewBag.listClassificationType = await BindClassificationTypes();
            return View(Const.Form, model.FirstOrDefault());
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
    public async Task DownloadTemplate()
    {
        try
        {
            string fileName = $"{Const.Template}-{nameof(TrxClassification).ToCleanNameOf()}";
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet(nameof(TrxClassification).ToCleanNameOf());
            ISheet excelSheetParent = workbook.CreateSheet(nameof(MstTypeClassification).ToCleanNameOf());

            IRow row = excelSheet.CreateRow(0);
            IRow rowParent = excelSheetParent.CreateRow(0);

            row.CreateCell(0).SetCellValue(nameof(TrxClassification.ClassificationCode));
            row.CreateCell(1).SetCellValue(nameof(TrxClassification.ClassificationName));
            row.CreateCell(2).SetCellValue(nameof(MstTypeClassification.TypeClassificationCode));

            rowParent.CreateCell(0).SetCellValue(nameof(MstTypeClassification.TypeClassificationCode));
            rowParent.CreateCell(1).SetCellValue(nameof(MstTypeClassification.TypeClassificationName));

            var dataclassificationType = await _classificationTypeService.GetAll();

            int no = 1;
            foreach (var item in dataclassificationType)
            {
                rowParent = excelSheetParent.CreateRow(no);

                rowParent.CreateCell(0).SetCellValue(item.TypeClassificationCode);
                rowParent.CreateCell(1).SetCellValue(item.TypeClassificationName);
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
            string fileName = nameof(TrxClassification).ToCleanNameOf();
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);

            var data = await _classificationService.GetAll();
            var type = await _classificationTypeService.GetAll();

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet(nameof(TrxClassification).ToCleanNameOf());

            IRow row = excelSheet.CreateRow(0);

            row.CreateCell(0).SetCellValue(nameof(TrxClassification.ClassificationCode));
            row.CreateCell(1).SetCellValue(nameof(TrxClassification.ClassificationName));
            row.CreateCell(2).SetCellValue(nameof(MstTypeClassification.TypeClassificationCode));
            row.CreateCell(3).SetCellValue(nameof(MstTypeClassification.TypeClassificationName));

            int no = 1;
            foreach (var item in data)
            {
                var typeData = type.Where(x => x.TypeClassificationId == item.TypeClassificationId).FirstOrDefault();
                if (typeData != null)
                {
                    row = excelSheet.CreateRow(no);
                    row.CreateCell(0).SetCellValue(item.ClassificationCode);
                    row.CreateCell(1).SetCellValue(item.ClassificationName);
                    row.CreateCell(2).SetCellValue(typeData.TypeClassificationCode);
                    row.CreateCell(3).SetCellValue(typeData.TypeClassificationName);
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
            var result = Extensions.Global.ImportExcel(file, Const.Upload, string.Empty);

            var type = await _classificationTypeService.GetAll();

            List<TrxClassification> models = new();
            TrxClassification model;

            foreach (DataRow row in result.Rows)
            {
                var dataType = type.Where(x => x.TypeClassificationCode == row[2].ToString()).FirstOrDefault();
                if (dataType != null)
                {
                    model = new();
                    model.ClassificationId = Guid.NewGuid();
                    model.ClassificationCode = row[0].ToString();
                    model.ClassificationName = row[1].ToString();
                    model.TypeClassificationId = dataType.TypeClassificationId;
                    model.IsActive = true;
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;

                    models.Add(model);
                }
            }
            await _classificationService.InsertBulk(models);

            return RedirectToIndex();
        }
        catch (Exception ex)
        {
            throw new Exception();
        }
    }
    #endregion
    #region HELPER
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.Classification, new { Area = Const.MasterData });
    #endregion
}
