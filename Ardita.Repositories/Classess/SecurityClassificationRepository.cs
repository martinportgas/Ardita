using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Linq.Dynamic.Core;
using Ardita.Extensions;

namespace Ardita.Repositories.Classess;

public class SecurityClassificationRepository : ISecurityClassificationRepository
{
    private readonly BksArditaDevContext _context;
    private readonly ILogChangesRepository _logChangesRepository;
    public SecurityClassificationRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
    {
        _context = context;
        _logChangesRepository = logChangesRepository;
    }

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

            //Log
            if (result > 0)
            {
                try
                {
                    await _logChangesRepository.CreateLog<MstSecurityClassification>(GlobalConst.Delete, model.CreatedBy, new List<MstSecurityClassification> { data }, new List<MstSecurityClassification> {  });
                }
                catch (Exception ex) { }
            }
        }
        return result;
    }

    public async Task<IEnumerable<MstSecurityClassification>> GetAll() => await _context.MstSecurityClassifications.Where(x => x.IsActive == true).ToListAsync();
    

    public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
    {
        var result = await _context.MstSecurityClassifications
            .Where(x => (x.SecurityClassificationCode + x.SecurityClassificationName).Contains(model.searchValue) && x.IsActive)
            .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
            .Skip(model.skip).Take(model.pageSize)
            .Select(x => new {
                x.SecurityClassificationId,
                x.SecurityClassificationCode,
                x.SecurityClassificationName
            })
            .ToListAsync();

        return result;
    }
    public async Task<int> GetCountByFilterModel(DataTableModel model)
    {
        var result = await _context.MstSecurityClassifications
            .Where(x => (x.SecurityClassificationCode + x.SecurityClassificationName).Contains(model.searchValue) && x.IsActive)
            .CountAsync();

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

            //Log
            if (result > 0)
            {
                try
                {
                    await _logChangesRepository.CreateLog<MstSecurityClassification>(GlobalConst.New, model.CreatedBy, new List<MstSecurityClassification> {  }, new List<MstSecurityClassification> { model });
                }
                catch (Exception ex) { }
            }
        }
        return result;
    }

    public async Task<bool> InsertBulk(List<MstSecurityClassification> mstSecurityClassifications)
    {
        bool result = false;
        if (mstSecurityClassifications.Count() > 0)
        {
            await _context.AddRangeAsync(mstSecurityClassifications);
            await _context.SaveChangesAsync();
            result = true;

            //Log
            if (result)
            {
                try
                {
                    await _logChangesRepository.CreateLog<MstSecurityClassification>(GlobalConst.New, mstSecurityClassifications.FirstOrDefault()!.CreatedBy, new List<MstSecurityClassification> { }, mstSecurityClassifications);
                }
                catch (Exception ex) { }
            }
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

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<MstSecurityClassification>(GlobalConst.Update, (Guid)model.UpdatedBy!, new List<MstSecurityClassification> { data }, new List<MstSecurityClassification> { model });
                    }
                    catch (Exception ex) { }
                }
            }
        }
        return result;
    }
}
