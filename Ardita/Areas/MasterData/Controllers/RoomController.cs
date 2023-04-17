using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area("MasterData")]
    public class RoomController : Controller
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }
        public IActionResult Index()
        {
            return View();
        }
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
            //var classificationTypeData = await _classificationTypeService.GetAll();

            //ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
            return View();
        }
        public async Task<IActionResult> Update(Guid Id)
        {
            var data = await _roomService.GetById(Id);
            if (data.Count() > 0)
            {
                //var classificationTypeData = await _classificationTypeService.GetAll();

                //ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
                return View(data.FirstOrDefault());
            }
            else
            {
                return RedirectToAction("Index", "Room", new { Area = "MasterData" });
            }
        }
        public async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _roomService.GetById(Id);
            if (data.Count() > 0)
            {
                //var classificationTypeData = await _classificationTypeService.GetAll();

                //ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
                return View(data.FirstOrDefault());
            }
            else
            {
                return RedirectToAction("Index", "Room", new { Area = "MasterData" });
            }
        }
        public async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _roomService.GetById(Id);
            if (data.Count() > 0)
            {
                //var classificationTypeData = await _classificationTypeService.GetAll();

                //ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
                return View(data.FirstOrDefault());
            }
            else
            {
                return RedirectToAction("Index", "Room", new { Area = "MasterData" });
            }
        }

        // POST: FloorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(TrxRoom model)
        {
            int result = 0;
            if (model != null)
            {
                if (model.RoomId != Guid.Empty)
                {
                    model.ArchiveUnitId = new Guid("B0ADFB41-1516-4145-8623-D0118FBE646D");
                    model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdatedDate = DateTime.Now;
                    result = await _roomService.Update(model);
                }

                else
                {
                    model.ArchiveUnitId = new Guid("B0ADFB41-1516-4145-8623-D0118FBE646D");
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;
                    result = await _roomService.Insert(model);
                }
            }
            return RedirectToAction("Index", "Room", new { Area = "MasterData" });
        }

        // POST: FloorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(TrxRoom model)
        {
            int result = 0;
            if (model != null && model.RoomId != Guid.Empty)
            {
                result = await _roomService.Delete(model);
            }
            return RedirectToAction("Index", "Room", new { Area = "MasterData" });
        }
    }
}
