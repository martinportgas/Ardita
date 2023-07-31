﻿using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Linq.Dynamic.Core;

namespace Ardita.Repositories.Classess;

public class ArchiveUnitRepository : IArchiveUnitRepository
{
    private readonly BksArditaDevContext _context;

    public ArchiveUnitRepository(BksArditaDevContext context) => _context = context;

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
            }
        }
        return result;
    }

    public async Task<IEnumerable<TrxArchiveUnit>> GetAll() 
        => await _context.TrxArchiveUnits
        .Include(x=> x.Company)
        .Where(x => x.IsActive == true)
        .Where(x => x.Company.IsActive == true)
        .AsNoTracking()
        .ToListAsync();

    public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
    {
        var result = await _context.TrxArchiveUnits
                .Include(x => x.Company)
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
