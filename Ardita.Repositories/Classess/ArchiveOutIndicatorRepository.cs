using Ardita.Extensions;
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

    private readonly ILogChangesRepository _logChangesRepository;

    public ArchiveOutIndicatorRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
    {
        _context = context;
        _logChangesRepository = logChangesRepository;
    }
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

            //Log
            if (result > 0)
            {
                try
                {
                    await _logChangesRepository.CreateLog<TrxArchiveOutIndicator>(GlobalConst.New, (Guid)model!.CreatedBy!, new List<TrxArchiveOutIndicator> {  }, new List<TrxArchiveOutIndicator> { model });
                }
                catch (Exception ex) { }
            }
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

            //Log
            if (result)
            {
                try
                {
                    await _logChangesRepository.CreateLog<TrxArchiveOutIndicator>(GlobalConst.New, (Guid)models.FirstOrDefault()!.CreatedBy!, new List<TrxArchiveOutIndicator> { }, models);
                }
                catch (Exception ex) { }
            }
        }
        return result;
    }
    public async Task<int> Update(TrxArchiveOutIndicator model)
    {
        int result = 0;
        var data = await _context.TrxArchiveOutIndicators.AsNoTracking().FirstOrDefaultAsync(x => x.ArchiveOutIndicatorId == model.ArchiveOutIndicatorId);
        if (data != null)
        {
            _context.Update(model);
            result = await _context.SaveChangesAsync();

            //Log
            if (result > 0)
            {
                try
                {
                    await _logChangesRepository.CreateLog<TrxArchiveOutIndicator>(GlobalConst.Update, (Guid)model!.UpdatedBy!, new List<TrxArchiveOutIndicator> { data }, new List<TrxArchiveOutIndicator> { model });
                }
                catch (Exception ex) { }
            }
        }
        return result;
    }
}
