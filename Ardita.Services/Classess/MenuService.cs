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
        public MenuService(
            IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<int> Delete(MstMenu model)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MstMenu>> GetAll(string par = " 1=1 ")
        {
            return _menuRepository.GetAll(par);
        }

        public Task<MstMenu> GetById(Guid id)
        {
            return _menuRepository.GetById(id);
        }

        public async Task<DataTableResponseModel<MstMenu>> GetListMenu(DataTablePostModel model)
        {
            try
            {
                var dataCount = await _menuRepository.GetCount();

                var filterData = new DataTableModel();

                filterData.sortColumn = model.columns[model.order[0].column].data;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;

                var results = await _menuRepository.GetByFilterModel(filterData);

                var responseModel = new DataTableResponseModel<MstMenu>();

                responseModel.draw = model.draw;
                responseModel.recordsTotal = dataCount;
                responseModel.recordsFiltered = dataCount;
                responseModel.data = results.ToList();

                return responseModel;
            }
            catch (Exception ex)
            {
                return null;
            }
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
