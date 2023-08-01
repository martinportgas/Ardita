using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorize]
    [Area(GlobalConst.MasterData)]
    public class SubTypeStorageController : BaseController<IdxSubTypeStorage>
    {
        public SubTypeStorageController(ISubTypeStorageService subTypeStorageService, 
            IArchiveUnitService archiveUnitService,
            ITypeStorageService typeStorageService, 
            IGmdService gmdService
            )
        {
            _subTypeStorageService = subTypeStorageService;
            _archiveUnitService = archiveUnitService;
            _typeStorageService = typeStorageService;
            _gmdService = gmdService;
        }

        #region MAIN ACTION
        public override async Task<ActionResult> Index() => await base.Index();

        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                var result = await _subTypeStorageService.GetList(model);
                return Json(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override async Task<IActionResult> Add()
        {
            await BindAllDropdown(Guid.Empty);
            return View(GlobalConst.Form, new MstSubTypeStorage());
        }

        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _subTypeStorageService.GetById(Id);
            if (data is not null)
            {
                await BindAllDropdown(Id);
                return View(GlobalConst.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }

        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _subTypeStorageService.GetById(Id);
            if (data is not null)
            {
                await BindAllDropdown(Id);
                return View(GlobalConst.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }

        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _subTypeStorageService.GetById(Id);
            if (data is not null)
            {
                await BindAllDropdown(Id);
                return View(GlobalConst.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(MstSubTypeStorage model)
        {
            if (model != null && model.SubTypeStorageId != Guid.Empty)
            {
                await _subTypeStorageService.Delete(model);
            }
            return RedirectToIndex();
        }

        protected async Task BindAllDropdown(Guid Id)
        {
            ViewBag.listArchiveUnit = await BindArchiveUnits();
            ViewBag.listTypeStorage = await BindTypeStorage();
            ViewBag.subDetail = await _subTypeStorageService.GetAllBySubTypeStorageId(Id);
            ViewBag.listGmd = await BindGmds();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Save(IdxSubTypeStorage model)
        {
            if (model != null)
            {
                //Insert Header
                MstSubTypeStorage header = new();
                header.SubTypeStorageId = model.SubTypeStorageId == Guid.Empty ? new Guid() : model.SubTypeStorageId;
                header.SubTypeStorageCode = Request.Form["txtCode"];
                header.SubTypeStorageName = Request.Form["txtName"];
                header.CreatedBy = AppUsers.CurrentUser(User).UserId;
                header.CreatedDate = DateTime.Now;

                if(model.SubTypeStorageId != Guid.Empty)
                    await _subTypeStorageService.Update(header);
                else
                    await _subTypeStorageService.Insert(header);

                //Insert Detail
                IdxSubTypeStorage idxSubTypeStorage;
                var listTypeStorages = Request.Form["typeStorage[]"].ToArray();


                await _subTypeStorageService.DeleteIDXSubTypeStorage(header.SubTypeStorageId);

                for (int i = 0; i < listTypeStorages.Length; i++)
                {
                    var idxSubTypeStorages = listTypeStorages[i];

                    Guid typeStorageId = Guid.Empty;
                    Guid.TryParse(idxSubTypeStorages, out typeStorageId);

                    if (!string.IsNullOrEmpty(idxSubTypeStorages))
                    {
                        idxSubTypeStorage = new();
                        idxSubTypeStorage.TypeStorageId = typeStorageId;
                        idxSubTypeStorage.SubTypeStorageId = header.SubTypeStorageId;
                        idxSubTypeStorage.CreatedBy = AppUsers.CurrentUser(User).UserId;
                        idxSubTypeStorage.CreatedDate = DateTime.Now;

                        await _subTypeStorageService.InsertIDXSubTypeStorage(idxSubTypeStorage);
                    }
                }

                var listDetail = Request.Form[GlobalConst.DetailArray].ToArray();

                MstSubTypeStorageDetail MstSubTypeStorageDetail;


                await _subTypeStorageService.DeleteGMDSubTypeStorage(header.SubTypeStorageId);

                for (int i = 0; i < listDetail.Length; i++)
                {
                    var idxSubTypeStorages = listDetail[i].Split('#');

                    Guid typeStorageId = Guid.Empty;
                    Guid.TryParse(idxSubTypeStorages[0], out typeStorageId);

                    if (!string.IsNullOrEmpty(idxSubTypeStorages[0]))
                    {
                        MstSubTypeStorageDetail = new();
                        MstSubTypeStorageDetail.GmdDetailId = typeStorageId;
                        MstSubTypeStorageDetail.Size = int.Parse(idxSubTypeStorages[1]);
                        MstSubTypeStorageDetail.SubTypeStorageId = header.SubTypeStorageId;
                        MstSubTypeStorageDetail.CreatedBy = AppUsers.CurrentUser(User).UserId;
                        MstSubTypeStorageDetail.CreatedDate = DateTime.Now;

                        await _subTypeStorageService.InsertGMDSubTypeStorage(MstSubTypeStorageDetail);
                    }
                }
            }

            return RedirectToIndex();
        }
        
        #endregion

        #region HELPER
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.SubTypeStorage, new { Area = GlobalConst.MasterData });
        #endregion

        #region EXPORT IMPORT Excel
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

                List<DataTable> listData = new List<DataTable>() {
                new DataTable()
            };

                string templateName = nameof(MstSubTypeStorage).ToCleanNameOf();
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
                string templateName = nameof(MstSubTypeStorage).ToCleanNameOf();
                string fileName = nameof(MstSubTypeStorage).ToCleanNameOf();
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                var rows = await _subTypeStorageService.GetAll();

                List<DataTable> listData = new List<DataTable>() {
                rows.Select(x => new
                {
                    x.SubTypeStorageId,
                    x.SubTypeStorageCode,
                    x.SubTypeStorageName
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
                        var typeStorages = await _subTypeStorageService.GetAll();

                        List<MstSubTypeStorage> models = new();
                        MstSubTypeStorage model;

                        bool valid = true;
                        int errorCount = 0;

                        result.Columns.Add("Keterangan");
                        foreach (DataRow row in result.Rows)
                        {
                            string error = string.Empty;
                            if (string.IsNullOrEmpty(row[1].ToString()))
                            {
                                valid = false;
                                error = "_Kode Sub Media Penyimpanan harus diisi";
                            }
                            else if (string.IsNullOrEmpty(row[2].ToString()))
                            {
                                valid = false;
                                error = "_Nama Sub Media Penyimpanan harus diisi";
                            }
                            else if (typeStorages.Where(x => x.SubTypeStorageCode == row[1].ToString()).Count() > 0)
                            {
                                valid = false;
                                error = "_Kode Sub Media Penyimpanan sudah ada";
                            }


                            if (valid)
                            {
                                model = new();
                                model.SubTypeStorageId = Guid.NewGuid();
                                model.SubTypeStorageCode = row[1].ToString();
                                model.SubTypeStorageName = row[2].ToString();
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
                            await _subTypeStorageService.InsertBulk(models);
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
    }
}
