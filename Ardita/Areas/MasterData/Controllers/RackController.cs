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
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(Const.MasterData)]
    public class RackController : BaseController<TrxRack>
    {
        public RackController(
            IHostingEnvironment hostingEnvironment,
            IRackService rackService,
            IRoomService roomService,
            IFloorService floorService,
            IArchiveUnitService archiveUnitService
            )
        {
            _hostingEnvironment = hostingEnvironment;
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
                var result = await _rackService.GetListClassification(model);
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

            return View(Const.Form, new TrxRack());
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _rackService.GetById(Id);

            if (data.Count() > 0)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                ViewBag.listRooms = await BindRooms();

                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _rackService.GetById(Id);
            if (data.Count() > 0)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                ViewBag.listRooms = await BindRooms();

                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _rackService.GetById(Id);
            if (data.Count() > 0)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                ViewBag.listRooms = await BindRooms();

                return View(Const.Form, data.FirstOrDefault());
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload()
        {
            try
            {
                IFormFile file = Request.Form.Files[0];
                var result = Extensions.Global.ImportExcel(file, Const.Upload, _hostingEnvironment.WebRootPath);

                var rooms = await _roomService.GetAll();
                var floors = await _floorService.GetAll();

                List<TrxRack> racks = new();
                TrxRack rack;

                foreach (DataRow row in result.Rows)
                {
                    rack = new();
                    rack.RackId = Guid.NewGuid();
                    rack.RoomId = rooms.Where(x => x.RoomCode == row[1].ToString()).FirstOrDefault().RoomId;
                    rack.RackCode = row[2].ToString();
                    rack.RackName = row[3].ToString();
                    rack.Length = Convert.ToInt32(row[4]);
                    rack.IsActive = true;
                    rack.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    rack.CreatedDate = DateTime.Now;

                    racks.Add(rack);
                }
                await _rackService.InsertBulk(racks);
                return RedirectToIndex();
            }
            catch (Exception)
            {

                throw new Exception();
            }
        }
        public async Task Export()
        {
            try
            {
                string fileName = nameof(TrxRack).Replace(Const.Trx, string.Empty);
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                var racks = await _rackService.GetAll();
                var rooms = await _roomService.GetAll();
                var floors = await _floorService.GetAll();
                var archives = await _archiveUnitService.GetAll();

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxRack).Replace(Const.Trx, string.Empty));

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue(Const.No);
                row.CreateCell(1).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitName));
                row.CreateCell(2).SetCellValue(nameof(TrxFloor.FloorName));
                row.CreateCell(3).SetCellValue(nameof(TrxRoom.RoomName));
                row.CreateCell(4).SetCellValue(nameof(TrxRack.RackCode));
                row.CreateCell(5).SetCellValue(nameof(TrxRack.RackName));
                row.CreateCell(6).SetCellValue(nameof(TrxRack.Length));

                int no = 1;
                foreach (var item in racks)
                {
                    row = excelSheet.CreateRow(no);

                    var roomDetail = rooms.Where(x => x.RoomId == item.RoomId).FirstOrDefault();
                    var floorDetail = floors.Where(x => x.FloorId == roomDetail.RoomId).FirstOrDefault();

                    var archiveUnitName = archives.Where(x => x.ArchiveUnitId == floorDetail.ArchiveUnitId).FirstOrDefault().ArchiveUnitName;
                    var floorName = floors.Where(x => x.FloorId == floorDetail.FloorId).FirstOrDefault().FloorName;
                    var roomName = roomDetail.RoomName;

                    row.CreateCell(0).SetCellValue(no);
                    row.CreateCell(1).SetCellValue(archiveUnitName);
                    row.CreateCell(2).SetCellValue(floorName);
                    row.CreateCell(3).SetCellValue(roomName);
                    row.CreateCell(4).SetCellValue(item.RackCode);
                    row.CreateCell(5).SetCellValue(item.RackName);
                    row.CreateCell(6).SetCellValue(item.Length.ToString());

                    no += 1;
                }
                workbook.WriteExcelToResponse(HttpContext, fileName);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public async Task DownloadTemplate()
        {
            try
            {
                string fileName = $"{Const.Template}-{nameof(TrxRack).Replace(Const.Trx, string.Empty)}";
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxRack).Replace(Const.Trx, string.Empty));
                ISheet excelSheetRooms = workbook.CreateSheet(nameof(TrxRoom).Replace(Const.Trx, string.Empty));

                IRow row = excelSheet.CreateRow(0);
                IRow rowRoom = excelSheetRooms.CreateRow(0);

                row.CreateCell(0).SetCellValue(Const.No);
                row.CreateCell(1).SetCellValue(nameof(TrxRoom.RoomCode));
                row.CreateCell(2).SetCellValue(nameof(TrxRack.RackCode));
                row.CreateCell(3).SetCellValue(nameof(TrxRack.RackName));
                row.CreateCell(4).SetCellValue(nameof(TrxRack.Length));


                rowRoom.CreateCell(0).SetCellValue(Const.No);
                rowRoom.CreateCell(1).SetCellValue(nameof(TrxRoom.RoomCode));
                rowRoom.CreateCell(2).SetCellValue(nameof(TrxRoom.RoomName));
                rowRoom.CreateCell(3).SetCellValue(nameof(TrxFloor.FloorName));
                rowRoom.CreateCell(4).SetCellValue(nameof(TrxRoom.ArchiveRoomType));
                rowRoom.CreateCell(5).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitName));

                var dataRooms = await _roomService.GetAll();
                var dataFloors = await _floorService.GetAll();
                var dataArchiveUnits = await _archiveUnitService.GetAll();

                int no = 1;
                foreach (var item in dataRooms)
                {
                    rowRoom = excelSheetRooms.CreateRow(no);

                    var floorDetail = dataFloors.Where(x => x.FloorId == item.FloorId).FirstOrDefault();

                    var floorName = floorDetail.FloorName;
                    var archiveUnitName = dataArchiveUnits.Where(x => x.ArchiveUnitId == floorDetail.ArchiveUnitId).FirstOrDefault().ArchiveUnitName;
                   

                    rowRoom.CreateCell(0).SetCellValue(no);
                    rowRoom.CreateCell(1).SetCellValue(item.RoomCode);
                    rowRoom.CreateCell(2).SetCellValue(item.RoomName);
                    rowRoom.CreateCell(3).SetCellValue(item.ArchiveRoomType);
                    rowRoom.CreateCell(4).SetCellValue(floorName);
                    rowRoom.CreateCell(5).SetCellValue(archiveUnitName);
                    no += 1;
                }
                workbook.WriteExcelToResponse(HttpContext, fileName);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.Rack, new { Area = Const.MasterData });
    }
}
