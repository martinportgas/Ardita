using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess;

public class SecurityClassificationService : ISecurityClassificationService
{
    private readonly ISecurityClassificationRepository _SecurityClassificationRepository;

    public SecurityClassificationService(ISecurityClassificationRepository SecurityClassificationRepository) => _SecurityClassificationRepository = SecurityClassificationRepository;

    public async Task<int> Delete(MstSecurityClassification model) => await _SecurityClassificationRepository.Delete(model);

    public async Task<IEnumerable<MstSecurityClassification>> GetAll() => await _SecurityClassificationRepository.GetAll();

    public async Task<IEnumerable<MstSecurityClassification>> GetById(Guid id)
    => await _SecurityClassificationRepository.GetById(id);

    public async Task<DataTableResponseModel<MstSecurityClassification>> GetList(DataTablePostModel model)
    {
        try
        {
            var dataCount = await _SecurityClassificationRepository.GetCount();

            var filterData = new DataTableModel
            {
                sortColumn = model.columns[model.order[0].column].data,
                sortColumnDirection = model.order[0].dir,
                searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value,
                pageSize = model.length,
                skip = model.start
            };

            var results = await _SecurityClassificationRepository.GetByFilterModel(filterData);

            var responseModel = new DataTableResponseModel<MstSecurityClassification>
            {
                draw = model.draw,
                recordsTotal = dataCount,
                recordsFiltered = dataCount,
                data = results.ToList()
            };

            return responseModel;
        }
        catch (Exception)
        {
            return null;
        }

    }

    public async Task<int> Insert(MstSecurityClassification model) => await _SecurityClassificationRepository.Insert(model);

    public async Task<int> Update(MstSecurityClassification model) => await _SecurityClassificationRepository.Update(model);
}
