using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area("MasterData")]
    public class RowController : Controller
    {

        private readonly IRowService _rowService;
        private readonly ILevelService _levelService;
        private readonly IRackService _rackService;
        private readonly IRoomService _roomService;
        private readonly IFloorService _floorService;
        private readonly IArchiveUnitService _archiveUnitService;

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
        public IActionResult Index() => View();
        public async Task<JsonResult> GetData(DataTablePostModel model)
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
        public async Task<IActionResult> Add()
        {
            ViewBag.listArchiveUnits = await BindArchiveUnits();
            ViewBag.listFloors = await BindFloors();
            ViewBag.listRooms = await BindRooms();
            ViewBag.listRacks = await BindRacks();
            ViewBag.listLevels = await BindLevels();

            return View(Const.Form, new TrxRow());
        }
        public async Task<IActionResult> Update(Guid Id)
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
        public async Task<IActionResult> Remove(Guid Id)
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
        public async Task<IActionResult> Detail(Guid Id)
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
        public async Task<IActionResult> Save(TrxRow model)
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
        public async Task<IActionResult> Delete(TrxRow model)
        {
            if (model != null && model.RowId != Guid.Empty)
            {
                await _rowService.Delete(model);
            }
            return RedirectToIndex();
        }
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.Row, new { Area = Const.MasterData });
        private async Task<List<SelectListItem>> BindRacks()
        {
            var archiveUnits = await _rackService.GetAll();

            return archiveUnits.Select(x => new SelectListItem
            {
                Value = x.RackId.ToString(),
                Text = x.RackName
            }).ToList();
        }
        private async Task<List<SelectListItem>> BindRooms()
        {
            var archiveUnits = await _roomService.GetAll();

            return archiveUnits.Select(x => new SelectListItem
            {
                Value = x.RoomId.ToString(),
                Text = x.RoomName
            }).ToList();
        }
        private async Task<List<SelectListItem>> BindFloors()
        {
            var archiveUnits = await _floorService.GetAll();

            return archiveUnits.Select(x => new SelectListItem
            {
                Value = x.FloorId.ToString(),
                Text = x.FloorName
            }).ToList();
        }
        private async Task<List<SelectListItem>> BindArchiveUnits()
        {
            var archiveUnits = await _archiveUnitService.GetAll();

            return archiveUnits.Select(x => new SelectListItem
            {
                Value = x.ArchiveUnitId.ToString(),
                Text = x.ArchiveUnitName
            }).ToList();
        }
        private async Task<List<SelectListItem>> BindLevels()
        {
            var archiveUnits = await _levelService.GetAll();

            return archiveUnits.Select(x => new SelectListItem
            {
                Value = x.LevelId.ToString(),
                Text = x.LevelName
            }).ToList();
        }
    }
}
