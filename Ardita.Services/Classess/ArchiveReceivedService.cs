using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess;

public class ArchiveReceivedService : IArchiveReceivedService
{
    private readonly IArchiveReceivedRepository _archiveReceivedRepository;
    public ArchiveReceivedService(IArchiveReceivedRepository archiveReceivedRepository)
    {
        _archiveReceivedRepository = archiveReceivedRepository;
    }

    public async Task<DataTableResponseModel<object>> GetList(DataTablePostModel model)
    {
        try
        {
            var filterData = new DataTableModel
            {
                sortColumn = model.columns[model.order[0].column].data,
                sortColumnDirection = model.order[0].dir,
                searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value,
                pageSize = model.length,
                skip = model.start
            };

            var dataCount = await _archiveReceivedRepository.GetCountByFilterDataArchiveMovement(filterData);
            var results = await _archiveReceivedRepository.GetByFilterModelArchiveMovement(filterData);

            var responseModel = new DataTableResponseModel<object>
            {
                draw = model.draw,
                recordsTotal = dataCount,
                recordsFiltered = dataCount,
                data = results.ToList()
            };

            return responseModel;
        }
        catch (Exception ex)
        {
            return new DataTableResponseModel<object>
            {
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<int> Update(TrxArchiveMovement model)
    {
        return await _archiveReceivedRepository.Update(model);
    }
}
