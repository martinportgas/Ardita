using Ardita.Controllers;
using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(GlobalConst.MasterData)]
    public class RowController : BaseController<TrxRow>
    {
        public RowController(
            IRowService rowService,
            ILevelService levelService,
            IRackService rackService,
            IRoomService roomService,
            IFloorService floorService,
            IArchiveUnitService archiveUnitService
            )
        {
            _rowService = rowService;
            _levelService = levelService;
            _rackService = rackService;
            _roomService = roomService;
            _floorService = floorService;
            _archiveUnitService = archiveUnitService;
        }
        public override async Task<ActionResult> Index() => await base.Index();

        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                var result = await _rowService.GetList(model);
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
            ViewBag.listFloors = await BindFloors();
            ViewBag.listRooms = await BindRooms();
            ViewBag.listRacks = await BindRacks();
            ViewBag.listLevels = await BindLevels();

            return View(GlobalConst.Form, new TrxRow());
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _rowService.GetById(Id);

            if (data != null)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                ViewBag.listRooms = await BindRooms();
                ViewBag.listRacks = await BindRacks();
                ViewBag.listLevels = await BindLevels();

                return View(GlobalConst.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _rowService.GetById(Id);
            if (data != null)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                ViewBag.listRooms = await BindRooms();
                ViewBag.listRacks = await BindRacks();
                ViewBag.listLevels = await BindLevels();

                return View(GlobalConst.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _rowService.GetById(Id);
            if (data != null)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                ViewBag.listRooms = await BindRooms();
                ViewBag.listRacks = await BindRacks();
                ViewBag.listLevels = await BindLevels();

                return View(GlobalConst.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Save(TrxRow model)
        {
            if (model != null)
            {
                if (model.RowId != Guid.Empty)
                {
                    model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdatedDate = DateTime.Now;
                    await _rowService.Update(model);
                }
                else
                {
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;
                    await _rowService.Insert(model);
                }
            }
            return RedirectToIndex();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Delete(TrxRow model)
        {
            if (model != null && model.RowId != Guid.Empty)
            {
                await _rowService.Delete(model);
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
                        var levels = await _levelService.GetAll();
                        var rowDetails = await _rowService.GetAll();

                        List<TrxRow> trxRows = new();
                        TrxRow objRow;
                        bool valid = true;
                        int errorCount = 0;

                        result.Columns.Add("Keterangan");
                        foreach (DataRow row in result.Rows)
                        {
                            string error = string.Empty;
                            if (string.IsNullOrEmpty(row[1].ToString()))
                            {
                                valid = false;
                                error = "_Kode Baris harus diisi";
                            }
                            if (string.IsNullOrEmpty(row[2].ToString()))
                            {
                                valid = false;
                                error = "_Nama Baris harus diisi";
                            }
                            if (rowDetails.Where(x => x.RowCode == row[1].ToString()).Count() > 0)
                            {
                                valid = false;
                                error = "_Kode Baris sudah ada";
                            }
                            else
                            {
                                if (trxRows.Where(x => x.RowCode == row[1].ToString()).Count() > 0)
                                {
                                    valid = false;
                                    error = "_Kode Baris sudah ada";
                                }
                            }
                            if (levels.Where(x => x.LevelCode == row[3].ToString()).Count() == 0)
                            {
                                valid = false;
                                error = "_Kode Tingkat tidak ditemukan";
                            }


                            if (valid)
                            {
                                objRow = new();
                                objRow.RowId = Guid.NewGuid();
                                objRow.LevelId = levels.Where(x => x.LevelCode.Contains(row[3].ToString())).FirstOrDefault().LevelId;
                                objRow.RowCode = row[1].ToString();
                                objRow.RowName = row[2].ToString();
                                objRow.IsActive = true;
                                objRow.CreatedBy = AppUsers.CurrentUser(User).UserId;
                                objRow.CreatedDate = DateTime.Now;

                                trxRows.Add(objRow);

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
                            await _rowService.InsertBulk(trxRows);
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
        public async Task<IActionResult> Export()
        {
            try
            {
                string templateName = nameof(TrxRow).ToCleanNameOf();
                string fileName = nameof(TrxRow).ToCleanNameOf();
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                var rows = await _rowService.GetAll();

                List<DataTable> listData = new List<DataTable>() {
                rows.Select(x => new
                {
                    x.RowId,
                    x.RowCode,
                    x.RowName,
                    x.Level.LevelName,
                    x.Level.Rack.RackName,
                    x.Level.Rack.Room.RoomName,
                    x.Level.Rack.Room.Floor.FloorName,
                    x.Level.Rack.Room.Floor.ArchiveUnit.ArchiveUnitName
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
                var dataLevels = await _levelService.GetAll();

                List<DataTable> listData = new List<DataTable>() {
                new DataTable(),
                dataLevels.Select(x => new {
                    x.LevelId,
                    x.LevelCode,
                    x.LevelName,
                    x.Rack.RackCode,
                    x.Rack.RackName,
                    x.Rack.Room.RoomCode,
                    x.Rack.Room.RoomName,
                    x.Rack.Room.Floor.FloorCode,
                    x.Rack.Room.Floor.FloorName,
                    x.Rack.Room.Floor.ArchiveUnit.ArchiveUnitName,
                }).ToList().ToDataTable()
            };

                string templateName = nameof(TrxRow).ToCleanNameOf();
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
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.Row, new { Area = GlobalConst.MasterData });
    }
}
