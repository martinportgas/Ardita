using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Linq.Dynamic.Core;
using Ardita.Extensions;

namespace Ardita.Repositories.Classess;

public class ArchiveCreatorRepository : IArchiveCreatorRepository
{
    private readonly BksArditaDevContext _context;

    private readonly ILogChangesRepository _logChangesRepository;

    public ArchiveCreatorRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
    {
        _context = context;
        _logChangesRepository = logChangesRepository;
    }

    public async Task<int> Delete(MstCreator model)
    {
        int result = 0;

        if (model.CreatorId != Guid.Empty)
        {
            var data = await _context.MstCreators.AsNoTracking().FirstAsync(x => x.CreatorId == model.CreatorId);
            if (data != null)
            {
                data.IsActive = false;
                data.UpdatedDate = model.UpdatedDate;
                data.UpdatedBy = model.UpdatedBy;
                _context.MstCreators.Update(data);
                result = await _context.SaveChangesAsync();

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<MstCreator>(GlobalConst.Delete, (Guid)model!.UpdatedBy!, new List<MstCreator> { data }, new List<MstCreator> { });
                    }
                    catch (Exception ex) { }
                }
            }
        }
        return result;
    }

    public async Task<IEnumerable<MstCreator>> GetAll(string par = " 1=1 ") 
        => await _context.MstCreators
        .Include(x => x.TrxArchives)
        .Include(x=> x.ArchiveUnit)
        .ThenInclude(x=> x.Company)
        .Where(x => x.IsActive == true)
        .Where(par)
        .ToListAsync();

    public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
    {
        var result = await _context.MstCreators
                .Include(x => x.ArchiveUnit.Company)
                .Where(x => (x.CreatorCode + x.CreatorName + x.ArchiveUnit.ArchiveUnitName + x.ArchiveUnit.Company.CompanyName).Contains(model.searchValue) && x.IsActive == true)
                .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                .Skip(model.skip).Take(model.pageSize)
                .Select(x => new {
                   x.CreatorId,
                   x.CreatorCode,
                   x.CreatorName,
                   x.ArchiveUnit.ArchiveUnitName,
                   x.ArchiveUnit.Company.CompanyName
                })
                .ToListAsync();

        return result;
    }
    public async Task<int> GetCountByFilterModel(DataTableModel model)
    {
        var result = await _context.MstCreators
                .Include(x => x.ArchiveUnit.Company)
                .Where(x => (x.CreatorCode + x.CreatorName).Contains(model.searchValue) && x.IsActive == true)
                .CountAsync();

        return result;
    }

    public async Task<IEnumerable<MstCreator>> GetById(Guid id) => await _context.MstCreators.Where(x => x.CreatorId== id).Include(x => x.ArchiveUnit).AsNoTracking().ToListAsync();

    public async Task<int> GetCount() => await _context.MstCreators.CountAsync(x => x.IsActive == true);

    public async Task<int> Insert(MstCreator model)
    {
        int result = 0;

        if (model != null)
        {
            model.IsActive = true;
            _context.MstCreators.Add(model);
            result = await _context.SaveChangesAsync();

            //Log
            if (result > 0)
            {
                try
                {
                    await _logChangesRepository.CreateLog<MstCreator>(GlobalConst.New, (Guid)model!.CreatedBy!, new List<MstCreator> {  }, new List<MstCreator> { model });
                }
                catch (Exception ex) { }
            }
        }
        return result;
    }

    public async Task<bool> InsertBulk(List<MstCreator> mstCreators)
    {
        bool result = false;
        if (mstCreators.Count() > 0)
        {
            await _context.AddRangeAsync(mstCreators);
            await _context.SaveChangesAsync();
            result = true;

            //Log
            if (result)
            {
                try
                {
                    await _logChangesRepository.CreateLog<MstCreator>(GlobalConst.New, (Guid)mstCreators.FirstOrDefault()!.CreatedBy!, new List<MstCreator> {  }, mstCreators);
                }
                catch (Exception ex) { }
            }
        }
        return result;
    }

    public async Task<int> Update(MstCreator model)
    {
        int result = 0;

        if (model != null && model.CreatorId != Guid.Empty)
        {
            var data = await _context.MstCreators.AsNoTracking().FirstAsync(x => x.CreatorId == model.CreatorId);
            if (data != null)
            {
                model.IsActive = data.IsActive;
                model.CreatedBy = data.CreatedBy;
                model.CreatedDate = data.CreatedDate;
                _context.MstCreators.Update(model);
                result = await _context.SaveChangesAsync();

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<MstCreator>(GlobalConst.Update, (Guid)model!.UpdatedBy!, new List<MstCreator> { data }, new List<MstCreator> { model });
                    }
                    catch (Exception ex) { }
                }
            }
        }
        return result;
    }
}
