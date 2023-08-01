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

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorize]
    [Area(GlobalConst.MasterData)]
    public class ArchiveTypeController : BaseController<MstArchiveType>
    {
        #region MEMBER AND CTR
        public ArchiveTypeController(IArchiveTypeService archiveTypeService)
        {
            _archiveTypeService = archiveTypeService;
        }
        #endregion

        #region MAIN ACTION
        public override async Task<ActionResult> Index() => await base.Index();

        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                var result = await _archiveTypeService.GetList(model);

                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override async Task<IActionResult> Add()
        {
            return View(GlobalConst.Form, new MstArchiveType());
        }

        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _archiveTypeService.GetById(Id);
            if (data.Any())
            {
                return View(GlobalConst.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }

        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _archiveTypeService.GetById(Id);
            if (data.Any())
            {
                return View(GlobalConst.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }

        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _archiveTypeService.GetById(Id);
            if (data.Any())
            {
                return View(GlobalConst.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Save(MstArchiveType model)
        {
            if (model != null)
            {
                if (model.ArchiveTypeId != Guid.Empty)
                {
                    model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdatedDate = DateTime.Now;
                    await _archiveTypeService.Update(model);
                }

                else
                {
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;
                    await _archiveTypeService.Insert(model);
                }
            }
            return RedirectToIndex();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Delete(MstArchiveType model)
        {
            if (model != null && model.ArchiveTypeId != Guid.Empty)
            {
                await _archiveTypeService.Delete(model);
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
                        List<MstArchiveType> mstArchiveTypes = new();
                        MstArchiveType mstArchiveType;

                        bool valid = true;
                        int errorCount = 0;

                        result.Columns.Add("Keterangan");

                        var archiveTypeDetail = await _archiveTypeService.GetAll();

                        foreach (DataRow row in result.Rows)
                        {
                            string error = string.Empty;

                            if (archiveTypeDetail.Where(x => x.ArchiveTypeCode == row[1].ToString()).Count() > 0)
                            {
                                valid = false;
                                error = "_Kode Tingkat Perkembangan sudah ada";
                            }
                            else
                            {
                                if (mstArchiveTypes.Where(x => x.ArchiveTypeCode == row[1].ToString()).Count() > 0)
                                {
                                    valid = false;
                                    error = "_Kode Tingkat Perkembangan sudah ada";
                                }
                            }

                            if (valid)
                            {
                                mstArchiveType = new();
                                mstArchiveType.ArchiveTypeId = Guid.NewGuid();

                                mstArchiveType.ArchiveTypeCode = row[1].ToString();
                                mstArchiveType.ArchiveTypeName = row[2].ToString();

                                mstArchiveType.IsActive = true;
                                mstArchiveType.CreatedBy = AppUsers.CurrentUser(User).UserId;
                                mstArchiveType.CreatedDate = DateTime.Now;

                                mstArchiveTypes.Add(mstArchiveType);
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
                            await _archiveTypeService.InsertBulk(mstArchiveTypes);

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
                string fileName = nameof(MstArchiveType).ToCleanNameOf();
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                var archiveTypes = await _archiveTypeService.GetAll();

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(MstArchiveType).ToCleanNameOf());

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue(GlobalConst.No);
                row.CreateCell(1).SetCellValue(nameof(MstArchiveType.ArchiveTypeCode));
                row.CreateCell(2).SetCellValue(nameof(MstArchiveType.ArchiveTypeName));

                int no = 1;
                foreach (var item in archiveTypes)
                {
                    row = excelSheet.CreateRow(no);
                    row.CreateCell(0).SetCellValue(no);
                    row.CreateCell(1).SetCellValue(item.ArchiveTypeCode);
                    row.CreateCell(2).SetCellValue(item.ArchiveTypeName);
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
                string fileName = $"{GlobalConst.Template}-{nameof(MstArchiveType).ToCleanNameOf()}";
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(MstArchiveType).ToCleanNameOf());

                IRow row = excelSheet.CreateRow(0);

                //Archive Type
                row.CreateCell(0).SetCellValue(GlobalConst.No);
                row.CreateCell(1).SetCellValue(nameof(MstArchiveType.ArchiveTypeCode));
                row.CreateCell(2).SetCellValue(nameof(MstArchiveType.ArchiveTypeName));

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
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.ArchiveType, new { Area = GlobalConst.MasterData });

        #endregion
    }
}
