using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.RolePages;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Classess
{
    public class RolePageService : IRolePageService
    {
        private readonly IRolePageRepository _rolePageRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPageRepository _pageRepository;
        private readonly ISubMenuRepository _subMenuRepository;

        public RolePageService(
            IRolePageRepository rolePageRepository,
            IRoleRepository roleRepository,
            IPageRepository pageRepository,
            ISubMenuRepository subMenuRepository
            )
        {
            _rolePageRepository = rolePageRepository;
            _roleRepository = roleRepository;
            _pageRepository = pageRepository;
            _subMenuRepository = subMenuRepository;
        }

        public async Task<int> Delete(MstRolePage model)
        {
            return await _rolePageRepository.Delete(model);
        }

        public async Task<IEnumerable<MstRolePage>> GetAll()
        {
            var results = await _rolePageRepository.GetAll();
            return results;
        }

        public async Task<IEnumerable<MstRolePage>> GetById(Guid id)
        {
            var results = await _rolePageRepository.GetById(id);
            return results;
        }

        public async Task<RolePageListViewModel> GetListRolePages(Guid id)
        {
            var rolePageListViewModel = new RolePageListViewModel();

            var roleResult = await _roleRepository.GetById(id);
            var rolePageResult = await _rolePageRepository.GetAll();
            var pageResult = await _pageRepository.GetAll();
            var subMenuResult = await _subMenuRepository.GetAll();

            var resultDetail = (from rolepage in rolePageResult
                                join role in roleResult on rolepage.RoleId equals role.RoleId
                                join page in pageResult on rolepage.PageId equals page.PageId
                                join subMenu in subMenuResult on page.SubmenuId equals subMenu.SubmenuId
                                where role.RoleId == id
                                select new RolePageListViewDetailModel
                                { 
                                    RolePageId = rolepage.RolePageId,
                                    PageId = page.PageId,
                                    PageName = page.Name,
                                    pagePath = page.Path,
                                    SubMenuId = subMenu.SubmenuId,
                                    SubMenuName = subMenu.Name,
                                    SubMenuPath = subMenu.Path
                                }
                );

            var resultPageSubMenu = (from page in pageResult
                                     join subMenu in subMenuResult on page.SubmenuId equals subMenu.SubmenuId
                                     select new RolePageListViewDetailPageSubMenuModel 
                                     { 
                                        PageId = page.PageId,
                                        PageName = page.Name,
                                        PagePath = page.Path,
                                        SubMenuId = subMenu.SubmenuId,
                                        SubMenuName = subMenu.Name,
                                        SubMenuPath = subMenu.Path
                                     }
                                     );

            rolePageListViewModel.role = roleResult.FirstOrDefault();
            rolePageListViewModel.page = pageResult.FirstOrDefault();
            rolePageListViewModel.rolePageSubMenu = resultPageSubMenu;
            rolePageListViewModel.rolePages = resultDetail;

            return rolePageListViewModel;
        }

        public async Task<int> Insert(MstRolePage model)
        {
            return await _rolePageRepository.Insert(model);
        }

        public async Task<int> Update(MstRolePage model)
        {
            return await _rolePageRepository.Update(model);
        }
    }
}
