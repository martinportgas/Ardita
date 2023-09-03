using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ardita.Repositories.Classess;

public class ArchiveApprovalRepository : IArchiveApprovalRepository
{
    private readonly BksArditaDevContext _context;

    private readonly ILogChangesRepository _logChangesRepository;

    public ArchiveApprovalRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
    {
        _context = context;
        _logChangesRepository = logChangesRepository;
    }

    public Task<int> Delete(TrxApproval model)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TrxApproval>> GetAll(string par = " 1=1 ")
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
    {
        var result = (bool)model.IsArchiveActive ? await _context.VwArchiveApprovals
            .Where(x => x.EmployeeId == model.EmployeeId &&
            (
            x.Title
            + x.RegistrationNumber
            + x.ApprovalType
            )
            .Contains(model.searchValue))
            .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
            .Skip(model.skip).Take(model.pageSize)
            .Select(x => new
            {
                x.ApprovalId,
                x.TransId,
                x.StatusId,
                x.ApprovalCode,
                x.ApprovalDate,
                x.ApprovalType,
                x.ApprovalLevel,
                x.Title,
                x.RegistrationNumber,
                x.Note,
                x.EmployeeId,
                x.CreatedBy,
                x.CreatedDate
            })
            .ToListAsync() : await _context.VwArchiveApprovalInActives
            .Where(x => x.EmployeeId == model.EmployeeId &&
            (
            x.Title
            + x.RegistrationNumber
            + x.ApprovalType
            )
            .Contains(model.searchValue))
            .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
            .Skip(model.skip).Take(model.pageSize)
            .Select(x => new
            {
                x.ApprovalId,
                x.TransId,
                x.StatusId,
                x.ApprovalCode,
                x.ApprovalDate,
                x.ApprovalType,
                x.ApprovalLevel,
                x.Title,
                x.RegistrationNumber,
                x.Note,
                x.EmployeeId,
                x.CreatedBy,
                x.CreatedDate
            })
            .ToListAsync();

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
    public async Task<int> GetCountByFilterModel(DataTableModel model)
    {
        return (bool)model.IsArchiveActive ? await _context.VwArchiveApprovals
            .Where(x => x.EmployeeId == model.EmployeeId && (x.Title + x.RegistrationNumber + x.ApprovalType)
            .Contains(model.searchValue))
            .CountAsync() : await _context.VwArchiveApprovalInActives
            .Where(x => x.EmployeeId == model.EmployeeId && (x.Title + x.RegistrationNumber + x.ApprovalType)
            .Contains(model.searchValue))
            .CountAsync();
    }

    public async Task<int> Insert(TrxApproval model)
    {
        int result = 0;

        if (model != null)
        {
            _context.TrxApprovals.Add(model);
            result = await _context.SaveChangesAsync();

            //Log
            if (result > 0)
            {
                try
                {
                    await _logChangesRepository.CreateLog<TrxApproval>(GlobalConst.New, (Guid)model!.CreatedBy!, new List<TrxApproval> {  }, new List<TrxApproval> { model });
                }
                catch (Exception ex) { }
            }
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

            //Log
            if (result)
            {
                try
                {
                    await _logChangesRepository.CreateLog<TrxApproval>(GlobalConst.New, (Guid)models.FirstOrDefault()!.CreatedBy!, new List<TrxApproval> { }, models);
                }
                catch (Exception ex) { }
            }
        }
        return result;
    }
    public Task<int> Update(TrxApproval model)
    {
        throw new NotImplementedException();
    }
}
