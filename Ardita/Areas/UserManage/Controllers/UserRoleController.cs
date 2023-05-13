using Ardita.Areas.UserManage.Models;
using Ardita.Controllers;
using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.UserRoles;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NPOI.SS.Formula.Atp;

namespace Ardita.Areas.UserManage.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(GlobalConst.UserManage)]
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
            var data = await _userService.GetById(Id);

            if (data != null)
            {
                ViewBag.listUserRole = await _userService.GetIdxUserRoleByUserId(Id);
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
            Guid RoleId = Guid.Empty;
            if (model != null)
            {
                Guid.TryParse(Request.Form["UserId"], out UserId);
                Guid.TryParse(Request.Form["RoleId"], out RoleId);

                model.UserRoleId = Guid.NewGuid();
                model.UserId = UserId;
                model.RoleId = RoleId;
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                await _userRoleService.Insert(model);

                ViewBag.listUserRole = await _userService.GetIdxUserRoleByUserId(UserId);
                ViewBag.listRoles = await BindRoles();
            }
            return RedirectToAction(GlobalConst.Detail, GlobalConst.UserRole, new { Area = GlobalConst.UserManage, Id = UserId });
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _userRoleService.GetById(Id);
            
            if (data != null)
            {
                await _userRoleService.Delete(data.FirstOrDefault());

                ViewBag.listUserRole = await _userService.GetIdxUserRoleByUserId(data.FirstOrDefault().UserId);
                ViewBag.listRoles = await BindRoles();
            }
            return RedirectToAction(GlobalConst.Detail, GlobalConst.UserRole, new { Areas = GlobalConst.UserManage, Id = data.FirstOrDefault().UserId });
        }
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.UserRole, new { Area = GlobalConst.UserManage });
    }
}
