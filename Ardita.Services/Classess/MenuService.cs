using Ardita.Models.DbModels;
using Ardita.Models.ViewModels.Pages;
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
        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
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
