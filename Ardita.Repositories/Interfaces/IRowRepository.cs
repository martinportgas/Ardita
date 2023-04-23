using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IRowRepository
    {
        Task<IEnumerable<TrxRow>> GetById(Guid id);
        Task<IEnumerable<TrxRow>> GetAll();
        Task<IEnumerable<TrxRow>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount();
        Task<int> Insert(TrxRow model);
        Task<int> Delete(TrxRow model);
        Task<int> Update(TrxRow model);
    }
}
