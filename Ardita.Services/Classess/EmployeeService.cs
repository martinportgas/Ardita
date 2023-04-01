using Ardita.Models.DbModels;
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

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
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
