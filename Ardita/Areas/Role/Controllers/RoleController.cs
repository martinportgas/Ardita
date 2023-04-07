using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ardita.Services.Interfaces;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Models.DbModels;

namespace Ardita.Areas.Role.Controllers
{
    [Authorize]
    [Area("Role")]
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(MstRole model)
        {
            int result = 0;
            if (model != null)
            {

                if (model.RoleId != Guid.Empty)
                    result = await _roleService.Update(model);
                else
                    result = await _roleService.Insert(model);

            }
            return RedirectToAction("Index", "Role", new { Area = "Role" });
        }
    }
}
