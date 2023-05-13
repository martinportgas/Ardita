using Ardita.Areas.UserManage.Models;
using Ardita.Controllers;
using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.UserManage.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(GlobalConst.UserManage)]
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
                return View(GlobalConst.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }

        }

        public async Task<IActionResult> UpdateSubMenu(Guid Id) 
        {
            var data = await _subMenuService.GetById(Id);

            if (data != null)
            {
                return View(GlobalConst.SubMenuForm, data);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveSubMenu(MstSubmenu model)
        {
            if (model != null)
            {
                if (model.SubmenuId != Guid.Empty)
                {
                    model.UpdateBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdateDate = DateTime.Now;
                    await _subMenuService.Update(model);
                }
            }
            return RedirectToIndex();
        }
        public async Task<IActionResult> AddPage(Guid Id)
        {
            ViewBag.SubMenuId = Id;
            return View(GlobalConst.PageForm, new PageInsertViewModel());

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SavePage(PageInsertViewModel model)
        {
            int result = 0;
            if (model != null)
            {
                if (model.page.PageId != Guid.Empty)
                {
                    model.page.UpdateBy = AppUsers.CurrentUser(User).UserId;
                    model.page.UpdateDate = DateTime.Now;
                    result = await _pageService.Update(model.page);
                }
                else
                {
                    model.page.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.page.CreatedDate = DateTime.Now;
                    result = await _pageService.Insert(model.page);
                }

                var listPath = Request.Form["pagePath[]"];
                var listname = Request.Form["namePath[]"];

                if (listPath.Count > 0)
                {
                    result = await _pageService.DeleteDetail(model.page.PageId);

                    MstPageDetail objPageDetail;
                    for (int i = 0; i < listPath.Count; i++)
                    {
                        var path = listPath[i];
                        var name = listname[i];

                        if (!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(name))
                        {
                            objPageDetail = new();
                            objPageDetail.PageId = model.page.PageId;
                            objPageDetail.Path = path;
                            objPageDetail.Name = name;
                            objPageDetail.CreatedBy = AppUsers.CurrentUser(User).UserId;
                            objPageDetail.CreatedDate = DateTime.Now;

                            result = await _pageService.InsertDetail(objPageDetail);
                        }
                    }
                }
            }
            return RedirectToAction(GlobalConst.Detail, GlobalConst.Menu, new { Area = GlobalConst.UserManage, Id = model.page.SubmenuId });
        }

        public async Task<IActionResult> UpdatePage(Guid Id)
        {
            PageInsertViewModel model = new();
            model.pageDetail = await _pageService.GetDetailByMainId(Id);

            var pages = await _pageService.GetById(Id);
            if (pages!= null)
            {
                model.page = pages;
                return View(GlobalConst.PageUpdateForm, model);
            }
            else
            {
                return RedirectToAction(GlobalConst.Detail, GlobalConst.Menu, new { Area = GlobalConst.UserManage, Id = model.page.SubmenuId });
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _subMenuService.GetById(Id);
            ViewBag.TitlePageSubMenu = data.Name;
            ViewBag.SubMenuId = Id.ToString();

            HttpContext.Session.SetString(nameof(MstSubmenu.SubmenuId), Id.ToString());
            return View(GlobalConst.SubMenuPageDetail);
        }
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.Menu, new { Area = GlobalConst.UserManage });
    }
}
