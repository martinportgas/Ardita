using Ardita.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardita.Models.ViewModels.Employees;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<MstEmployee> GetById(Guid id);
        Task<IEnumerable<MstEmployee>> GetAll();
        Task<DataTableResponseModel<object>> GetListEmployee(DataTablePostModel model);
        Task<int> Insert(MstEmployee model);
        Task<bool> InsertBulk(List<MstEmployee> employees);
        Task<int> Delete(MstEmployee model);
        Task<int> Update(MstEmployee model);
        Task<IEnumerable<MstEmployee>> GetListEmployeeBySubSubjectClassificationId(Guid Id);
    }
}
