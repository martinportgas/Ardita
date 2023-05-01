using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
namespace Ardita.Areas.MasterData.Controllers;

[CustomAuthorize]
[Area(Const.MasterData)]
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

        return View(Const.Form, new TrxArchiveUnit());
    }

    public override async Task<IActionResult> Update(Guid Id)
    {
        var data = await _archiveUnitService.GetById(Id);
        if (data != null)
        {
            ViewBag.listCompany = await BindCompanies();

            return View(Const.Form, data);
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
            ViewBag.listCompany = await BindCompanies();

            return View(Const.Form, data);
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
            ViewBag.listCompany = await BindCompanies();

            return View(Const.Form, data);
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload()
    {
        try
        {
            IFormFile file = Request.Form.Files[0];

            var result = Extensions.Global.ImportExcel(file, Const.Upload, string.Empty);
            var companies = await _companyService.GetAll();


            List<TrxArchiveUnit> trxArchiveUnits = new();
            TrxArchiveUnit trxArchiveUnit;

            foreach (DataRow row in result.Rows)
            {
                trxArchiveUnit = new();
                trxArchiveUnit.ArchiveUnitId = Guid.NewGuid();

                trxArchiveUnit.CompanyId = companies.Where(x => x.CompanyCode.Contains(row[1].ToString())).FirstOrDefault().CompanyId;
                trxArchiveUnit.ArchiveUnitCode = row[2].ToString();
                trxArchiveUnit.ArchiveUnitName = row[3].ToString();
                trxArchiveUnit.ArchiveUnitAddress = row[4].ToString();
                trxArchiveUnit.ArchiveUnitPhone = row[5].ToString();
                trxArchiveUnit.ArchiveUnitEmail = row[6].ToString();


                trxArchiveUnit.IsActive = true;
                trxArchiveUnit.CreatedBy = AppUsers.CurrentUser(User).UserId;
                trxArchiveUnit.CreatedDate = DateTime.Now;

                trxArchiveUnits.Add(trxArchiveUnit);
            }
            await _archiveUnitService.InsertBulk(trxArchiveUnits);
            return RedirectToIndex();
        }
        catch (Exception)
        {

            throw new Exception();
        }

    }
    public async Task Export()
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

            row.CreateCell(0).SetCellValue(Const.No);
            row.CreateCell(1).SetCellValue(nameof(MstCompany.CompanyName));
            row.CreateCell(1).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitCode));
            row.CreateCell(2).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitName));
            row.CreateCell(3).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitAddress));
            row.CreateCell(4).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitPhone));
            row.CreateCell(5).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitEmail));

            int no = 1;
            foreach (var item in archiveUnits)
            {
                row = excelSheet.CreateRow(no);
                row.CreateCell(0).SetCellValue(no);
                row.CreateCell(1).SetCellValue(item.Company.CompanyName);
                row.CreateCell(1).SetCellValue(item.ArchiveUnitCode);
                row.CreateCell(2).SetCellValue(item.ArchiveUnitName);
                row.CreateCell(3).SetCellValue(item.ArchiveUnitAddress);
                row.CreateCell(4).SetCellValue(item.ArchiveUnitPhone);
                row.CreateCell(5).SetCellValue(item.ArchiveUnitEmail);

                no += 1;
            }
            workbook.WriteExcelToResponse(HttpContext, fileName);
        }
        catch (Exception ex)
        {
            throw new Exception();
        }
    }
    public async Task DownloadTemplate()
    {
        try
        {
            string fileName = $"{Const.Template}-{nameof(TrxArchiveUnit).ToCleanNameOf()}";
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet(nameof(TrxArchiveUnit).Replace(Const.Trx, string.Empty));
            ISheet excelSheetCompanies = workbook.CreateSheet(nameof(MstCompany).ToCleanNameOf());

            IRow row = excelSheet.CreateRow(0);
            IRow rowCompanies = excelSheetCompanies.CreateRow(0);

            //Archive Units
            row.CreateCell(0).SetCellValue(Const.No);
            row.CreateCell(1).SetCellValue(nameof(MstCompany.CompanyCode));
            row.CreateCell(2).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitCode));
            row.CreateCell(3).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitName));
            row.CreateCell(4).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitAddress));
            row.CreateCell(5).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitPhone));
            row.CreateCell(6).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitEmail));

            //Companies
            rowCompanies.CreateCell(0).SetCellValue(Const.No);
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
            workbook.WriteExcelToResponse(HttpContext, fileName);
        }
        catch (Exception)
        {
            throw new Exception();
        }
    }
    #endregion

    #region HELPER
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.ArchiveUnit, new { Area = Const.MasterData });
    #endregion
}
