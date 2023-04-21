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
    public class FloorController : Controller
    {
        #region MEMBER AND CTR
        private readonly IFloorService _floorService;
        private readonly IArchiveUnitService _archiveUnitService;
        public FloorController(IFloorService floorService, IArchiveUnitService archiveUnitService)
        {
            _floorService = floorService;
            _archiveUnitService = archiveUnitService;
        }
        #endregion
        #region MAIN
        public IActionResult Index() => View();
        public async Task<JsonResult> GetData(DataTablePostModel model)
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
        public async Task<IActionResult> Add()
        {
            
            ViewBag.listArchiveUnits = await BindArchiveUnit();

            return View(Const.Form, new TrxFloor());
        }
        public async Task<IActionResult> Update(Guid Id)
        {
            var data = await _floorService.GetById(Id);
            if (data.Count() > 0)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnit();
                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _floorService.GetById(Id);
            if (data.Count() > 0)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnit();
                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _floorService.GetById(Id);
            if (data.Count() > 0)
            {
                ViewBag.listArchiveUnits = await BindArchiveUnit();
                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(TrxFloor model)
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
        public async Task<IActionResult> Delete(TrxFloor model)
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

        private async Task<List<SelectListItem>> BindArchiveUnit()
        {
            var archiveUnits = await _archiveUnitService.GetAll();

            return archiveUnits.Select(x => new SelectListItem
            {
                Value = x.ArchiveUnitId.ToString(),
                Text = x.ArchiveUnitName
            }).ToList();
        }
        #endregion

    }
}
