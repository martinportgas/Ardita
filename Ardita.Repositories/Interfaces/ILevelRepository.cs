using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface ILevelRepository
    {
        Task<TrxLevel> GetById(Guid id);
        Task<IEnumerable<TrxLevel>> GetAll(string par = " 1=1 ");
        Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount(DataTableModel model);
        Task<int> Insert(TrxLevel model);
        Task<bool> InsertBulk(List<TrxLevel> levels);
        Task<int> Delete(TrxLevel model);
        Task<int> Update(TrxLevel model);
    }
}
