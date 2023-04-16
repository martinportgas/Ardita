using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Pages;
using Ardita.Models.ViewModels.Positions;
using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Classess
{
    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;
        private readonly ISubMenuRepository _subMenuRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IPageDetailRepository _pageDetailRepository;

        public PageService(
            IPageRepository pageRepository,
            ISubMenuRepository subMenuRepository,
            IMenuRepository menuRepository,
            IPageDetailRepository pageDetailRepository
            )
        {
            _pageRepository = pageRepository;
            _subMenuRepository = subMenuRepository;
            _menuRepository = menuRepository;
            _pageDetailRepository = pageDetailRepository;
        }

        public async Task<int> Delete(MstPage model)
        {
            return await _pageRepository.Delete(model);
        }
        public async Task<int> DeleteDetail(Guid id)
        {
            return await _pageDetailRepository.DeleteByMainId(id);
        }

        public async Task<IEnumerable<MstPage>> GetAll()
        {
            return await _pageRepository.GetAll();
        }

        public async Task<IEnumerable<MstPage>> GetById(Guid id)
        {
            return await _pageRepository.GetById(id);
        }
        public async Task<IEnumerable<MstPageDetail>> GetDetailByMainId(Guid id)
        {
            return await _pageDetailRepository.GetByMainId(id);
        }

        public async Task<PageListViewModel> GetListPage(DataTableModel tableModel)
        {
            var pageListViewModel = new PageListViewModel();
            
            var pageResult = await _pageRepository.GetAll();
            var subMenuResult = await _subMenuRepository.GetAll();
            var menuResult = await _menuRepository.GetAll();

            var results = (from page in pageResult
                           join subMenu in subMenuResult on page.SubmenuId equals subMenu.SubmenuId
                           join menu in menuResult on subMenu.MenuId equals menu.MenuId
                           select new PageListViewDetailModel
                           {
                              PageId = page.PageId,
                              PageName = page.Name,
                              PagePath = page.Path,
                              PageIsActive = page.IsActive,
                              SubMenuId = subMenu.SubmenuId,
                              SubMenuName = subMenu.Name,
                              SubMenuPath = subMenu.Path,
                              SubmIsActive = subMenu.IsActive,
                              MenuId = menu.MenuId,
                              MenuName = menu.Name,
                              MenuPath = menu.Path,
                           });

            if (!string.IsNullOrEmpty(tableModel.searchValue))
            {
                results = results.Where(
                    x =>
                     (x.PageName != null ? x.PageName.ToUpper().Contains(tableModel.searchValue.ToUpper()) : false)
                    || (x.SubMenuName != null ? x.SubMenuName.ToUpper().Contains(tableModel.searchValue.ToUpper()) : false)
                );
            }
            tableModel.recordsTotal = results.Count();
            var data = results.Skip(tableModel.skip).Take(tableModel.pageSize).ToList();

            pageListViewModel.draw = tableModel.draw;
            pageListViewModel.recordsFiltered = tableModel.recordsTotal;
            pageListViewModel.recordsTotal = tableModel.recordsTotal;
            pageListViewModel.data = data;

            return pageListViewModel;
        }

        public async Task<int> Insert(MstPage model)
        {
            return await _pageRepository.Insert(model);
        }
        public async Task<int> InsertDetail(MstPageDetail model)
        {
            return await _pageDetailRepository.Insert(model);
        }

        public async Task<int> Update(MstPage model)
        {
            return await _pageRepository.Update(model);
        }
    }
}
