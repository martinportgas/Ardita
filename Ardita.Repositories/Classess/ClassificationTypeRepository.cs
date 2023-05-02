using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
            var results = await _context.MstTypeClassifications.Where(x => x.IsActive == true).ToListAsync();
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
        public async Task<IEnumerable<MstTypeClassification>> GetByFilterModel(DataTableModel model)
        {
            IEnumerable<MstTypeClassification> result;

            var propertyInfo = typeof(MstTypeClassification).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var propertyName = propertyInfo == null ? typeof(MstTypeClassification).GetProperties()[0].Name : propertyInfo.Name;

            if (model.sortColumnDirection.ToLower() == "asc")
            {
                result = await _context.MstTypeClassifications
                .Where(x => x.IsActive == true && x.TypeClassificationName.Contains(model.searchValue))
                .OrderBy(x => EF.Property<MstTypeClassification>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }
            else
            {
                result = await _context.MstTypeClassifications
                .Where(x => x.IsActive == true && x.TypeClassificationName.Contains(model.searchValue))
                .OrderByDescending(x => EF.Property<MstTypeClassification>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }

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
