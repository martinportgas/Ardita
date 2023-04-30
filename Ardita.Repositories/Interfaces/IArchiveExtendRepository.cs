using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IArchiveExtendRepository
    {
        Task<IEnumerable<TrxArchiveExtend>> GetById(Guid id);
        Task<IEnumerable<TrxArchiveExtend>> GetAll();
        Task<IEnumerable<TrxArchiveExtend>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount();
        Task<int> Insert(TrxArchiveExtend model);
        Task<bool> InsertBulk(List<TrxArchiveExtend> models);
        Task<int> Delete(TrxArchiveExtend model);
        Task<int> Update(TrxArchiveExtend model);
    }
}
