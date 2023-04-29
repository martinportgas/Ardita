using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface IArchiveUnitService
{
    Task<TrxArchiveUnit> GetById(Guid id);
    Task<IEnumerable<TrxArchiveUnit>> GetAll();
    Task<DataTableResponseModel<TrxArchiveUnit>> GetList(DataTablePostModel model);
    Task<int> Insert(TrxArchiveUnit model);
    Task<int> Delete(TrxArchiveUnit model);
    Task<int> Update(TrxArchiveUnit model);
}
