﻿using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(Const.MasterData)]
    public class RoomController : BaseController<TrxRoom>
    {
        public RoomController(IHostingEnvironment hostingEnvironment, IRoomService roomService, IFloorService floorService, IArchiveUnitService archiveUnitService)
        {
            _hostingEnvironment = hostingEnvironment;
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
            return View(Const.Form, new TrxRoom());
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _roomService.GetById(Id);
            
            if (data.Count() > 0)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                ViewBag.listFloors = await BindFloors();
                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _roomService.GetById(Id);
            if (data.Count() > 0)
            {
               ViewBag.listFloors = await BindFloors();
                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _roomService.GetById(Id);
            if (data.Count() > 0)
            {
                ViewBag.listFloors = await BindFloors();
                return View(Const.Form, data.FirstOrDefault());
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
                var result = Extensions.Global.ImportExcel(file, Const.Upload, _hostingEnvironment.WebRootPath);

                var floors = await _floorService.GetAll();

                List<TrxRoom> rooms = new();
                TrxRoom room;

                foreach (DataRow row in result.Rows)
                {
                    room = new();
                    room.RoomId = Guid.NewGuid();
                    room.FloorId = floors.Where(x => x.FloorCode == row[1].ToString()).FirstOrDefault().FloorId;
                    room.RoomCode = row[2].ToString();
                    room.RoomName = row[3].ToString();
                    room.ArchiveRoomType = row[4].ToString();
                    room.IsActive = true;
                    room.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    room.CreatedDate = DateTime.Now;

                    rooms.Add(room);
                }
                await _roomService.InsertBulk(rooms);
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
                string fileName = nameof(TrxRoom).Replace(Const.Trx, string.Empty);
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                var rooms = await _roomService.GetAll();
                var floors = await _floorService.GetAll();
                var archives = await _archiveUnitService.GetAll();

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxRoom).Replace(Const.Trx, string.Empty));

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue(Const.No);
                row.CreateCell(1).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitName));
                row.CreateCell(2).SetCellValue(nameof(TrxFloor.FloorName));
                row.CreateCell(3).SetCellValue(nameof(TrxRoom.RoomCode));
                row.CreateCell(4).SetCellValue(nameof(TrxRoom.RoomName));
                row.CreateCell(5).SetCellValue(nameof(TrxRoom.ArchiveRoomType));

                int no = 1;
                foreach (var item in rooms)
                {
                    row = excelSheet.CreateRow(no);

                    var floorDetail = floors.Where(x => x.FloorId == item.FloorId).FirstOrDefault();
                    
                    var archiveUnitName = archives.Where(x => x.ArchiveUnitId == floorDetail.ArchiveUnitId).FirstOrDefault().ArchiveUnitName;
                    var floorName = floors.Where(x => x.FloorId == floorDetail.FloorId).FirstOrDefault().FloorName;

                    row.CreateCell(0).SetCellValue(no);
                    row.CreateCell(1).SetCellValue(archiveUnitName);
                    row.CreateCell(2).SetCellValue(floorName);
                    row.CreateCell(3).SetCellValue(item.RoomCode);
                    row.CreateCell(4).SetCellValue(item.RoomName);
                    row.CreateCell(5).SetCellValue(item.ArchiveRoomType);

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
                string fileName = $"{Const.Template}-{nameof(TrxRoom).Replace(Const.Trx, string.Empty)}";
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxRoom).Replace(Const.Trx, string.Empty));
                ISheet excelSheetFloors = workbook.CreateSheet(nameof(TrxFloor).Replace(Const.Trx, string.Empty));

                IRow row = excelSheet.CreateRow(0);
                IRow rowFloor = excelSheetFloors.CreateRow(0);

                row.CreateCell(0).SetCellValue(Const.No);
                row.CreateCell(1).SetCellValue(nameof(TrxFloor.FloorCode));
                row.CreateCell(2).SetCellValue(nameof(TrxRoom.RoomCode));
                row.CreateCell(3).SetCellValue(nameof(TrxRoom.RoomName));
                row.CreateCell(4).SetCellValue(nameof(TrxRoom.ArchiveRoomType));


                rowFloor.CreateCell(0).SetCellValue(Const.No);
                rowFloor.CreateCell(1).SetCellValue(nameof(TrxFloor.FloorCode));
                rowFloor.CreateCell(2).SetCellValue(nameof(TrxFloor.FloorName));
                rowFloor.CreateCell(3).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitName));

                var dataFloors = await _floorService.GetAll();
                var dataArchiveUnits = await _archiveUnitService.GetAll();

                int no = 1;
                foreach (var item in dataFloors)
                {
                    rowFloor = excelSheetFloors.CreateRow(no);

                    var archiveUnitName = dataArchiveUnits.Where(x => x.ArchiveUnitId == item.ArchiveUnitId).FirstOrDefault().ArchiveUnitName;

                    rowFloor.CreateCell(0).SetCellValue(no);
                    rowFloor.CreateCell(1).SetCellValue(item.FloorCode);
                    rowFloor.CreateCell(2).SetCellValue(item.FloorName);
                    rowFloor.CreateCell(3).SetCellValue(archiveUnitName);
                    no += 1;
                }
                workbook.WriteExcelToResponse(HttpContext, fileName);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.Room, new { Area = Const.MasterData });
    }
}
