using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess;

public class ArchiveApprovalService : IArchiveApprovalService
{
    private readonly IArchiveApprovalRepository _archiveApprovalRepository;

    public ArchiveApprovalService(IArchiveApprovalRepository archiveApprovalRepository) => _archiveApprovalRepository = archiveApprovalRepository;

    public Task<int> Delete(TrxApproval model)
    {
        throw new NotImplementedException();
    }
    public async Task<int> DeleteByTransIdandApprovalCode(Guid id, string approvalCode)
    {
        return await _archiveApprovalRepository.DeleteByTransIdandApprovalCode(id, approvalCode);
    }

    public Task<IEnumerable<TrxApproval>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TrxApproval>> GetById(Guid id)
    {
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<TrxApproval>> GetByTransIdandApprovalCode(Guid id, string approvalCode)
    {
        return await _archiveApprovalRepository.GetByTransIdandApprovalCode(id, approvalCode);
    }

    public Task<DataTableResponseModel<TrxApproval>> GetList(DataTablePostModel model)
    {
        throw new NotImplementedException();
    }

    public Task<int> Insert(TrxApproval model)
    {
        throw new NotImplementedException();
    }
    public async Task<bool> InsertBulk(List<TrxApproval> models)
    {
        return await _archiveApprovalRepository.InsertBulk(models);
    }

    public Task<int> Update(TrxApproval model)
    {
        throw new NotImplementedException();
    }
}
