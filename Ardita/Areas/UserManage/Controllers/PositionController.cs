using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ardita.Services.Classess;
using Ardita.Models.ViewModels;
using Ardita.Areas.UserManage.Models;
using Ardita.Models.DbModels;
using Ardita.Areas.UserManage.Models;
using Ardita.Globals;
using Ardita.Controllers;
using Ardita.Extensions;

namespace Ardita.Areas.UserManage.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(Const.UserManage)]
    public class PositionController : BaseController<MstPosition>
    {
        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }
        public override async Task<ActionResult> Index() => await base.Index();

        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {

            try
            {
                var result = await _positionService.GetListPositions(model);
                return Json(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override async Task<IActionResult> Add()
        {

            return View(Const.Form, new MstPosition());
        }

        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _positionService.GetById(Id);

            if (data != null)
            {
                return View(Const.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }

        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _positionService.GetById(Id);

            if (data != null)
            {
                return View(Const.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _positionService.GetById(Id);

            if (data != null)
            {
                return View(Const.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public  override async Task<IActionResult> Save(MstPosition model)
        {
            if (model != null)
            {

                if (model.PositionId != Guid.Empty)
                {
                    model.UpdateBy = AppUsers.CurrentUser(User).UserId.ToString();
                    model.UpdateDate = DateTime.Now;
                     await _positionService.Update(model);
                }
                else
                {
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId.ToString();
                    model.CreatedDate = DateTime.Now;
                    await _positionService.Insert(model);
                }

            }
            return RedirectToIndex();
        }

        public async Task<IActionResult> Delete(MstPosition model)
        {
            if (model != null)
            {

                if (model.PositionId != Guid.Empty)
                {

                    model.UpdateBy = AppUsers.CurrentUser(User).UserId.ToString();
                    model.UpdateDate = DateTime.Now;

                    await _positionService.Delete(model);
                }


            }
            return RedirectToIndex();
        }

        private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.Position, new { Area = Const.UserManage });
    }
}
