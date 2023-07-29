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
            IUserRoleService userRoleService,
            IArchiveUnitService archiveUnitService,
            IArchiveCreatorService archiveCreatorService
            )
        {
            _userService = userService;
            _roleService = roleService;
            _userRoleService = userRoleService;
            _archiveUnitService = archiveUnitService;
            _archiveCreatorService = archiveCreatorService;
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
                ViewBag.listUserRole = await _userRoleService.GetIdxUserRoleByUserId(Id);
                ViewBag.listRoles = await BindRoles();
                ViewBag.listArchiveUnit = await BindArchiveUnits();
                ViewBag.listCreator = await BindArchiveCreators();
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
            Guid ArchiveUnitId;
            Guid CreatorId;
            if (model != null)
            {
                Guid.TryParse(Request.Form["UserId"], out UserId);
                Guid.TryParse(Request.Form["RoleId"], out RoleId);
                Guid.TryParse(Request.Form["ArchiveUnitId"], out ArchiveUnitId);
                Guid.TryParse(Request.Form["CreatorId"], out CreatorId);
                bool IsPrimary = Request.Form["IsPrimary"] == GlobalConst.Yes;

                int countPrimary = await _userRoleService.GetCountIsPrimaryByUserId(UserId);

                var data = await _userRoleService.GetByUserAndRoleId(UserId, RoleId, ArchiveUnitId, CreatorId);
                bool isNew = data == null;

                var listUserRole = await _userRoleService.GetIdxUserRoleByUserId(UserId);
                var count = isNew ? 0 : 1;
                if (listUserRole.Count() > count)
                {
                    if(isNew && IsPrimary || !isNew)
                    {
                        foreach (IdxUserRole item in listUserRole)
                        {
                            item.IsPrimary = !IsPrimary;
                            if (item.RoleId == RoleId)
                                item.IsPrimary = IsPrimary;
                            await _userRoleService.Update(item);

                            if (!IsPrimary)
                                break;
                        }
                    }
                }

                if (isNew)
                {
                    model.UserRoleId = Guid.NewGuid();
                    model.UserId = UserId;
                    model.RoleId = RoleId;
                    model.ArchiveUnitId = ArchiveUnitId;
                    model.CreatorId = CreatorId;
                    model.IsPrimary = IsPrimary || countPrimary == 0;
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;
                    await _userRoleService.Insert(model);
                }

                ViewBag.listUserRole = await _userRoleService.GetIdxUserRoleByUserId(UserId);
                ViewBag.listRoles = await BindRoles();
            }
            return RedirectToAction(GlobalConst.Detail, GlobalConst.UserRole, new { Area = GlobalConst.UserManage, Id = UserId });
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _userRoleService.GetById(Id);
            
            if (data != null)
            {
                await _userRoleService.Delete(data);

                int countPrimary = await _userRoleService.GetCountIsPrimaryByUserId(data.UserId);

                var dataUserRole = await _userRoleService.GetIdxUserRoleByUserId(data.UserId);

                if(countPrimary == 0)
                {
                    if(dataUserRole.Count() > 0)
                    {
                        foreach (IdxUserRole item in dataUserRole)
                        {
                            item.IsPrimary = true;
                            await _userRoleService.Update(item);
                            break;
                        }
                    }
                }

                ViewBag.listUserRole = dataUserRole;
                ViewBag.listRoles = await BindRoles();
            }
            return RedirectToAction(GlobalConst.Detail, GlobalConst.UserRole, new { Areas = GlobalConst.UserManage, Id = data.UserId });
        }
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.UserRole, new { Area = GlobalConst.UserManage });
    }
}
