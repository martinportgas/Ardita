using Ardita.Models;
using Ardita.Models.DbModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ardita.Views.ViewComponents
{
    public class MenuViewComponent : ViewComponent 
    {
        private readonly IUserService _userService;
        public MenuViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var role = claimsIdentity.FindFirst("RoleId").Value;

            var results = new MenuModel();
            

            Guid guid = new Guid(role);

            var getMenu = await _userService.GetUserMenu(guid);
            var menu = getMenu.GroupBy(x=> x.MenuId)
                .Select(x => new MstMenu { MenuId = x.Key, Name = x.FirstOrDefault().MenuName, Path = x.FirstOrDefault().MenuPath, Sort = x.FirstOrDefault().MenuSort })
                .OrderBy(x => x.Sort)
                .ToList();

            var getSubMenu = await _userService.GetUserMenu(guid);
            var subMenu = getSubMenu.GroupBy(g=>g.SubMenuId).Select(x => 
            new MstSubmenu { 
                SubmenuId = x.FirstOrDefault().SubMenuId, 
                Name = x.FirstOrDefault().SubMenuName,
                MenuId = x.FirstOrDefault().MenuId,
                Path = x.FirstOrDefault().SubMenuPath,
                Sort = x.FirstOrDefault().SubMenuSort
            }).OrderBy(x => x.Sort).ToList();


            results.menu = menu;
            results.subMenu = subMenu;

            return View(results);
        }
    }
}
