using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IClassificationRepository
    {
        Task<IEnumerable<TrxClassification>> GetById(Guid id);
        Task<IEnumerable<TrxClassification>> GetAll();
        Task<IEnumerable<TrxClassification>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount();
        Task<int> Insert(TrxClassification model);
        Task<bool> InsertBulk(List<TrxClassification> models);
        Task<int> Delete(TrxClassification model);
        Task<int> Update(TrxClassification model);
    }
}
