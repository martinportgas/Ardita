using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Ardita.Models.ViewModels.Users;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Classess
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly ISubMenuRepository _subMenuRepository;
        private readonly IPageRepository _pageRepository;
        private readonly IRolePageRepository _rolePageRepository;

        public UserService(IUserRepository userRepository, 
            IRoleRepository roleRepository, 
            IUserRoleRepository userRoleRepository, 
            IEmployeeRepository employeeRepository,
            IPositionRepository positionRepository,
            IMenuRepository menuRepository,
            ISubMenuRepository subMenuRepository,
            IPageRepository pageRepository,
            IRolePageRepository rolePageRepository
            )
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _employeeRepository = employeeRepository;
            _positionRepository = positionRepository;
            _menuRepository = menuRepository;
            _subMenuRepository = subMenuRepository;
            _pageRepository = pageRepository;
            _rolePageRepository = rolePageRepository;
        }
        public async Task<int> Delete(MstUser model)
        {
            return await _userRepository.Delete(model);
        }

        public async Task<IEnumerable<MstUser>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task<IEnumerable<MstUser>> GetById(Guid id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<UserListViewModel> GetListUsers(DataTableModel tableModel)
        {
            var userListViewModel = new UserListViewModel();
            var userResult = await _userRepository.GetAll();
            var employeeResult = await _employeeRepository.GetAll();
            var positionResult = await _positionRepository.GetAll();

            var results = (from user in userResult
                        join employee in employeeResult on user.EmployeeId equals employee.EmployeeId
                        join position in positionResult on employee.PositionId equals position.PositionId
                        select new UserListViewDetailModel
                        {
                            UserId = user.UserId,
                            UserName = user.Username,
                            EmployeeName = employee.Name,
                            EmployeePosition = position.Name,
                            IsActive = user.IsActive
                        });

            if (!string.IsNullOrEmpty(tableModel.searchValue))
            {
                results = results.Where(
                    x =>
                    (x.UserName != null ? x.UserName.ToUpper().Contains(tableModel.searchValue.ToUpper()) : false)
                    || (x.EmployeeName != null ? x.EmployeeName.ToUpper().Contains(tableModel.searchValue.ToUpper()) : false)
                    || (x.EmployeePosition != null ? x.EmployeePosition.ToUpper().Contains(tableModel.searchValue.ToUpper()) : false)
                );
            }

            tableModel.recordsTotal = results.Count();
            var data = results.Skip(tableModel.skip).Take(tableModel.pageSize).ToList();

            userListViewModel.draw = tableModel.draw;
            userListViewModel.recordsFiltered = tableModel.recordsTotal;
            userListViewModel.recordsTotal = tableModel.recordsTotal;
            userListViewModel.data = data;


            return userListViewModel;
        }

        public async Task<List<Claim>> GetLogin(string username, string password)
        {       
            var user = await _userRepository.GetAll();
            var role = await _roleRepository.GetAll();
            var userRole = await _userRoleRepository.GetAll();
            var employee = await _employeeRepository.GetAll();
            var position = await _positionRepository.GetAll();

            var result = (from usr in user
                          join ur in userRole on usr.UserId equals ur.UserId
                          join r in role on ur.RoleId equals r.RoleId
                          join e in employee on usr.EmployeeId equals e.EmployeeId
                          join p in position on e.PositionId equals p.PositionId
                          where usr.Username == username && usr.Password == password
                          && usr.IsActive == true && r.IsActive==true && e.IsActive==true
                          select new
                          {
                              Username = usr.Username,
                              UserId = usr.UserId,
                              RoleId = r.RoleId,
                              RoleCode = r.Code,
                              RoleName = r.Name,
                              EmployeeNIK = e.Nik,
                              EmployeeName = e.Name,
                              PositionId = p.PositionId,
                              CompanyId = e.CompanyId,
                              EmployeeId = e.EmployeeId,
                          }
                ).ToList().FirstOrDefault();

            List<Claim> claims = null;
            if (result != null)
            {
                claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Role, result.RoleCode),
                    new Claim("Username" ,result.Username),
                    new Claim("UserId" ,result.UserId.ToString()),
                    new Claim("RoleId" ,result.RoleId.ToString()),
                    new Claim("RoleCode" ,result.RoleCode.ToString()),
                    new Claim("RoleName", result.RoleName),
                    new Claim("EmployeeNIK", result.EmployeeNIK),
                    new Claim("EmployeeName", result.EmployeeName),
                    new Claim("PositionId", result.PositionId.ToString()),
                    new Claim("CompanyId", result.CompanyId.ToString()),
                    new Claim("EmployeeId", result.EmployeeId.ToString())
                };
            }
       
            return claims;
        }

        public async Task<List<UserMenuListViewModel>> GetUserMenu(Guid id)
        {
            var menuResults = await _menuRepository.GetAll();
            var subMenuResults = await _subMenuRepository.GetAll();
            var pageResults = await _pageRepository.GetAll();
            var rolePageResults = await _rolePageRepository.GetAll();
            var roleResults = await _roleRepository.GetAll();
        

            var results = (from menu in menuResults
                          join subMenu in subMenuResults on menu.MenuId equals subMenu.MenuId
                          join page in pageResults on subMenu.SubmenuId equals page.SubmenuId
                          join rolePage in rolePageResults on page.PageId equals rolePage.PageId
                          join role in roleResults on rolePage.RoleId equals role.RoleId
                          where role.RoleId == id && menu.IsActive==true && subMenu.IsActive==true
                          && page.IsActive==true && role.IsActive==true
                          select new UserMenuListViewModel
                          {
                             MenuId = menu.MenuId,
                             MenuName = menu.Name,
                             MenuPath = menu.Path,
                             MenuSort = menu.Sort,
                             SubMenuId = subMenu.SubmenuId,
                             SubMenuName = subMenu.Name,
                             SubMenuPath = subMenu.Path,
                             SubMenuSort = subMenu.Sort,
                             PageId = page.PageId,
                             PageName = page.Name,
                             PagePath = page.Path,
                             RoleId = role.RoleId,
                             RoleName = role.Name
                          }
                ).ToList();
            

            return results;
        }

        public async Task<int> Insert(MstUser model)
        {
            return await _userRepository.Insert(model);
        }

        public async Task<bool> InsertBulk(List<MstUser> users)
        {
            return await _userRepository.InsertBulk(users);
        }

        public async Task<int> Update(MstUser model)
        {
            return await _userRepository.Update(model);
        }
    }
}
