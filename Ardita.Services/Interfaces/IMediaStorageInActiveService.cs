using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface IMediaStorageInActiveService
{
    Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
}
