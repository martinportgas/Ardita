using Ardita.Controllers;
using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
namespace Ardita.Areas.MasterData.Controllers;

[CustomAuthorize]
[Area(GlobalConst.MasterData)]
public class ArchiveUnitController : BaseController<TrxArchiveUnit>
{
    #region MEMBER AND CTR
    public ArchiveUnitController(IArchiveUnitService archiveUnitService, ICompanyService companyService)
    {
        _archiveUnitService = archiveUnitService;
        _companyService = companyService;
    }
    #endregion

    #region MAIN ACTION
    public override async Task<ActionResult> Index() => await base.Index();

    public override async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            var result = await _archiveUnitService.GetList(model);

            return Json(result);

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public override async Task<IActionResult> Add()
    {
        ViewBag.listCompany = await BindCompanies();

        return View(GlobalConst.Form, new TrxArchiveUnit());
    }

    public override async Task<IActionResult> Update(Guid Id)
    {
        var data = await _archiveUnitService.GetById(Id);
        if (data != null)
        {
            if(data.Latitude != null)
                ViewBag.Lat = data.Latitude.ToString()!.Contains(",") ? data.Latitude.ToString()!.Replace(",", ".") : data.Latitude.ToString();
            if (data.Longitude != null)
                ViewBag.Long = data.Longitude.ToString()!.Contains(",") ? data.Longitude.ToString()!.Replace(",", ".") : data.Longitude.ToString();
            ViewBag.listCompany = await BindCompanies();

            return View(GlobalConst.Form, data);
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public override async Task<IActionResult> Remove(Guid Id)
    {
        var data = await _archiveUnitService.GetById(Id);
        if (data != null)
        {
            if (data.Latitude != null)
                ViewBag.Lat = data.Latitude.ToString()!.Contains(",") ? data.Latitude.ToString()!.Replace(",", ".") : data.Latitude.ToString();
            if (data.Longitude != null)
                ViewBag.Long = data.Longitude.ToString()!.Contains(",") ? data.Longitude.ToString()!.Replace(",", ".") : data.Longitude.ToString();
            ViewBag.listCompany = await BindCompanies();

            return View(GlobalConst.Form, data);
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public override async Task<IActionResult> Detail(Guid Id)
    {
        var data = await _archiveUnitService.GetById(Id);
        if (data != null)
        {
            if (data.Latitude != null)
                ViewBag.Lat = data.Latitude.ToString()!.Contains(",") ? data.Latitude.ToString()!.Replace(",", ".") : data.Latitude.ToString();
            if (data.Longitude != null)
                ViewBag.Long = data.Longitude.ToString()!.Contains(",") ? data.Longitude.ToString()!.Replace(",", ".") : data.Longitude.ToString();
            ViewBag.listCompany = await BindCompanies();

            return View(GlobalConst.Form, data);
        }
        else
        {
            return RedirectToIndex();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Save(TrxArchiveUnit model)
    {
        if (model != null)
        {
            var cultureInfo = System.Globalization.CultureInfo.CurrentCulture;
            var Lat = Request.Form["Lat"].ToString();
            var Long = Request.Form["Long"].ToString();
            model.Latitude = cultureInfo.ToString().Contains("ID") ? decimal.Parse(Lat.Replace(".", ",")) : decimal.Parse(Lat);
            model.Longitude = cultureInfo.ToString().Contains("ID") ? decimal.Parse(Long.Replace(".", ",")) : decimal.Parse(Long);
            if (model.ArchiveUnitId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _archiveUnitService.Update(model);
            }

            else
            {
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                await _archiveUnitService.Insert(model);
            }
        }
        return RedirectToIndex();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Delete(TrxArchiveUnit model)
    {
        if (model != null && model.ArchiveUnitId != Guid.Empty)
        {
            await _archiveUnitService.Delete(model);
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
                    List<TrxArchiveUnit> trxArchiveUnits = new();
                    TrxArchiveUnit trxArchiveUnit;

                    bool valid = true;
                    int errorCount = 0;

                    result.Columns.Add("Keterangan");

                    var companies = await _companyService.GetAll();
                    var archiveUnit = await _archiveUnitService.GetAll();

                    foreach (DataRow row in result.Rows)
                    {
                        string error = string.Empty;

                        if (companies.Where(x => x.CompanyCode == row[1].ToString()).Count() == 0)
                        {
                            valid = false;
                            error = "_Kode Perusahaan tidak ditemukan";
                        }
                        
                        if (archiveUnit.Where(x => x.ArchiveUnitCode == row[2].ToString()).Count() > 0)
                        {
                            valid = false;
                            error = "_Kode Lokasi Simpan sudah ada, silahkan gunakan kode yang lain";
                        }
                        else
                        {
                            if (trxArchiveUnits.Where(x => x.ArchiveUnitCode == row[2].ToString()).Count() > 0)
                            {
                                valid = false;
                                error = "_Kode Lokasi Simpan sudah ada, silahkan gunakan kode yang lain";
                            }
                        }
                        if (!decimal.TryParse(row[7].ToString(), out decimal Lat))
                        {
                            valid = false;
                            error = "_Latitude Harus Desimal";
                        }
                        if (!decimal.TryParse(row[8].ToString(), out decimal Long))
                        {
                            valid = false;
                            error = "_Longitude Harus Desimal";
                        }

                        if (valid)
                        {
                            trxArchiveUnit = new();
                            trxArchiveUnit.ArchiveUnitId = Guid.NewGuid();
                            trxArchiveUnit.CompanyId = companies.Where(x => x.CompanyCode == row[1].ToString()).FirstOrDefault()!.CompanyId;
                            trxArchiveUnit.ArchiveUnitCode = row[2].ToString();
                            trxArchiveUnit.ArchiveUnitName = row[3].ToString();
                            trxArchiveUnit.ArchiveUnitAddress = row[4].ToString();
                            trxArchiveUnit.ArchiveUnitPhone = row[5].ToString();
                            trxArchiveUnit.ArchiveUnitEmail = row[6].ToString();
                            trxArchiveUnit.Latitude = Lat; 
                            trxArchiveUnit.Longitude = Long;
                            trxArchiveUnit.IsActive = true;
                            trxArchiveUnit.CreatedBy = AppUsers.CurrentUser(User).UserId;
                            trxArchiveUnit.CreatedDate = DateTime.Now;
                            trxArchiveUnits.Add(trxArchiveUnit);
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
                        await _archiveUnitService.InsertBulk(trxArchiveUnits);

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
            string fileName = nameof(TrxArchiveUnit).ToCleanNameOf();
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);

            var archiveUnits = await _archiveUnitService.GetAll();

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet(nameof(TrxArchiveUnit).ToCleanNameOf());

            IRow row = excelSheet.CreateRow(0);

            row.CreateCell(0).SetCellValue(GlobalConst.No);
            row.CreateCell(1).SetCellValue(nameof(MstCompany.CompanyName));
            row.CreateCell(2).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitCode));
            row.CreateCell(3).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitName));
            row.CreateCell(4).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitAddress));
            row.CreateCell(5).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitPhone));
            row.CreateCell(6).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitEmail));
            row.CreateCell(7).SetCellValue(nameof(TrxArchiveUnit.Latitude));
            row.CreateCell(8).SetCellValue(nameof(TrxArchiveUnit.Longitude));

