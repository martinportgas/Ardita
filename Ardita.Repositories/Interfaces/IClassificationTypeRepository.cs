using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IClassificationTypeRepository
    {
        Task<IEnumerable<MstTypeClassification>> GetById(Guid id);
        Task<IEnumerable<MstTypeClassification>> GetAll();
        Task<IEnumerable<MstTypeClassification>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount();
        Task<int> Insert(MstTypeClassification model);
        Task<bool> InsertBulk(List<MstTypeClassification> models);
        Task<int> Delete(MstTypeClassification model);
        Task<int> Update(MstTypeClassification model);
    }
}
