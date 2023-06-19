using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ardita.Repositories.Classess;

public class ArchiveOutIndicatorRepository : IArchiveOutIndicatorRepository
{
    private readonly BksArditaDevContext _context;

    public ArchiveOutIndicatorRepository(BksArditaDevContext context) => _context = context;
    public async Task<TrxArchiveOutIndicator> GetByUseAndDate(Guid archiveId, string usedBy, DateTime usedDate)
    {
        return await _context.TrxArchiveOutIndicators.AsNoTracking().Where(x => x.ArchiveId == archiveId && x.UsedBy.ToLower() == usedBy.ToLower() && x.UsedDate.Date == usedDate.Date).OrderByDescending(x => x.CreatedDate).FirstOrDefaultAsync();
    }
    public async Task<IEnumerable<TrxArchiveOutIndicator>> GetByMediaStorageId(Guid mediaStorageId)
    {
        return await _context.TrxArchiveOutIndicators.Include(x => x.Archive.ArchiveType).Include(x => x.Archive.Creator).AsNoTracking().Where(x => x.MediaStorageId == mediaStorageId).OrderBy(x => x.CreatedDate).ToListAsync();
    }

    public async Task<int> Insert(TrxArchiveOutIndicator model)
    {
        int result = 0;

        if (model != null)
        {
            _context.TrxArchiveOutIndicators.Add(model);
            result = await _context.SaveChangesAsync();
        }
        return result;
    }
    public async Task<bool> InsertBulk(List<TrxArchiveOutIndicator> models)
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
    public async Task<int> Update(TrxArchiveOutIndicator model)
    {
        int result = 0;

        if (model != null)
        {
            _context.Update(model);
            result = await _context.SaveChangesAsync();
        }
        return result;
    }
}
