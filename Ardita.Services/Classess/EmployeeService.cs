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

        public async Task<MstEmployee> GetById(Guid id)
        {
            return await _employeeRepository.GetById(id);
        }
        public async Task<DataTableResponseModel<MstEmployee>> GetListEmployee(DataTablePostModel model)
        {
            try
            {
                var dataCount = await _employeeRepository.GetCount();

                var filterData = new DataTableModel();

                filterData.sortColumn = model.columns[model.order[0].column].data;
                filterData.sortColumnDirection = model.order[0].dir;
                filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
                filterData.pageSize = model.length;
                filterData.skip = model.start;

                var results = await _employeeRepository.GetByFilterModel(filterData);

                var responseModel = new DataTableResponseModel<MstEmployee>();

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
