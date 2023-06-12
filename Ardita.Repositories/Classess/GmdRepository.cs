using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ardita.Repositories.Classess;

public class GmdRepository : IGmdRepository
{
    private readonly BksArditaDevContext _context;

    public GmdRepository(BksArditaDevContext context) => _context = context;

    public async Task<int> Delete(MstGmd model)
    {
        int result = 0;

        if (model.GmdId != Guid.Empty)
        {
            var data = await _context.MstGmds.AsNoTracking().FirstAsync(x => x.GmdId == model.GmdId);
            if (data != null)
            {
                model.IsActive = false;
                model.CreatedBy = data.CreatedBy;
                model.CreatedDate = data.CreatedDate;
                _context.Update(model);
                result = await _context.SaveChangesAsync();
            }
        }
        return result;
    }

    public async Task<IEnumerable<MstGmd>> GetAll() => await _context.MstGmds.Where(x => x.IsActive == true).AsNoTracking().ToListAsync();

    public async Task<IEnumerable<MstGmdDetail>> GetDetailByGmdId(Guid Id) => await _context.MstGmdDetails.Where(x => x.GmdId == Id).AsNoTracking().ToListAsync();
    
    public async Task<IEnumerable<MstGmd>> GetByFilterModel(DataTableModel model)
    {
        IEnumerable<MstGmd> result;

        var propertyInfo = typeof(MstGmd).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        var propertyName = propertyInfo == null ? typeof(MstGmd).GetProperties()[0].Name : propertyInfo.Name;

        if (model.sortColumnDirection.ToLower() == "asc")
        {
            result = await _context.MstGmds
                .Where(x => (x.GmdCode + x.GmdName)
                .Contains(model.searchValue) && x.IsActive)
                .OrderBy(x => EF.Property<MstGmd>(x, propertyName)).ThenBy(x => x.CreatedDate).ThenBy(x => x.UpdatedDate)
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
        }
        else
        {
            result = await _context.MstGmds
                .Where(x => (x.GmdCode + x.GmdName)
                .Contains(model.searchValue) && x.IsActive)
                .OrderByDescending(x => EF.Property<MstGmd>(x, propertyName)).ThenBy(x => x.CreatedDate).ThenBy(x => x.UpdatedDate)
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
        }

        return result;
    }

    public async Task<IEnumerable<MstGmd>> GetById(Guid id) 
        => await _context.MstGmds
        .Include(x => x.MstGmdDetails)
        .Where(x => x.GmdId== id)
        .AsNoTracking()
        .ToListAsync();

    public async Task<int> GetCount() => await _context.MstGmds.CountAsync(x => x.IsActive == true);

    public async Task<int> Insert(MstGmd model, List<MstGmdDetail> details)
    {
        int result = 0;
        List<MstGmdDetail> gmdDetails = new();


        if (model != null)
        {
            model.IsActive = true;
            _context.MstGmds.Add(model);
            result = await _context.SaveChangesAsync();

            foreach (var item in details)
            {
                item.CreatedDate = model.CreatedDate;
                item.CreatedBy = model.CreatedBy;
                item.GmdId = model.GmdId;

                gmdDetails.Add(item);
            }

            _context.MstGmdDetails.AddRange(gmdDetails);
            result += await _context.SaveChangesAsync();
        }
        return result;
    }

    public async Task<bool> InsertBulk(List<MstGmd> mstGmds)
    {
        bool result = false;
        if (mstGmds.Count() > 0)
        {
            await _context.AddRangeAsync(mstGmds);
            await _context.SaveChangesAsync();
            result = true;
        }
        return result;
    }

    public async Task<int> Update(MstGmd model, List<MstGmdDetail> details)
    {
        int result = 0;
        List<MstGmdDetail> gmdDetails = new();
        List<MstGmdDetail> oldGmdDetails = new();

        if (model != null && model.GmdId != Guid.Empty)
        {
            var data = await _context.MstGmds.AsNoTracking().FirstAsync(x => x.GmdId == model.GmdId);
            if (data != null)
            {
                //Insert Header
                model.CreatedBy = data.CreatedBy;
                model.CreatedDate = data.CreatedDate;
                model.IsActive = data.IsActive;
                _context.Update(model);
                result = await _context.SaveChangesAsync();

                //Insert Detail
                oldGmdDetails = await _context.MstGmdDetails.AsNoTracking().Where(x => x.GmdId == model.GmdId).ToListAsync();
                if (oldGmdDetails.Any())
                {
                    _context.MstGmdDetails.RemoveRange(oldGmdDetails);
                    result += await _context.SaveChangesAsync();
                }


                foreach (var item in details)
                {
                    item.CreatedDate = model.CreatedDate;
                    item.CreatedBy = model.CreatedBy;
                    item.GmdId = model.GmdId;

                    gmdDetails.Add(item);
                }

                _context.MstGmdDetails.AddRange(gmdDetails);
                result += await _context.SaveChangesAsync();
            }
        }
        return result;
    }

    public async Task<MstGmdDetail> GetDetailById(Guid Id) => await _context.MstGmdDetails.AsNoTracking().FirstAsync(x => x.GmdDetailId == Id);

    public async Task<IEnumerable<MstGmdDetail>> GetAllDetail() => await _context.MstGmdDetails.AsNoTracking().ToListAsync();
}
