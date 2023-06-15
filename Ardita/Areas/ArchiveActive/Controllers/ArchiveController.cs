using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
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
            model.whereClause = GlobalConst.WhereClauseArchiveRegist;
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
        var model = new TrxArchive
        {
            ArchiveCode = "Auto Generated"
        };

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
    public override async Task<IActionResult> Submit(TrxArchive model)
    {
        var listArchive = Request.Form[GlobalConst.listArchive].ToArray();
        if (listArchive.Length > 0)
        {
            for (int i = 0; i < listArchive.Length; i++)
            {
                Guid archiveId = Guid.Empty;
                Guid.TryParse(listArchive[i], out archiveId);
                var data = await _archiveService.GetById(archiveId);
                if (data != null)
                {
                    data.StatusId = (int)GlobalConst.STATUS.Submit;
                    data.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                    data.UpdatedDate = DateTime.Now;

                    await _archiveService.Submit(data);
                }
            }
            TempData[GlobalConst.Notification] = GlobalConst.Success;
        }
        else
        {
            TempData[GlobalConst.Notification] = GlobalConst.NothingSelected;
        }

        return RedirectToIndex();
    }
    #endregion

    #region EXPORT/IMPORT
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
                var archiveAll = await _archiveService.GetAll();
                var Gmds = await _gmdService.GetAllDetail();
                var SubSubjecClassifications = await _classificationSubSubjectService.GetAll();
                var SecurityClassifications = await _securityClassificationService.GetAll();
                var Creators = await _archiveCreatorService.GetAll();
                var ArchiveTypes = await _archiveTypeService.GetAll();
                var ArchiveOwners = await _archiveOwnerService.GetAll();

                if (result.Rows.Count > 0)
                {
                    List<TrxArchive> trxArchives = new();
                    TrxArchive trxArchive;
                    bool valid = true;
                    int errorCount = 0;
                    result.Columns.Add("Keterangan");
                    foreach (DataRow row in result.Rows)
                    {
                        string error = string.Empty;
                        var GmdDetailData = Gmds.Where(x => x.Name.ToLower() == row[1].ToString().ToLower()).FirstOrDefault();
                        if (GmdDetailData == null)
                        {
                            valid = false;
                            error += "_Bentuk Media Arsip Tidak Valid";
                        }
                        var subSubjectClassificationData = SubSubjecClassifications.Where(x => x.SubSubjectClassificationCode.ToLower() == row[2].ToString().ToLower()).FirstOrDefault();
                        if (subSubjectClassificationData == null)
                        {
                            valid = false;
                            error += "_Subjek Klasifikasi Tidak Valid";
                        }
                        var securityClassificationData = SecurityClassifications.Where(x => x.SecurityClassificationCode.ToLower() == row[3].ToString().ToLower()).FirstOrDefault();
                        if (securityClassificationData == null)
                        {
                            valid = false;
                            error += "_Klasifikasi Keamanan Tidak Valid";
                        }
                        if (row[4].ToString()!.ToLower() != "internal" && row[4].ToString()!.ToLower() != "eksternal")
                        {
                            valid = false;
                            error += "_Tipe Pengirim Tidak Valid";
                        }
                        var archiveOwnerData = ArchiveOwners.Where(x => x.ArchiveOwnerCode.ToLower() == row[5].ToString().ToLower()).FirstOrDefault();
                        if (archiveOwnerData == null)
                        {
                            valid = false;
                            error += "_Asal Arsip Tidak Valid";
                        }
                        var dataDocNo = archiveAll.Where(x => x.DocumentNo.ToLower() == row[7].ToString().ToLower()).FirstOrDefault();
                        if (dataDocNo != null)
                        {
                            valid = false;
                            error += "_Nomor Dokumen Sudah Teregister";
                        }
                        else
                        {
                            dataDocNo = trxArchives.Where(x => x.DocumentNo.ToLower() == row[7].ToString().ToLower()).FirstOrDefault();
                            {
                                valid = false;
                                error += "_Duplikat Nomor Dokumen";
                            }
                        }
                        var archiveTypeData = ArchiveTypes.Where(x => x.ArchiveTypeCode.ToLower() == row[9].ToString().ToLower()).FirstOrDefault();
                        if (archiveTypeData == null)
                        {
                            valid = false;
                            error += "_Tipe Arsip Tidak Valid";
                        }
                        DateTime dateArchive = DateTime.Now;
                        if (!DateTime.TryParse(row[10].ToString(), out dateArchive))
                        {
                            valid = false;
                            error += "_Tanggal Arsip Tidak Valid";
                        }
                        int activeRetention = 0;
                        if (!int.TryParse(row[11].ToString(), out activeRetention))
                        {
                            valid = false;
                            error += "_Retensi Aktif Tidak Valid";
                        }
                        int inActiveRetention = 0;
                        if (!int.TryParse(row[12].ToString(), out inActiveRetention))
                        {
                            valid = false;
                            error += "_Retensi In Aktif Tidak Valid";
                        }
                        int total = 0;
                        if (!int.TryParse(row[13].ToString(), out total))
                        {
                            valid = false;
                            error += "_Jumlah Tidak Valid";
                        }
                        if (valid)
                        {
                            trxArchive = new();
                            trxArchive.ArchiveId = Guid.NewGuid();
                            trxArchive.GmdId = GmdDetailData.GmdId;
                            trxArchive.GmdDetailId = GmdDetailData.GmdDetailId;
                            trxArchive.SubSubjectClassificationId = subSubjectClassificationData!.SubSubjectClassificationId;
                            trxArchive.SecurityClassificationId = securityClassificationData!.SecurityClassificationId;
                            trxArchive.CreatorId = (Guid)subSubjectClassificationData.CreatorId!;
                            trxArchive.TypeSender = row[4].ToString()!;
                            trxArchive.ArchiveOwnerId = archiveOwnerData!.ArchiveOwnerId;
                            trxArchive.Keyword = row[6].ToString()!;
                            trxArchive.ArchiveCode = string.Empty;
                            trxArchive.DocumentNo = row[7].ToString()!;
                            trxArchive.TitleArchive = row[8].ToString()!;
                            trxArchive.ArchiveTypeId = archiveTypeData!.ArchiveTypeId;
                            trxArchive.CreatedDateArchive = dateArchive;
                            trxArchive.ActiveRetention = activeRetention;
                            trxArchive.InactiveRetention = inActiveRetention;
                            trxArchive.Volume = total;
                            trxArchive.IsActive = true;
                            trxArchive.CreatedBy = AppUsers.CurrentUser(User).UserId;
                            trxArchive.CreatedDate = DateTime.Now;
                            trxArchive.StatusId = (int)GlobalConst.STATUS.Draft;

                            trxArchives.Add(trxArchive);
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
                        await _archiveService.InsertBulk(trxArchives);
                }
                return View(GlobalConst.UploadForm);
            }
            else
            {
                TempData["errorCount"] = 100000001;
                return RedirectToAction(GlobalConst.UploadForm);
            }
        }
        catch (Exception ex)
        {
            TempData["errorCount"] = 100000001;
            return RedirectToAction(GlobalConst.UploadForm);
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
            var dataGMDDetail = await _gmdService.GetAllDetail();
            var dataSubSubjectClassification = await _classificationSubSubjectService.GetAll();
            var dataSecurityClassification = await _securityClassificationService.GetAll();
            var dataArchiveOwner = await _archiveOwnerService.GetAll();
            var dataArchiveType = await _archiveTypeService.GetAll();

            List<DataTable> listData = new List<DataTable>() {
                new DataTable(),
                dataGMDDetail.Select(x => new { x.GmdDetailId, x.Name, x.Unit }).ToList().ToDataTable(),
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
        ViewBag.listGmdDetail = await BindGmdDetail();
        ViewBag.listSubSubjectClasscification = await BindSubSubjectClasscifications();
        ViewBag.listSecurityClassification = await BindSecurityClassifications();
        ViewBag.listArchiveOwner = await BindArchiveOwners();
        ViewBag.listArchiveType = await BindArchiveTypes();
    }
    [HttpGet]
    public async Task<IActionResult> BindDownload(Guid Id)
    {
        var data = await _fileArchiveDetailService.GetById(Id);
        if (data != null)
        {
            string path = string.Concat(data.FilePath, data.FileNameEncrypt);
            if (System.IO.File.Exists(path))
            {
                return File(System.IO.File.OpenRead(path), "application/octet-stream", data.FileName);
            }
        }
        return File(new byte[] { }, "application/octet-stream", "NotFound.txt");
    }
    #endregion
}
