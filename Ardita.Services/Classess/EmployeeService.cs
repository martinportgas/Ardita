using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Employees;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

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
                           join position in positionResult on employee.PositionId equals position.PositionId
                           select new EmployeeListViewDetailModel
                           {
                               EmployeeId = employee.EmployeeId,
                               EmployeeNIK = employee.Nik,
                               EmployeeName = employee.Name,
                               EmployeeEmail = employee.Email,
                               EmployeeGender = employee.Gender,
                               EmployeePlaceOfBirth = employee.PlaceOfBirth,
                               EmployeeDateOfBirth = employee.DateOfBirth,
                               EmployeeAddress = employee.Address,
                               EmployeePhone = employee.Phone,
                               EmployeeProfilePict = employee.ProfilePicture,
                               EmployeeLevel = employee.EmployeeLevel,
                               PositionId = position.PositionId,
                               PositionName = position.Name
                           }
                );

            if (!string.IsNullOrEmpty(tableModel.searchValue))
            {
                results = results.Where(
                    x => (x.EmployeeNIK != null ? x.EmployeeNIK.ToUpper().Contains(tableModel.searchValue.ToUpper()) : false)
                    || (x.EmployeeName != null ? x.EmployeeName.ToUpper().Contains(tableModel.searchValue.ToUpper()) : false)
                    || (x.EmployeeEmail != null ? x.EmployeeEmail.ToUpper().Contains(tableModel.searchValue.ToUpper()) : false)
                    || (x.EmployeeGender != null ? x.EmployeeGender.ToUpper().Contains(tableModel.searchValue.ToUpper()) : false)
                    || (x.EmployeePlaceOfBirth != null ? x.EmployeePlaceOfBirth.ToUpper().Contains(tableModel.searchValue.ToUpper()) : false)
                    || (x.PositionName != null ? x.PositionName.ToUpper().Contains(tableModel.searchValue.ToUpper()) : false)
                );
            }

            if (!(string.IsNullOrEmpty(tableModel.sortColumn)))
            {
                var param = tableModel.sortColumn;
                var propertyInfo = typeof(EmployeeListViewDetailModel).GetProperty(param);

                if (tableModel.sortColumnDirection == "asc")
                {
                    results = results.OrderBy(x => propertyInfo.GetValue(x, null));
                }
                else
                {
                    results = results.OrderByDescending(x => propertyInfo.GetValue(x, null));
                }
                

               
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

        public async Task<bool> InsertBulk(List<MstEmployee> employees)
        {
            return await _employeeRepository.InsertBulk(employees);
        }

        public async Task<int> Update(MstEmployee model)
        {
            return await _employeeRepository.Update(model);
        }
    }
}
