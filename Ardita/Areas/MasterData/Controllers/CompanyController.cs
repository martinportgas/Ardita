﻿using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
namespace Ardita.Areas.MasterData.Controllers;

[CustomAuthorize]
[Area(Const.MasterData)]
public class CompanyController : BaseController<MstCompany>
{
    private IWebHostEnvironment _webHostEnvironment;

    public CompanyController(IWebHostEnvironment webHostEnvironment, ICompanyService companyService)
    {
        _webHostEnvironment = webHostEnvironment;
        _companyService = companyService;
    }

    public override async Task<ActionResult> Index() => await base.Index();

    public override async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            var result = await _companyService.GetListCompanies(model);

            return Json(result);

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public override async Task<IActionResult> Add()
    {
        var Company = new MstCompany();
        ViewBag.CurrentAction = Const.Add;

        await Task.Delay(0);

        return View(Const.Form, Company);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Save(MstCompany model)
    {
        if (model != null)
        {

            if (model.CompanyId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _companyService.Update(model);
            }
            else
            {
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                await _companyService.Insert(model);
            }

        }
        return RedirectToIndex();
    }

    public override async Task<IActionResult> Update(Guid Id)
    {
        var listCompany = await _companyService.GetById(Id);

        if (listCompany.Any())
        {
            return View(Const.Form, listCompany.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public override async Task<IActionResult> Detail(Guid Id)
    {
        var listCompany = await _companyService.GetById(Id);

        if (listCompany.Any())
        {
            return View(Const.Form, listCompany.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public override async Task<IActionResult> Remove(Guid Id)
    {
        var listCompany = await _companyService.GetById(Id);

        if (listCompany.Any())
        {
            return View(Const.Form, listCompany.FirstOrDefault());
        }
        else
        {
            return RedirectToIndex();
        }
    }

    public override async Task<IActionResult> Delete(MstCompany model)
    {
        if (model != null)
        {
            model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
            model.UpdatedDate = DateTime.Now;
            await _companyService.Delete(model);

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

            List<MstCompany> companies = new();
            MstCompany company;

            foreach (DataRow row in result.Rows)
            {
                company = new();
                company.CompanyId = Guid.NewGuid();

                company.CompanyCode = row[1].ToString();
                company.CompanyName = row[2].ToString();
                company.Address = row[3].ToString();
                company.Telepone = row[4].ToString();
                company.Email = row[5].ToString();
                company.IsActive = true;
                company.CreatedBy = AppUsers.CurrentUser(User).UserId;
                company.CreatedDate = DateTime.Now;

                companies.Add(company);
            }
            await _companyService.InsertBulk(companies);
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
            string fileName = nameof(MstCompany).ToCleanNameOf();
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);


            var companies = await _companyService.GetAll();

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet(nameof(MstCompany).ToCleanNameOf());

            IRow row = excelSheet.CreateRow(0);

            row.CreateCell(0).SetCellValue(Const.No);
            row.CreateCell(1).SetCellValue(nameof(MstCompany.CompanyCode));
            row.CreateCell(2).SetCellValue(nameof(MstCompany.CompanyName));
            row.CreateCell(3).SetCellValue(nameof(MstCompany.Address));
            row.CreateCell(4).SetCellValue(nameof(MstCompany.Telepone));
            row.CreateCell(5).SetCellValue(nameof(MstCompany.Email));

            int no = 1;
            foreach (var item in companies)
            {
                row = excelSheet.CreateRow(no);
                row.CreateCell(0).SetCellValue(no);
                row.CreateCell(1).SetCellValue(item.CompanyCode);
                row.CreateCell(2).SetCellValue(item.CompanyName);
                row.CreateCell(3).SetCellValue(item.Address);
                row.CreateCell(4).SetCellValue(item.Telepone);
                row.CreateCell(5).SetCellValue(item.Email);

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
            string fileName = $"{Const.Template}-{nameof(MstCompany).ToCleanNameOf()}";
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet(nameof(MstCompany).ToCleanNameOf());

            IRow row = excelSheet.CreateRow(0);

            row.CreateCell(0).SetCellValue(Const.No);
            row.CreateCell(1).SetCellValue(nameof(MstCompany.CompanyCode));
            row.CreateCell(2).SetCellValue(nameof(MstCompany.CompanyName));
            row.CreateCell(3).SetCellValue(nameof(MstCompany.Address));
            row.CreateCell(4).SetCellValue(nameof(MstCompany.Telepone));
            row.CreateCell(5).SetCellValue(nameof(MstCompany.Email));

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
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.Company, new { Area = Const.MasterData });

}
