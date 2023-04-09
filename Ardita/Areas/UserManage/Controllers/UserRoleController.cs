using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Ardita.Models.DbModels;
using Ardita.Areas.UserManage.Models;
using Ardita.Models.ViewModels.UserRoles;

namespace Ardita.Areas.UserManage.Controllers
{
    [Authorize]
    [Area("UserManage")]
    public class UserRoleController : Controller
    {
        private readonly IUserRoleService _userRoleService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        public UserRoleController(
            IUserRoleService userRoleService,
            IUserService userService,
            IRoleService roleService
            )
        {
            _userRoleService = userRoleService;
            _userService = userService;
            _roleService = roleService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetUsers()
        {
            try
            {
                var model = new DataTableModel();

                model.draw = Request.Form["draw"].FirstOrDefault();
                model.start = Request.Form["start"].FirstOrDefault();
                model.length = Request.Form["length"].FirstOrDefault();
                model.sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                model.sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                model.searchValue = Request.Form["search[value]"].FirstOrDefault();
                model.pageSize = model.length != null ? Convert.ToInt32(model.length) : 0;
                model.skip = model.start != null ? Convert.ToInt32(model.start) : 0;
                model.recordsTotal = 0;

                var result = await _userService.GetListUsers(model);

                var jsonResult = new
                {
                    draw = result.draw,
                    recordsFiltered = result.recordsFiltered,
                    recordsTotal = result.recordsTotal,
                    data = result.data
                };
                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(UserRoleListViewModel model)
        {
            
            int result = 0;
            if (model != null)
            {
                var objUserRole = new MstUserRole();
                objUserRole.UserId = model.UserRole.UserId;
                objUserRole.RoleId = model.UserRole.RoleId;
                objUserRole.CreatedBy = model.UserRole.UserId;
                objUserRole.CreatedDate = DateTime.Now;

                result = await _userRoleService.Insert(objUserRole);
            }
            return RedirectToAction("Detail", "UserRole", new { Area = "UserManage", id = model.UserRole.UserId });
        }
        public async Task<IActionResult> Detail(Guid id)
        {
            var userRoleDetails = await _userRoleService.GetListUserRoles(id);
            
            return View(userRoleDetails);
        }
        public async Task<IActionResult> Remove(Guid id)
        {
            var userRoleDetails = await _userRoleService.GetById(id);
            var userRoleObj = new MstUserRole();

            if (userRoleDetails.Count() > 0)
            {
                userRoleObj = userRoleDetails.FirstOrDefault();

                var delete = await _userRoleService.Delete(userRoleObj);
            }
            return RedirectToAction("Detail", "UserRole", new { Area = "UserManage", id=userRoleObj.UserId });
        }
    }
}