            int no = 1;
            foreach (var item in archiveUnits)
            {
                row = excelSheet.CreateRow(no);
                row.CreateCell(0).SetCellValue(no);
                row.CreateCell(1).SetCellValue(item.Company.CompanyName);
                row.CreateCell(2).SetCellValue(item.ArchiveUnitCode);
                row.CreateCell(3).SetCellValue(item.ArchiveUnitName);
                row.CreateCell(4).SetCellValue(item.ArchiveUnitAddress);
                row.CreateCell(5).SetCellValue(item.ArchiveUnitPhone);
                row.CreateCell(6).SetCellValue(item.ArchiveUnitEmail);
                row.CreateCell(7).SetCellValue(item.Latitude.ToString());
                row.CreateCell(8).SetCellValue(item.Longitude.ToString());

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
            string fileName = $"{GlobalConst.Template}-{nameof(TrxArchiveUnit).ToCleanNameOf()}";
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet(nameof(TrxArchiveUnit).ToCleanNameOf());
            ISheet excelSheetCompanies = workbook.CreateSheet(nameof(MstCompany).ToCleanNameOf());

            IRow row = excelSheet.CreateRow(0);
            IRow rowCompanies = excelSheetCompanies.CreateRow(0);

            //Archive Units
            row.CreateCell(0).SetCellValue(GlobalConst.No);
            row.CreateCell(1).SetCellValue(nameof(MstCompany.CompanyCode));
            row.CreateCell(2).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitCode));
            row.CreateCell(3).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitName));
            row.CreateCell(4).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitAddress));
            row.CreateCell(5).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitPhone));
            row.CreateCell(6).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitEmail));
            row.CreateCell(7).SetCellValue(nameof(TrxArchiveUnit.Latitude));
            row.CreateCell(8).SetCellValue(nameof(TrxArchiveUnit.Longitude));

            //Companies
            rowCompanies.CreateCell(0).SetCellValue(GlobalConst.No);
            rowCompanies.CreateCell(1).SetCellValue(nameof(MstCompany.CompanyCode));
            rowCompanies.CreateCell(2).SetCellValue(nameof(MstCompany.CompanyName));
            rowCompanies.CreateCell(3).SetCellValue(nameof(MstCompany.Address));
            rowCompanies.CreateCell(4).SetCellValue(nameof(MstCompany.Telepone));
            rowCompanies.CreateCell(5).SetCellValue(nameof(MstCompany.Email));

            var dataCompanies = await _companyService.GetAll();

            int no = 1;
            foreach (var item in dataCompanies)
            {
                rowCompanies = excelSheetCompanies.CreateRow(no);

                rowCompanies.CreateCell(0).SetCellValue(no);
                rowCompanies.CreateCell(1).SetCellValue(item.CompanyCode);
                rowCompanies.CreateCell(2).SetCellValue(item.CompanyName);
                rowCompanies.CreateCell(3).SetCellValue(item.Address);
                rowCompanies.CreateCell(4).SetCellValue(item.Telepone);
                rowCompanies.CreateCell(5).SetCellValue(item.Email);
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
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.ArchiveUnit, new { Area = GlobalConst.MasterData });
    #endregion
}
