using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface IArchiveReceivedService
{
    Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
    Task<int> Update(TrxArchiveMovement model);

}
