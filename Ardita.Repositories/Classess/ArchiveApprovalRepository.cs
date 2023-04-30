using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ardita.Repositories.Classess;

public class ArchiveApprovalRepository : IArchiveApprovalRepository
{
    private readonly BksArditaDevContext _context;

    public ArchiveApprovalRepository(BksArditaDevContext context) => _context = context;

    public Task<int> Delete(TrxApproval model)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TrxApproval>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TrxApproval>> GetByFilterModel(DataTableModel model)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TrxApproval>> GetById(Guid id)
    {
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<TrxApproval>> GetByTransIdandApprovalCode(Guid id, string approvalCode)
    {
        return await _context.TrxApprovals.Include(x => x.Employee).Where(x => x.TransId == id && x.ApprovalCode == approvalCode).ToListAsync();
    }
    public async Task<int> DeleteByTransIdandApprovalCode(Guid id, string approvalCode)
    {
        int result = 0;
        if (id != null)
        {
            _context.Database.ExecuteSqlRaw($" delete from dbo.TRX_APPROVAL where approval_code='{approvalCode}' and trans_id='{id}'");
            result = await _context.SaveChangesAsync();
        }
        return result;
    }

    public Task<int> GetCount()
    {
        throw new NotImplementedException();
    }

    public Task<int> Insert(TrxApproval model)
    {
        throw new NotImplementedException();
    }
    public async Task<bool> InsertBulk(List<TrxApproval> models)
    {
        bool result = false;
        if (models.Count() > 0)
        {
            await _context.BulkInsertAsync(models);
            result = true;
        }
        return result;
    }
    public Task<int> Update(TrxApproval model)
    {
        throw new NotImplementedException();
    }
}
