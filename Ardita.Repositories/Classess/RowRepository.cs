﻿using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ardita.Repositories.Classess;

public class RowRepository : IRowRepository
{
    private readonly BksArditaDevContext _context;

    public RowRepository(BksArditaDevContext context)
    {
        _context = context;
    }

    public async Task<int> Delete(TrxRow model)
    {
        int result = 0;

        if (model != null)
        {
            if (model.RowId != Guid.Empty)
            {
                var data = await _context.TrxRows.AsNoTracking().FirstAsync(x => x.RowId == model.RowId && x.IsActive == true);
                if (data != null)
                {
                    model.CreatedBy = data.CreatedBy;
                    model.CreatedDate = data.CreatedDate;
                    model.IsActive = false;

                    model.Level = null;
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }

        }
        return result;
    }

    public async Task<IEnumerable<TrxRow>> GetAll()
    {
        var results = await _context.TrxRows
            .Include(x => x.TrxMediaStorages)
            .Include(x => x.Level!.Rack!.Room!.Floor!.ArchiveUnit)
            .Where(x => x.IsActive == true)
            .Where(x => x.Level!.IsActive == true)
            .Where(x => x.Level!.Rack!.IsActive == true)
            .Where(x => x.Level!.Rack!.Room!.IsActive == true)
            .Where(x => x.Level!.Rack!.Room!.Floor!.IsActive == true)
            .Where(x => x.Level!.Rack!.Room!.Floor!.ArchiveUnit!.IsActive == true)
            .AsNoTracking()
            .ToListAsync();
        return results;
    }

    public async Task<IEnumerable<TrxRow>> GetByFilterModel(DataTableModel model)
    {
        IEnumerable<TrxRow> result;

        var propertyInfo = typeof(TrxLevel).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        var propertyName = propertyInfo == null ? typeof(TrxLevel).GetProperties()[0].Name : propertyInfo.Name;

        if (model.sortColumnDirection.ToLower() == "asc")
        {
            result = await _context.TrxRows
            .Include(x => x.Level.Rack.Room)
            .Where(
                x => (x.RowId + x.RowName).Contains(model.searchValue) &&
                x.IsActive == true
                )
            .OrderBy(x => EF.Property<TrxRow>(x, propertyName))
            .Skip(model.skip).Take(model.pageSize)
            .ToListAsync();
        }
        else
        {
            result = await _context.TrxRows
            .Include(x => x.Level.Rack.Room)
            .Where(
                x => (x.RowId + x.RowName).Contains(model.searchValue) &&
                x.IsActive == true
                )
            .OrderByDescending(x => EF.Property<TrxClassification>(x, propertyName))
            .Skip(model.skip).Take(model.pageSize)
            .ToListAsync();
        }

        return result;
    }

    public async Task<TrxRow> GetById(Guid id)
    {
        var result = await _context.TrxRows
            .Include(x => x.Level.Rack.Room.Floor.ArchiveUnit)
            .AsNoTracking()
            .FirstAsync(x => x.RowId == id && x.IsActive == true);
        return result;
    }

    public async Task<int> GetCount()
    {
        var results = await _context.TrxRows.AsNoTracking().Where(x => x.IsActive == true).CountAsync();
        return results;
    }

    public async Task<IEnumerable<TrxRow>> GetRowAvailable(IEnumerable<TrxRow> listRowNotAvailable)
    {
        var results = await _context.TrxRows
            .Include(x => x.TrxMediaStorages)
            .Include(x => x.Level!.Rack!.Room!.Floor!.ArchiveUnit)
            .Where(x => x.IsActive == true)
            .Where(x => x.Level!.IsActive == true)
            .Where(x => x.Level!.Rack!.IsActive == true)
            .Where(x => x.Level!.Rack!.Room!.IsActive == true)
            .Where(x => x.Level!.Rack!.Room!.Floor!.IsActive == true)
            .Where(x => x.Level!.Rack!.Room!.Floor!.ArchiveUnit!.IsActive == true)
            .Where(x => !listRowNotAvailable.Contains(x))
            .AsNoTracking()
            .ToListAsync();

        return results;
    }

    public async Task<IEnumerable<TrxRow>> GetRowNotAvailable()
    {
        var result = await _context.TrxRows
            .Include(x => x.TrxMediaStorageInActives)
            .Where(x => x.TrxMediaStorageInActives!.FirstOrDefault()!.IsActive == true)
            .AsNoTracking()
            .ToListAsync();

        return result;
    }

    public async Task<int> Insert(TrxRow model)
    {
        int result = 0;

        if (model != null)
        {
            model.IsActive = true;
            model.Level = null;
            _context.TrxRows.Add(model);
            result = await _context.SaveChangesAsync();
        }
        return result;
    }

    public async Task<bool> InsertBulk(List<TrxRow> rows)
    {
        bool result = false;
        if (rows.Count() > 0)
        {
            await _context.AddRangeAsync(rows);
            await _context.SaveChangesAsync();
            result = true;
        }
        return result;
    }

    public async Task<int> Update(TrxRow model)
    {
        int result = 0;

        if (model != null)
        {
            if (model.RowId != Guid.Empty)
            {
                var data = await _context.TrxRows.AsNoTracking().FirstAsync(x => x.RowId == model.RowId);
                if (data != null)
                {
                    model.CreatedBy = data.CreatedBy;
                    model.CreatedDate = data.CreatedDate;
                    model.IsActive = true;

                    model.Level = null;
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }

        }
        return result;
    }
}
