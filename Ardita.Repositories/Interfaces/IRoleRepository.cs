using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<MstRole> GetById(Guid id);
        Task<IEnumerable<MstRole>> GetAll(string par = " 1=1 ");
        Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount(DataTableModel model);
        Task<int> Insert(MstRole model);
        Task<int> Delete(MstRole model);
        Task<int> Update(MstRole model);
    }
}
