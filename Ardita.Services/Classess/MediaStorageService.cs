using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess;

public class MediaStorageService : IMediaStorageService
{
    public Task<int> Delete(TrxMediaStorage model)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TrxMediaStorage>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<TrxMediaStorage> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<DataTableResponseModel<TrxMediaStorage>> GetList(DataTablePostModel model)
    {
        throw new NotImplementedException();
    }

    public Task<int> Insert(TrxMediaStorage model, List<TrxMediaStorageDetail> detail)
    {
        throw new NotImplementedException();
    }

    public Task<int> Update(TrxMediaStorage model)
    {
        throw new NotImplementedException();
    }
}
