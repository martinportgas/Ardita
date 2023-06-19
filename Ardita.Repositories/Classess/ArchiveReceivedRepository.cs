using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ardita.Repositories.Classess;

public class ArchiveReceivedRepository : IArchiveReceivedRepository
{
    private readonly BksArditaDevContext _context;

    public ArchiveReceivedRepository(BksArditaDevContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<object>> GetByFilterModelArchiveMovement(DataTableModel model)
    {
        var result = await _context.TrxArchiveMovements
                .Include(x => x.StatusReceivedNavigation)
                .Where(x => x.StatusId == (int)GlobalConst.STATUS.Approved && (x.MovementCode + x.MovementName + x.Note + x.StatusReceivedNavigation!.Name).Contains(model.searchValue))
                .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                .Skip(model.skip).Take(model.pageSize)
                .Select(x => new {
                    x.ArchiveMovementId,
                    x.MovementCode, 
                    x.MovementName,
                    x.StatusReceived,
                    x.Note,
                    x.StatusReceivedNavigation!.Color,
                    Status = x.StatusReceivedNavigation!.Name
                })
                .ToListAsync();

        return result;
    }

    public async Task<int> GetCountByFilterDataArchiveMovement(DataTableModel model)
    {
        var result = await _context.TrxArchiveMovements
                 .Include(x => x.Status)
                 .Where(x => x.StatusId == (int)GlobalConst.STATUS.Approved && (x.MovementCode + x.MovementName + x.StatusReceivedNavigation!.Name).Contains(model.searchValue!))
                 .CountAsync();

        return result;
    }

    public async Task<TrxArchiveMovement> GetById(Guid id)
    {
        var result = await _context.TrxArchiveMovements.AsNoTracking().FirstOrDefaultAsync(x => x.ArchiveMovementId == id);

        return result ?? new TrxArchiveMovement();
    }

    public async Task<int> Update(TrxArchiveMovement model)
    {
        int result = 0;

        if (model != null && model.ArchiveMovementId != Guid.Empty)
        {
            var data = await _context.TrxArchiveMovements.AsNoTracking().FirstAsync(x => x.ArchiveMovementId == model.ArchiveMovementId);
            if (data != null)
            {
                data.StatusReceived = (int)GlobalConst.STATUS.ArchiveReceived;
                data.DescriptionReceived = model.DescriptionReceived;
                data.ReceivedBy = model.ReceivedBy;
                data.DateReceived = model.DateReceived;
                _context.Update(data);
                result = await _context.SaveChangesAsync();
            }
        }
        return result;
    }
}
