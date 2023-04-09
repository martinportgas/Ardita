using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ardita.Services.Interfaces;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Models.DbModels;
using Ardita.Areas.UserManage.Models;

namespace Ardita.Areas.UserManage.Controllers
{
    [Authorize]
    [Area("UserManage")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
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
        public IActionResult Add()
        {
            return View();
        }
        public async Task<IActionResult> Update(Guid Id)
        {
            MstRole role = new();

            var roles = await _roleService.GetById(Id);
            if (roles.Count() > 0)
            {
                role = roles.FirstOrDefault();
                return View(role);
            }
            else
            {
                return RedirectToAction("Index", "Role", new { Area = "UserManage" });
            }
        }
        public async Task<IActionResult> Remove(Guid Id)
        {
            MstRole role = new();

            var roles = await _roleService.GetById(Id);
            if (roles.Count() > 0)
            {
                role = roles.FirstOrDefault();
                return View(role);
            }
            else
            {
                return RedirectToAction("Index", "Role", new { Area = "UserManage" });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(MstRole model)
        {
            int result = 0;
            if (model != null)
            {
                if (model.RoleId != Guid.Empty)
                {
                    model.UpdateBy = new Guid(User.FindFirst("UserId").Value);
                    model.UpdateDate = DateTime.Now;
                    result = await _roleService.Update(model);
                }
                else
                {
                    model.CreatedBy = new Guid(User.FindFirst("UserId").Value);
                    model.CreatedDate = DateTime.Now;
                    result = await _roleService.Insert(model);
                }

            }
            return RedirectToAction("Index", "Role", new { Area = "UserManage" });
        }
        public async Task<IActionResult> Delete(MstRole model)
        {
            int result = 0;
            if (model != null)
            {
                if (model.RoleId != Guid.Empty)
                {
                    model.UpdateBy = new Guid(User.FindFirst("UserId").Value);
                    model.UpdateDate = DateTime.Now;
                    result = await _roleService.Delete(model);
                }
            }
            return RedirectToAction("Index", "Role", new { Area = "UserManage" });
        }
    }
}
