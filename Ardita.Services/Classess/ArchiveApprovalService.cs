using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess;

public class ArchiveApprovalService : IArchiveApprovalService
{
    private readonly IArchiveApprovalRepository _archiveApprovalRepository;
    private readonly IArchiveExtendRepository _archiveExtendRepository;
    private readonly IArchiveDestroyRepository _archiveDestroyRepository;
    private readonly IArchiveMovementRepository _archiveMovementRepository;

    public ArchiveApprovalService(IArchiveApprovalRepository archiveApprovalRepository,
        IArchiveExtendRepository archiveExtendRepository,
        IArchiveDestroyRepository archiveDestroyRepository,
        IArchiveMovementRepository archiveMovementRepository)
    {
        _archiveApprovalRepository = archiveApprovalRepository;
        _archiveExtendRepository = archiveExtendRepository;
        _archiveDestroyRepository = archiveDestroyRepository;
        _archiveMovementRepository = archiveMovementRepository;
    }

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

    public async Task<DataTableResponseModel<object>> GetList(DataTablePostModel model)
    {
        try
        {

            var filterData = new DataTableModel();

            filterData.sortColumn = model.columns[model.order[0].column].data;
            filterData.sortColumnDirection = model.order[0].dir;
            filterData.searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value;
            filterData.pageSize = model.length;
            filterData.skip = model.start;
            filterData.EmployeeId = model.EmployeeId;
            filterData.IsArchiveActive = model.IsArchiveActive;

            var dataCount = await _archiveApprovalRepository.GetCountByFilterModel(filterData);
            var results = await _archiveApprovalRepository.GetByFilterModel(filterData);

            var responseModel = new DataTableResponseModel<object>();

            responseModel.draw = model.draw;
            responseModel.recordsTotal = dataCount;
            responseModel.recordsFiltered = dataCount;
            responseModel.data = results.ToList();

            return responseModel;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<int> Insert(TrxApproval model)
    {
        return await _archiveApprovalRepository.Insert(model);
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
