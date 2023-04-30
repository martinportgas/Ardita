using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IArchiveExtendDetailRepository
    {
        Task<IEnumerable<TrxArchiveExtendDetail>> GetById(Guid id);
        Task<IEnumerable<TrxArchiveExtendDetail>> GetByMainId(Guid id);
        Task<IEnumerable<TrxArchiveExtendDetail>> GetAll();
        Task<IEnumerable<TrxArchiveExtendDetail>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount();
        Task<int> Insert(TrxArchiveExtendDetail model);
        Task<bool> InsertBulk(List<TrxArchiveExtendDetail> models);
        Task<int> Delete(TrxArchiveExtendDetail model);
        Task<int> DeleteByMainId(Guid id);
        Task<int> Update(TrxArchiveExtendDetail model);
    }
}
