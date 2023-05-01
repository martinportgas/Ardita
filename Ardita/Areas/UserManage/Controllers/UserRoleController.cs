using Ardita.Controllers;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.UserRoles;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.UserManage.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(Const.UserManage)]
    public class UserRoleController : BaseController<MstUser>
    {
        public UserRoleController(
            IUserService userService
            )
        {
            _userService = userService;
        }
        public override async Task<ActionResult> Index() => await base.Index();
        [HttpPost]
        //public async Task<JsonResult> GetData()
        //{
        //    try
        //    {
        //        var model = new DataTableModel();

        //        model.draw = Request.Form["draw"].FirstOrDefault();
        //        model.start = Request.Form["start"].FirstOrDefault();
        //        model.length = Request.Form["length"].FirstOrDefault();
        //        model.sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
        //        model.sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
        //        model.searchValue = Request.Form["search[value]"].FirstOrDefault();
        //        model.pageSize = model.length != null ? Convert.ToInt32(model.length) : 0;
        //        model.skip = model.start != null ? Convert.ToInt32(model.start) : 0;
        //        model.recordsTotal = 0;

        //        var result = await _userService.GetListUsers(model);

        //        var jsonResult = new
        //        {
        //            draw = result.draw,
        //            recordsFiltered = result.recordsFiltered,
        //            recordsTotal = result.recordsTotal,
        //            data = result.data
        //        };
        //        return Json(jsonResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Save(UserRoleListViewModel model)
        //{

        //    int result = 0;
        //    if (model != null)
        //    {
        //        var objUserRole = new IdxUserRole();
        //        objUserRole.UserId = model.UserRole.UserId;
        //        objUserRole.RoleId = model.UserRole.RoleId;
        //        objUserRole.CreatedBy = model.UserRole.UserId;
        //        objUserRole.CreatedDate = DateTime.Now;

        //        result = await _userRoleService.Insert(objUserRole);
        //    }
        //    return RedirectToIndex();
        //}
        //public async Task<IActionResult> Detail(Guid id)
        //{
        //    var userRoleDetails = await _userRoleService.GetListUserRoles(id);

        //    return View(userRoleDetails);
        //}
        //public async Task<IActionResult> Remove(Guid id)
        //{
        //    var userRoleDetails = await _userRoleService.GetById(id);
        //    var userRoleObj = new IdxUserRole();

        //    if (userRoleDetails.Count() > 0)
        //    {
        //        userRoleObj = userRoleDetails.FirstOrDefault();

        //        var delete = await _userRoleService.Delete(userRoleObj);
        //    }
        //    return RedirectToIndex();
        //}
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.User, new { Area = Const.UserManage });
    }
}
