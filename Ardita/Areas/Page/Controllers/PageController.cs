using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Ardita.Areas.Page.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ardita.Models.DbModels;
using Ardita.Services.Classess;

namespace Ardita.Areas.Page.Controllers
{
    [Authorize]
    [Area("Page")]
    public class PageController : Controller
    {
        private readonly IPageService _pageService;
        private readonly IMenuService _menuService;
        private readonly ISubMenuService _subMenuService;

        public PageController(
            IPageService pageService,
            ISubMenuService subMenuService,
            IMenuService menuService
            )
        {
            _pageService = pageService;
            _menuService = menuService;
            _subMenuService = subMenuService;
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

                var result = await _pageService.GetListPage(model);

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
        public async Task<IActionResult> Add()
        {

            var MenuResult = await _menuService.GetMenuToLookUp();
            var subMenuResult = await _subMenuService.GetSubMenuTypeToLookUp();
            var viewModel = new PageInsertViewModel();
            viewModel.MenuTypes = MenuResult.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
            viewModel.subMenuTypes = subMenuResult.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(PageInsertViewModel model)
        {
            int result = 0;
            if (model.page != null)
            {
                model.page.CreatedBy = new Guid(User.FindFirst("UserId").Value);
                model.page.CreatedDate = DateTime.Now;
                result = await _pageService.Insert(model.page);
            }
            return RedirectToAction("Index", "Page", new { Area = "Page" });
        }
    }
}
