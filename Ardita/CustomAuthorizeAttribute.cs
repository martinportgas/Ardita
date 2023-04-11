using Ardita.Models.DbModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

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
            var rolePages = new List<MstRolePage>();
            var roles = new List<MstRole>();

            using (var dbContext = new BksArditaDevContext())
            {
                pages = dbContext.MstPages.ToList();
                rolePages = dbContext.MstRolePages.ToList();
                roles = dbContext.MstRoles.ToList();
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
                actionName = "View";
            else if (actionName.ToString() == "GetDataTreeView")
                actionName = "View";
            else if (actionName.ToString() == "Index")
                actionName = "View";
            else if (actionName.ToString() == "Add")
                actionName = "Create";
            else if (actionName.ToString() == "Save")
                actionName = "Create";
            else if (actionName.ToString() == "Edit")
                actionName = "Update";
            else if (actionName.ToString() == "Remove")
                actionName = "Delete";



            var fullPath = $"{areaName}/{controllerName}/{actionName}";

            if (fullPath != "General/Home/View") 
            {
                var results = (from page in pages
                               join rolePage in rolePages on page.PageId equals rolePage.PageId
                               join role in roles on rolePage.RoleId equals role.RoleId
                               where
                                    page.Path == fullPath &&
                                    role.Code == userRoleCode.ToString()
                               select new
                               {
                                   Page = page.Path
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