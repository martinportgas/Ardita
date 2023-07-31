using Ardita.Controllers;
using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
namespace Ardita.Areas.MasterData.Controllers;

[CustomAuthorize]
[Area(GlobalConst.MasterData)]
public class ArchiveCreatorController : BaseController<MstCreator>
{
    #region MEMBER AND CTR
    public ArchiveCreatorController(IArchiveUnitService archiveUnitService, IArchiveCreatorService archiveCreatorService)
    {
        _archiveUnitService = archiveUnitService;
        _archiveCreatorService = archiveCreatorService;
    }
    #endregion

    #region MAIN ACTION
    public override async Task<ActionResult> Index() => await base.Index();

    public override async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            var result = await _archiveCreatorService.GetList(model);

            return Json(result);

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public override async Task<IActionResult> Add()
    {
        ViewBag.listArchiveUnit = await BindArchiveUnits();

        await Task.Delay(0);

        return View(GlobalConst.Form, new MstCreator());
    }

    public override async Task<IActionResult> Update(Guid Id)
    {
        var data = await _archiveCreatorService.GetById(Id);
        if (data.Any())
        {
            ViewBag.listArchiveUnit = await BindArchiveUnits();

            return View(GlobalConst.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public override async Task<IActionResult> Remove(Guid Id)
    {
        var data = await _archiveCreatorService.GetById(Id);
        if (data.Any())
        {
            ViewBag.listArchiveUnit = await BindArchiveUnits();

            return View(GlobalConst.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public override async Task<IActionResult> Detail(Guid Id)
    {
        var data = await _archiveCreatorService.GetById(Id);
        if (data.Any())
        {
            ViewBag.listArchiveUnit = await BindArchiveUnits();

            return View(GlobalConst.Form, data.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Save(MstCreator model)
    {
        if (model != null)
        {
            if (model.CreatorId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _archiveCreatorService.Update(model);
            }

            else
            {
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                await _archiveCreatorService.Insert(model);
            }
        }
        return RedirectToIndex();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Delete(MstCreator model)
    {
        if (model != null && model.CreatorId != Guid.Empty)
        {
            await _archiveCreatorService.Delete(model);
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
                var archiveUnits = await _archiveUnitService.GetAll();

                if (result.Rows.Count > 0)
                {
                    List<MstCreator> mstCreators = new();
                    MstCreator mstCreator;


                    bool valid = true;
                    int errorCount = 0;

                    result.Columns.Add("Keterangan");

                    var archiveCreatorDetail = await _archiveCreatorService.GetAll();
                    var archiveUnit = await _archiveUnitService.GetAll();


                    foreach (DataRow row in result.Rows)
                    {
                        string error = string.Empty;

                        if (archiveCreatorDetail.Where(x => x.CreatorCode == row[2].ToString()).Count() > 0)
                        {
                            valid = false;
                            error = "_Kode Pencipta sudah ada";
                        }

                        if (archiveUnit.Where(x => x.ArchiveUnitCode == row[3].ToString()).Count() == 0)
                        {
                            valid = false;
                            error = "_Kode Lokasi Simpan tidak ditemukabn";
                        }

                        if (valid)
                        {
                            mstCreator = new();
                            mstCreator.CreatorId = Guid.NewGuid();

                            mstCreator.CreatorCode = row[1].ToString();
                            mstCreator.CreatorName = row[2].ToString();
                            mstCreator.ArchiveUnitId = archiveUnits.Where(x => x.ArchiveUnitCode.Contains(row[3].ToString())).FirstOrDefault().ArchiveUnitId;


                            mstCreator.IsActive = true;
                            mstCreator.CreatedBy = AppUsers.CurrentUser(User).UserId;
                            mstCreator.CreatedDate = DateTime.Now;

                            mstCreators.Add(mstCreator);
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
                        await _archiveCreatorService.InsertBulk(mstCreators);
                }
                return View(GlobalConst.UploadForm);
            }
            else
            {
                TempData["errorCount"] = 100000001;
                return RedirectToAction(GlobalConst.UploadForm);
            }
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
            string fileName = nameof(MstCreator).ToCleanNameOf();
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);

            var archiveCreators = await _archiveCreatorService.GetAll();

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet(nameof(MstCreator).ToCleanNameOf());

            IRow row = excelSheet.CreateRow(0);

            row.CreateCell(0).SetCellValue(GlobalConst.No);
            row.CreateCell(3).SetCellValue("Kode Unit Pencipta");
            row.CreateCell(4).SetCellValue("Nama Unit Pencipta");
            row.CreateCell(2).SetCellValue("Lokasi Simpan");
            row.CreateCell(1).SetCellValue("Perusahaan");

            int no = 1;
            foreach (var item in archiveCreators)
            {
                row = excelSheet.CreateRow(no);
                row.CreateCell(0).SetCellValue(no);
                row.CreateCell(3).SetCellValue(item.CreatorCode);
                row.CreateCell(4).SetCellValue(item.CreatorName);
                row.CreateCell(2).SetCellValue(item.ArchiveUnit.ArchiveUnitName);
                row.CreateCell(1).SetCellValue(item.ArchiveUnit.Company.CompanyName);
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
            string fileName = $"{GlobalConst.Template}-{nameof(MstCreator).ToCleanNameOf()}";
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet(nameof(MstCreator).ToCleanNameOf());
            ISheet excelSheetArchiveUnits = workbook.CreateSheet(nameof(TrxArchiveUnit).ToCleanNameOf());

            IRow row = excelSheet.CreateRow(0);
            IRow rowArchiveUnits = excelSheetArchiveUnits.CreateRow(0);

            //Archive Creators
            row.CreateCell(0).SetCellValue(GlobalConst.No);
            row.CreateCell(1).SetCellValue("Kode Unit Pencipta");
            row.CreateCell(2).SetCellValue("Nama Unit Pencipta");
            row.CreateCell(3).SetCellValue("kode Lokasi Simpan");

            //Archive Units
            rowArchiveUnits.CreateCell(0).SetCellValue(GlobalConst.No);
            rowArchiveUnits.CreateCell(3).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitCode));
            rowArchiveUnits.CreateCell(4).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitName));
            rowArchiveUnits.CreateCell(2).SetCellValue(nameof(MstCompany.CompanyName));

            var dataArchiveUnits = await _archiveUnitService.GetAll();

            int no = 1;
            foreach (var item in dataArchiveUnits)
            {
                rowArchiveUnits = excelSheetArchiveUnits.CreateRow(no);

                rowArchiveUnits.CreateCell(0).SetCellValue(no);
                rowArchiveUnits.CreateCell(3).SetCellValue(item.ArchiveUnitCode);
                rowArchiveUnits.CreateCell(4).SetCellValue(item.ArchiveUnitName);
                rowArchiveUnits.CreateCell(2).SetCellValue(item.Company.CompanyName);
                no += 1;
            }
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
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.ArchiveCreator, new { Area = GlobalConst.MasterData });

    #endregion
}
