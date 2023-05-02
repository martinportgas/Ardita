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

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(Const.MasterData)]
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

            if (data != null)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                ViewBag.listRooms = await BindRooms();
                ViewBag.listRacks = await BindRacks();
                ViewBag.listLevels = await BindLevels();

                return View(Const.Form, data);
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

                return View(Const.Form, data);
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

                return View(Const.Form, data);
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
                var result = Extensions.Global.ImportExcel(file, Const.Upload, string.Empty);

                var levels = await _levelService.GetAll();

                List<TrxRow> trxRows = new();
                TrxRow objRow;

                foreach (DataRow row in result.Rows)
                {
                    objRow = new();
                    objRow.RowId = Guid.NewGuid();
                    objRow.LevelId = levels.Where(x => x.LevelCode.Contains(row[1].ToString())).FirstOrDefault().LevelId;
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
                string fileName = nameof(TrxRow).ToCleanNameOf();
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                var rows = await _rowService.GetAll();

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxLevel).ToCleanNameOf());

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
                    row.CreateCell(0).SetCellValue(no);
                    row.CreateCell(1).SetCellValue(item.Level.Rack.Room.Floor.ArchiveUnit.ArchiveUnitName);
                    row.CreateCell(2).SetCellValue(item.Level.Rack.Room.Floor.FloorName);
                    row.CreateCell(3).SetCellValue(item.Level.Rack.Room.RoomName);
                    row.CreateCell(4).SetCellValue(item.Level.Rack.RackName);
                    row.CreateCell(5).SetCellValue(item.Level.LevelName);
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
                string fileName = $"{Const.Template}-{nameof(TrxRow).ToCleanNameOf()}";
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxRow).ToCleanNameOf());
                ISheet excelSheetLevels = workbook.CreateSheet(nameof(TrxLevel).ToCleanNameOf());

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

                int no = 1;
                foreach (var item in dataLevels)
                {
                    rowLevel = excelSheetLevels.CreateRow(no);

                    rowLevel.CreateCell(0).SetCellValue(no);
                    rowLevel.CreateCell(1).SetCellValue(item.LevelCode);
                    rowLevel.CreateCell(2).SetCellValue(item.LevelName);
                    rowLevel.CreateCell(3).SetCellValue(item.Rack.RackName);
                    rowLevel.CreateCell(4).SetCellValue(item.Rack.Room.RoomName);
                    rowLevel.CreateCell(5).SetCellValue(item.Rack.Room.Floor.FloorName);
                    rowLevel.CreateCell(6).SetCellValue(item.Rack.Room.Floor.ArchiveUnit.ArchiveUnitName);
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
