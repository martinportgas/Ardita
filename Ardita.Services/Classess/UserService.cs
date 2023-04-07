﻿using Ardita.Models.DbModels;
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

        public UserService(IUserRepository userRepository, 
            IRoleRepository roleRepository, 
            IUserRoleRepository userRoleRepository, 
            IEmployeeRepository employeeRepository,
            IPositionRepository positionRepository
            )
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _employeeRepository = employeeRepository;
            _positionRepository = positionRepository;
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
                        join position in positionResult on employee.PositionId equals position.PosittionId
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
                    x => x.UserName.Contains(tableModel.searchValue)
                    || x.EmployeeName.Contains(tableModel.searchValue)
                    || x.EmployeePosition.Contains(tableModel.searchValue)
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

            var result = (from usr in user
                          join ur in userRole on usr.UserId equals ur.UserId
                          join r in role on ur.RoleId equals r.RoleId
                          join e in employee on usr.EmployeeId equals e.EmployeeId
                          where usr.Username == username && usr.Password == password
                          select new
                          {
                              Username = usr.Username,
                              UserId = usr.UserId,
                              RoleId = r.RoleId,
                              RoleCode = r.Code,
                              RoleName = r.Name,
                              EmployeeNIK = e.Nik,
                              EmployeeName = e.Name
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
                    new Claim("RoleName", result.RoleName),
                    new Claim("EmployeeNIK", result.EmployeeNIK),
                    new Claim("EmployeeName", result.EmployeeName)
                };
            }
       
            return claims;
        }

        public async Task<int> Insert(MstUser model)
        {
            return await _userRepository.Insert(model);
        }

        public async Task<int> Update(MstUser model)
        {
            return await _userRepository.Update(model);
        }

        public void Upload(MstUser model)
        {
            _userRepository.Upload(model);
        }
    }
}
