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
        Task<IEnumerable<MstEmployee>> GetById(Guid id);
        Task<IEnumerable<MstEmployee>> GetAll();
        Task<EmployeeListViewModel> GetListEmployee(DataTableModel tableModel);
        Task<int> Insert(MstEmployee model);
        Task<int> Delete(MstEmployee model);
        Task<int> Update(MstEmployee model);
    }
}
