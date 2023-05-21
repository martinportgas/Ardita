using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;

namespace Ardita.Repositories.Classess;

public class SubTypeStorageRepository : ISubTypeStorageRepository
{
    private readonly BksArditaDevContext _context;

    public SubTypeStorageRepository(BksArditaDevContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<MstSubTypeStorage>> GetAllByTypeStorageId(Guid ID)
    {
        var result = from idx in _context.IdxSubTypeStorages
                     join mst in _context.MstSubTypeStorages on idx.SubTypeStorageId equals mst.SubTypeStorageId
                     join trx in _context.TrxTypeStorages on idx.TypeStorageId equals trx.TypeStorageId
                     where idx.TypeStorageId == ID
                     select mst;

        await Task.Delay(0);

        return result;
    }
}
