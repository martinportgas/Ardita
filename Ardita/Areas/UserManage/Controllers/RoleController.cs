using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ardita.Services.Interfaces;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Models.DbModels;
using Ardita.Areas.UserManage.Models;
using Ardita.Controllers;

using Ardita.Extensions;

namespace Ardita.Areas.UserManage.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(GlobalConst.UserManage)]
    public class RoleController : BaseController<MstRole>
    {
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public override async Task<ActionResult> Index() => await base.Index();
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                var result = await _roleService.GetListRoles(model);

                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override async Task<IActionResult> Add()
        {
            return View(GlobalConst.Form, new MstRole());
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            MstRole role = new();

            var roles = await _roleService.GetById(Id);
            if (roles != null)
            { 
                return View(GlobalConst.Form, roles);
            }
            else
            {
                return RedirectToIndex();
            }
        }

        public override async Task<IActionResult> Detail(Guid Id)
        {
            MstRole role = new();

            var roles = await _roleService.GetById(Id);
            if (roles != null)
            {
                return View(GlobalConst.Form, roles);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            MstRole role = new();

            var roles = await _roleService.GetById(Id);
            if (roles != null)
            {
                return View(GlobalConst.Form, roles);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Save(MstRole model)
        {
            int result = 0;
            if (model != null)
            {
                if (model.RoleId != Guid.Empty)
                {
                    model.UpdateBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdateDate = DateTime.Now;
                    result = await _roleService.Update(model);
                }
                else
                {
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;
                    result = await _roleService.Insert(model);
                }

            }
            return RedirectToIndex();
        }
        public override async Task<IActionResult> Delete(MstRole model)
        {
            int result = 0;
            if (model != null)
            {
                if (model.RoleId != Guid.Empty)
                {
                    model.UpdateBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdateDate = DateTime.Now;
                    result = await _roleService.Delete(model);
                }
            }
            return RedirectToIndex();
        }
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.Role, new { Area = GlobalConst.UserManage });
    }
}
