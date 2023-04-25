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
    public class RowController : BaseController<TrxRow>
    {
        public RowController(
            IHostingEnvironment hostingEnvironment,
            IRowService rowService,
            ILevelService levelService,
            IRackService rackService,
            IRoomService roomService,
            IFloorService floorService,
            IArchiveUnitService archiveUnitService
            )
        {
            _hostingEnvironment = hostingEnvironment;
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
                var result = await _rowService.GetListClassification(model);
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

            return View(Const.Form, new TrxRow());
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _rowService.GetById(Id);

            if (data.Count() > 0)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                ViewBag.listRooms = await BindRooms();
                ViewBag.listRacks = await BindRacks();
                ViewBag.listLevels = await BindLevels();

                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _rowService.GetById(Id);
            if (data.Count() > 0)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                ViewBag.listRooms = await BindRooms();
                ViewBag.listRacks = await BindRacks();
                ViewBag.listLevels = await BindLevels();

                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _rowService.GetById(Id);
            if (data.Count() > 0)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                ViewBag.listRooms = await BindRooms();
                ViewBag.listRacks = await BindRacks();
                ViewBag.listLevels = await BindLevels();

                return View(Const.Form, data.FirstOrDefault());
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload()
        {
            try
            {
                IFormFile file = Request.Form.Files[0];
                var result = Extensions.Global.ImportExcel(file, Const.Upload, _hostingEnvironment.WebRootPath);

                var levels = await _levelService.GetAll();

                List<TrxRow> trxRows = new();
                TrxRow objRow;

                foreach (DataRow row in result.Rows)
                {
                    objRow = new();
                    objRow.RowId = Guid.NewGuid();
                    objRow.LevelId = levels.Where(x => x.LevelCode == row[1].ToString()).FirstOrDefault().LevelId;
                    objRow.RowCode = row[2].ToString();
                    objRow.RowName = row[3].ToString();
                    objRow.IsActive = true;
                    objRow.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    objRow.CreatedDate = DateTime.Now;

                    trxRows.Add(objRow);
                }
                await _rowService.InsertBulk(trxRows);
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
                string fileName = nameof(TrxRow).Replace(Const.Trx, string.Empty);
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                var rows = await _rowService.GetAll();
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
                row.CreateCell(5).SetCellValue(nameof(TrxLevel.LevelName));
                row.CreateCell(6).SetCellValue(nameof(TrxRow.RowCode));
                row.CreateCell(7).SetCellValue(nameof(TrxRow.RowName));

                int no = 1;
                foreach (var item in rows)
                {
                    row = excelSheet.CreateRow(no);

                    var levelDetail = levels.Where(x => x.LevelId == item.LevelId).FirstOrDefault();
                    var rackDetail = racks.Where(x => x.RackId == levelDetail.RackId).FirstOrDefault();
                    var roomDetail = rooms.Where(x => x.RoomId == rackDetail.RoomId).FirstOrDefault();
                    var floorDetail = floors.Where(x => x.FloorId == roomDetail.FloorId).FirstOrDefault();

                    var archiveUnitName = archives.Where(x => x.ArchiveUnitId == floorDetail.ArchiveUnitId).FirstOrDefault().ArchiveUnitName;
                    var floorName = floors.Where(x => x.FloorId == floorDetail.FloorId).FirstOrDefault().FloorName;
                    var roomName = roomDetail.RoomName;
                    var rackName = rackDetail.RackName;
                    var levelName = levelDetail.LevelName;

                    row.CreateCell(0).SetCellValue(no);
                    row.CreateCell(1).SetCellValue(archiveUnitName);
                    row.CreateCell(2).SetCellValue(floorName);
                    row.CreateCell(3).SetCellValue(roomName);
                    row.CreateCell(4).SetCellValue(rackName);
                    row.CreateCell(5).SetCellValue(levelName);
                    row.CreateCell(6).SetCellValue(item.RowCode);
                    row.CreateCell(7).SetCellValue(item.RowName);

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
                string fileName = $"{Const.Template}-{nameof(TrxRow).Replace(Const.Trx, string.Empty)}";
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxRow).Replace(Const.Trx, string.Empty));
                ISheet excelSheetLevels = workbook.CreateSheet(nameof(TrxLevel).Replace(Const.Trx, string.Empty));

                IRow row = excelSheet.CreateRow(0);
                IRow rowLevel = excelSheetLevels.CreateRow(0);

                row.CreateCell(0).SetCellValue(Const.No);
                row.CreateCell(1).SetCellValue(nameof(TrxLevel.LevelCode));
                row.CreateCell(2).SetCellValue(nameof(TrxRow.RowCode));
                row.CreateCell(3).SetCellValue(nameof(TrxRow.RowName));


                rowLevel.CreateCell(0).SetCellValue(Const.No);
                rowLevel.CreateCell(1).SetCellValue(nameof(TrxLevel.LevelCode));
                rowLevel.CreateCell(2).SetCellValue(nameof(TrxLevel.LevelName));
                rowLevel.CreateCell(3).SetCellValue(nameof(TrxRack.RackName));
                rowLevel.CreateCell(4).SetCellValue(nameof(TrxRoom.RoomName));
                rowLevel.CreateCell(5).SetCellValue(nameof(TrxFloor.FloorName));
                rowLevel.CreateCell(6).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitName));

                var dataLevels = await _levelService.GetAll();
                var dataRacks = await _rackService.GetAll();
                var dataRooms = await _roomService.GetAll();
                var dataFloors = await _floorService.GetAll();
                var dataArchiveUnits = await _archiveUnitService.GetAll();

                int no = 1;
                foreach (var item in dataLevels)
                {
                    rowLevel = excelSheetLevels.CreateRow(no);

                    var rackDetail = dataRacks.Where(x => x.RackId == item.RackId).FirstOrDefault();
                    var rackName = rackDetail.RackName;

                    var roomDetail = dataRooms.Where(x => x.RoomId == rackDetail.RoomId).FirstOrDefault();
                    var roomName = roomDetail.RoomName;

                    var floorDetail = dataFloors.Where(x => x.FloorId == roomDetail.FloorId).FirstOrDefault();
                    var floorName = floorDetail.FloorName;

                    var archiveUnitName = dataArchiveUnits.Where(x => x.ArchiveUnitId == floorDetail.ArchiveUnitId).FirstOrDefault().ArchiveUnitName;


                    rowLevel.CreateCell(0).SetCellValue(no);
                    rowLevel.CreateCell(1).SetCellValue(item.LevelCode);
                    rowLevel.CreateCell(2).SetCellValue(item.LevelName);
                    rowLevel.CreateCell(3).SetCellValue(rackName);
                    rowLevel.CreateCell(4).SetCellValue(roomName);
                    rowLevel.CreateCell(5).SetCellValue(floorName);
                    rowLevel.CreateCell(6).SetCellValue(archiveUnitName);
                    no += 1;
                }
                workbook.WriteExcelToResponse(HttpContext, fileName);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.Row, new { Area = Const.MasterData });
    }
}
