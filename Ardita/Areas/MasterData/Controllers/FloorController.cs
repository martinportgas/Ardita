using Ardita.Controllers;
using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.Streaming;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using System.Data;
using System.Text;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(GlobalConst.MasterData)]
    public class FloorController : BaseController<TrxFloor>
    {
        #region MEMBER AND CTR
        public FloorController(
            IFloorService floorService, 
            IArchiveUnitService archiveUnitService)
        {
            _floorService = floorService;
            _archiveUnitService = archiveUnitService;
        }
        #endregion
        #region MAIN
        public override async Task<ActionResult> Index() => await base.Index();
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                var result = await _floorService.GetListClassification(model);

                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override async Task<IActionResult> Add()
        {

            ViewBag.listArchiveUnits = await BindArchiveUnits();

            return View(GlobalConst.Form, new TrxFloor());
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _floorService.GetById(Id);
            if (data != null)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                return View(GlobalConst.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _floorService.GetById(Id);
            if (data != null)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                return View(GlobalConst.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _floorService.GetById(Id);
            if (data != null)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                return View(GlobalConst.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Save(TrxFloor model)
        {
            if (model != null)
            {
                if (model.FloorId != Guid.Empty)
                {
                    model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdatedDate = DateTime.Now;
                    await _floorService.Update(model);
                }

                else
                {
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;
                    await _floorService.Insert(model);
                }
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
                        var ArchiveUnits = await _archiveUnitService.GetAll();
                        var floorDetails = await _floorService.GetAll();

                        List<TrxFloor> floors = new();
                        TrxFloor floor;


                        bool valid = true;
                        int errorCount = 0;

                        result.Columns.Add("Keterangan");

                        foreach (DataRow row in result.Rows)
                        {
                            string error = string.Empty;

                            if (floorDetails.Where(x => x.FloorCode == row[1].ToString()).Count() > 0)
                            {
                                valid = false;
                                error = "_Kode Lantai sudah ada";
                            }
                            else if (ArchiveUnits.Where(x => x.ArchiveUnitCode == row[3].ToString()).Count() == 0)
                            {
                                valid = false;
                                error = "_Kode Unit Kearsipan tidak ditemukan";
                            }

                            if (valid)
                            {

                                floor = new();
                                floor.FloorId = Guid.NewGuid();

                                floor.ArchiveUnitId = ArchiveUnits.Where(x => x.ArchiveUnitCode.Contains(row[3].ToString())).FirstOrDefault().ArchiveUnitId;
                                floor.FloorCode = row[1].ToString();
                                floor.FloorName = row[2].ToString();
                                floor.IsActive = true;
                                floor.CreatedBy = AppUsers.CurrentUser(User).UserId;
                                floor.CreatedDate = DateTime.Now;

                                floors.Add(floor);
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
                            await _floorService.InsertBulk(floors);
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
                string fileName = nameof(TrxFloor).ToCleanNameOf();
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                var floors = await _floorService.GetAll();
               
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxFloor).ToCleanNameOf());

                IRow row = excelSheet.CreateRow(0);

                
                row.CreateCell(0).SetCellValue(nameof(GlobalConst.No));
                row.CreateCell(1).SetCellValue(nameof(TrxFloor.FloorCode));
                row.CreateCell(2).SetCellValue(nameof(TrxFloor.FloorName));
                row.CreateCell(3).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitName));

                int no = 1;
                foreach (var item in floors)
                {
                    row = excelSheet.CreateRow(no);
                    row.CreateCell(0).SetCellValue(GlobalConst.No);
                    row.CreateCell(1).SetCellValue(item.FloorCode);
                    row.CreateCell(2).SetCellValue(item.FloorName);
                    row.CreateCell(3).SetCellValue(item.ArchiveUnit.ArchiveUnitName);

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
                string fileName = $"{GlobalConst.Template}-{nameof(TrxFloor).ToCleanNameOf()}";
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxFloor).ToCleanNameOf());
                ISheet excelSheetArchiceUnit = workbook.CreateSheet(nameof(TrxArchiveUnit).ToCleanNameOf());

                IRow row = excelSheet.CreateRow(0);
                IRow rowArchive = excelSheetArchiceUnit.CreateRow(0);

                row.CreateCell(0).SetCellValue(nameof(GlobalConst.No));
                row.CreateCell(1).SetCellValue(nameof(TrxFloor.FloorCode));
                row.CreateCell(2).SetCellValue(nameof(TrxFloor.FloorName)); 
                row.CreateCell(3).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitCode));


                rowArchive.CreateCell(0).SetCellValue(nameof(GlobalConst.No));
                rowArchive.CreateCell(1).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitCode));
                rowArchive.CreateCell(2).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitName));

                var dataArchive = await _archiveUnitService.GetAll();

                int no = 1;
                foreach (var item in dataArchive)
                {
                    rowArchive = excelSheetArchiceUnit.CreateRow(no);

                    rowArchive.CreateCell(0).SetCellValue(no);
                    rowArchive.CreateCell(1).SetCellValue(item.ArchiveUnitCode);
                    rowArchive.CreateCell(2).SetCellValue(item.ArchiveUnitName);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Delete(TrxFloor model)
        {
            if (model != null && model.FloorId != Guid.Empty)
            {
                await _floorService.Delete(model);
            }
            return RedirectToIndex();
        }
        #endregion
        #region HELPER
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.Floor, new { Area = GlobalConst.MasterData });
        #endregion
  
    }
}
