using Ardita.Controllers;
using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NPOI.POIFS.Properties;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace Ardita.Areas.MasterData.Controllers;

[CustomAuthorize]
[Area(GlobalConst.MasterData)]
public class TypeStorageController : BaseController<TrxTypeStorage>
{
    public TypeStorageController(IArchiveUnitService archiveUnitService, IRoomService roomService, ITypeStorageService typeStorage, IGmdService gmdService)
    {
        _archiveUnitService = archiveUnitService;
        _roomService = roomService;
        _typeStorageService = typeStorage;
        _gmdService = gmdService;
    }

    #region MAIN ACTION
    public override async Task<ActionResult> Index() => await base.Index();

    public override async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            var result = await _typeStorageService.GetList(model);
            return Json(result);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public override async Task<IActionResult> Add()
    {
        await BindAllDropdown();

        return View(GlobalConst.Form, new TrxTypeStorage());
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
    public override async Task<IActionResult> Save(TrxTypeStorage model)
    {
        if (model != null)
        {
            var listDetail = Request.Form[GlobalConst.DetailArray].ToArray();


            if (model.TypeStorageId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _typeStorageService.Update(model, listDetail!);
            }

            else
            {
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                await _typeStorageService.Insert(model, listDetail!);
            }
        }
        return RedirectToIndex();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Delete(TrxTypeStorage model)
    {
        if (model != null && model.TypeStorageId != Guid.Empty)
        {
            await _typeStorageService.Delete(model);
        }
        return RedirectToIndex();
    }
    public async Task<IActionResult> DownloadTemplate()
    {
        try
        {
            string fileName = $"{GlobalConst.Template}-{nameof(TrxTypeStorage).ToCleanNameOf()}";
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet(nameof(TrxTypeStorage).ToCleanNameOf());
            ISheet excelSheetParent = workbook.CreateSheet("Parent " + nameof(TrxTypeStorage).ToCleanNameOf());
            ISheet excelSheetArchiveUnit = workbook.CreateSheet(nameof(TrxArchiveUnit).ToCleanNameOf());

            IRow row = excelSheet.CreateRow(0);
            IRow rowParent = excelSheetParent.CreateRow(0);
            IRow rowArchiveUnit = excelSheetArchiveUnit.CreateRow(0);

            row.CreateCell(0).SetCellValue(nameof(TrxTypeStorage.TypeStorageCode));
            row.CreateCell(1).SetCellValue(nameof(TrxTypeStorage.TypeStorageName));
            row.CreateCell(2).SetCellValue("Parent " + nameof(TrxTypeStorage.TypeStorageCode));
            row.CreateCell(3).SetCellValue(nameof(TrxTypeStorage.ArchiveUnit.ArchiveUnitCode));
            row.CreateCell(4).SetCellValue(nameof(TrxTypeStorage.Volume));

            rowParent.CreateCell(0).SetCellValue("No");
            rowParent.CreateCell(1).SetCellValue(nameof(TrxTypeStorage.TypeStorageCode));
            rowParent.CreateCell(2).SetCellValue(nameof(TrxTypeStorage.TypeStorageName));
            var parent = await _typeStorageService.GetAll();
            int no = 1;
            if (parent.Any())
            {
                foreach(var item in parent)
                {
                    rowParent = excelSheetParent.CreateRow(no);

                    rowParent.CreateCell(0).SetCellValue(no);
                    rowParent.CreateCell(1).SetCellValue(item.TypeStorageCode);
                    rowParent.CreateCell(2).SetCellValue(item.TypeStorageName);
                    no++;
                }
            }

            rowArchiveUnit.CreateCell(0).SetCellValue("No");
            rowArchiveUnit.CreateCell(1).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitCode));
            rowArchiveUnit.CreateCell(2).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitName));
            var archiveUnit = await _archiveUnitService.GetAll();
            if (archiveUnit.Any())
            {
                no = 1;
                foreach (var item in archiveUnit)
                {
                    rowArchiveUnit = excelSheetArchiveUnit.CreateRow(no);

                    rowArchiveUnit.CreateCell(0).SetCellValue(no);
                    rowArchiveUnit.CreateCell(1).SetCellValue(item.ArchiveUnitCode);
                    rowArchiveUnit.CreateCell(2).SetCellValue(item.ArchiveUnitName);
                    no++;
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
    public async Task<IActionResult> Export()
    {
        try
        {
            string fileName = nameof(TrxTypeStorage).ToCleanNameOf();
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);

            var data = await _typeStorageService.GetAll();

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet(nameof(TrxTypeStorage).ToCleanNameOf());

            IRow row = excelSheet.CreateRow(0);

            row.CreateCell(0).SetCellValue(nameof(TrxTypeStorage.TypeStorageCode));
            row.CreateCell(1).SetCellValue(nameof(TrxTypeStorage.TypeStorageName));
            row.CreateCell(2).SetCellValue("Parent " + nameof(TrxTypeStorage.TypeStorageCode));
            row.CreateCell(3).SetCellValue("Parent " + nameof(TrxTypeStorage.TypeStorageName));
            row.CreateCell(4).SetCellValue(nameof(TrxTypeStorage.ArchiveUnit.ArchiveUnitCode));
            row.CreateCell(5).SetCellValue(nameof(TrxTypeStorage.ArchiveUnit.ArchiveUnitName));
            row.CreateCell(6).SetCellValue(nameof(TrxTypeStorage.Volume));

            int no = 1;
            foreach (var item in data)
            {
                row = excelSheet.CreateRow(no);
                row.CreateCell(0).SetCellValue(item.TypeStorageCode);
                row.CreateCell(1).SetCellValue(item.TypeStorageName);
                row.CreateCell(2).SetCellValue(string.Empty);
                row.CreateCell(3).SetCellValue(string.Empty);
                

                var archiveUnit = await _archiveUnitService.GetById(item.ArchiveUnitId);
                if (archiveUnit != null)
                {
                    row.CreateCell(4).SetCellValue(archiveUnit.ArchiveUnitCode);
                    row.CreateCell(5).SetCellValue(archiveUnit.ArchiveUnitName);
                }
                row.CreateCell(6).SetCellValue(item.Volume);

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
    public async Task<IActionResult> Upload()
    {
        IFormFile file = Request.Form.Files[0];
        var result = Extensions.Global.ImportExcel(file, GlobalConst.Upload, string.Empty);

        var type = await _typeStorageService.GetAll();
        var archiveUnit = await _archiveUnitService.GetAll();

        List<TrxTypeStorage> models = new();
        TrxTypeStorage model;

        foreach (DataRow row in result.Rows)
        {
            if (!string.IsNullOrEmpty(row[3].ToString()))
            {
                model = new();
                model.TypeStorageId = Guid.NewGuid();
                model.TypeStorageCode = row[0].ToString();
                model.TypeStorageName = row[1].ToString();
                model.ArchiveUnitId = archiveUnit.Where(x => x.ArchiveUnitCode.Contains(row[3].ToString())).FirstOrDefault().ArchiveUnitId;
                int volume = 0;
                int.TryParse(row[4].ToString(), out volume);
                model.Volume = volume;
                model.IsActive = true;
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;

                models.Add(model);
            } 
        }
        await _typeStorageService.InsertBulk(models);

        return RedirectToIndex();
    }
    #endregion

    #region HELPER
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.TypeStorage, new { Area = GlobalConst.MasterData });

    protected async Task BindAllDropdown()
    {
        ViewBag.listArchiveUnit = await BindArchiveUnits();
        ViewBag.listTypeStorage = await BindTypeStorage();
        ViewBag.listGmd = await BindGmds();
    }

    private async Task<IActionResult> InitFormView(Guid Id)
    {
        var data = await _typeStorageService.GetById(Id);
        if (data is not null)
        {
            await BindAllDropdown();

            return View(GlobalConst.Form, data);
        }
        else
        {
            return RedirectToIndex();
        }
    }
    #endregion
}
