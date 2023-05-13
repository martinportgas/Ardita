using Ardita.Controllers;
using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.UserManage.Controllers
{
    [CustomAuthorizeAttribute]
    [Area("UserManage")]
    public class RolePageController : BaseController<IdxRolePage>
    {
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
        public override async Task<ActionResult> Index() => await base.Index();
        [HttpPost]
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                var result = await _roleService.GetListRoles(model);

                return Json(result);

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
        public override async Task<IActionResult> Save(IdxRolePage model)
        {
            try {

                string urlRedirect = "";
                string msg = "";
                Guid roleId = Guid.Empty;
                if (Guid.TryParse(Request.Form["role"].FirstOrDefault(), out roleId))
                {
                    var listPageId = Request.Form["listPage[]"];
                    if (listPageId.Count > 0)
                    {
                        var delete = _rolePageService.DeleteByRoleId(roleId);

                        List<IdxRolePage> models = new();
                        IdxRolePage objRolePage;

                        for (int i = 0; i < listPageId.Count; i++)
                        {
                            Guid pageId = Guid.Empty;
                            if (Guid.TryParse(listPageId[i], out pageId))
                            {
                                var page = await _pageService.GetById(pageId);
                                if (page != null)
                                {
                                    objRolePage = new();
                                    objRolePage.RolePageId = Guid.NewGuid();
                                    objRolePage.RoleId = roleId;
                                    objRolePage.PageId = pageId;
                                    objRolePage.CreatedBy = AppUsers.CurrentUser(User).UserId;
                                    objRolePage.CreatedDate = DateTime.Now;

                                    models.Add(objRolePage);
                                }
                            }
                        }

                        await _rolePageService.InsertBulk(models);
                    }
                    msg = "Berhasil Tersimpan";
                }
                else
                {
                    msg = "Role Not Found";
                    urlRedirect = Url.Action(GlobalConst.Index, GlobalConst.RolePage, new { Area = GlobalConst.UserManage });
                }
                return Json(new { code = 200, msg = msg, redirect = urlRedirect });
            } catch (Exception ex) 
            {
                return Json(new { code = 400, msg = ex.Message, redirect = Url.Action(GlobalConst.Index, GlobalConst.RolePage, new { Area = GlobalConst.UserManage }) });
            }
        }

        [HttpGet]
        public override async Task<IActionResult> Detail(Guid id)
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
