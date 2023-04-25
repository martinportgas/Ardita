using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ardita.Repositories.Classess;

public class ArchiveRepository : IArchiveRepository
{
    private readonly BksArditaDevContext _context;

    public ArchiveRepository(BksArditaDevContext context) => _context = context;

    public Task<int> Delete(TrxArchive model)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TrxArchive>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TrxArchive>> GetByFilterModel(DataTableModel model)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TrxArchive>> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetCount()
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
