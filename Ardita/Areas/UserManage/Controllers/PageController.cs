using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Ardita.Areas.UserManage.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ardita.Models.DbModels;
using Ardita.Services.Classess;
using Ardita.Controllers;
using Ardita.Globals;

namespace Ardita.Areas.UserManage.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(Const.UserManage)]
    public class PageController : BaseController<MstPage>
    {

        public PageController(
            IPageService pageService
            )
        {
            _pageService = pageService;
        }
        public override async Task<ActionResult> Index() => await base.Index();
        [HttpPost]
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {

            try
            {
                var result = await _pageService.GetListPage(model);
                return Json(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override async Task<IActionResult> Add()
        {
            return View(Const.Form);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(PageInsertViewModel model)
        {
            int result = 0;
            if (model != null)
            {
                if (model.page.PageId != Guid.Empty)
                {
                    model.page.UpdateBy = new Guid(User.FindFirst("UserId").Value);
                    model.page.UpdateDate = DateTime.Now;
                    result = await _pageService.Update(model.page);
                }
                else
                {
                    model.page.CreatedBy = new Guid(User.FindFirst("UserId").Value);
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

                        if(!string.IsNullOrEmpty(path) && !string.IsNullOrEmpty(name))
                        {
                            objPageDetail = new();
                            objPageDetail.PageId = model.page.PageId;
                            objPageDetail.Path = path;
                            objPageDetail.Name = name;
                            objPageDetail.CreatedBy = new Guid(User.FindFirst("UserId").Value);
                            objPageDetail.CreatedDate = DateTime.Now;

                            result = await _pageService.InsertDetail(objPageDetail);
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Page", new { Area = "UserManage" });
        }
        public async Task<IActionResult> Delete(PageInsertViewModel model)
        {
            int result = 0;
            if (model.page != null)
            {
                if (model.page.PageId != Guid.Empty)
                {
                    model.page.UpdateBy = new Guid(User.FindFirst("UserId").Value);
                    model.page.UpdateDate = DateTime.Now;
                    result = await _pageService.Delete(model.page);
                }
            }
            return RedirectToAction("Index", "Page", new { Area = "UserManage" });
        }

        private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.Page, new { Area = Const.UserManage });
        //public async Task<IActionResult> Update(Guid Id)
        //{
        //    PageInsertViewModel model = new();
        //    var MenuResult = await _menuService.GetMenuToLookUp();
        //    var subMenuResult = await _subMenuService.GetSubMenuTypeToLookUp();

        //    model.MenuTypes = MenuResult.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
        //    model.subMenuTypes = subMenuResult.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
        //    model.pageDetail = await _pageService.GetDetailByMainId(Id);

        //    var pages = await _pageService.GetById(Id);
        //    if (pages.Count() > 0)
        //    {
        //        model.page = pages.FirstOrDefault();
        //        return View(model);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Page", new { Area = "UserManage" });
        //    }
        //}
        //public async Task<IActionResult> Remove(Guid Id)
        //{
        //    PageInsertViewModel model = new();
        //    var MenuResult = await _menuService.GetMenuToLookUp();
        //    var subMenuResult = await _subMenuService.GetSubMenuTypeToLookUp();

        //    model.MenuTypes = MenuResult.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
        //    model.subMenuTypes = subMenuResult.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();

        //    var pages = await _pageService.GetById(Id);
        //    if (pages.Count() > 0)
        //    {
        //        model.page = pages.FirstOrDefault();
        //        return View(model);
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Page", new { Area = "UserManage" });
        //    }
        //}
    }
}
