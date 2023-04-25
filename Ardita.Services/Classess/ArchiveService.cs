using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess;

public class ArchiveService : IArchiveService
{
    private readonly IArchiveUnitRepository _archiveUnitRepository;

    public ArchiveService(IArchiveUnitRepository archiveUnitRepository) => _archiveUnitRepository = archiveUnitRepository;

    public Task<int> Delete(TrxArchive model)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TrxArchive>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TrxArchive>> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<DataTableResponseModel<TrxArchive>> GetList(DataTablePostModel model)
    {
        throw new NotImplementedException();
    }

    public Task<int> Insert(TrxArchive model)
    {
        throw new NotImplementedException();
    }

    public Task<int> Update(TrxArchive model)
    {
        throw new NotImplementedException();
    }
}
