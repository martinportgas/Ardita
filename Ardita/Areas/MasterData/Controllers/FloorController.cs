﻿using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.Streaming;
using NPOI.XSSF.UserModel;
using NPOI.XWPF.UserModel;
using System.Data;
using System.Text;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(Const.MasterData)]
    public class FloorController : BaseController<TrxFloor>
    {
        #region MEMBER AND CTR
        public FloorController(
            IHostingEnvironment hostingEnvironment,
            IFloorService floorService, 
            IArchiveUnitService archiveUnitService)
        {
            _hostingEnvironment = hostingEnvironment;
            _floorService = floorService;
            _archiveUnitService = archiveUnitService;
        }
        #endregion
        #region MAIN
        public override async Task<ActionResult> Index() => await base.Index();
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                var result = await _floorService.GetListClassification(model);

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

            return View(Const.Form, new TrxFloor());
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _floorService.GetById(Id);
            if (data.Count() > 0)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _floorService.GetById(Id);
            if (data.Count() > 0)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _floorService.GetById(Id);
            if (data.Count() > 0)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnits();
                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Save(TrxFloor model)
        {
            if (model != null)
            {
                if (model.FloorId != Guid.Empty)
                {
                    model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdatedDate = DateTime.Now;
                    await _floorService.Update(model);
                }

                else
                {
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;
                    await _floorService.Insert(model);
                }
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

                var ArchiveUnits = await _archiveUnitService.GetAll();

                List<TrxFloor> floors = new();
                TrxFloor floor;

                foreach (DataRow row in result.Rows)
                {
                    floor = new();
                    floor.FloorId = Guid.NewGuid();
                    floor.ArchiveUnitId = ArchiveUnits.Where(x => x.ArchiveUnitCode == row[0].ToString()).FirstOrDefault().ArchiveUnitId;
                    floor.FloorCode = row[1].ToString();
                    floor.FloorName = row[2].ToString();
                    floor.IsActive = true;
                    floor.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    floor.CreatedDate = DateTime.Now;

                    floors.Add(floor);
                }
                await _floorService.InsertBulk(floors);
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
                string fileName = nameof(TrxFloor).Replace(Const.Trx, string.Empty);
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                var floors = await _floorService.GetAll();
                var archives = await _archiveUnitService.GetAll();

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxFloor).Replace(Const.Trx, string.Empty));

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitName));
                row.CreateCell(1).SetCellValue(nameof(TrxFloor.FloorCode));
                row.CreateCell(2).SetCellValue(nameof(TrxFloor.FloorName));

                int no = 1;
                foreach (var item in floors)
                {
                    row = excelSheet.CreateRow(no);
                    var archiveUnit = archives.Where(x => x.ArchiveUnitId == item.ArchiveUnitId).FirstOrDefault().ArchiveUnitName;
                    row.CreateCell(0).SetCellValue(archiveUnit);
                    row.CreateCell(1).SetCellValue(item.FloorCode);
                    row.CreateCell(2).SetCellValue(item.FloorName);

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
                string fileName = $"{Const.Template}-{nameof(TrxFloor).Replace(Const.Trx, string.Empty)}";
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxFloor).Replace(Const.Trx, string.Empty));
                ISheet excelSheetArchiceUnit = workbook.CreateSheet(nameof(TrxArchiveUnit).Replace(Const.Trx, string.Empty));

                IRow row = excelSheet.CreateRow(0);
                IRow rowArchive = excelSheetArchiceUnit.CreateRow(0);

                row.CreateCell(0).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitCode));
                row.CreateCell(1).SetCellValue(nameof(TrxFloor.FloorCode));
                row.CreateCell(2).SetCellValue(nameof(TrxFloor.FloorName));


                rowArchive.CreateCell(0).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitCode));
                rowArchive.CreateCell(1).SetCellValue(nameof(TrxArchiveUnit.ArchiveUnitName));

                var dataArchive = await _archiveUnitService.GetAll();

                int no = 1;
                foreach (var item in dataArchive)
                {
                    rowArchive = excelSheetArchiceUnit.CreateRow(no);

                    rowArchive.CreateCell(0).SetCellValue(item.ArchiveUnitCode);
                    rowArchive.CreateCell(1).SetCellValue(item.ArchiveUnitName);
                    no += 1;
                }
                workbook.WriteExcelToResponse(HttpContext, fileName);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Delete(TrxFloor model)
        {
            if (model != null && model.FloorId != Guid.Empty)
            {
                await _floorService.Delete(model);
            }
            return RedirectToIndex();
        }
        #endregion
        #region HELPER
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.Floor, new { Area = Const.MasterData });
        #endregion
  
    }
}
