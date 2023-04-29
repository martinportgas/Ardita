using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Ardita.Areas.MasterData.Controllers
{

    [CustomAuthorizeAttribute]
    [Area(Const.MasterData)]
    public class LevelController : BaseController<TrxLevel>
    {
        public LevelController(
            IHostingEnvironment hostingEnvironment,
            ILevelService levelService,
            IRackService rackService,
            IRoomService roomService,
            IFloorService floorService,
            IArchiveUnitService archiveUnitService
            )
        {
            _hostingEnvironment = hostingEnvironment;
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
                var result = await _levelService.GetListClassification(model);
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

            return View(Const.Form, new TrxLevel());
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _levelService.GetById(Id);

            if (data.Count() > 0)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                ViewBag.listRooms = await BindRooms();
                ViewBag.listRacks = await BindRacks();

                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _levelService.GetById(Id);
            if (data.Count() > 0)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                ViewBag.listRooms = await BindRooms();
                ViewBag.listRacks = await BindRacks();

                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _levelService.GetById(Id);
            if (data.Count() > 0)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                ViewBag.listRooms = await BindRooms();
                ViewBag.listRacks = await BindRacks();

                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Save(TrxLevel model)
        {
            if (model != null)
            {
                if (model.LevelId != Guid.Empty)
                {
                    model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdatedDate = DateTime.Now;
                    await _levelService.Update(model);
                }
                else
                {
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;
                    await _levelService.Insert(model);
                }
            }
            return RedirectToIndex();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Delete(TrxLevel model)
        {
            if (model != null && model.LevelId != Guid.Empty)
            {
                await _levelService.Delete(model);
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

                var racks = await _rackService.GetAll();

                List<TrxLevel> levels = new();
                TrxLevel level;

                foreach (DataRow row in result.Rows)
                {
                    level = new();
                    level.LevelId = Guid.NewGuid();
                    level.RackId = racks.Where(x => x.RackCode == row[1].ToString()).FirstOrDefault().RackId;
                    level.LevelCode = row[2].ToString();
                    level.LevelName = row[3].ToString();
                    level.IsActive = true;
                    level.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    level.CreatedDate = DateTime.Now;

                    levels.Add(level);
                }
                await _levelService.InsertBulk(levels);
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
                string fileName = nameof(TrxLevel).Replace(Const.Trx, string.Empty);
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                var levels = await _levelService.GetAll();
                var racks = await _rackService.GetAll();
                var rooms = await _roomService.GetAll();
                var floors = await _floorService.GetAll();
                var archives = await _archiveUnitService.GetAll();

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxLevel).Replace(Const.Trx, string.Empty));

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue(Const.No);
                row.CreateCell(1).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitName));
                row.CreateCell(2).SetCellValue(nameof(TrxFloor.FloorName));
                row.CreateCell(3).SetCellValue(nameof(TrxRoom.RoomName));
                row.CreateCell(4).SetCellValue(nameof(TrxRack.RackName));
                row.CreateCell(5).SetCellValue(nameof(TrxLevel.LevelCode));
                row.CreateCell(6).SetCellValue(nameof(TrxLevel.LevelName));

                int no = 1;
                foreach (var item in levels)
                {
                    row = excelSheet.CreateRow(no);

                    var rackDetail = racks.Where(x => x.RackId == item.RackId).FirstOrDefault();
                    var roomDetail = rooms.Where(x => x.RoomId == rackDetail.RoomId).FirstOrDefault();
                    var floorDetail = floors.Where(x => x.FloorId == roomDetail.FloorId).FirstOrDefault();

                    var archiveUnitName = archives.Where(x => x.ArchiveUnitId == floorDetail.ArchiveUnitId).FirstOrDefault().ArchiveUnitName;
                    var floorName = floors.Where(x => x.FloorId == floorDetail.FloorId).FirstOrDefault().FloorName;
                    var roomName = roomDetail.RoomName;
                    var rackName = rackDetail.RackName;

                    row.CreateCell(0).SetCellValue(no);
                    row.CreateCell(1).SetCellValue(archiveUnitName);
                    row.CreateCell(2).SetCellValue(floorName);
                    row.CreateCell(3).SetCellValue(roomName);
                    row.CreateCell(4).SetCellValue(rackName);
                    row.CreateCell(5).SetCellValue(item.LevelCode);
                    row.CreateCell(6).SetCellValue(item.LevelName);

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
                string fileName = $"{Const.Template}-{nameof(TrxLevel).Replace(Const.Trx, string.Empty)}";
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxLevel).Replace(Const.Trx, string.Empty));
                ISheet excelSheetRacks = workbook.CreateSheet(nameof(TrxRack).Replace(Const.Trx, string.Empty));

                IRow row = excelSheet.CreateRow(0);
                IRow rowRack = excelSheetRacks.CreateRow(0);

                row.CreateCell(0).SetCellValue(Const.No);
                row.CreateCell(1).SetCellValue(nameof(TrxRack.RackCode));
                row.CreateCell(2).SetCellValue(nameof(TrxLevel.LevelCode));
                row.CreateCell(3).SetCellValue(nameof(TrxLevel.LevelName));


                rowRack.CreateCell(0).SetCellValue(Const.No);
                rowRack.CreateCell(1).SetCellValue(nameof(TrxRack.RackCode));
                rowRack.CreateCell(2).SetCellValue(nameof(TrxRack.RackName));
                rowRack.CreateCell(3).SetCellValue(nameof(TrxRoom.RoomName));
                rowRack.CreateCell(4).SetCellValue(nameof(TrxFloor.FloorName));
                rowRack.CreateCell(5).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitName));

                var dataRacks = await _rackService.GetAll();
                var dataRooms = await _roomService.GetAll();
                var dataFloors = await _floorService.GetAll();
                var dataArchiveUnits = await _archiveUnitService.GetAll();

                int no = 1;
                foreach (var item in dataRacks)
                {
                    rowRack = excelSheetRacks.CreateRow(no);

                    var roomDetail = dataRooms.Where(x => x.RoomId == item.RoomId).FirstOrDefault();
                    var roomName = roomDetail.RoomName;

                    var floorDetail = dataFloors.Where(x => x.FloorId == roomDetail.FloorId).FirstOrDefault();
                    var floorName = floorDetail.FloorName;

                    var archiveUnitName = dataArchiveUnits.Where(x => x.ArchiveUnitId == floorDetail.ArchiveUnitId).FirstOrDefault().ArchiveUnitName;


                    rowRack.CreateCell(0).SetCellValue(no);
                    rowRack.CreateCell(1).SetCellValue(item.RackCode);
                    rowRack.CreateCell(2).SetCellValue(item.RackName);
                    rowRack.CreateCell(3).SetCellValue(roomName);
                    rowRack.CreateCell(4).SetCellValue(floorName);
                    rowRack.CreateCell(5).SetCellValue(archiveUnitName);
                    no += 1;
                }
                workbook.WriteExcelToResponse(HttpContext, fileName);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.Level, new { Area = Const.MasterData });

    }
}
