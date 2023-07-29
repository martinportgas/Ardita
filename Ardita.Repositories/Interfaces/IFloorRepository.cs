using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IFloorRepository
    {
        Task<TrxFloor> GetById(Guid id);
        Task<IEnumerable<TrxFloor>> GetAll();
        Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount(DataTableModel model);
        Task<int> Insert(TrxFloor model);
        Task<bool> InsertBulk(List<TrxFloor> floors);
        Task<int> Delete(TrxFloor model);
        Task<int> Update(TrxFloor model);
    }
}
