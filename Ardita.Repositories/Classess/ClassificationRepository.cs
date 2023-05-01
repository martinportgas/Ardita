using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Classess
{
    public class ClassificationRepository : IClassificationRepository
    {
        private readonly BksArditaDevContext _context;
        public ClassificationRepository(BksArditaDevContext context)
        {
            _context = context;
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
                }
            }
            return result;
        }

        public async Task<IEnumerable<TrxClassification>> GetAll()
        {
            var results = await _context.TrxClassifications.Where(x => x.IsActive == true).ToListAsync();
            return results;
        }
        public async Task<int> GetCount()
        {
            var results = await _context.TrxClassifications.Where(x => x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<TrxClassification> GetById(Guid id)
        {
            var result = await _context.TrxClassifications.AsNoTracking().FirstOrDefaultAsync(x => x.IsActive == true && x.ClassificationId == id);
            return result;
        }
        public async Task<IEnumerable<TrxClassification>> GetByFilterModel(DataTableModel model)
        {
            IEnumerable<TrxClassification> result;

            var propertyInfo = typeof(TrxClassification).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var propertyName = propertyInfo == null ? typeof(TrxClassification).GetProperties()[0].Name : propertyInfo.Name;

            if (model.sortColumnDirection.ToLower() == "asc")
            {
                result = await _context.TrxClassifications
                .Where(x => x.IsActive == true && (x.ClassificationCode + x.ClassificationName).Contains(model.searchValue))
                .OrderBy(x => EF.Property<TrxClassification>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }
            else
            {
                result = await _context.TrxClassifications
                .Where(x => x.IsActive == true && (x.ClassificationCode + x.ClassificationName).Contains(model.searchValue))
                .OrderByDescending(x => EF.Property<TrxClassification>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }

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
            }
            return result;
        }
        public async Task<bool> InsertBulk(List<TrxClassification> models)
        {
            bool result = false;
            if (models.Count() > 0)
            {
                await _context.BulkInsertAsync(models);
                result = true;
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
                }
            }
            return result;
        }
    }
}
