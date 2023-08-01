using Ardita.Controllers;
using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
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
        return await InitFormView(Id);
    }

    public override async Task<IActionResult> Remove(Guid Id)
    {
        return await InitFormView(Id);
    }

    public override async Task<IActionResult> Detail(Guid Id)
    {
        return await InitFormView(Id);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Save(MstGmd model)
    {
        if (model != null)
        {
            var listDetail = Request.Form[GlobalConst.DetailArray].ToArray();

            if (model.GmdId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _gmdService.Update(model, listDetail!);
            }

            else
            {
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                await _gmdService.Insert(model, listDetail!);
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
    public async Task<IActionResult> UploadForm()
    {
        await Task.Delay(0);
        ViewBag.errorCount = TempData["errorCount"] == null ? -1 : TempData["errorCount"];
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
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
                    List<MstGmd> gmds = new();
                    MstGmd gmd;

                    bool valid = true;
                    int errorCount = 0;

                    result.Columns.Add("Keterangan");

                    var gmdDetails = await _gmdService.GetAll();



                    foreach (DataRow row in result.Rows)
                    {
                        string error = string.Empty;

                        if (gmdDetails.Where(x => x.GmdCode == row[1].ToString()).Count() > 0)
                        {
                            valid = false;
                            error = "_Kode GMD sudah ada";
                        }
                        else
                        {
                            if (gmds.Where(x => x.GmdCode == row[1].ToString()).Count() > 0)
                            {
                                valid = false;
                                error = "_Kode GMD sudah ada";
                            }
                        }

                        if (valid)
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
                        else
                        {
                            errorCount++;
                        }
                        row["Keterangan"] = error;
                    }
                    ViewBag.result = JsonConvert.SerializeObject(result);
                    ViewBag.errorCount = errorCount;

                    if (valid)
                        await _gmdService.InsertBulk(gmds);
                }
            }
            else
            {
                TempData["errorCount"] = 100000001;
                return RedirectToAction(GlobalConst.UploadForm);
            }
            return View(GlobalConst.UploadForm);
        }
        catch (Exception)
        {

            TempData["errorCount"] = 100000001;
            return RedirectToAction(GlobalConst.UploadForm);
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

    private async Task<IActionResult> InitFormView(Guid Id)
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
    #endregion
}
