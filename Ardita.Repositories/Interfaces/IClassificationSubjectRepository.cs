using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IClassificationSubjectRepository
    {
        Task<IEnumerable<TrxSubjectClassification>> GetById(Guid id);
        Task<IEnumerable<TrxSubjectClassification>> GetAll();
        Task<IEnumerable<TrxSubjectClassification>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount();
        Task<int> Insert(TrxSubjectClassification model);
        Task<bool> InsertBulk(List<TrxSubjectClassification> models);
        Task<int> Delete(TrxSubjectClassification model);
        Task<int> Update(TrxSubjectClassification model);
    }
}
