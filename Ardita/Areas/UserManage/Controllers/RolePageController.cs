using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.UserManage.Controllers
{
    [CustomAuthorizeAttribute]
    [Area("UserManage")]
    public class RolePageController : Controller
    {
        private readonly IRolePageService _rolePageService;
        private readonly IRoleService _roleService;
        private readonly IPageService _pageService;
        public RolePageController(
            IRolePageService rolePageService,
            IRoleService roleService,
            IPageService pageService
            )
        {
            _rolePageService = rolePageService;
            _roleService = roleService;
            _pageService = pageService;
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
        public async Task<JsonResult> GetDataTreeView(Guid id)
        {
            try
            {
                var rolePageDetails = await _rolePageService.GetTreeRolePages(id);
                return Json(rolePageDetails);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Save()
        //{
        //    string urlRedirect = "";
        //    string msg = "";
        //    Guid roleId = Guid.Empty;
        //    if (Guid.TryParse(Request.Form["role"].FirstOrDefault(), out roleId))
        //    {
        //        var listPageId = Request.Form["listPage[]"];
        //        if (listPageId.Count > 0)
        //        {
        //            var delete = _rolePageService.DeleteByRoleId(roleId);

        //            List<IdxRolePage> models = new();
        //            IdxRolePage objRolePage;

        //            for (int i = 0; i < listPageId.Count; i++)
        //            {
        //                Guid pageId = Guid.Empty;
        //                if (Guid.TryParse(listPageId[i], out pageId))
        //                {
        //                    var page = await _pageService.GetById(pageId);
        //                    if (page.Count() > 0)
        //                    {
        //                        objRolePage = new();
        //                        objRolePage.RolePageId = Guid.NewGuid();
        //                        objRolePage.RoleId = roleId;
        //                        objRolePage.PageId = pageId;
        //                        objRolePage.CreatedBy = new Guid(User.FindFirst("UserId").Value);
        //                        objRolePage.CreatedDate = DateTime.Now;

        //                        models.Add(objRolePage);
        //                    }
        //                }
        //            }

        //            await _rolePageService.InsertBulk(models);
        //        }
        //        msg = "Berhasil Tersimpan";
        //    }
        //    else
        //    {
        //        msg = "Role Not Found";
        //        urlRedirect = Url.Action("Index", "RolePage", new { Area = "UserManage" });
        //    }
        //    return Json(new { code = 200, msg = msg, redirect = urlRedirect });
        //}
        public async Task<IActionResult> Detail(Guid id)
        {
            var rolePageDetails = await _rolePageService.GetListRolePages(id);
            return View(rolePageDetails);
        }
        public async Task<IActionResult> Remove(Guid id)
        {
            var rolePageDetails = await _rolePageService.GetById(id);
            var rolePageObj = new IdxRolePage();
            if (rolePageDetails.Count() > 0)
            {
                rolePageObj.RolePageId = rolePageDetails.FirstOrDefault().RolePageId;
                rolePageObj.RoleId = rolePageDetails.FirstOrDefault().RoleId;
                rolePageObj.PageId = rolePageDetails.FirstOrDefault().PageId;

                var delete = await _rolePageService.Delete(rolePageObj);
            }
            return RedirectToAction("Detail", "RolePage", new { Area = "UserManage", id = rolePageObj.RoleId });
        }
    }
}
