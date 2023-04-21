using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area("MasterData")]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;
        private readonly IFloorService _floorService;
        private readonly IArchiveUnitService _archiveUnitService;

        public RoomController(IRoomService roomService, IFloorService floorService, IArchiveUnitService archiveUnitService)
        {
            _roomService = roomService;
            _floorService = floorService;
            _archiveUnitService = archiveUnitService;
        }
        public IActionResult Index() => View();
        public async Task<JsonResult> GetData(DataTablePostModel model)
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
        public async Task<IActionResult> Add() 
        {
            ViewBag.listFloors = await BindFloors();
            ViewBag.listArchiveUnits = await BindArchiveUnits();
            return View(Const.Form, new TrxRoom());
        }
        public async Task<IActionResult> Update(Guid Id)
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
        public async Task<IActionResult> Remove(Guid Id)
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
        public async Task<IActionResult> Detail(Guid Id)
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
        public async Task<IActionResult> Save(TrxRoom model)
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
        public async Task<IActionResult> Delete(TrxRoom model)
        {
            if (model != null && model.RoomId != Guid.Empty)
            {
                await _roomService.Delete(model);
            }
            return RedirectToIndex();
        }

        private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.Room, new { Area = Const.MasterData });
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

        public async Task<JsonResult> BindFloors(string Id)
        {
            List<TrxFloor> listFloors = new();
            Guid ArchiveUnitId = new Guid(Id);

            var data = await _floorService.GetAll();
            listFloors = data.Where(x => x.ArchiveUnitId == ArchiveUnitId).ToList();
            return Json(listFloors);
            
        }
    }
}
