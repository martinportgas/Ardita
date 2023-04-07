using Ardita.Models;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Views.ViewComponents
{
    public class MenuViewComponent : ViewComponent 
    {
        private readonly IMenuService _menuService;
        private readonly ISubMenuService _subMenuService;
        public MenuViewComponent(IMenuService menuService,
            ISubMenuService subMenuService)
        {

            _menuService = menuService;
            _subMenuService = subMenuService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        { 
            var results = new MenuModel();

            var menu = await _menuService.GetAll();
            var subMenu = await _subMenuService.GetAll();

            results.menu = menu;
            results.subMenu = subMenu;

            return View(results);
        }
    }
}
