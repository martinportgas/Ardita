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

namespace Ardita.Areas.MasterData.Controllers
{

    [CustomAuthorizeAttribute]
    [Area(Const.MasterData)]
    public class LevelController : BaseController<TrxLevel>
    {
        public LevelController(
            ILevelService levelService,
            IRackService rackService,
            IRoomService roomService,
            IFloorService floorService,
            IArchiveUnitService archiveUnitService
            )
        {
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

            if (data != null)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                ViewBag.listRooms = await BindRooms();
                ViewBag.listRacks = await BindRacks();

                return View(Const.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _levelService.GetById(Id);
            if (data != null)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                ViewBag.listRooms = await BindRooms();
                ViewBag.listRacks = await BindRacks();

                return View(Const.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _levelService.GetById(Id);
            if (data != null)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                ViewBag.listRooms = await BindRooms();
                ViewBag.listRacks = await BindRacks();

                return View(Const.Form, data);
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
                var result = Extensions.Global.ImportExcel(file, Const.Upload, string.Empty);
                var racks = await _rackService.GetAll();

                List<TrxLevel> levels = new();
                TrxLevel level;

                foreach (DataRow row in result.Rows)
                {
                    level = new();
                    level.LevelId = Guid.NewGuid();

                    level.RackId = racks.Where(x => x.RackCode.Contains(row[1].ToString())).FirstOrDefault().RackId;
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
                string fileName = nameof(TrxLevel).ToCleanNameOf();
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                var levels = await _levelService.GetAll();

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
                    row.CreateCell(0).SetCellValue(no);
                    row.CreateCell(1).SetCellValue(item.Rack.Room.Floor.ArchiveUnit.ArchiveUnitName);
                    row.CreateCell(2).SetCellValue(item.Rack.Room.Floor.FloorName);
                    row.CreateCell(3).SetCellValue(item.Rack.Room.RoomName);
                    row.CreateCell(4).SetCellValue(item.Rack.RackName);
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
                string fileName = $"{Const.Template}-{nameof(TrxLevel).ToCleanNameOf()}";
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

                int no = 1;
                foreach (var item in dataRacks)
                {
                    rowRack = excelSheetRacks.CreateRow(no);
                    rowRack.CreateCell(0).SetCellValue(no);
                    rowRack.CreateCell(1).SetCellValue(item.RackCode);
                    rowRack.CreateCell(2).SetCellValue(item.RackName);
                    rowRack.CreateCell(3).SetCellValue(item.Room.RoomName);
                    rowRack.CreateCell(4).SetCellValue(item.Room.Floor.FloorName);
                    rowRack.CreateCell(5).SetCellValue(item.Room.Floor.ArchiveUnit.ArchiveUnitName);
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
