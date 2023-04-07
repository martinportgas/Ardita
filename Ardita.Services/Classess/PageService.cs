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

        public PageService(
            IPageRepository pageRepository,
            ISubMenuRepository subMenuRepository
            )
        {
            _pageRepository = pageRepository;
            _subMenuRepository = subMenuRepository;
        }

        public async Task<int> Delete(MstPage model)
        {
            return await _pageRepository.Delete(model);
        }

        public async Task<IEnumerable<MstPage>> GetAll()
        {
            return await _pageRepository.GetAll();
        }

        public async Task<IEnumerable<MstPage>> GetById(Guid id)
        {
            return await _pageRepository.GetById(id);
        }

        public async Task<PageListViewModel> GetListPage(DataTableModel tableModel)
        {
            var pageListViewModel = new PageListViewModel();
            
            var pageResult = await _pageRepository.GetAll();
            var subMenuResult = await _subMenuRepository.GetAll();

            var results = (from page in pageResult
                           join subMenu in subMenuResult on page.SubmenuId equals subMenu.SubmenuId
                           select new PageListViewDetailModel
                           {
                              PageId = page.PageId,
                              PageName = page.Name,
                              PagePath = page.Path,
                              PageIsActive = page.IsActive,
                              SubMenuId = subMenu.SubmenuId,
                              SubMenuName = subMenu.Name,
                              SubMenuPath = subMenu.Path,
                              SubmIsActive = subMenu.IsActive
                           });

            if (!string.IsNullOrEmpty(tableModel.searchValue))
            {
                results = results.Where(
                    x => x.PageName.Contains(tableModel.searchValue)
                    || x.SubMenuName.Contains(tableModel.searchValue)
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

        public async Task<int> Update(MstPage model)
        {
            return await _pageRepository.Update(model);
        }
    }
}
