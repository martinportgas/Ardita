using Ardita.Controllers;
using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace Ardita.Areas.MasterData.Controllers
{

    [CustomAuthorizeAttribute]
    [Area(GlobalConst.MasterData)]
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
                var result = await _levelService.GetList(model);
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

            return View(GlobalConst.Form, new TrxLevel());
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

                return View(GlobalConst.Form, data);
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

                return View(GlobalConst.Form, data);
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

                return View(GlobalConst.Form, data);
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
                    var levelDetails = await _levelService.GetAll();
                    var rackDetails = await _rackService.GetAll();

                    if (result.Rows.Count > 0)
                    {
                        List<TrxLevel> levels = new();
                        TrxLevel level;
                        bool valid = true;
                        int errorCount = 0;

                        result.Columns.Add("Keterangan");
                        foreach (DataRow row in result.Rows)
                        {
                            string error = string.Empty;

                            if (levelDetails.Where(x => x.LevelCode == row[1].ToString()).Count() > 0)
                            {
                                valid = false;
                                error = "_Kode Tingkat sudah ada";
                            }
                            else
                            {
                                if (levels.Where(x => x.LevelCode == row[1].ToString()).Count() > 0)
                                {
                                    valid = false;
                                    error = "_Kode Tingkat sudah ada";
                                }
                            }
                            if (string.IsNullOrEmpty(row[1].ToString()))
                            {
                                valid = false;
                                error = "_Kode Tingkat harus diisi";
                            }
                            if (string.IsNullOrEmpty(row[2].ToString()))
                            {
                                valid = false;
                                error = "_Nama Tingkat harus diisi";
                            }
                            if (rackDetails.Where(x=>x.RackCode == row[3].ToString()).ToList().Count == 0)
                            {
                                valid = false;
                                error = "_Kode Rak tidak ditemukan";
                            }

                            if (valid)
                            {
                                level = new();
                                level.LevelId = Guid.NewGuid();

                                level.RackId = rackDetails.Where(x => x.RackCode.Contains(row[3].ToString())).FirstOrDefault().RackId;
                                level.LevelCode = row[1].ToString();
                                level.LevelName = row[2].ToString();
                                level.IsActive = true;
                                level.CreatedBy = AppUsers.CurrentUser(User).UserId;
                                level.CreatedDate = DateTime.Now;

                                levels.Add(level);
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
                            await _levelService.InsertBulk(levels);
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
                string templateName = nameof(TrxLevel).ToCleanNameOf();
                string fileName = nameof(TrxLevel).ToCleanNameOf();
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                var levels = await _levelService.GetAll();

                List<DataTable> listData = new List<DataTable>() {
                levels.Select(x => new
                {
                    x.LevelId,
                    x.LevelCode,
                    x.LevelName,
                    x.Rack.RackName,
                    x.Rack.Room.RoomName,
                    x.Rack.Room.ArchiveRoomType,
                    x.Rack.Room.Floor.FloorName,
                    x.Rack.Room.Floor.ArchiveUnit.ArchiveUnitName
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
                var dataRacks = await _rackService.GetAll();

                List<DataTable> listData = new List<DataTable>() {
                new DataTable(),
                dataRacks.Select(x => new 
                { 
                   x.RackId,
                   x.RackCode,
                   x.RackName,
                   x.Room.RoomCode,
                   x.Room.RoomName,
                   x.Room.ArchiveRoomType,
                   x.Room.Floor.FloorName,
                   x.Room.Floor.ArchiveUnit.ArchiveUnitName
                }).ToList().ToDataTable(),
                GlobalConst.dataRoomType()
            };

                string templateName = nameof(TrxLevel).ToCleanNameOf();
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

        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.Level, new { Area = GlobalConst.MasterData });

    }
}
