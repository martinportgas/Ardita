using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface IMediaStorageInActiveService
{
    Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
    Task<IEnumerable<object>> GetDetails(Guid id);
    Task<IEnumerable<object>> GetDetailArchive(Guid id);
}
