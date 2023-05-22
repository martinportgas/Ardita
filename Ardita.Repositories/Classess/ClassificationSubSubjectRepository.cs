using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Reflection;

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
                var data = _context.TrxSubSubjectClassifications.AsNoTracking().Where(x => x.SubSubjectClassificationId == model.SubSubjectClassificationId).FirstOrDefault();
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

        public async Task<IEnumerable<TrxSubSubjectClassification>> GetAll()
        {
            var results = await _context.TrxSubSubjectClassifications.Include(x => x.Creator).Where(x => x.IsActive == true).ToListAsync();
            return results;
        }

        public async Task<int> GetCount()
        {
            var results = await _context.TrxSubSubjectClassifications.Where(x => x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<TrxSubSubjectClassification> GetById(Guid id)
        {
            var result = await _context.TrxSubSubjectClassifications
                .Include(x => x.Creator)
                .Include(x => x.SubjectClassification.Classification.TypeClassification)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.SubSubjectClassificationId == id && x.IsActive == true);
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
                .Where(x => x.IsActive == true && (x.SubSubjectClassificationCode + x.SubSubjectClassificationName).Contains(model.searchValue))
                .OrderBy(x => EF.Property<TrxSubSubjectClassification>(x, propertyName))
                .Skip(model.skip).Take(model.pageSize)
                .ToListAsync();
            }
            else
            {
                result = await _context.TrxSubSubjectClassifications
                .Where(x => x.IsActive == true && (x.SubSubjectClassificationCode + x.SubSubjectClassificationName).Contains(model.searchValue))
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
                model.IsActive = true;
                _context.TrxSubSubjectClassifications.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
        public async Task<bool> InsertBulk(List<TrxSubSubjectClassification> models)
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
        public async Task<int> Update(TrxSubSubjectClassification model)
        {
            int result = 0;

            if (model != null && model.SubSubjectClassificationId != Guid.Empty)
            {
                var data = await _context.TrxSubSubjectClassifications.AsNoTracking().FirstOrDefaultAsync(x => x.SubSubjectClassificationId == model.SubSubjectClassificationId);
                if (data != null)
                {
                    model.Creator = null;
                    model.SubjectClassification = null;
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }

        public async Task<IEnumerable<TrxSubSubjectClassification>> GetByArchiveUnit(List<string> listArchiveUnitCode)
        {
            return await _context.TrxSubSubjectClassifications.Include(c => c.Creator!.ArchiveUnit).Where($"{(listArchiveUnitCode.Count > 0 ? "@0.Contains(Creator.ArchiveUnit.ArchiveUnitCode)" : "1=1")} ", listArchiveUnitCode).ToListAsync();
        }
    }
}
