using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface IArchiveRetentionService
{
    Task<IEnumerable<TrxArchive>> GetById(Guid id);
    Task<IEnumerable<VwArchiveRetentionInActive>> GetInActiveAll(string par = " 1=1 ");
    Task<IEnumerable<VwArchiveRetention>> GetAll(string par = " 1=1 ");
    Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
    Task<int> Insert(TrxArchive model);
    Task<int> Delete(TrxArchive model);
    Task<int> Update(TrxArchive model);
}
