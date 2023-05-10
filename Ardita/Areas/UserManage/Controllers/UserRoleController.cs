using Ardita.Areas.UserManage.Models;
using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.UserRoles;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Atp;

namespace Ardita.Areas.UserManage.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(Const.UserManage)]
    public class UserRoleController : BaseController<IdxUserRole>
    {
        public UserRoleController(
            IUserService userService,
            IRoleService roleService,
            IUserRoleService userRoleService
            )
        {
            _userService = userService;
            _roleService = roleService;
            _userRoleService = userRoleService;
        }
        public override async Task<ActionResult> Index() => await base.Index();
        [HttpPost]
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {

            try
            {
                var result = await _userService.GetListUsers(model);
                return Json(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _userService.GetIdxUserRoleByUserId(Id);

            
            if (data.Count() > 0)
            {
                ViewBag.listRoles = await BindRoles();
                return View(data);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Save(IdxUserRole model)
        {
            Guid UserId = Guid.Empty;
            if (model != null)
            {
                UserId = model.UserId;
                model.UserRoleId = Guid.NewGuid();
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                await _userRoleService.Insert(model);

                ViewBag.listRoles = await BindRoles();
            }
            return RedirectToAction(Const.Detail, Const.UserRole, new { Area = Const.UserManage, Id = UserId });
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _userRoleService.GetById(Id);
            
            if (data != null)
            {
               
                await _userRoleService.Delete(data.FirstOrDefault());
                ViewBag.listRoles = await BindRoles();
            }
            return RedirectToAction(Const.Detail, Const.UserRole, new { Areas = Const.UserManage, Id = data.FirstOrDefault().UserId });
        }
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.UserRole, new { Area = Const.UserManage });
    }
}
