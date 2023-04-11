using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.RolePages;
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
    public class RolePageService : IRolePageService
    {
        private readonly IRolePageRepository _rolePageRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPageRepository _pageRepository;
        private readonly ISubMenuRepository _subMenuRepository;
        private readonly IMenuRepository _menuRepository;

        public RolePageService(
            IRolePageRepository rolePageRepository,
            IRoleRepository roleRepository,
            IPageRepository pageRepository,
            ISubMenuRepository subMenuRepository,
            IMenuRepository menuRepository
            )
        {
            _rolePageRepository = rolePageRepository;
            _roleRepository = roleRepository;
            _pageRepository = pageRepository;
            _subMenuRepository = subMenuRepository;
            _menuRepository = menuRepository;
        }

        public async Task<int> Delete(MstRolePage model)
        {
            return await _rolePageRepository.Delete(model);
        }
        public async Task<int> DeleteByRoleId(Guid roleId)
        {
            return await _rolePageRepository.DeleteByRoleId(roleId);
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
        public async Task<IEnumerable<RolePageTreeViewModel>> GetTreeRolePages(Guid id)
        {
            var rolePageTreeViewModel = new RolePageTreeViewModel();

            var rolePageResult = await _rolePageRepository.GetAll();
            var pageResult = await _pageRepository.GetAll();
            var subMenuResult = await _subMenuRepository.GetAll();
            var menuResult = await _menuRepository.GetAll();

            var resultMenu = (from menu in menuResult
                              where menu.IsActive = true && menu.Path != "General"
                                select new RolePageTreeViewModel
                                {
                                    id = menu.MenuId.ToString(),
                                    parent = "#",
                                    text = menu.Name,
                                    state = new RolePageTreeViewStateModel
                                    {
                                        opened = false,
                                        selected = false,
                                    }
                                }
                );

            var resultSubMenu = (from subMenu in subMenuResult
                                 where subMenu.IsActive = true
                                 select new RolePageTreeViewModel
                                 {
                                     id = subMenu.SubmenuId.ToString(),
                                     parent = subMenu.MenuId.ToString(),
                                     text = subMenu.Name,
                                     state = new RolePageTreeViewStateModel
                                     {
                                         opened = false,
                                         selected = false,
                                     }
                                 }
                );

            var resultPage = (from page in pageResult
                              where page.IsActive = true
                              select new RolePageTreeViewModel
                              {
                                  id = page.PageId.ToString(),
                                  parent = page.SubmenuId.ToString(),
                                  text = page.Name,
                                  state = new RolePageTreeViewStateModel
                                  {
                                      opened = false,
                                      selected = (from rolepage in rolePageResult
                                                  where rolepage.RoleId == id && rolepage.PageId == page.PageId select rolepage).Count() > 0,
                                  }
                              }
                );

            return resultMenu.Union(resultSubMenu).Union(resultPage);
        }

        public async Task<int> Insert(MstRolePage model)
        {
            return await _rolePageRepository.Insert(model);
        }
        public async Task<bool> InsertBulk(List<MstRolePage> model)
        {
            return await _rolePageRepository.InsertBulk(model);
        }

        public async Task<int> Update(MstRolePage model)
        {
            return await _rolePageRepository.Update(model);
        }
    }
}
