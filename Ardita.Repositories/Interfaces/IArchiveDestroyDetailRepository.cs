using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IArchiveDestroyDetailRepository
    {
        Task<IEnumerable<TrxArchiveDestroyDetail>> GetById(Guid id);
        Task<IEnumerable<TrxArchiveDestroyDetail>> GetByMainId(Guid id);
        Task<IEnumerable<TrxArchiveDestroyDetail>> GetAll();
        Task<IEnumerable<TrxArchiveDestroyDetail>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount();
        Task<int> Insert(TrxArchiveDestroyDetail model);
        Task<bool> InsertBulk(List<TrxArchiveDestroyDetail> models);
        Task<int> Delete(TrxArchiveDestroyDetail model);
        Task<int> DeleteByMainId(Guid id);
        Task<int> Update(TrxArchiveDestroyDetail model);
    }
}
