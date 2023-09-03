using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ardita.Repositories.Classess
{
    public class ClassificationRepository : IClassificationRepository
    {
        private readonly BksArditaDevContext _context;
        private readonly ILogChangesRepository _logChangesRepository;
        public ClassificationRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
        {
            _context = context;
            _logChangesRepository = logChangesRepository;
        }
        public async Task<int> Delete(TrxClassification model)
        {
            int result = 0;

            if (model.ClassificationId != Guid.Empty)
            {
                var data = _context.TrxClassifications.AsNoTracking().Where(x => x.ClassificationId == model.ClassificationId).FirstOrDefault();
                if (data != null)
                {
                    data.IsActive = false;
                    data.UpdatedBy = model.UpdatedBy;
                    data.UpdatedDate = DateTime.Now;
                    _context.Update(data);
                    result = await _context.SaveChangesAsync();

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<TrxClassification>(GlobalConst.Delete, (Guid)model.UpdatedBy!, new List<TrxClassification> { data }, new List<TrxClassification> { });
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return result;
        }

        public async Task<IEnumerable<TrxClassification>> GetAll(string par = " 1=1 ")
        {
            var results = await _context.TrxClassifications.Include(x => x.Creator.ArchiveUnit).Where(x => x.IsActive == true)
                .Where(par).AsNoTracking().ToListAsync();
            return results;
        }
        public async Task<int> GetCount(DataTableModel model)
        {
            var result = await _context.TrxClassifications
                    .Include(x => x.TypeClassification)
                    .Include(x => x.Creator)
                    .Where(x => x.IsActive == true && (x.ClassificationCode + x.ClassificationName + x.TypeClassification.TypeClassificationName + x.Creator.CreatorName).Contains(model.searchValue))
                    .CountAsync();
            return result;
        }

        public async Task<TrxClassification> GetById(Guid id)
        {
            var result = await _context.TrxClassifications.AsNoTracking().FirstOrDefaultAsync(x => x.IsActive == true && x.ClassificationId == id);
            return result;
        }
        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var result = await _context.TrxClassifications
                 .Include(x => x.TypeClassification)
                 .Include(x => x.Creator)
                 .Where(x => x.IsActive == true && (x.ClassificationCode + x.ClassificationName + x.TypeClassification.TypeClassificationName + x.Creator.CreatorName).Contains(model.searchValue))
                 .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                 .Skip(model.skip).Take(model.pageSize)
                 .Select(x => new {
                     x.ClassificationId,
                     x.ClassificationCode,
                     x.ClassificationName,
                     x.TypeClassification.TypeClassificationName,
                     x.Creator.CreatorName
                 })
                 .ToListAsync();

            return result;
        }

        public async Task<int> Insert(TrxClassification model)
        {
            int result = 0;

            if (model != null)
            {
                model.IsActive = true;
                _context.TrxClassifications.Add(model);
                result = await _context.SaveChangesAsync();

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<TrxClassification>(GlobalConst.New, (Guid)model.CreatedBy!, new List<TrxClassification> {  }, new List<TrxClassification> { model });
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }
        public async Task<bool> InsertBulk(List<TrxClassification> models)
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
                        await _logChangesRepository.CreateLog<TrxClassification>(GlobalConst.New, (Guid)models.FirstOrDefault()!.UpdatedBy!, new List<TrxClassification> {  }, models);
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }

        public async Task<int> Update(TrxClassification model)
        {
            int result = 0;

            if (model != null && model.ClassificationId != Guid.Empty)
            {
                var data = await _context.TrxClassifications.AsNoTracking().FirstOrDefaultAsync(x => x.ClassificationId == model.ClassificationId);
                if (data != null)
                {
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<TrxClassification>(GlobalConst.Update, (Guid)model.UpdatedBy!, new List<TrxClassification> { data }, new List<TrxClassification> { model });
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return result;
        }
    }
}
