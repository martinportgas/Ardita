using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IArchiveRentRepository
    {
        Task<IEnumerable<TrxArchiveRent>> GetById(Guid id);
        Task<IEnumerable<TrxArchiveRent>> GetAll();
        Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
        Task<int> GetCountByFilterModel(DataTableModel model);
        Task<int> Insert(TrxArchiveRent model);
        Task<int> Delete(TrxArchiveRent model);
        Task<int> Update(TrxArchiveRent model);

    }
}
