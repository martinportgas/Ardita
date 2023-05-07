using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Employees;
using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IClassificationPermissionRepository _classificationPermissionRepository;
        public EmployeeService(
            IEmployeeRepository employeeRepository, 
            IPositionRepository positionRepository,
            IClassificationPermissionRepository classificationPermissionRepository)
        {
            _employeeRepository = employeeRepository;
            _positionRepository = positionRepository;
            _classificationPermissionRepository = classificationPermissionRepository;
        }

        public async Task<int> Delete(MstEmployee model)
        {
            return await _employeeRepository.Delete(model);
        }

        public async Task<IEnumerable<MstEmployee>> GetAll()
        {
            return await _employeeRepository.GetAll();
        }

        public async Task<MstEmployee> GetById(Guid id)
        {
            return await _employeeRepository.GetById(id);
        }
        public async Task<DataTableResponseModel<object>> GetListEmployee(DataTablePostModel model)
        {
            try
            {
                

                var filterData = new DataTableModel();
                

                filterData.sortColumn = model.columns[model.order[0].column].name;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;

                var results = await _employeeRepository.GetByFilterModel(filterData);

                var responseModel = new DataTableResponseModel<object>();
                var dataCount = await _employeeRepository.GetCount(filterData);

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
        public async Task<IEnumerable<MstEmployee>> GetListEmployeeBySubSubjectClassificationId(Guid Id)
        {
            var listPermission = await _classificationPermissionRepository.GetByMainId(Id);
            var listPosition = await _positionRepository.GetAll();
            var listEmp = await _employeeRepository.GetAll();

            var result = (from emp in listEmp
                          join pos in listPosition on emp.PositionId equals pos.PositionId
                          join prs in listPermission on pos.PositionId equals prs.PositionId
                          select emp).ToList();

            return result;
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
