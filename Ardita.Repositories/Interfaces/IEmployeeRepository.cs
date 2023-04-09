using Ardita.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<MstEmployee>> GetById(Guid id);
        Task<IEnumerable<MstEmployee>> GetAll();
        Task<int> Insert(MstEmployee model);
        Task<bool> InsertBulk(List<MstEmployee> employees);
        Task<int> Delete(MstEmployee model);
        Task<int> Update(MstEmployee model);
    }
}
