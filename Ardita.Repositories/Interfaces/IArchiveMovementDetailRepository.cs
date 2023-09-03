using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IArchiveMovementDetailRepository
    {
        Task<IEnumerable<TrxArchiveMovementDetail>> GetById(Guid id);
        Task<IEnumerable<TrxArchiveMovementDetail>> GetByMainId(Guid id);
        Task<IEnumerable<TrxArchiveMovementDetail>> GetAll(string par = " 1=1 ");
        Task<IEnumerable<TrxArchiveMovementDetail>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount();
        Task<int> Insert(TrxArchiveMovementDetail model);
        Task<bool> InsertBulk(List<TrxArchiveMovementDetail> models);
        Task<int> Delete(TrxArchiveMovementDetail model);
        Task<int> DeleteByMainId(Guid id);
        Task<int> Update(TrxArchiveMovementDetail model);
    }
}
