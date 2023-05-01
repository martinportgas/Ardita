using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Microsoft.Extensions.Primitives;

namespace Ardita.Services.Interfaces;

public interface IArchiveService
{
    Task<TrxArchive> GetById(Guid id);
    Task<IEnumerable<TrxArchive>> GetAll();
    Task<DataTableResponseModel<TrxArchive>> GetList(DataTablePostModel model);
    Task<int> Insert(TrxArchive model, StringValues modelDetail);
    Task<int> Delete(TrxArchive model);
    Task<int> Update(TrxArchive model);
}
