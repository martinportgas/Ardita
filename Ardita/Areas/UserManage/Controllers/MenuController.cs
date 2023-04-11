using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.UserManage.Controllers
{
    [CustomAuthorizeAttribute]
    [Area("UserManage")]
    public class MenuController : Controller
    {
        private readonly IMenuService _menuService;
        private readonly ISubMenuService _subMenu;

        public MenuController(
            IMenuService menuService,
            ISubMenuService subMenuService)
        {
            _menuService = menuService;
            _subMenu = subMenuService;
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

                var result = await _menuService.GetListMenuWithSubMenu(model);

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
        public async Task<IActionResult> Update(Guid Id)
        {
            var data = await _subMenu.GetById(Id);
            if (data.Count() > 0)
            {
                var model = new MstSubmenu();
                model.SubmenuId = data.FirstOrDefault().SubmenuId;
                model.Name = data.FirstOrDefault().Name;
                model.Path = data.FirstOrDefault().Path;
                model.Sort = data.FirstOrDefault().Sort;

                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Menu", new { Area = "UserManage" });
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(MstSubmenu model)
        {
            int result = 0;
            if (model != null)
            {
                if (model.SubmenuId != Guid.Empty)
                {
                    model.UpdateBy = new Guid(User.FindFirst("UserId").Value);
                    model.UpdateDate = DateTime.Now;
                    result = await _subMenu.Update(model);
                }
            }
            return RedirectToAction("Index", "Menu", new { Area = "UserManage" });
        }
    }
}
