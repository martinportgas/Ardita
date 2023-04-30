using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Repositories.Interfaces;

public interface IArchiveApprovalRepository
{
    Task<IEnumerable<TrxApproval>> GetById(Guid id);
    Task<IEnumerable<TrxApproval>> GetByTransIdandApprovalCode(Guid id, string approvalCode);
    Task<IEnumerable<TrxApproval>> GetAll();
    Task<IEnumerable<TrxApproval>> GetByFilterModel(DataTableModel model);
    Task<int> GetCount();
    Task<int> Insert(TrxApproval model);
    Task<bool> InsertBulk(List<TrxApproval> models);
    Task<int> Delete(TrxApproval model);
    Task<int> Update(TrxApproval model);
    Task<int> DeleteByTransIdandApprovalCode(Guid id, string approvalCode);
}
