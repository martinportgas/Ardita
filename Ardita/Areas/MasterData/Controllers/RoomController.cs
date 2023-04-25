using Ardita.Controllers;
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
    public class RoomController : BaseController<TrxRoom>
    {
        public RoomController(IRoomService roomService, IFloorService floorService, IArchiveUnitService archiveUnitService)
        {
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
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.Room, new { Area = Const.MasterData });
    }
}
