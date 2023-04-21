using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Ardita.Repositories.Classess;

public class SecurityClassificationRepository : ISecurityClassificationRepository
{
    private readonly BksArditaDevContext _context;

    public SecurityClassificationRepository(BksArditaDevContext context) => _context = context;

    public async Task<int> Delete(MstSecurityClassification model)
    {
        int result = 0;

        var data = await _context.MstSecurityClassifications.AsNoTracking().FirstAsync(x => x.SecurityClassificationId == model.SecurityClassificationId);
        if (model.SecurityClassificationId != Guid.Empty)
        {
            model.IsActive = false;
            model.CreatedBy = data.CreatedBy;
            model.CreatedDate = data.CreatedDate;
            _context.MstSecurityClassifications.Remove(model);
            result = await _context.SaveChangesAsync();
        }
        return result;
    }

    public async Task<IEnumerable<MstSecurityClassification>> GetAll() => await _context.MstSecurityClassifications.Where(x => x.IsActive == true).ToListAsync();
    

    public async Task<IEnumerable<MstSecurityClassification>> GetByFilterModel(DataTableModel model)
    {
        IEnumerable<MstSecurityClassification> result;

        var propertyInfo = typeof(MstSecurityClassification).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        var propertyName = propertyInfo == null ? typeof(MstSecurityClassification).GetProperties()[0].Name : propertyInfo.Name;

        if (model.sortColumnDirection.ToLower() == "asc")
        {
            result = await _context.MstSecurityClassifications
            .Where(x => (x.SecurityClassificationCode + x.SecurityClassificationName).Contains(model.searchValue) && x.IsActive)
            .OrderBy(x => EF.Property<MstSecurityClassification>(x, propertyName))
            .Skip(model.skip).Take(model.pageSize)
            .ToListAsync();
        }
        else
        {
            result = await _context.MstSecurityClassifications
            .Where(x => (x.SecurityClassificationCode + x.SecurityClassificationName).Contains(model.searchValue) && x.IsActive)
            .OrderByDescending(x => EF.Property<MstSecurityClassification>(x, propertyName))
            .Skip(model.skip).Take(model.pageSize)
            .ToListAsync();
        }

        return result;
    }

    public async Task<IEnumerable<MstSecurityClassification>> GetById(Guid id) => await _context.MstSecurityClassifications.Where(x => x.SecurityClassificationId == id).ToListAsync();

    public async Task<int> GetCount() => await _context.MstSecurityClassifications.CountAsync(x => x.IsActive == true);

    public async Task<int> Insert(MstSecurityClassification model)
    {
        int result = 0;

        if (model != null)
        {
            model.IsActive = true;
            _context.MstSecurityClassifications.Add(model);
            result = await _context.SaveChangesAsync();
        }
        return result;
    }

    public async Task<int> Update(MstSecurityClassification model)
    {
        int result = 0;

        if (model != null && model.SecurityClassificationId != Guid.Empty)
        {
            var data = await _context.MstSecurityClassifications.AsNoTracking().FirstAsync(x => x.SecurityClassificationId == model.SecurityClassificationId);
            if (data != null)
            {
                model.CreatedBy = data.CreatedBy;
                model.CreatedDate = data.CreatedDate;
                model.IsActive = data.IsActive;
                _context.Update(model);
                result = await _context.SaveChangesAsync();
            }
        }
        return result;
    }
}
