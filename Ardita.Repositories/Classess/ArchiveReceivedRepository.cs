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
        var User = AppUsers.CurrentUser(model.SessionUser);
        var result = await _context.TrxArchiveMovements
                .Include(x => x.TrxArchiveMovementDetails).ThenInclude(x => x.Archive.SubSubjectClassification.SubjectClassification.Classification)
                .Include(x => x.TrxArchiveMovementDetails).ThenInclude(x => x.Archive.TrxMediaStorageDetails).ThenInclude(x => x.MediaStorage.Row.Level.Rack.Room.Floor)
                .Include(x => x.TrxArchiveMovementDetails).ThenInclude(x => x.Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive.Row.Level.Rack.Room.Floor)
                .Include(x => x.StatusReceivedNavigation)
                .Include(x => x.CreatedByNavigation.Creator)
                .Include(x => x.CreatedByNavigation.Employee)
                .Include(x => x.ReceivedByNavigation.Employee)
                .Where(x => x.StatusId == (int)GlobalConst.STATUS.Approved && (x.MovementCode + x.MovementName + x.Note + x.StatusReceivedNavigation!.Name).Contains(model.searchValue!))
                .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.ArchiveUnitIdDestination == User.ArchiveUnitId))
                .Where(model.advanceSearch!.Search)
                .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                .Skip(model.skip).Take(model.pageSize)
                .Select(x => new {
                    x.ArchiveMovementId,
                    x.MovementCode,
                    DateReceived = x.DateReceived.ToString(),
                    x.ReceivedNumber,
                    x.CreatedByNavigation.Creator!.CreatorName,
                    CreatedBy = x.CreatedByNavigation.Employee.Name,
                    ReceivedBy = x.ReceivedByNavigation != null ? x.ReceivedByNavigation.Employee.Name : "",
                    x.StatusReceived,
                    x.DescriptionReceived,
                    x.StatusReceivedNavigation!.Color,
                    Status = x.StatusReceivedNavigation!.Name,
                    x.ArchiveUnitIdFromNavigation.ArchiveUnitName
                })
                .ToListAsync();

        return result;
    }

    public async Task<int> GetCountByFilterDataArchiveMovement(DataTableModel model)
    {
        var User = AppUsers.CurrentUser(model.SessionUser);
        var result = await _context.TrxArchiveMovements
                .Include(x => x.TrxArchiveMovementDetails).ThenInclude(x => x.Archive.SubSubjectClassification.SubjectClassification.Classification)
                .Include(x => x.TrxArchiveMovementDetails).ThenInclude(x => x.Archive.TrxMediaStorageDetails).ThenInclude(x => x.MediaStorage.Row.Level.Rack.Room.Floor)
                .Include(x => x.TrxArchiveMovementDetails).ThenInclude(x => x.Archive.TrxMediaStorageInActiveDetails).ThenInclude(x => x.MediaStorageInActive.Row.Level.Rack.Room.Floor)
                .Include(x => x.StatusReceivedNavigation)
                .Include(x => x.CreatedByNavigation.Creator)
                .Include(x => x.CreatedByNavigation.Employee)
                .Include(x => x.ReceivedByNavigation.Employee)
                .Where(x => x.StatusId == (int)GlobalConst.STATUS.Approved && (x.MovementCode + x.MovementName + x.Note + x.StatusReceivedNavigation!.Name).Contains(model.searchValue!))
                .Where(x => (User.ArchiveUnitId == Guid.Empty ? true : x.ArchiveUnitIdDestination == User.ArchiveUnitId))
                .Where(model.advanceSearch!.Search)
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
            var data = await _context.TrxArchiveMovements.Include(x => x.ArchiveUnitIdDestinationNavigation).AsNoTracking().FirstAsync(x => x.ArchiveMovementId == model.ArchiveMovementId);
            if (data != null)
            {
                var statusWait = (int)GlobalConst.STATUS.ArchiveNotReceived;
                var count = await _context.TrxArchiveMovements.AsNoTracking().Where(x => x.StatusReceived != statusWait).CountAsync();
                bool available = false;
                var receivedNumber = string.Empty;
                while(available == false)
                {
                    receivedNumber = $"{data.ArchiveUnitIdDestinationNavigation.ArchiveUnitCode}.{count.ToString("D3")}.{DateTime.Now.ToString("MM.yyyy")}";
                    var coundCode = await _context.TrxArchiveMovements.AsNoTracking().Where(x => x.ReceivedNumber == receivedNumber).CountAsync();
                    if (coundCode > 0)
                        count++;
                    else
                        available = true;
                }

                data.ArchiveUnitIdDestinationNavigation = null;
                data.ReceivedNumber = receivedNumber;
                data.StatusReceived = model.StatusReceived;
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
