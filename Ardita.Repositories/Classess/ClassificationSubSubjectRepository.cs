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
    internal class ClassificationSubSubjectRepository : IClassificationSubSubjectRepository
    {
        private readonly BksArditaDevContext _context;
        public ClassificationSubSubjectRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(TrxSubSubjectClassification model)
        {
            int result = 0;

            if (model.SubSubjectClassificationId != Guid.Empty)
            {
                _context.TrxSubSubjectClassifications.Remove(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<IEnumerable<TrxSubSubjectClassification>> GetAll()
        {
            var results = await _context.TrxSubSubjectClassifications.ToListAsync();
            return results;
        }
        public async Task<int> GetCount()
        {
            var results = await _context.TrxSubSubjectClassifications.CountAsync();
            return results;
        }

        public async Task<IEnumerable<TrxSubSubjectClassification>> GetById(Guid id)
        {
            var result = await _context.TrxSubSubjectClassifications.AsNoTracking().Where(x => x.SubSubjectClassificationId == id).ToListAsync();
            return result;
        }
        public async Task<IEnumerable<TrxSubSubjectClassification>> GetByFilterModel(DataTableModel model)
        {
            IEnumerable<TrxSubSubjectClassification> result;

            var propertyInfo = typeof(TrxSubSubjectClassification).GetProperty(model.sortColumn, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var propertyName = propertyInfo == null ? typeof(TrxSubSubjectClassification).GetProperties()[0].Name : propertyInfo.Name;

            if (model.sortColumnDirection.ToLower() == "asc")
            {
                result = await _context.TrxSubSubjectClassifications
                .Where(x => (x.SubSubjectClassificationCode + x.SubSubjectClassificationName).Contains(model.searchValue))
                .OrderBy(x => EF.Property<TrxSubSubjectClassification>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }
            else
            {
                result = await _context.TrxSubSubjectClassifications
                .Where(x => (x.SubSubjectClassificationCode + x.SubSubjectClassificationName).Contains(model.searchValue))
                .OrderByDescending(x => EF.Property<TrxSubSubjectClassification>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }

            return result;
        }

        public async Task<int> Insert(TrxSubSubjectClassification model)
        {
            int result = 0;

            if (model != null)
            {
                _context.TrxSubSubjectClassifications.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<int> Update(TrxSubSubjectClassification model)
        {
            int result = 0;

            if (model != null && model.SubSubjectClassificationId != Guid.Empty)
            {
                var data = await _context.TrxSubSubjectClassifications.AsNoTracking().Where(x => x.SubSubjectClassificationId == model.SubSubjectClassificationId).ToListAsync();
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
