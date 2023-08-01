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
    [CustomAuthorizeAttribute]
    [Area(GlobalConst.MasterData)]
    public class RackController : BaseController<TrxRack>
    {
        public RackController(
            IRackService rackService,
            IRoomService roomService,
            IFloorService floorService,
            IArchiveUnitService archiveUnitService
            )
        {
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
                var result = await _rackService.GetList(model);
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

            return View(GlobalConst.Form, new TrxRack());
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _rackService.GetById(Id);

            if (data != null)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                ViewBag.listRooms = await BindRooms();

                return View(GlobalConst.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _rackService.GetById(Id);
            if (data != null)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                ViewBag.listRooms = await BindRooms();

                return View(GlobalConst.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _rackService.GetById(Id);
            if (data != null)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                ViewBag.listRooms = await BindRooms();

                return View(GlobalConst.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Save(TrxRack model)
        {
            if (model != null)
            {
                if (model.RackId != Guid.Empty)
                {
                    model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdatedDate = DateTime.Now;
                    await _rackService.Update(model);
                }

                else
                {
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;
                    await _rackService.Insert(model);
                }
            }
            return RedirectToIndex();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Delete(TrxRack model)
        {
            if (model != null && model.RackId != Guid.Empty)
            {
                await _rackService.Delete(model);
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
                        var rooms = await _roomService.GetAll();
                        var floors = await _floorService.GetAll();
                        var racksDetail = await _rackService.GetAll();

                        List<TrxRack> racks = new();
                        TrxRack rack;
                        bool valid = true;
                        int errorCount = 0;

                        result.Columns.Add("Keterangan");
                        foreach (DataRow row in result.Rows)
                        {
                            string error = string.Empty;

                            if (racksDetail.Where(x => x.RackCode == row[1].ToString()).Count() > 0)
                            {
                                valid = false;
                                error = "_Kode Rak sudah ada";
                            }
                            else
                            {
                                if (racks.Where(x => x.RackCode == row[1].ToString()).Count() > 0)
                                {
                                    valid = false;
                                    error = "_Kode Rak sudah ada";
                                }
                            }
                            if (rooms.Where(x => x.RoomCode == row[4].ToString()).Count() == 0)
                            {
                                valid = false;
                                error = "_Kode Ruangan tidak ditemukan";
                            }
                            if (!int.TryParse(row[3].ToString(), out int n))
                            {
                                valid = false;
                                error = "_Kolom Panjang harus angka!";
                            }
                            if (string.IsNullOrEmpty(row[1].ToString()))
                            {
                                valid = false;
                                error = "_Kode Rak harus diisi!";
                            }
                            if (string.IsNullOrEmpty(row[2].ToString()))
                            {
                                valid = false;
                                error = "_Nama Rak harus diisi!";
                            }

                            if (valid)
                            {
                                rack = new();
                                rack.RackId = Guid.NewGuid();
                                rack.RoomId = rooms.Where(x => x.RoomCode == row[4].ToString()).FirstOrDefault().RoomId;
                                rack.RackCode = row[1].ToString();
                                rack.RackName = row[2].ToString();
                                rack.Length = Convert.ToInt32(row[3]);
                                rack.IsActive = true;
                                rack.CreatedBy = AppUsers.CurrentUser(User).UserId;
                                rack.CreatedDate = DateTime.Now;

                                racks.Add(rack);
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
                            await _rackService.InsertBulk(racks);

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
                string templateName = nameof(TrxRack).ToCleanNameOf();
                string fileName = nameof(TrxRack).ToCleanNameOf();
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                var racks = await _rackService.GetAll();

                List<DataTable> listData = new List<DataTable>() {
                racks.Select(x => new
                {
                    x.RackId,
                    x.RackCode,
                    x.RackName,
                    x.Length,
                    x.Room.RoomName,
                    x.Room.ArchiveRoomType,
                    x.Room.Floor.FloorName,
                    x.Room.Floor.ArchiveUnit.ArchiveUnitName
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
                var dataRooms = await _roomService.GetAll();

                List<DataTable> listData = new List<DataTable>() {
                new DataTable(),
                dataRooms.Select(x => new { 
                    x.RoomId, 
                    x.RoomCode, 
                    x.RoomName, 
                    x.ArchiveRoomType, 
                    x.Floor.FloorCode, 
                    x.Floor.FloorName, 
                    x.Floor.ArchiveUnit.ArchiveUnitName 
                }).ToList().ToDataTable(),
                GlobalConst.dataRoomType()
            };

                string templateName = nameof(TrxRack).ToCleanNameOf();
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
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.Rack, new { Area = GlobalConst.MasterData });
    }
}
