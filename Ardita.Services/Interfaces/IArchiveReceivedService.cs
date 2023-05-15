using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface IArchiveReceivedService
{
    Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
}
