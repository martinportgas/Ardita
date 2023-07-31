using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IRackRepository
    {
        Task<TrxRack> GetById(Guid id);
        Task<IEnumerable<TrxRack>> GetAll();
        Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount(DataTableModel model);
        Task<int> Insert(TrxRack model);
        Task<bool> InsertBulk(List<TrxRack> racks);
        Task<int> Delete(TrxRack model);
        Task<int> Update(TrxRack model);
    }
}
