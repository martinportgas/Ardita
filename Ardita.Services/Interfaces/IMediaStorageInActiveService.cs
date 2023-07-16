using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Microsoft.Extensions.Primitives;

namespace Ardita.Services.Interfaces;

public interface IMediaStorageInActiveService
{
    Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
    Task<IEnumerable<VwArchiveRent>> GetDetails(Guid id);
    Task<IEnumerable<object>> GetDetailArchive(Guid id);
    Task<int> Insert(TrxMediaStorageInActive model, string[] listSts, string[] listArchive);
    Task<int> Update(TrxMediaStorageInActive model, string[] listSts, string[] listArchive);
    Task<TrxMediaStorageInActive> GetById(Guid id);
    Task<int> Delete(Guid ID);
    Task<IEnumerable<VwArchiveRent>> GetDetails(string archiveName, Guid subSubjectId);
    Task<IEnumerable<VwArchiveRent>> GetDetailStorages(Guid Id, int Sort);
}
