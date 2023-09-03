using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess;

public class ArchiveCreatorService : IArchiveCreatorService
{
    private readonly IArchiveCreatorRepository _archiveCreatorRepository;

    public ArchiveCreatorService(IArchiveCreatorRepository archiveCreatorRepository) => _archiveCreatorRepository = archiveCreatorRepository;

    public async Task<int> Delete(MstCreator model) => await _archiveCreatorRepository.Delete(model);

    public async Task<IEnumerable<MstCreator>> GetAll(string par = " 1=1 ") => await _archiveCreatorRepository.GetAll(par);

    public async Task<IEnumerable<MstCreator>> GetById(Guid id) => await _archiveCreatorRepository.GetById(id);

    public async Task<DataTableResponseModel<object>> GetList(DataTablePostModel model)
    {
        try
        {
            var filterData = new DataTableModel
            {
                sortColumn = model.columns[model.order[0].column].name,
                sortColumnDirection = model.order[0].dir,
                searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value,
                pageSize = model.length,
                skip = model.start
            };

            var results = await _archiveCreatorRepository.GetByFilterModel(filterData);
            var dataCount = await _archiveCreatorRepository.GetCountByFilterModel(filterData);

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
            return null;
        }

    }

    public async Task<int> Insert(MstCreator model) => await _archiveCreatorRepository.Insert(model);

    public async Task<bool> InsertBulk(List<MstCreator> mstCreators)
    {
        return await _archiveCreatorRepository.InsertBulk(mstCreators);
    }

    public async Task<int> Update(MstCreator model) => await _archiveCreatorRepository.Update(model);
}
