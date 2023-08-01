using Ardita.Controllers;
using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(GlobalConst.MasterData)]
    public class RoomController : BaseController<TrxRoom>
    {
        public RoomController(IRoomService roomService, IFloorService floorService, IArchiveUnitService archiveUnitService)
        {
            _roomService = roomService;
            _floorService = floorService;
            _archiveUnitService = archiveUnitService;
        }
        public override async Task<ActionResult> Index() => await base.Index();
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                var result = await _roomService.GetListClassification(model);
                return Json(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override async Task<IActionResult> Add() 
        {
            ViewBag.listFloors = await BindFloors();
            ViewBag.listArchiveUnits = await BindArchiveUnits();
            return View(GlobalConst.Form, new TrxRoom());
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _roomService.GetById(Id);
            
            if (data != null)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                return View(GlobalConst.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _roomService.GetById(Id);
            if (data != null)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                return View(GlobalConst.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _roomService.GetById(Id);
            if (data != null)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                return View(GlobalConst.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Save(TrxRoom model)
        {
            if (model != null)
            {
                if (model.RoomId != Guid.Empty)
                {
                    model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdatedDate = DateTime.Now;
                    await _roomService.Update(model);
                }

                else
                {
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;
                    await _roomService.Insert(model);
                }
            }
            return RedirectToIndex();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Delete(TrxRoom model)
        {
            if (model != null && model.RoomId != Guid.Empty)
            {
                await _roomService.Delete(model);
            }
            return RedirectToIndex();
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
                        var floors = await _floorService.GetAll();
                        var roomDetails = await _roomService.GetAll();

                        List<TrxRoom> rooms = new();
                        TrxRoom room;
                        bool valid = true;
                        int errorCount = 0;

                        result.Columns.Add("Keterangan");
                        foreach (DataRow row in result.Rows)
                        {
                            string error = string.Empty;

                            if (roomDetails.Where(x => x.RoomCode == row[1].ToString()).Count() > 0)
                            {
                                valid = false;
                                error = "_Kode Ruangan sudah ada";
                            }
                            else
                            {
                                if (rooms.Where(x => x.RoomCode == row[1].ToString()).Count() > 0)
                                {
                                    valid = false;
                                    error = "_Kode Ruangan sudah ada";
                                }
                            }
                            if (string.IsNullOrEmpty(row[1].ToString()))
                            {
                                valid = false;
                                error = "_Kode Ruangan harus diisi";
                            }
                            if (string.IsNullOrEmpty(row[2].ToString()))
                            {
                                valid = false;
                                error = "_Nama Ruangan harus diisi";
                            }
                            if (string.IsNullOrEmpty(row[3].ToString()))
                            {
                                valid = false;
                                error = "_Tipe Ruangan harus diisi";
                            }
                            if (row[3].ToString() != "Unit Kearsipan" && row[3].ToString() != "Unit Pengolah")
                            {
                                valid = false;
                                error = "_Tipe Ruangan tidak sesuai!";
                            }
                            if (floors.Where(x => x.FloorCode == row[4].ToString()).Count() == 0)
                            {
                                valid = false;
                                error = "_Kode Lantai tidak ditemukan";
                            }

                            if (valid)
                            {
                                room = new();
                                room.RoomId = Guid.NewGuid();
                                room.FloorId = floors.Where(x => x.FloorCode.Contains(row[4].ToString())).FirstOrDefault().FloorId;
                                room.RoomCode = row[1].ToString();
                                room.RoomName = row[2].ToString();
                                room.ArchiveRoomType = row[3].ToString();
                                room.IsActive = true;
                                room.CreatedBy = AppUsers.CurrentUser(User).UserId;
                                room.CreatedDate = DateTime.Now;

                                rooms.Add(room);
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
                            await _roomService.InsertBulk(rooms);
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
        public async Task<IActionResult> UploadForm()
        {
            await Task.Delay(0);
            ViewBag.errorCount = TempData["errorCount"] == null ? -1 : TempData["errorCount"];
            return View();
        }
        public async Task<IActionResult> Export()
        {
            try
            {
                string templateName = nameof(TrxRoom).ToCleanNameOf();
                string fileName = nameof(TrxRoom).ToCleanNameOf();
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                var rooms = await _roomService.GetAll();

                List<DataTable> listData = new List<DataTable>() {
                rooms.Select(x => new
                {
                    x.RoomId,
                    x.RoomCode,
                    x.RoomName,
                    x.ArchiveRoomType,
                    x.Floor.FloorCode,
                    x.Floor.FloorName,
                    x.Floor.ArchiveUnit.ArchiveUnitName
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
                var dataFloors = await _floorService.GetAll();

                List<DataTable> listData = new List<DataTable>() {
                new DataTable(),
                dataFloors.Select(x => new { x.FloorId, x.FloorCode, x.FloorName, x.ArchiveUnit.ArchiveUnitName }).ToList().ToDataTable(),
                GlobalConst.dataRoomType()
            };

                string templateName = nameof(TrxRoom).ToCleanNameOf();
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
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.Room, new { Area = GlobalConst.MasterData });
    }
}
