using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess;

public class ArchiveCreatorService : IArchiveCreatorService
{
    private readonly IArchiveCreatorRepository _archiveCreatorRepository;

    public ArchiveCreatorService(IArchiveCreatorRepository archiveCreatorRepository) => _archiveCreatorRepository = archiveCreatorRepository;

    public async Task<int> Delete(MstCreator model) => await _archiveCreatorRepository.Delete(model);

    public async Task<IEnumerable<MstCreator>> GetAll() => await _archiveCreatorRepository.GetAll();

    public async Task<IEnumerable<MstCreator>> GetById(Guid id) => await _archiveCreatorRepository.GetById(id);

    public async Task<DataTableResponseModel<MstCreator>> GetList(DataTablePostModel model)
    {
        try
        {
            var dataCount = await _archiveCreatorRepository.GetCount();

            var filterData = new DataTableModel
            {
                sortColumn = model.columns[model.order[0].column].data,
                sortColumnDirection = model.order[0].dir,
                searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value,
                pageSize = model.length,
                skip = model.start
            };

            var results = await _archiveCreatorRepository.GetByFilterModel(filterData);

            var responseModel = new DataTableResponseModel<MstCreator>
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
            return null;
        }

    }

    public async Task<int> Insert(MstCreator model) => await _archiveCreatorRepository.Insert(model);

    public async Task<int> Update(MstCreator model) => await _archiveCreatorRepository.Update(model);
}
