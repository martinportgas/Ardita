using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        Task<TrxRoom> GetById(Guid id);
        Task<IEnumerable<TrxRoom>> GetAll();
        Task<IEnumerable<TrxRoom>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount();
        Task<int> Insert(TrxRoom model);
        Task<bool> InsertBulk(List<TrxRoom> rooms);
        Task<int> Delete(TrxRoom model);
        Task<int> Update(TrxRoom model);
    }
}
