﻿using Ardita.Models.DbModels;
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

            if (actionName.ToString() == "GetData")
                actionName = "Index";
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
            if (fullPath != "General/Home/Index")
            {
                var results = (from page in pages
                               join pageDetail in pageDetails on page.PageId equals pageDetail.PageId
                               join rolePage in rolePages on page.PageId equals rolePage.PageId
                               join role in roles on rolePage.RoleId equals role.RoleId
                               join subMenu in subMenus on page.SubmenuId equals subMenu.SubmenuId
                               join menu in menus on subMenu.MenuId equals menu.MenuId
                               where
                                    (pageDetail.Path == fullPath || (actionName == "Index" ? menu.Name == areaName && subMenu.Name == controllerName: false) ) &&
                                    role.Code == userRoleCode.ToString()
                               select new
                               {
                                   Page = pageDetail.Path
                               }
                );

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