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
public class GmdController : BaseController<MstGmd>
{
    #region MEMBER AND CTR
    public GmdController(IGmdService gmdService) => _gmdService = gmdService;
    #endregion

    #region MAIN ACTION
    public override async Task<ActionResult> Index() => await base.Index();

    public override async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            var result = await _gmdService.GetList(model);

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
        return View(GlobalConst.Form, new MstGmd());
    }

    public override async Task<IActionResult> Update(Guid Id)
    {
        var data = await _gmdService.GetById(Id);
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
        var data = await _gmdService.GetById(Id);
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
        var data = await _gmdService.GetById(Id);
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
    public override async Task<IActionResult> Save(MstGmd model)
    {
        if (model != null)
        {
            if (model.GmdId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _gmdService.Update(model);
            }

            else
            {
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                await _gmdService.Insert(model);
            }
        }
        return RedirectToIndex();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Delete(MstGmd model)
    {
        if (model != null && model.GmdId != Guid.Empty)
        {
            model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
            model.UpdatedDate = DateTime.Now;
            await _gmdService.Delete(model);
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

            List<MstGmd> gmds = new();
            MstGmd gmd;

            foreach (DataRow row in result.Rows)
            {
                gmd = new();
                gmd.GmdId = Guid.NewGuid();

                gmd.GmdCode = row[1].ToString();
                gmd.GmdName = row[2].ToString();

                gmd.IsActive = true;
                gmd.CreatedBy = AppUsers.CurrentUser(User).UserId;
                gmd.CreatedDate = DateTime.Now;

                gmds.Add(gmd);
            }
            await _gmdService.InsertBulk(gmds);
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
            string fileName = nameof(MstGmd).ToCleanNameOf();
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);

            var gmds = await _gmdService.GetAll();

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet(nameof(MstGmd).ToCleanNameOf());

            IRow row = excelSheet.CreateRow(0);

            row.CreateCell(0).SetCellValue(GlobalConst.No);
            row.CreateCell(1).SetCellValue(nameof(MstGmd.GmdCode));
            row.CreateCell(2).SetCellValue(nameof(MstGmd.GmdName));

            int no = 1;
            foreach (var item in gmds)
            {
                row = excelSheet.CreateRow(no);
                row.CreateCell(0).SetCellValue(no);
                row.CreateCell(1).SetCellValue(item.GmdCode);
                row.CreateCell(2).SetCellValue(item.GmdName);

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
            string fileName = $"{GlobalConst.Template}-{nameof(MstGmd).ToCleanNameOf()}";
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet(nameof(MstGmd).ToCleanNameOf());

            IRow row = excelSheet.CreateRow(0);

            row.CreateCell(0).SetCellValue(GlobalConst.No);
            row.CreateCell(1).SetCellValue(nameof(MstGmd.GmdCode));
            row.CreateCell(2).SetCellValue(nameof(MstGmd.GmdName));

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
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.Gmd, new { Area = GlobalConst.MasterData });
    #endregion
}
