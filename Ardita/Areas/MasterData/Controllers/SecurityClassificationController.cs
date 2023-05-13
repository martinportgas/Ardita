using Ardita.Controllers;
using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace Ardita.Areas.MasterData.Controllers;

[CustomAuthorize]
[Area(GlobalConst.MasterData)]
public class SecurityClassificationController : BaseController<MstSecurityClassification>
{

    #region MEMBER AND CTR

    public SecurityClassificationController(ISecurityClassificationService securityClassificationService) => _securityClassificationService = securityClassificationService;
    #endregion

    #region MAIN ACTION
    public override async Task<ActionResult> Index() => await base.Index();

    public override async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            var result = await _securityClassificationService.GetList(model);

            return Json(result);

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public override async Task<IActionResult> Add()
    {
        await Task.Delay(0);
        return View(GlobalConst.Form, new MstSecurityClassification());
    }

    public override async Task<IActionResult> Update(Guid Id)
    {
        var data = await _securityClassificationService.GetById(Id);
        if (data.Any())
        {
            return View(GlobalConst.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public override async Task<IActionResult> Remove(Guid Id)
    {
        var data = await _securityClassificationService.GetById(Id);
        if (data.Any())
        {
            return View(GlobalConst.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public override async Task<IActionResult> Detail(Guid Id)
    {
        var data = await _securityClassificationService.GetById(Id);
        if (data.Any())
        {
            return View(GlobalConst.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Save(MstSecurityClassification model)
    {
        if (model != null)
        {
            if (model.SecurityClassificationId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _securityClassificationService.Update(model);
            }

            else
            {
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                await _securityClassificationService.Insert(model);
            }
        }
        return RedirectToIndex();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Delete(MstSecurityClassification model)
    {
        if (model != null && model.SecurityClassificationId != Guid.Empty)
        {
            await _securityClassificationService.Delete(model);
        }
        return RedirectToIndex();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload()
    {
        try
        {
            IFormFile file = Request.Form.Files[0];

            var result = Extensions.Global.ImportExcel(file, GlobalConst.Upload, string.Empty);

            List<MstSecurityClassification> securityClassifications = new();
            MstSecurityClassification securityClassification;

            foreach (DataRow row in result.Rows)
            {
                securityClassification = new();
                securityClassification.SecurityClassificationId = Guid.NewGuid();

                securityClassification.SecurityClassificationCode = row[1].ToString();
                securityClassification.SecurityClassificationName = row[2].ToString();

                securityClassification.IsActive = true;
                securityClassification.CreatedBy = AppUsers.CurrentUser(User).UserId;
                securityClassification.CreatedDate = DateTime.Now;

                securityClassifications.Add(securityClassification);
            }
            await _securityClassificationService.InsertBulk(securityClassifications);
            return RedirectToIndex();
        }
        catch (Exception)
        {

            throw new Exception();
        }

    }
    public async Task<IActionResult> Export()
    {
        try
        {
            string fileName = nameof(MstSecurityClassification).ToCleanNameOf();
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);

            var securityClassifications = await _securityClassificationService.GetAll();

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet(nameof(MstSecurityClassification).ToCleanNameOf());

            IRow row = excelSheet.CreateRow(0);

            row.CreateCell(0).SetCellValue(GlobalConst.No);
            row.CreateCell(1).SetCellValue(nameof(MstSecurityClassification.SecurityClassificationCode));
            row.CreateCell(2).SetCellValue(nameof(MstSecurityClassification.SecurityClassificationName));

            int no = 1;
            foreach (var item in securityClassifications)
            {
                row = excelSheet.CreateRow(no);
                row.CreateCell(0).SetCellValue(no);
                row.CreateCell(1).SetCellValue(item.SecurityClassificationCode);
                row.CreateCell(2).SetCellValue(item.SecurityClassificationName);

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
    public async Task<IActionResult> DownloadTemplate()
    {
        try
        {
            string fileName = $"{GlobalConst.Template}-{nameof(MstSecurityClassification).ToCleanNameOf()}";
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet(nameof(MstSecurityClassification).ToCleanNameOf());

            IRow row = excelSheet.CreateRow(0);

            row.CreateCell(0).SetCellValue(GlobalConst.No);
            row.CreateCell(1).SetCellValue(nameof(MstSecurityClassification.SecurityClassificationCode));
            row.CreateCell(2).SetCellValue(nameof(MstSecurityClassification.SecurityClassificationName));

            using (var exportData = new MemoryStream())
            {
                workbook.Write(exportData);
                byte[] bytes = exportData.ToArray();
                return File(bytes, GlobalConst.EXCEL_FORMAT_TYPE, $"{fileName}.xlsx");
            }
        }
        catch (Exception)
        {
            throw new Exception();
        }
    }
    #endregion

    #region HELPER
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.SecurityClassification, new { Area = GlobalConst.MasterData });
    #endregion
}
