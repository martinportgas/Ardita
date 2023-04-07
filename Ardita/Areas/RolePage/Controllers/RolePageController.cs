using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ardita.Services.Interfaces;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels.UserRoles;
using Ardita.Models.ViewModels.RolePages;

namespace Ardita.Areas.RolePage.Controllers
{
    [Authorize]
    [Area("RolePage")]
    public class RolePageController : Controller
    {
        private readonly IRolePageService _rolePageService;
        private readonly IRoleService _roleService;
        public RolePageController(
            IRolePageService rolePageService,
            IRoleService roleService
            )
        {
            _rolePageService = rolePageService;
            _roleService = roleService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetData()
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

                var result = await _roleService.GetListRole(model);

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
        public async Task<IActionResult> Save(RolePageListViewModel model)
        {

            int result = 0;
            if (model != null)
            {
                var objRolePage = new MstRolePage();
                objRolePage.RolePageId = Guid.NewGuid();
                objRolePage.RoleId = model.rolePage.RoleId;
                objRolePage.PageId = model.rolePage.PageId;
                objRolePage.CreatedBy = new Guid(User.FindFirst("UserId").Value);
                objRolePage.CreatedDate = DateTime.Now;

                result = await _rolePageService.Insert(objRolePage);
            }
            return RedirectToAction("Detail", "RolePage", new { Area = "RolePage", id = model.rolePage.RoleId });
        }
        public async Task<IActionResult> Detail(Guid id)
        {
            var rolePageDetails = await _rolePageService.GetListRolePages(id);
            return View(rolePageDetails);
        }
        public async Task<IActionResult> Remove(Guid id)
        {
            var rolePageDetails = await _rolePageService.GetById(id);
            var rolePageObj = new MstRolePage();
            if (rolePageDetails.Count() > 0)
            {
                rolePageObj.RolePageId = rolePageDetails.FirstOrDefault().RolePageId;
                rolePageObj.RoleId = rolePageDetails.FirstOrDefault().RoleId;
                rolePageObj.PageId = rolePageDetails.FirstOrDefault().PageId;

                var delete = await _rolePageService.Delete(rolePageObj);
            }
            return RedirectToAction("Detail", "RolePage", new { Area = "RolePage", id = rolePageObj.RoleId });
        }
    }
}
