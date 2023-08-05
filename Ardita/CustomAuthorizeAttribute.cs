using Ardita.Extensions;
using Ardita.Models.DbModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Ardita
{
    public class CustomAuthorizeAttribute : ActionFilterAttribute
    {

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {

        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var pages = new List<MstPage>();
            var pageDetails = new List<MstPageDetail>();
            var rolePages = new List<IdxRolePage>();
            var roles = new List<MstRole>();
            var subMenus = new List<MstSubmenu>();
            var menus = new List<MstMenu>();

            using (var dbContext = new BksArditaDevContext())
            {
                pages = dbContext.MstPages.ToList();
                pageDetails = dbContext.MstPageDetails.ToList();
                rolePages = dbContext.IdxRolePages.ToList();
                roles = dbContext.MstRoles.ToList();
                subMenus = dbContext.MstSubmenus.ToList();
                menus = dbContext.MstMenus.ToList();
            }

            var user = filterContext.HttpContext.User as System.Security.Claims.ClaimsPrincipal;
            if (!user.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(
                   new RouteValueDictionary {
                        { "controller", "Authentication" },
                        { "action", "Login" },
                        { "area", null}}
                   );
                return;
            }
            var userRoleCode = user.Identities.FirstOrDefault().FindFirst("RoleCode").Value;

            if (userRoleCode == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                   new RouteValueDictionary {
                        { "controller", "Error" },
                        { "action", "ErrorAuthorized" },
                        { "area", "General"} }
                   );
                return;
            }

            var areaName = filterContext.RouteData.Values["area"];
            var controllerName = filterContext.RouteData.Values["controller"];
            var actionName = filterContext.RouteData.Values["action"];

            /*
                Note:
                    Saat Index dipanggil, secara otomatis GetData juga dipanggil menggunakan Ajax
                    Maka Jika Actionnya GetData Then Action Name di Set jadi Index
             */

            try 
            {
                if (actionName.ToString() == GlobalConst.Index)
                {

                    var activity = new LogActivity();
                    activity.UserId = new Guid(user.Identities.FirstOrDefault().FindFirst(GlobalConst.UserId).Value);
                    activity.Username = user.Identities.FirstOrDefault().FindFirst(GlobalConst.Username).Value;
                    activity.ActivityDate = DateTime.Now;

                    if (controllerName.ToString() == GlobalConst.Home)
                    {
                        activity.PageName = GlobalConst.Home;
                    }
                    else
                    {
                        activity.PageId = subMenus.FirstOrDefault(x => x.Path == controllerName.ToString()).SubmenuId;
                        activity.PageName = subMenus.FirstOrDefault(x => x.Path == controllerName.ToString()).Name;
                    }


                    using (var dbContext = new BksArditaDevContext())
                    {
                        dbContext.Add(activity);
                        dbContext.SaveChanges();
                    }

                }

            }
            catch (Exception ex)
            { }

          

            if (actionName.ToString() == "GetData")
                actionName = GlobalConst.Index;
            else if (actionName.ToString().Contains("Bind"))
                actionName = "Index";
            else if (actionName.ToString() == "GetPageData")
                actionName = "Index";
            else if (actionName.ToString() == "DetailArchive")
                actionName = "Detail";

            //else if (actionName.ToString() == "GetDataTreeView")
            //    actionName = "View";
            //else if (actionName.ToString() == "Index")
            //    actionName = "View";
            //else if (actionName.ToString() == "Add")
            //    actionName = "Create";
            //else if (actionName.ToString() == "Save")
            //    actionName = "Create";
            //else if (actionName.ToString() == "Edit")
            //    actionName = "Update";
            //else if (actionName.ToString() == "Remove")
            //    actionName = "Delete";



            var fullPath = $"{areaName}/{controllerName}/{actionName}";
            if (areaName.ToString() != GlobalConst.General && !actionName.ToString().ToLower().Contains("approval"))
            {
                var results = (from page in pages
                               join pageDetail in pageDetails on page.PageId equals pageDetail.PageId
                               join rolePage in rolePages on page.PageId equals rolePage.PageId
                               join role in roles on rolePage.RoleId equals role.RoleId
                               join subMenu in subMenus on page.SubmenuId equals subMenu.SubmenuId
                               join menu in menus on subMenu.MenuId equals menu.MenuId
                               where
                                    menu.Path.ToLower() == areaName.ToString().ToLower() &&
                                    (subMenu.Path.ToLower() == controllerName.ToString().ToLower() || pageDetail.Path.ToLower().Contains(controllerName.ToString().ToLower())) &&
                                    role.Code == userRoleCode.ToString()
                               select new
                               {
                                   Page = pageDetail.Path
                               }
                );

                if(actionName.ToString() != "Index")
                {
                    results = results.Where(x => x.Page == fullPath);
                }

                if (results.Count() == 0)
                {
                    filterContext.Result = new RedirectToRouteResult(
                       new RouteValueDictionary {
                        { "controller", "Error" },
                        { "action", "ErrorAuthorized" },
                        { "area", "General"} }
                       );
                    return;
                }
            }
        }

    }
}