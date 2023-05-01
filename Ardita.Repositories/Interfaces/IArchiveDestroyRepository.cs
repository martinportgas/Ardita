using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IArchiveDestroyRepository
    {
        Task<TrxArchiveDestroy> GetById(Guid id);
        Task<IEnumerable<TrxArchiveDestroy>> GetAll();
        Task<IEnumerable<TrxArchiveDestroy>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount();
        Task<int> Insert(TrxArchiveDestroy model);
        Task<bool> InsertBulk(List<TrxArchiveDestroy> models);
        Task<int> Delete(TrxArchiveDestroy model);
        Task<int> Update(TrxArchiveDestroy model);
        Task<int> Submit(TrxArchiveDestroy model);
    }
}
