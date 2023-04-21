using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces
{
    public interface ISecurityClassificationRepository
    {
        Task<IEnumerable<MstSecurityClassification>> GetById(Guid id);
        Task<IEnumerable<MstSecurityClassification>> GetAll();
        Task<IEnumerable<MstSecurityClassification>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount();
        Task<int> Insert(MstSecurityClassification model);
        Task<int> Delete(MstSecurityClassification model);
        Task<int> Update(MstSecurityClassification model);
    }
}
