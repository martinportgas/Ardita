using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.UserManage.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(Const.UserManage)]
    public class MenuController : BaseController<MstMenu>
    {
        public MenuController(
            IMenuService menuService,
            ISubMenuService subMenuService,
            IPageService pageService
            
            )
        {
            _menuService = menuService;
            _subMenuService = subMenuService;
            _pageService = pageService;
        }

        public override async Task<ActionResult> Index() => await base.Index();

        [HttpPost]
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                var result = await _menuService.GetListMenu(model);
                return Json(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<JsonResult> GetPageData(DataTablePostModel model)
        {
            try
            {
                model.SubMenuId = new Guid(HttpContext.Session.GetString(nameof(MstSubmenu.SubmenuId)));
                var result = await _pageService.GetListPage(model);
                HttpContext.Session.Remove(nameof(MstSubmenu.SubmenuId));

                return Json(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _menuService.GetById(Id);
            if (data != null)
            {
                return View(Const.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Save(MstMenu model)
        {
            int result = 0;
            if (model != null)
            {
                if (model.MenuId != Guid.Empty)
                {
                    model.UpdateBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdateDate = DateTime.Now;
                    result = await _menuService.Update(model);
                }
            }
            return RedirectToIndex();
        }
    
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _subMenuService.GetById(Id);
            ViewBag.TitlePageSubMenu = data.Name;

            HttpContext.Session.SetString(nameof(MstSubmenu.SubmenuId), Id.ToString());
            return View("SubMenuPageDetail");
        }
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.Menu, new { Area = Const.UserManage });
    }
}
