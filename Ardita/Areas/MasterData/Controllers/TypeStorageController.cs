using Ardita.Controllers;
using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
    public async Task<IActionResult> UploadForm()
    {
        await Task.Delay(0);
        ViewBag.errorCount = TempData["errorCount"] == null ? -1 : TempData["errorCount"];
        return View();
    }
    public async Task<IActionResult> DownloadTemplate()
    {
        try
        {
            var dataRef = await _archiveUnitService.GetAll();

            List<DataTable> listData = new List<DataTable>() {
                new DataTable(),
                dataRef.Select(x => new {
                    x.ArchiveUnitId,
                    x.ArchiveUnitCode,
                    x.ArchiveUnitName,
                    x.Company.CompanyName
                }).ToList().ToDataTable()
            };

            string templateName = nameof(TrxTypeStorage).ToCleanNameOf();
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
        catch (Exception)
        {
            throw new Exception();
        }
    }
    public async Task<IActionResult> Export()
    {
        try
        {
            string templateName = nameof(TrxTypeStorage).ToCleanNameOf();
            string fileName = nameof(TrxTypeStorage).ToCleanNameOf();
            fileName = fileName.ToFileNameDateTimeStringNow(fileName);

            var rows = await _typeStorageService.GetAll();

            List<DataTable> listData = new List<DataTable>() {
                rows.Select(x => new
                {
                    x.TypeStorageId,
                    x.TypeStorageCode,
                    x.TypeStorageName,
                    x.ArchiveUnit.ArchiveUnitName,
                    x.ArchiveUnit.Company.CompanyName
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
                    var archiveUnits = await _archiveUnitService.GetAll();
                    var typeStorages = await _typeStorageService.GetAll();

                    List<TrxTypeStorage> models = new();
                    TrxTypeStorage model;

                    bool valid = true;
                    int errorCount = 0;

                    result.Columns.Add("Keterangan");
                    foreach (DataRow row in result.Rows)
                    {
                        string error = string.Empty;
                        if (string.IsNullOrEmpty(row[1].ToString()))
                        {
                            valid = false;
                            error = "_Kode Media Penyimpanan harus diisi";
                        }
                        if (string.IsNullOrEmpty(row[2].ToString()))
                        {
                            valid = false;
                            error = "_Nama Media Penyimpanan harus diisi";
                        }
                        if (typeStorages.Where(x => x.TypeStorageCode == row[1].ToString()).Count() > 0)
                        {
                            valid = false;
                            error = "_Kode Media Penyimpanan sudah ada";
                        }
                        else
                        {
                            if (models.Where(x => x.TypeStorageCode == row[1].ToString()).Count() > 0)
                            {
                                valid = false;
                                error = "_Kode Media Penyimpanan sudah ada";
                            }
                        }
                        if (archiveUnits.Where(x => x.ArchiveUnitCode == row[3].ToString()).Count() == 0)
                        {
                            valid = false;
                            error = "_Kode Lokasi Simpan tidak ditemukan";
                        }


                        if (valid)
                        {
                            model = new();
                            model.TypeStorageId = Guid.NewGuid();
                            model.TypeStorageCode = row[1].ToString();
                            model.TypeStorageName = row[2].ToString();
                            model.ArchiveUnitId = archiveUnits.Where(x => x.ArchiveUnitCode.Contains(row[3].ToString())).FirstOrDefault().ArchiveUnitId;
                            model.Volume = 0;
                            model.IsActive = true;
                            model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                            model.CreatedDate = DateTime.Now;


                            models.Add(model);

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
                        await _typeStorageService.InsertBulk(models);
                }
                else
                {
                    TempData["errorCount"] = 100000001;
                    return RedirectToAction(GlobalConst.UploadForm);
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
