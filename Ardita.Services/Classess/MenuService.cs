using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Menus;
using Ardita.Models.ViewModels.Pages;
using Ardita.Models.ViewModels.Positions;
using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Classess
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly ISubMenuRepository _subMenuRepository;
        public MenuService(
            IMenuRepository menuRepository,
            ISubMenuRepository subMenuRepository)
        {
            _menuRepository = menuRepository;
            _subMenuRepository = subMenuRepository;
        }

        public async Task<int> Delete(MstMenu model)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MstMenu>> GetAll()
        {
            return _menuRepository.GetAll();
        }

        public Task<IEnumerable<MstMenu>> GetById(Guid id)
        {
            return _menuRepository.GetById(id);
        }

        public async Task<MenuListViewModel> GetListMenuWithSubMenu(DataTableModel tableModel)
        {
            var menuListViewModel = new MenuListViewModel();

            var menuResult = await _menuRepository.GetAll();
            var SubMenuResult = await _subMenuRepository.GetAll();

            var results = (from menu in menuResult
                           join subMenu in SubMenuResult on menu.MenuId equals subMenu.MenuId
                           select new MenuListViewDetailModel
                           {
                               MenuId = menu.MenuId,
                               SubMenuId = subMenu.MenuId,
                               SubMenuName = subMenu.Name,
                               SubMenuPath = subMenu.Path,
                               SubMenuSort = subMenu.Sort
                           });
            //if (!(string.IsNullOrEmpty(tableModel.sortColumn) && string.IsNullOrEmpty(tableModel.sortColumnDirection)))
            //{
            //    results = results.OrderBy(tableModel.sortColumn + " " + tableModel.sortColumnDirection);
            //}

            if (!string.IsNullOrEmpty(tableModel.searchValue))
            {
                results = results.Where(
                    x => x.SubMenuName.ToUpper().Contains(tableModel.searchValue.ToUpper())
                );
            }

            tableModel.recordsTotal = results.Count();
            var data = results.Skip(tableModel.skip).Take(tableModel.pageSize).ToList();

            menuListViewModel.draw = tableModel.draw;
            menuListViewModel.recordsFiltered = tableModel.recordsTotal;
            menuListViewModel.recordsTotal = tableModel.recordsTotal;
            menuListViewModel.data = data;

            return menuListViewModel;
        }

        public async Task<List<MenuTypes>> GetMenuToLookUp()
        {
            List<MenuTypes> ListResult = new List<MenuTypes>();
            MenuTypes menu;
            var model = await _menuRepository.GetAll();
            foreach (var item in model)
            {
                menu = new MenuTypes();
                menu.Id = item.MenuId;
                menu.Name = item.Name;

                ListResult.Add(menu);
            }
            return ListResult;
        }

        public Task<int> Insert(MstMenu model)
        {
            return _menuRepository.Insert(model);
        }

        public Task<int> Update(MstMenu model)
        {
            return _menuRepository.Update(model);
        }
    }
}
