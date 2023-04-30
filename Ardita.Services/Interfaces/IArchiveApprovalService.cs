using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface IArchiveApprovalService
{
    Task<IEnumerable<TrxApproval>> GetById(Guid id);
    Task<IEnumerable<TrxApproval>> GetByTransIdandApprovalCode(Guid id, string approvalCode);
    Task<IEnumerable<TrxApproval>> GetAll();
    Task<DataTableResponseModel<TrxApproval>> GetList(DataTablePostModel model);
    Task<int> Insert(TrxApproval model);
    Task<int> Delete(TrxApproval model);
    Task<int> Update(TrxApproval model);
    Task<int> DeleteByTransIdandApprovalCode(Guid id, string approvalCode);
    Task<bool> InsertBulk(List<TrxApproval> models);
}
