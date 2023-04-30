using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface IMediaStorageService
{
    Task<TrxMediaStorage> GetById(Guid id);
    Task<IEnumerable<TrxMediaStorage>> GetAll();
    Task<DataTableResponseModel<TrxMediaStorage>> GetList(DataTablePostModel model);
    Task<int> Insert(TrxMediaStorage model, List<TrxMediaStorageDetail> detail);
    Task<int> Delete(TrxMediaStorage model);
    Task<int> Update(TrxMediaStorage model);
}
