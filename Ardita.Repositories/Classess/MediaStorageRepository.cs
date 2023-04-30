using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;

namespace Ardita.Repositories.Classess;

public class MediaStorageRepository : IMediaStorageRepository
{
    public Task<int> Delete(TrxMediaStorage model)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TrxMediaStorage>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TrxMediaStorage>> GetByFilterModel(DataTableModel model)
    {
        throw new NotImplementedException();
    }

    public Task<TrxMediaStorage> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetCount()
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
