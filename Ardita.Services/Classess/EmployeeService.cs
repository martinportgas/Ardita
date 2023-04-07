﻿using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Employees;
using Ardita.Models.ViewModels.Users;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Classess
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPositionRepository _positionRepository;
        public EmployeeService(IEmployeeRepository employeeRepository, IPositionRepository positionRepository)
        {
            _employeeRepository = employeeRepository;
            _positionRepository = positionRepository;
        }

        public async Task<int> Delete(MstEmployee model)
        {
            return await _employeeRepository.Delete(model);
        }

        public async Task<IEnumerable<MstEmployee>> GetAll()
        {
            return await _employeeRepository.GetAll();
        }

        public async Task<IEnumerable<MstEmployee>> GetById(Guid id)
        {
            return await _employeeRepository.GetById(id);
        }
        public async Task<EmployeeListViewModel> GetListEmployee(DataTableModel tableModel)
        {
            var employeeListViewModel = new EmployeeListViewModel();

            var employeeResult = await _employeeRepository.GetAll();
            var positionResult = await _positionRepository.GetAll();

            var results = (from employee in employeeResult
                           join position in positionResult on employee.PositionId equals position.PosittionId
                           select new EmployeeListViewDetailModel 
                           {
                                EmployeeId = employee.EmployeeId,
                               EmployeeNIK = employee.Nik,
                               EmployeeName = employee.Name,
                               EmployeeEmail = employee.Email,
                               EmployeeGender = employee.Gender,
                               EmployePlaceOfBirth = employee.PlaceOfBirth,
                               EmployeeDateOfBirth =employee.DateOfBirth,
                               EmployeeAddress = employee.Address,
                               EmployeePhone = employee.Phone,
                               EmployeeProfilePict = employee.ProfilePicture,
                               EmployeeLevel = employee.EmployeeLevel,
                               PositionId = position.PosittionId,
                               PositionName = position.Name
                           }
                );

            if (!string.IsNullOrEmpty(tableModel.searchValue))
            {
                results = results.Where(
                    x => x.EmployeeNIK.Contains(tableModel.searchValue)
                    || x.EmployeeName.Contains(tableModel.searchValue)
                    || x.EmployeeEmail.Contains(tableModel.searchValue)
                    || x.EmployeeGender.Contains(tableModel.searchValue)
                    || x.EmployePlaceOfBirth.Contains(tableModel.searchValue)
                    || x.PositionName.Contains(tableModel.searchValue)
                );
            }

            tableModel.recordsTotal = results.Count();
            var data = results.Skip(tableModel.skip).Take(tableModel.pageSize).ToList();

            employeeListViewModel.draw = tableModel.draw;
            employeeListViewModel.recordsFiltered = tableModel.recordsTotal;
            employeeListViewModel.recordsTotal = tableModel.recordsTotal;
            employeeListViewModel.data = data;

            return employeeListViewModel;
        }

        public async Task<int> Insert(MstEmployee model)
        {
            return await _employeeRepository.Insert(model);
        }

        public async Task<int> Update(MstEmployee model)
        {
            return await _employeeRepository.Update(model);
        }
    }
}
