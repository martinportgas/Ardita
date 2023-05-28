using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Microsoft.Extensions.Primitives;

namespace Ardita.Services.Interfaces;

public interface IMediaStorageInActiveService
{
    Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
    Task<IEnumerable<object>> GetDetails(Guid id);
    Task<IEnumerable<object>> GetDetailArchive(Guid id);
    Task<int> Insert(TrxMediaStorageInActive model, string[] listSts, string[] listArchive);
}
