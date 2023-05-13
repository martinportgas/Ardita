using Ardita.Controllers;
using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload()
        {
            try
            {
                IFormFile file = Request.Form.Files[0];
                var result = Extensions.Global.ImportExcel(file, GlobalConst.Upload, string.Empty);

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
        public async Task<IActionResult> Export()
        {
            try
            {
                string fileName = nameof(TrxRack).ToCleanNameOf();
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                var racks = await _rackService.GetAll();

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxRack).ToCleanNameOf());

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue(GlobalConst.No);
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

                    row.CreateCell(0).SetCellValue(no);
                    row.CreateCell(1).SetCellValue(item.Room.Floor.ArchiveUnit.ArchiveUnitName);
                    row.CreateCell(2).SetCellValue(item.Room.Floor.FloorName);
                    row.CreateCell(3).SetCellValue(item.Room.RoomName);
                    row.CreateCell(4).SetCellValue(item.RackCode);
                    row.CreateCell(5).SetCellValue(item.RackName);
                    row.CreateCell(6).SetCellValue(item.Length.ToString());

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
                string fileName = $"{GlobalConst.Template}-{nameof(TrxRack).ToCleanNameOf()}";
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxRack).ToCleanNameOf());
                ISheet excelSheetRooms = workbook.CreateSheet(nameof(TrxRoom).ToCleanNameOf());

                IRow row = excelSheet.CreateRow(0);
                IRow rowRoom = excelSheetRooms.CreateRow(0);

                row.CreateCell(0).SetCellValue(GlobalConst.No);
                row.CreateCell(1).SetCellValue(nameof(TrxRoom.RoomCode));
                row.CreateCell(2).SetCellValue(nameof(TrxRack.RackCode));
                row.CreateCell(3).SetCellValue(nameof(TrxRack.RackName));
                row.CreateCell(4).SetCellValue(nameof(TrxRack.Length));


                rowRoom.CreateCell(0).SetCellValue(GlobalConst.No);
                rowRoom.CreateCell(1).SetCellValue(nameof(TrxRoom.RoomCode));
                rowRoom.CreateCell(2).SetCellValue(nameof(TrxRoom.RoomName));
                rowRoom.CreateCell(3).SetCellValue(nameof(TrxFloor.FloorName));
                rowRoom.CreateCell(4).SetCellValue(nameof(TrxRoom.ArchiveRoomType));
                rowRoom.CreateCell(5).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitName));

                var dataRooms = await _roomService.GetAll();

                int no = 1;
                foreach (var item in dataRooms)
                {
                    rowRoom = excelSheetRooms.CreateRow(no);

                    rowRoom.CreateCell(0).SetCellValue(no);
                    rowRoom.CreateCell(1).SetCellValue(item.RoomCode);
                    rowRoom.CreateCell(2).SetCellValue(item.RoomName);
                    rowRoom.CreateCell(3).SetCellValue(item.ArchiveRoomType);
                    rowRoom.CreateCell(4).SetCellValue(item.Floor.FloorName);
                    rowRoom.CreateCell(5).SetCellValue(item.Floor.ArchiveUnit.ArchiveUnitName);
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
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.Rack, new { Area = GlobalConst.MasterData });
    }
}
