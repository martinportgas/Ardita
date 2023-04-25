using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface IArchiveService
{
    Task<IEnumerable<TrxArchive>> GetById(Guid id);
    Task<IEnumerable<TrxArchive>> GetAll();
    Task<DataTableResponseModel<TrxArchive>> GetList(DataTablePostModel model);
    Task<int> Insert(TrxArchive model);
    Task<int> Delete(TrxArchive model);
    Task<int> Update(TrxArchive model);
}
