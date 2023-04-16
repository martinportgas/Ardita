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
        Task<IEnumerable<TrxLevel>> GetById(Guid id);
        Task<IEnumerable<TrxLevel>> GetAll();
        Task<IEnumerable<TrxLevel>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount();
        Task<int> Insert(TrxLevel model);
        Task<int> Delete(TrxLevel model);
        Task<int> Update(TrxLevel model);
    }
}
