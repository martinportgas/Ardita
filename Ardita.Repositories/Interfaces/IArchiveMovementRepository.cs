using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IArchiveMovementRepository
    {
        Task<TrxArchiveMovement> GetById(Guid id);
        Task<IEnumerable<TrxArchiveMovement>> GetAll();
        Task<IEnumerable<TrxArchiveMovement>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount();
        Task<int> Insert(TrxArchiveMovement model);
        Task<bool> InsertBulk(List<TrxArchiveMovement> models);
        Task<int> Delete(TrxArchiveMovement model);
        Task<int> Update(TrxArchiveMovement model);
        Task<int> Submit(TrxArchiveMovement model);
    }
}
