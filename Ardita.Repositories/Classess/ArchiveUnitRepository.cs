using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Linq.Dynamic.Core;
using Ardita.Extensions;
using Ardita.Models;

namespace Ardita.Repositories.Classess;

public class ArchiveUnitRepository : IArchiveUnitRepository
{
    private readonly BksArditaDevContext _context;
    private readonly ILogChangesRepository _logChangesRepository;
    public ArchiveUnitRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
    {
        _context = context;
        _logChangesRepository = logChangesRepository;
    }

    public async Task<int> Delete(TrxArchiveUnit model)
    {
        int result = 0;

        if (model.ArchiveUnitId != Guid.Empty)
        {
            var data = await _context.TrxArchiveUnits.AsNoTracking().FirstAsync(x => x.ArchiveUnitId== model.ArchiveUnitId);
            if (data != null)
            {
                data.IsActive = false;
                data.UpdatedDate = model.UpdatedDate;
                data.UpdatedBy = model.UpdatedBy;
                _context.TrxArchiveUnits.Update(data);
                result = await _context.SaveChangesAsync();

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<TrxArchiveUnit>(GlobalConst.Delete, (Guid)model.UpdatedBy!, new List<TrxArchiveUnit> { data }, new List<TrxArchiveUnit> { });
                    }
                    catch (Exception ex) { }
                }
            }
        }
        return result;
    }

    public async Task<IEnumerable<TrxArchiveUnit>> GetAll(string par = " 1=1 ") 
        => await _context.TrxArchiveUnits
        .Include(x=> x.Company)
        .Where(x => x.IsActive == true)
        .Where(x => x.Company.IsActive == true)
        .Where(par)
        .AsNoTracking()
        .ToListAsync();
    public async Task<IEnumerable<object>> GetArchiveUnitGroupByArchiveCount(GlobalSearchModel search, string par = " 1=1 ")
    {
        return await _context.TrxArchiveUnits
            .Include(x => x.Company)
            .Where(x => x.IsActive == true)
            .Where(x => x.Company.IsActive == true)
            .Where(par)
            .AsNoTracking()
            .Select(y => new
            {
                name = y.ArchiveUnitName,
                totalArchive = (_context.TrxArchives
                            .Include(x => x.SubSubjectClassification)
                            .Include(x => x.SecurityClassification)
                            .Include(x => x.Creator)
                            .Include(x => x.ArchiveOwner)
                            .Include(x => x.ArchiveType)
                            .Include(x => x.TrxFileArchiveDetails)
                            .Where(x => x.TrxFileArchiveDetails.FirstOrDefault() == null)
                            .Where(x => x.IsActive == true)
                            .Where(x => x.SubSubjectClassification.IsActive == true)
                            .Where(x => x.SecurityClassification.IsActive == true)
                            .Where(x => x.Creator.IsActive == true)
                            .Where(x => x.ArchiveOwner.IsActive == true)
                            .Where(x => x.ArchiveType.IsActive == true)
                            .Where(x => x.Creator.ArchiveUnitId == y.ArchiveUnitId)
                            .Where(x => search.StatusId == null ? true : x.StatusId == search.StatusId)
                            .Where(x => search.IsArchiveActive == null ? true : x.IsArchiveActive == search.IsArchiveActive)
                            .Where(x => search.ArchiveUnitId == null ? true : x.Creator.ArchiveUnitId == search.ArchiveUnitId)
                            .Where(x => search.CreatorId == null ? true : x.CreatorId == search.CreatorId)
                            .Count()),
                totalArchiveDigital = (_context.TrxArchives
                            .Include(x => x.SubSubjectClassification)
                            .Include(x => x.SecurityClassification)
                            .Include(x => x.Creator)
                            .Include(x => x.ArchiveOwner)
                            .Include(x => x.ArchiveType)
                            .Include(x => x.TrxFileArchiveDetails)
                            .Where(x => x.TrxFileArchiveDetails.FirstOrDefault() != null)
                            .Where(x => x.IsActive == true)
                            .Where(x => x.SubSubjectClassification.IsActive == true)
                            .Where(x => x.SecurityClassification.IsActive == true)
                            .Where(x => x.Creator.IsActive == true)
                            .Where(x => x.ArchiveOwner.IsActive == true)
                            .Where(x => x.ArchiveType.IsActive == true)
                            .Where(x => x.Creator.ArchiveUnitId == y.ArchiveUnitId)
                            .Where(x => search.StatusId == null ? true : x.StatusId == search.StatusId)
                            .Where(x => search.IsArchiveActive == null ? true : x.IsArchiveActive == search.IsArchiveActive)
                            .Where(x => search.ArchiveUnitId == null ? true : x.Creator.ArchiveUnitId == search.ArchiveUnitId)
                            .Where(x => search.CreatorId == null ? true : x.CreatorId == search.CreatorId)
                            .Count()),
                total = (_context.TrxArchives
                            .Include(x => x.SubSubjectClassification)
                            .Include(x => x.SecurityClassification)
                            .Include(x => x.Creator)
                            .Include(x => x.ArchiveOwner)
                            .Include(x => x.ArchiveType)
                            .Where(x => x.IsActive == true)
                            .Where(x => x.SubSubjectClassification.IsActive == true)
                            .Where(x => x.SecurityClassification.IsActive == true)
                            .Where(x => x.Creator.IsActive == true)
                            .Where(x => x.ArchiveOwner.IsActive == true)
                            .Where(x => x.ArchiveType.IsActive == true)
                            .Where(x => x.Creator.ArchiveUnitId == y.ArchiveUnitId)
                            .Where(x => search.StatusId == null ? true : x.StatusId == search.StatusId)
                            .Where(x => search.IsArchiveActive == null ? true : x.IsArchiveActive == search.IsArchiveActive)
                            .Where(x => search.ArchiveUnitId == null ? true : x.Creator.ArchiveUnitId == search.ArchiveUnitId)
                            .Where(x => search.CreatorId == null ? true : x.CreatorId == search.CreatorId)
                            .Count())
            })
            .OrderByDescending(x => x.total)
            .ToListAsync();
    }

    public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
    {
        var result = await _context.TrxArchiveUnits
                .Include(x => x.Company)
                .Where(x => x.Company.IsActive == true)
                .Where(x => (x.ArchiveUnitCode + x.ArchiveUnitName + x.Company.CompanyName).Contains(model.searchValue) && x.IsActive == true)
                .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                .Skip(model.skip).Take(model.pageSize)
                .Select(x => new {
                     x.ArchiveUnitId,
                     x.ArchiveUnitCode,
                     x.ArchiveUnitName,
                     x.Company.CompanyName
                 })
                 .ToListAsync();

        return result;
    }
    public async Task<int> GetCountByFilterModel(DataTableModel model)
    {
        var result = await _context.TrxArchiveUnits
                .Include(x => x.Company)
                .Where(x => x.Company.IsActive == true)
                .Where(x => (x.ArchiveUnitCode + x.ArchiveUnitName + x.Company.CompanyName).Contains(model.searchValue) && x.IsActive == true)
                .CountAsync();

        return result;
    }

    public async Task<TrxArchiveUnit> GetById(Guid id) => await _context.TrxArchiveUnits.Where(x => x.ArchiveUnitId == id).FirstAsync();

    public async Task<int> GetCount() => await _context.TrxArchiveUnits.CountAsync(x => x.IsActive == true);

    public async Task<int> Insert(TrxArchiveUnit model)
    {
        int result = 0;

        if (model != null)
        {
            model.IsActive = true;
            _context.TrxArchiveUnits.Add(model);
            result = await _context.SaveChangesAsync();

            //Log
            if (result > 0)
            {
                try
                {
                    await _logChangesRepository.CreateLog<TrxArchiveUnit>(GlobalConst.New, (Guid)model.CreatedBy!, new List<TrxArchiveUnit> {  }, new List<TrxArchiveUnit> { model});
                }
                catch (Exception ex) { }
            }
        }
        return result;
    }

    public async Task<bool> InsertBulk(List<TrxArchiveUnit> trxArchiveUnits)
    {
        bool result = false;
        if (trxArchiveUnits.Count() > 0)
        {
            await _context.AddRangeAsync(trxArchiveUnits);
            await _context.SaveChangesAsync();
            result = true;

            //Log
            if (result)
            {
                try
                {
                    await _logChangesRepository.CreateLog<TrxArchiveUnit>(GlobalConst.New, (Guid)trxArchiveUnits.FirstOrDefault()!.CreatedBy!, new List<TrxArchiveUnit> {  }, trxArchiveUnits);
                }
                catch (Exception ex) { }
            }
        }
        return result;
    }

    public async Task<int> Update(TrxArchiveUnit model)
    {
        int result = 0;

        if (model != null && model.ArchiveUnitId != Guid.Empty)
        {
            var data = await _context.TrxArchiveUnits.AsNoTracking().FirstAsync(x => x.ArchiveUnitId == model.ArchiveUnitId);
            if (data != null)
            {
                model.IsActive = data.IsActive;
                model.CreatedBy = data.CreatedBy;
                model.CreatedDate = data.CreatedDate;
                _context.TrxArchiveUnits.Update(model);
                result = await _context.SaveChangesAsync();

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<TrxArchiveUnit>(GlobalConst.Update, (Guid)model.UpdatedBy!, new List<TrxArchiveUnit> { data }, new List<TrxArchiveUnit> { model });
                    }
                    catch (Exception ex) { }
                }
            }
        }
        return result;
    }
    public async Task<IEnumerable<TrxArchiveUnit>> GetByListArchiveUnit(List<string> listArchiveUnitCode)
    {
        return await _context.TrxArchiveUnits
            .Where($"{(listArchiveUnitCode.Count > 0 ? "@0.Contains(ArchiveUnitCode)" : "1=1")} ", listArchiveUnitCode)
            .Where(x => x.IsActive == true)
            .ToListAsync();
    }
}
