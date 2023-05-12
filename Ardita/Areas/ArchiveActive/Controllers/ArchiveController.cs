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

namespace Ardita.Areas.ArchiveActive.Controllers;

[CustomAuthorize]
[Area(Const.ArchiveActive)]
public class ArchiveController : BaseController<TrxArchive>
{
    #region CTR
    public ArchiveController(
        IArchiveService archiveService, 
        IGmdService gmdService,
        IClassificationSubSubjectService classificationSubSubjectService,
        ISecurityClassificationService securityClassificationService,
        IArchiveCreatorService archiveCreatorService)
    {
        _archiveService = archiveService;
        _gmdService = gmdService;
        _classificationSubSubjectService = classificationSubSubjectService;
        _securityClassificationService = securityClassificationService;
        _archiveCreatorService = archiveCreatorService;
    }
    #endregion

    #region MAIN ACTION
    public override async Task<ActionResult> Index() => await base.Index();
    public override async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            var result = await _archiveService.GetList(model);

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

        return View(Const.Form, new TrxArchive());
    }
    public override async Task<IActionResult> Update(Guid Id)
    {
        var data = await _archiveService.GetById(Id);
        if (data is not null)
        {
            await BindAllDropdown();

            return View(Const.Form, data);
        }
        else
        {
            return RedirectToIndex();
        }
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Save(TrxArchive model)
    {
        if (model is not null)
        {
            var files = Request.Form[Const.Files];

            if (model.ArchiveId != Guid.Empty)
            {
                string[] filesDeleted = Request.Form[Const.IdFileDeletedArray].ToArray();

                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _archiveService.Update(model, files, filesDeleted);
            }
            else
            {
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                await _archiveService.Insert(model, files);
            }
        }
        return RedirectToIndex();
    }
    public override async Task<IActionResult> Remove(Guid Id)
    {
        var data = await _archiveService.GetById(Id);
        if (data is not null)
        {
            await BindAllDropdown();

            return View(Const.Form, data);
        }
        else
        {
            return RedirectToIndex();
        }
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Delete(TrxArchive model)
    {
        if (model != null && model.ArchiveId != Guid.Empty)
        {
            await _archiveService.Delete(model);
        }
        return RedirectToIndex();
    }
    public override async Task<IActionResult> Detail(Guid Id)
    {
        var data = await _archiveService.GetById(Id);
        if (data is not null)
        {
            await BindAllDropdown();

            return View(Const.Form, data);
        }
        else
        {
            return RedirectToIndex();
        }
    }
    #endregion

    #region EXPORT/IMPORT
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload()
    {
        try
        {
            IFormFile file = Request.Form.Files[0];

            var result = Extensions.Global.ImportExcel(file, Const.Upload, string.Empty);
            var Gmds = await _gmdService.GetAll();
            var SubSubjecClassifications = await _classificationSubSubjectService.GetAll();
            var SecurityClassifications = await _securityClassificationService.GetAll();
            var Creators = await _archiveCreatorService.GetAll();

            List<TrxArchive> trxArchives = new();
            TrxArchive trxArchive;
            foreach (DataRow row in result.Rows)
            {
                trxArchive = new();
                trxArchive.ArchiveId = Guid.NewGuid();
                trxArchive.GmdId = Gmds.Where(x => x.GmdCode == row[1].ToString()).FirstOrDefault().GmdId;
                trxArchive.SubSubjectClassificationId = SubSubjecClassifications.Where(x => x.SubSubjectClassificationCode == row[2].ToString()).FirstOrDefault().SubSubjectClassificationId;
                trxArchive.SecurityClassificationId = SecurityClassifications.Where(x => x.SecurityClassificationCode == row[3].ToString()).FirstOrDefault().SecurityClassificationId;
                trxArchive.CreatorId = Creators.Where(x => x.CreatorCode == row[4].ToString()).FirstOrDefault().CreatorId;
                trxArchive.TypeSender = row[5].ToString();
                trxArchive.Keyword = row[6].ToString();
                trxArchive.ArchiveCode = row[7].ToString();
                trxArchive.TitleArchive = row[8].ToString();
                trxArchive.TypeArchive = row[9].ToString();
                trxArchive.CreatedDateArchive = Convert.ToDateTime(row[10]);
                trxArchive.ActiveRetention = Convert.ToInt32(row[11]);
                trxArchive.InactiveRetention = Convert.ToInt32(row[12]);
                trxArchive.Volume = Convert.ToInt32(row[13]);
                trxArchive.IsActive = true;
                trxArchive.CreatedBy = AppUsers.CurrentUser(User).UserId;
                trxArchive.CreatedDate = DateTime.Now;
                trxArchive.StatusId = 1;

                trxArchives.Add(trxArchive);
            }
            await _archiveService.InsertBulk(trxArchives);
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
            string fileName = nameof(TrxArchive).ToCleanNameOf();
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);

            var archives = await _archiveService.GetAll();

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet(nameof(TrxArchive).ToCleanNameOf());

            IRow row = excelSheet.CreateRow(0);

            row.CreateCell(0).SetCellValue(Const.No);
            row.CreateCell(1).SetCellValue(nameof(MstGmd.GmdName).ToCleanNameOf());
            row.CreateCell(2).SetCellValue(nameof(TrxSubSubjectClassification.SubSubjectClassificationName).ToCleanNameOf());
            row.CreateCell(3).SetCellValue(nameof(MstSecurityClassification.SecurityClassificationName).ToCleanNameOf());
            row.CreateCell(4).SetCellValue(nameof(MstCreator.CreatorName).ToCleanNameOf());
            row.CreateCell(5).SetCellValue(nameof(TrxArchive.TypeSender).ToCleanNameOf());
            row.CreateCell(6).SetCellValue(nameof(TrxArchive.Keyword).ToCleanNameOf());
            row.CreateCell(7).SetCellValue(nameof(TrxArchive.ArchiveCode).ToCleanNameOf());
            row.CreateCell(8).SetCellValue(nameof(TrxArchive.TitleArchive).ToCleanNameOf());
            row.CreateCell(9).SetCellValue(nameof(TrxArchive.TypeArchive).ToCleanNameOf());
            row.CreateCell(10).SetCellValue(nameof(TrxArchive.CreatedDateArchive).ToCleanNameOf());
            row.CreateCell(11).SetCellValue(nameof(TrxArchive.ActiveRetention).ToCleanNameOf());
            row.CreateCell(12).SetCellValue(nameof(TrxArchive.InactiveRetention).ToCleanNameOf());
            row.CreateCell(13).SetCellValue(nameof(TrxArchive.Volume).ToCleanNameOf());

            int no = 1;
            foreach (var item in archives)
            {
                row = excelSheet.CreateRow(no);
                row.CreateCell(0).SetCellValue(no);
                row.CreateCell(1).SetCellValue(item.Gmd.GmdName);
                row.CreateCell(2).SetCellValue(item.SubSubjectClassification.SubSubjectClassificationName);
                row.CreateCell(3).SetCellValue(item.SecurityClassification.SecurityClassificationName);
                row.CreateCell(4).SetCellValue(item.Creator.CreatorName);
                row.CreateCell(5).SetCellValue(item.TypeSender);
                row.CreateCell(6).SetCellValue(item.Keyword);
                row.CreateCell(7).SetCellValue(item.ArchiveCode);
                row.CreateCell(8).SetCellValue(item.TitleArchive);
                row.CreateCell(9).SetCellValue(item.TypeArchive);
                row.CreateCell(10).SetCellValue(item.CreatedDateArchive.ToString());
                row.CreateCell(11).SetCellValue(item.ActiveRetention);
                row.CreateCell(12).SetCellValue(item.InactiveRetention);
                row.CreateCell(13).SetCellValue(item.Volume);

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
            string fileName = $"{Const.Template}-{nameof(TrxArchive).ToCleanNameOf()}";
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);

            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet(nameof(TrxArchive).ToCleanNameOf());
            ISheet excelSheetGMD = workbook.CreateSheet(nameof(MstGmd).ToCleanNameOf());
            ISheet excelSheetSubSubjectClassification = workbook.CreateSheet(nameof(TrxSubSubjectClassification).ToCleanNameOf());
            ISheet excelSheetSecurityClassification = workbook.CreateSheet(nameof(MstSecurityClassification).ToCleanNameOf());
            ISheet excelSheetCreator = workbook.CreateSheet(nameof(MstCreator).ToCleanNameOf());

            //Inititae Row
            IRow row = excelSheet.CreateRow(0);
            IRow rowGMD = excelSheetGMD.CreateRow(0);
            IRow rowSubSubjectClassification = excelSheetSubSubjectClassification.CreateRow(0);
            IRow rowSecurityClassification = excelSheetSecurityClassification.CreateRow(0);
            IRow rowCreator = excelSheetCreator.CreateRow(0);

            //Initiate Main Column
            row.CreateCell(0).SetCellValue(Const.No);
            row.CreateCell(1).SetCellValue(nameof(MstGmd.GmdCode).ToCleanNameOf());
            row.CreateCell(2).SetCellValue(nameof(TrxSubSubjectClassification.SubSubjectClassificationCode).ToCleanNameOf());
            row.CreateCell(3).SetCellValue(nameof(MstSecurityClassification.SecurityClassificationCode).ToCleanNameOf());
            row.CreateCell(4).SetCellValue(nameof(MstCreator.CreatorCode).ToCleanNameOf());
            row.CreateCell(5).SetCellValue(nameof(TrxArchive.TypeSender).ToCleanNameOf());
            row.CreateCell(6).SetCellValue(nameof(TrxArchive.Keyword).ToCleanNameOf());
            row.CreateCell(7).SetCellValue(nameof(TrxArchive.ArchiveCode).ToCleanNameOf());
            row.CreateCell(8).SetCellValue(nameof(TrxArchive.TitleArchive).ToCleanNameOf());
            row.CreateCell(9).SetCellValue(nameof(TrxArchive.TypeArchive).ToCleanNameOf());
            row.CreateCell(10).SetCellValue(nameof(TrxArchive.CreatedDateArchive).ToCleanNameOf());
            row.CreateCell(11).SetCellValue(nameof(TrxArchive.ActiveRetention).ToCleanNameOf());
            row.CreateCell(12).SetCellValue(nameof(TrxArchive.InactiveRetention).ToCleanNameOf());
            row.CreateCell(13).SetCellValue(nameof(TrxArchive.Volume).ToCleanNameOf());

            //Initiate Sheet GMD 
            rowGMD.CreateCell(0).SetCellValue(Const.No);
            rowGMD.CreateCell(1).SetCellValue(nameof(MstCreator.CreatorCode));
            rowGMD.CreateCell(2).SetCellValue(nameof(MstCreator.CreatorName));

            //Initiate Sub Subject Classification
            rowSubSubjectClassification.CreateCell(0).SetCellValue(Const.No);
            rowSubSubjectClassification.CreateCell(1).SetCellValue(nameof(TrxSubSubjectClassification.SubSubjectClassificationCode));
            rowSubSubjectClassification.CreateCell(2).SetCellValue(nameof(TrxSubSubjectClassification.SubSubjectClassificationName));

            //Initiate Security Classification
            rowSecurityClassification.CreateCell(0).SetCellValue(Const.No);
            rowSecurityClassification.CreateCell(1).SetCellValue(nameof(TrxSubSubjectClassification.SubSubjectClassificationCode));
            rowSecurityClassification.CreateCell(2).SetCellValue(nameof(TrxSubSubjectClassification.SubSubjectClassificationName));

            //Initiate Creator
            rowCreator.CreateCell(0).SetCellValue(Const.No);
            rowCreator.CreateCell(1).SetCellValue(nameof(MstCreator.CreatorCode));
            rowCreator.CreateCell(2).SetCellValue(nameof(MstCreator.CreatorName));


            //Get Data
            var dataGMD = await _gmdService.GetAll();
            var dataSubSubjectClassification = await _classificationSubSubjectService.GetAll();
            var dataSecurityClassification = await _securityClassificationService.GetAll();
            var dataCreator = await _archiveCreatorService.GetAll();

            //Assign GMD 
            int no = 1;
            foreach (var item in dataGMD)
            {
                rowGMD = excelSheetGMD.CreateRow(no);
                rowGMD.CreateCell(0).SetCellValue(no);
                rowGMD.CreateCell(1).SetCellValue(item.GmdCode);
                rowGMD.CreateCell(2).SetCellValue(item.GmdName);
                no += 1;
            }
            //Assign Sub Subject Classification
            no = 1;
            foreach (var item in dataSubSubjectClassification)
            {
                rowSubSubjectClassification = excelSheetSubSubjectClassification.CreateRow(no);
                rowSubSubjectClassification.CreateCell(0).SetCellValue(no);
                rowSubSubjectClassification.CreateCell(1).SetCellValue(item.SubSubjectClassificationCode);
                rowSubSubjectClassification.CreateCell(2).SetCellValue(item.SubSubjectClassificationName);
                no += 1;
            }
            //Assign Security Classification
            no = 1;
            foreach (var item in dataSecurityClassification)
            {
                rowSecurityClassification = excelSheetSecurityClassification.CreateRow(no);
                rowSecurityClassification.CreateCell(0).SetCellValue(no);
                rowSecurityClassification.CreateCell(1).SetCellValue(item.SecurityClassificationCode);
                rowSecurityClassification.CreateCell(2).SetCellValue(item.SecurityClassificationName);
                no += 1;
            }
            //Assign Creator
            no = 1;
            foreach (var item in dataCreator)
            {
                rowCreator = excelSheetCreator.CreateRow(no);
                rowCreator.CreateCell(0).SetCellValue(no);
                rowCreator.CreateCell(1).SetCellValue(item.CreatorCode);
                rowCreator.CreateCell(2).SetCellValue(item.CreatorName);
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
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.Archive, new { Area = Const.ArchiveActive });
    protected async Task BindAllDropdown()
    {
        ViewBag.listGmd = await BindGmds();
        ViewBag.listSubSubjectClasscification = await BindSubSubjectClasscifications();
        ViewBag.listSecurityClassification = await BindSecurityClassifications();
    }
    [HttpGet]
    public IActionResult BindDownload(string path) => File(System.IO.File.OpenRead(path), "application/octet-stream", Path.GetFileName(path));
    #endregion 
}
