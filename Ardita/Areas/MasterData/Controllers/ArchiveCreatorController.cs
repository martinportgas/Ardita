using Ardita.Controllers;
using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload()
    {
        try
        {
            IFormFile file = Request.Form.Files[0];

            var result = Extensions.Global.ImportExcel(file, GlobalConst.Upload, string.Empty);
            var archiveUnits = await _archiveUnitService.GetAll();


            List<MstCreator> mstCreators = new();
            MstCreator mstCreator;

            foreach (DataRow row in result.Rows)
            {
                mstCreator = new();
                mstCreator.CreatorId = Guid.NewGuid();

                mstCreator.ArchiveUnitId = archiveUnits.Where(x => x.ArchiveUnitCode.Contains(row[1].ToString())).FirstOrDefault().ArchiveUnitId;
                mstCreator.CreatorCode = row[2].ToString();
                mstCreator.CreatorName = row[3].ToString();


                mstCreator.IsActive = true;
                mstCreator.CreatedBy = AppUsers.CurrentUser(User).UserId;
                mstCreator.CreatedDate = DateTime.Now;

                mstCreators.Add(mstCreator);
            }
            await _archiveCreatorService.InsertBulk(mstCreators);
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
            string fileName = nameof(MstCreator).ToCleanNameOf();
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);

            var archiveCreators = await _archiveCreatorService.GetAll();

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet(nameof(MstCreator).ToCleanNameOf());

            IRow row = excelSheet.CreateRow(0);

            row.CreateCell(0).SetCellValue(GlobalConst.No);
            row.CreateCell(1).SetCellValue(nameof(MstCompany.CompanyName));
            row.CreateCell(2).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitName));
            row.CreateCell(3).SetCellValue(nameof(MstCreator.CreatorCode));
            row.CreateCell(4).SetCellValue(nameof(MstCreator.CreatorName));

            int no = 1;
            foreach (var item in archiveCreators)
            {
                row = excelSheet.CreateRow(no);
                row.CreateCell(0).SetCellValue(no);
                row.CreateCell(1).SetCellValue(item.ArchiveUnit.Company.CompanyName);
                row.CreateCell(2).SetCellValue(item.ArchiveUnit.ArchiveUnitName);
                row.CreateCell(3).SetCellValue(item.CreatorCode);
                row.CreateCell(4).SetCellValue(item.CreatorName);
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
            row.CreateCell(1).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitCode));
            row.CreateCell(2).SetCellValue(nameof(MstCreator.CreatorCode));
            row.CreateCell(3).SetCellValue(nameof(MstCreator.CreatorName));

            //Archive Units
            rowArchiveUnits.CreateCell(0).SetCellValue(GlobalConst.No);
            rowArchiveUnits.CreateCell(1).SetCellValue(nameof(MstCompany.CompanyCode));
            rowArchiveUnits.CreateCell(2).SetCellValue(nameof(MstCompany.CompanyName));
            rowArchiveUnits.CreateCell(3).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitCode));
            rowArchiveUnits.CreateCell(4).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitName));
            rowArchiveUnits.CreateCell(5).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitAddress));
            rowArchiveUnits.CreateCell(6).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitPhone));
            rowArchiveUnits.CreateCell(7).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitEmail));

            var dataArchiveUnits = await _archiveUnitService.GetAll();

            int no = 1;
            foreach (var item in dataArchiveUnits)
            {
                rowArchiveUnits = excelSheetArchiveUnits.CreateRow(no);

                rowArchiveUnits.CreateCell(0).SetCellValue(no);
                rowArchiveUnits.CreateCell(1).SetCellValue(item.Company.CompanyCode);
                rowArchiveUnits.CreateCell(2).SetCellValue(item.Company.CompanyName);
                rowArchiveUnits.CreateCell(3).SetCellValue(item.ArchiveUnitCode);
                rowArchiveUnits.CreateCell(4).SetCellValue(item.ArchiveUnitName);
                rowArchiveUnits.CreateCell(5).SetCellValue(item.ArchiveUnitAddress);
                rowArchiveUnits.CreateCell(6).SetCellValue(item.ArchiveUnitPhone);
                rowArchiveUnits.CreateCell(7).SetCellValue(item.ArchiveUnitEmail);
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
