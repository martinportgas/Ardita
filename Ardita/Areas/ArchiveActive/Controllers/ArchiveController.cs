using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace Ardita.Areas.ArchiveActive.Controllers;

[CustomAuthorize]
[Area(GlobalConst.ArchiveActive)]
public class ArchiveController : BaseController<TrxArchive>
{
    #region CTR
    public ArchiveController(
        IArchiveService archiveService, 
        IGmdService gmdService,
        IClassificationSubSubjectService classificationSubSubjectService,
        ISecurityClassificationService securityClassificationService,
        IArchiveCreatorService archiveCreatorService,
        IFileArchiveDetailService fileArchiveDetailService,
        IArchiveOwnerService archiveOwnerService,
        IArchiveTypeService archiveTypeService)
    {
        _archiveService = archiveService;
        _gmdService = gmdService;
        _classificationSubSubjectService = classificationSubSubjectService;
        _securityClassificationService = securityClassificationService;
        _archiveCreatorService = archiveCreatorService;
        _fileArchiveDetailService = fileArchiveDetailService;
        _archiveOwnerService = archiveOwnerService;
        _archiveTypeService = archiveTypeService;
    }
    #endregion

    #region MAIN ACTION
    public override async Task<ActionResult> Index() => await base.Index();
    public override async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            model.SessionUser = User;
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
        var model = new TrxArchive();
        model.ArchiveCode = "Auto Generated";

