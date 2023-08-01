using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ardita.Repositories.Classess
{
    public class ClassificationTypeRepository : IClassificationTypeRepository
    {
        private readonly BksArditaDevContext _context;
        public ClassificationTypeRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(MstTypeClassification model)
        {
            int result = 0;

            if (model.TypeClassificationId != Guid.Empty)
            {
                var data = _context.MstTypeClassifications.AsNoTracking().Where(x => x.TypeClassificationId == model.TypeClassificationId).FirstOrDefault();
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

        public async Task<IEnumerable<MstTypeClassification>> GetAll()
        {
            var results = await _context.MstTypeClassifications
                .Where(x => x.IsActive == true)
                .AsNoTracking()
                .ToListAsync();
            return results;
        }
        public async Task<int> GetCount()
        {
            var results = await _context.MstTypeClassifications.Where(x => x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<MstTypeClassification> GetById(Guid id)
        {
            var result = await _context.MstTypeClassifications.AsNoTracking().FirstOrDefaultAsync(x => x.TypeClassificationId == id && x.IsActive == true);
            return result;
        }
        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var result = await _context.MstTypeClassifications
            .Where(x => x.IsActive == true && x.TypeClassificationName.Contains(model.searchValue))
            .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
            .Skip(model.skip).Take(model.pageSize)
            .Select(x => new {
                x.TypeClassificationId,
                x.TypeClassificationCode,
                x.TypeClassificationName
            })
            .ToListAsync();

            return result;
        }
        public async Task<int> GetCountByFilterModel(DataTableModel model)
        {
            var result = await _context.MstTypeClassifications
            .Where(x => x.IsActive == true && x.TypeClassificationName.Contains(model.searchValue))
            .CountAsync();

            return result;
        }

        public async Task<int> Insert(MstTypeClassification model)
        {
            int result = 0;

            if (model != null)
            {
                model.IsActive = true;
                _context.MstTypeClassifications.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
        public async Task<bool> InsertBulk(List<MstTypeClassification> models)
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
        public async Task<int> Update(MstTypeClassification model)
        {
            int result = 0;

            if (model != null && model.TypeClassificationId != Guid.Empty)
            {
                var data = await _context.MstTypeClassifications.AsNoTracking().FirstOrDefaultAsync(x => x.TypeClassificationId == model.TypeClassificationId);
                if (data != null)
                {
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
