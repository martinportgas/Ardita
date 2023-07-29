using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Classess
{
    public class ClassificationSubjectRepository : IClassificationSubjectRepository
    {
        private readonly BksArditaDevContext _context;
        public ClassificationSubjectRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(TrxSubjectClassification model)
        {
            int result = 0;

            if (model.SubjectClassificationId != Guid.Empty)
            {
                var data = _context.TrxSubjectClassifications.AsNoTracking().Where(x => x.SubjectClassificationId == model.SubjectClassificationId).FirstOrDefault();
                if (data != null)
                {
                    data.IsActive = false;
                    data.UpdatedBy = model.UpdatedBy;
                    data.UpdatedDate = DateTime.Now;
                    _context.Update(data);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }

        public async Task<IEnumerable<TrxSubjectClassification>> GetAll()
        {
            var results = await _context.TrxSubjectClassifications
                .Include(x => x.Classification.Creator)
                .Where(x => x.IsActive == true)
                .Where(x => x.Classification.IsActive == true)
                .Where(x => x.Classification.Creator.IsActive == true)
                .AsNoTracking()
                .ToListAsync();
            return results;
        }
        public async Task<int> GetCount(DataTableModel model)
        {
            var result = await _context.TrxSubjectClassifications
                 .Include(x => x.Classification)
                 .Where(x => x.IsActive == true && (x.SubjectClassificationCode + x.SubjectClassificationName + x.Classification.ClassificationCode + x.Classification.ClassificationName).Contains(model.searchValue))
                 .Where(x => x.Classification.IsActive == true)
                .Where(x => x.Classification.Creator.IsActive == true)
                 .CountAsync();
            return result;
        }

        public async Task<TrxSubjectClassification> GetById(Guid id)
        {
            var result = await _context.TrxSubjectClassifications
                .Include(x => x.Classification.TypeClassification)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.IsActive == true && x.SubjectClassificationId == id);
            return result;
        }
        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var result = await _context.TrxSubjectClassifications
                 .Include(x => x.Classification)
                 .Where(x => x.IsActive == true && (x.SubjectClassificationCode + x.SubjectClassificationName + x.Classification.ClassificationCode + x.Classification.ClassificationName).Contains(model.searchValue))
                 .Where(x => x.Classification.IsActive == true)
                .Where(x => x.Classification.Creator.IsActive == true)
                 .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                 .Skip(model.skip).Take(model.pageSize)
                 .Select(x => new {
                     x.SubjectClassificationId,
                     x.SubjectClassificationCode,
                     x.SubjectClassificationName,
                     x.Classification.ClassificationCode,
                     x.Classification.ClassificationName
                 })
                 .ToListAsync();

            return result;
        }

        public async Task<int> Insert(TrxSubjectClassification model)
        {
            int result = 0;

            if (model != null)
            {
                model.IsActive = true;
                _context.TrxSubjectClassifications.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
        public async Task<bool> InsertBulk(List<TrxSubjectClassification> models)
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
        public async Task<int> Update(TrxSubjectClassification model)
        {
            int result = 0;

            if (model != null && model.SubjectClassificationId != Guid.Empty)
            {
                var data = await _context.TrxSubjectClassifications.AsNoTracking().FirstOrDefaultAsync(x => x.SubjectClassificationId == model.SubjectClassificationId);
                if (data != null)
                {
                    model.Classification = null;
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
