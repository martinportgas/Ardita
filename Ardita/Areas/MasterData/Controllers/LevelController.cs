using Ardita.Controllers;
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

        private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.Level, new { Area = Const.MasterData });

    }
}
