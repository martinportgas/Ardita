using Ardita.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Ardita.Models.DbModels;
using Ardita.Services.Classess;

namespace Ardita.Areas.General.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(GlobalConst.General)]
    public class ChangeRoleController : Controller
    {
        protected IRoleService _roleService { get; set; }
        protected IUserRoleService _userRoleService { get; set; }
        public ChangeRoleController(
            IRoleService roleService,
            IUserRoleService userRoleService
           )
        {
            _roleService = roleService;
            _userRoleService = userRoleService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                model.SessionUser = User;
                var result = await _userRoleService.GetList(model);

                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<IActionResult> ChangeRole(Guid Id)
        {
            var roleData = await _userRoleService.GetById(Id);

            TempData[GlobalConst.Notification] = GlobalConst.Failed;

            if (roleData != null)
            {
                SessionModel user = AppUsers.CurrentUser(User);

                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Role, roleData.Role.Code),
                    new Claim(GlobalConst.Username ,user.Username),
                    new Claim(GlobalConst.UserId ,user.UserId.ToString()),
                    new Claim(GlobalConst.RoleId ,roleData.RoleId.ToString()),
                    new Claim(GlobalConst.RoleCode ,roleData.Role.Code.ToString()),
                    new Claim(GlobalConst.RoleName, roleData.Role.Name),
                    new Claim(GlobalConst.EmployeeNIK, user.EmployeeNIK),
                    new Claim(GlobalConst.EmployeeName, user.EmployeeName),
                    new Claim(GlobalConst.EmployeeMail, string.IsNullOrEmpty(user.EmployeeMail) ? string.Empty : user.EmployeeMail),
                    new Claim(GlobalConst.EmployeePhone, string.IsNullOrEmpty(user.EmployeePhone) ? string.Empty : user.EmployeePhone),
                    new Claim(GlobalConst.PositionId, user.PositionId.ToString()),
                    new Claim(GlobalConst.CompanyId, user.CompanyId.ToString()!),
                    new Claim(GlobalConst.CompanyName, user.CompanyName),
                    new Claim(GlobalConst.EmployeeId, user.EmployeeId.ToString()),
                    new Claim(GlobalConst.ArchiveUnitId, roleData.ArchiveUnitId?.ToString() ?? string.Empty),
                    new Claim(GlobalConst.ArchiveUnitName, roleData.ArchiveUnit?.ArchiveUnitName ?? string.Empty),
                    new Claim(GlobalConst.CreatorId, roleData.CreatorId?.ToString() ?? string.Empty),
                    new Claim(GlobalConst.CreatorName, roleData.Creator?.CreatorName ?? string.Empty),
                    new Claim(GlobalConst.ArchiveUnitCode, user.ListArchiveUnitCode.Count > 0 ? string.Join(",", user.ListArchiveUnitCode.ToArray()) : string.Empty)
                };

                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties authenticationProperties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), authenticationProperties);

                TempData[GlobalConst.Notification] = GlobalConst.Success;
            }

            return RedirectToAction(GlobalConst.Index, GlobalConst.ChangeRole, new { area = GlobalConst.General });
        }
    }
}
