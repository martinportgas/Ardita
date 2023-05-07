using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface IArchiveRetentionService
{
    Task<IEnumerable<TrxArchive>> GetById(Guid id);
    Task<IEnumerable<VwArchiveRetention>> GetAll();
    Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
    Task<int> Insert(TrxArchive model);
    Task<int> Delete(TrxArchive model);
    Task<int> Update(TrxArchive model);
}