        return View(GlobalConst.Form, model);
    }
    public override async Task<IActionResult> Update(Guid Id)
    {
        var data = await _archiveService.GetById(Id);
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
    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Save(TrxArchive model)
    {
        if (model is not null)
        {
            var files = Request.Form[GlobalConst.Files];

            if (model.ArchiveId != Guid.Empty)
            {
                string[] filesDeleted = Request.Form[GlobalConst.IdFileDeletedArray].ToArray();

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

            return View(GlobalConst.Form, data);
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

            return View(GlobalConst.Form, data);
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

            var result = Extensions.Global.ImportExcel(file, GlobalConst.Upload, string.Empty);
            var Gmds = await _gmdService.GetAll();
            var SubSubjecClassifications = await _classificationSubSubjectService.GetAll();
            var SecurityClassifications = await _securityClassificationService.GetAll();
            var Creators = await _archiveCreatorService.GetAll();
            var ArchiveTypes = await _archiveTypeService.GetAll();
            var ArchiveOwners = await _archiveOwnerService.GetAll();

            List<TrxArchive> trxArchives = new();
            TrxArchive trxArchive;
            foreach (DataRow row in result.Rows)
            {
                var subSubjectClassificationData = SubSubjecClassifications.Where(x => x.SubSubjectClassificationCode == row[2].ToString()).FirstOrDefault();
                var securityClassificationData = SecurityClassifications.Where(x => x.SecurityClassificationCode == row[3].ToString()).FirstOrDefault();
                var archiveOwnerData = ArchiveOwners.Where(x => x.ArchiveOwnerCode == row[5].ToString()).FirstOrDefault();
                var archiveTypeData = ArchiveTypes.Where(x => x.ArchiveTypeCode == row[9].ToString()).FirstOrDefault();
                if (securityClassificationData != null && subSubjectClassificationData != null && archiveOwnerData != null && archiveTypeData != null)
                {
                    trxArchive = new();
                    trxArchive.ArchiveId = Guid.NewGuid();
                    trxArchive.GmdId = Gmds.Where(x => x.GmdCode == row[1].ToString()).FirstOrDefault()!.GmdId;
                    trxArchive.SubSubjectClassificationId = subSubjectClassificationData.SubSubjectClassificationId;
                    trxArchive.SecurityClassificationId = securityClassificationData.SecurityClassificationId;
                    trxArchive.CreatorId = (Guid)subSubjectClassificationData.CreatorId!;
                    trxArchive.TypeSender = row[4].ToString()!;
                    trxArchive.ArchiveOwnerId = archiveOwnerData.ArchiveOwnerId;
                    trxArchive.Keyword = row[6].ToString()!;
                    trxArchive.ArchiveCode = string.Empty;
                    trxArchive.DocumentNo = row[7].ToString()!;
                    trxArchive.TitleArchive = row[8].ToString()!;
                    trxArchive.ArchiveTypeId = archiveTypeData.ArchiveTypeId;
                    trxArchive.CreatedDateArchive = Convert.ToDateTime(row[10]);
                    trxArchive.ActiveRetention = Convert.ToInt32(row[11]);
                    trxArchive.InactiveRetention = Convert.ToInt32(row[12]);
                    trxArchive.Volume = Convert.ToInt32(row[13]);
                    trxArchive.IsActive = true;
                    trxArchive.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    trxArchive.CreatedDate = DateTime.Now;
                    trxArchive.StatusId = (int)GlobalConst.STATUS.Draft;

                    trxArchives.Add(trxArchive);
                }
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
            string templateName = nameof(TrxArchive).ToCleanNameOf();
            string fileName = nameof(TrxArchive).ToCleanNameOf();
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);

            var archives = await _archiveService.GetAll(AppUsers.CurrentUser(User).ListArchiveUnitCode);

            List<DataTable> listData = new List<DataTable>() {
                archives.Select(x => new
                {
                    x.ArchiveId,
                    x.ArchiveCode,
                    x.Gmd.GmdName,
                    x.SubSubjectClassification.SubSubjectClassificationName,
                    x.SecurityClassification.SecurityClassificationName,
                    x.TypeSender,
                    x.ArchiveOwner.ArchiveOwnerName,
                    x.Keyword,
                    x.DocumentNo,
                    x.TitleArchive,
                    x.ArchiveType.ArchiveTypeName,
                    x.CreatedDateArchive,
                    x.ActiveRetention,
                    x.InactiveRetention,
                    x.Volume
                }
                ).ToList().ToDataTable()
            };

            IWorkbook workbook = Global.GetExcelTemplate(templateName, listData, GlobalConst.Export.ToLower());

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
            var dataGMD = await _gmdService.GetAll();
            var dataSubSubjectClassification = await _classificationSubSubjectService.GetAll();
            var dataSecurityClassification = await _securityClassificationService.GetAll();
            var dataArchiveOwner = await _archiveOwnerService.GetAll();
            var dataArchiveType = await _archiveTypeService.GetAll();

            List<DataTable> listData = new List<DataTable>() {
                new DataTable(),
                dataGMD.Select(x => new { x.GmdId, x.GmdCode, x.GmdName }).ToList().ToDataTable(),
                dataSubSubjectClassification.Select(x => new { x.SubSubjectClassificationId, x.SubSubjectClassificationCode, x.SubSubjectClassificationName }).ToList().ToDataTable(),
                dataSecurityClassification.Select(x => new { x.SecurityClassificationId, x.SecurityClassificationCode, x.SecurityClassificationName }).ToList().ToDataTable(),
                dataArchiveOwner.Select(x => new { x.ArchiveOwnerId, x.ArchiveOwnerCode, x.ArchiveOwnerName }).ToList().ToDataTable(),
                dataArchiveType.Select(x => new { x.ArchiveTypeId, x.ArchiveTypeCode, x.ArchiveTypeName }).ToList().ToDataTable(),
                GlobalConst.dataSender(),
            };

            string templateName = nameof(TrxArchive).ToCleanNameOf();
            string fileName = $"{GlobalConst.Template}-{templateName}";
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);

            IWorkbook workbook = Global.GetExcelTemplate(templateName, listData, GlobalConst.Import.ToLower());

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
    #endregion

    #region HELPER
    private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.Archive, new { Area = GlobalConst.ArchiveActive });
    protected async Task BindAllDropdown()
    {
        ViewBag.listGmd = await BindGmds();
        ViewBag.listSubSubjectClasscification = await BindSubSubjectClasscifications();
        ViewBag.listSecurityClassification = await BindSecurityClassifications();
        ViewBag.listArchiveOwner = await BindArchiveOwners();
        ViewBag.listArchiveType = await BindArchiveTypes();
    }
    [HttpGet]
    public async Task<IActionResult> BindDownload(Guid Id)
    {
        var data = await _fileArchiveDetailService.GetById(Id);
        if(data != null)
        {
            string path = string.Concat(data.FilePath, data.FileNameEncrypt);
            if(System.IO.File.Exists(path))
            {
                return File(System.IO.File.OpenRead(path), "application/octet-stream", data.FileName);
            }
        }
        return File(new byte[] { }, "application/octet-stream", "NotFound.txt");
    } 
    #endregion 
}
