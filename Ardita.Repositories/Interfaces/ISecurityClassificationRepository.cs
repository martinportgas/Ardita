using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces
{
    public interface ISecurityClassificationRepository
    {
        Task<IEnumerable<MstSecurityClassification>> GetById(Guid id);
        Task<IEnumerable<MstSecurityClassification>> GetAll(string par = " 1=1 ");
        Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
        Task<int> GetCountByFilterModel(DataTableModel model);
        Task<int> GetCount();
        Task<int> Insert(MstSecurityClassification model);
        Task<bool> InsertBulk(List<MstSecurityClassification> mstSecurityClassifications);
        Task<int> Delete(MstSecurityClassification model);
        Task<int> Update(MstSecurityClassification model);
    }
}
