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

    public async Task<IEnumerable<VwArchiveApproval>> GetByFilterModel(DataTableModel model)
    {
        IEnumerable<VwArchiveApproval> result;

        var propertyInfo = typeof(VwArchiveApproval).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        var propertyName = propertyInfo == null ? typeof(VwArchiveApproval).GetProperties()[0].Name : propertyInfo.Name;

        if (model.sortColumnDirection.ToLower() == "asc")
        {
            result = await _context.VwArchiveApprovals
            .Where(x => x.EmployeeId == model.EmployeeId &&
            (
            x.Title
            + x.RegistrationNumber
            + x.ApprovalType
            )
            .Contains(model.searchValue))
            .OrderBy(x => EF.Property<VwArchiveApproval>(x, propertyName))
            .Skip(model.skip).Take(model.pageSize)
            .ToListAsync();
        }
        else
        {
            result = await _context.VwArchiveApprovals
            .Where(x => x.EmployeeId == model.EmployeeId &&
            (
            x.Title
            + x.RegistrationNumber
            + x.ApprovalType
            )
            .Contains(model.searchValue))
            .OrderByDescending(x => EF.Property<VwArchiveApproval>(x, propertyName))
            .Skip(model.skip).Take(model.pageSize)
            .ToListAsync();
        }

        return result;
    }

    public Task<IEnumerable<TrxApproval>> GetById(Guid id)
    {
        throw new NotImplementedException();
    }
    public async Task<IEnumerable<TrxApproval>> GetByTransIdandApprovalCode(Guid id, string approvalCode)
    {
        return await _context.TrxApprovals.Include(x => x.Employee).Where(x => x.TransId == id && x.ApprovalCode == approvalCode).OrderBy(x => x.ApprovalLevel).ToListAsync();
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

    public async Task<int> GetCount()
    {
        return await _context.VwArchiveApprovals.CountAsync();
    }

    public async Task<int> Insert(TrxApproval model)
    {
        int result = 0;

        if (model != null)
        {
            _context.TrxApprovals.Add(model);
            result = await _context.SaveChangesAsync();
        }
        return result;
    }
    public async Task<bool> InsertBulk(List<TrxApproval> models)
    {
        bool result = false;
        if (models.Count() > 0)
        {
            await _context.AddRangeAsync(models);
            await _context.SaveChangesAsync();
            result = true;
        }
        return result;
    }
    public Task<int> Update(TrxApproval model)
    {
        throw new NotImplementedException();
    }
}
