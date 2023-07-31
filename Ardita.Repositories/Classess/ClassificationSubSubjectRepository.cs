using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Security.Claims;

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
            var results = await _context.TrxSubSubjectClassifications
                .Include(x => x.Creator.ArchiveUnit)
                .Include(x => x.SubjectClassification)
                .AsNoTracking()
                .Where(x => x.IsActive == true)
                .Where(x => x.Creator!.IsActive == true)
                .ToListAsync();
            return results;
        }

        public async Task<int> GetCount(DataTableModel model)
        {
            var result = await _context.TrxSubSubjectClassifications
              .Include(x => x.SubjectClassification)
              .Include(x => x.Creator)
              .Where(x => x.IsActive == true && (x.SubSubjectClassificationCode + x.SubSubjectClassificationName + x.SubjectClassification.SubjectClassificationCode + x.SubjectClassification.SubjectClassificationName).Contains(model.searchValue))
              .Where(x => x.Creator!.IsActive == true)
              .CountAsync();
            return result;
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
        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var result = await _context.TrxSubSubjectClassifications
               .Include(x => x.SubjectClassification.Classification.TypeClassification)
               .Include(x => x.Creator)
               .Include(x => x.SecurityClassification)
               .Where(x => x.IsActive == true && (
                                x.SubSubjectClassificationCode + 
                                x.SubSubjectClassificationName + 
                                x.SubjectClassification.SubjectClassificationCode + 
                                x.SubjectClassification.SubjectClassificationName +
                                x.SubjectClassification.Classification.ClassificationName +
                                x.SubjectClassification.Classification.TypeClassification.TypeClassificationName +
                                x.SecurityClassification.SecurityClassificationName +
                                x.RetentionActive +
                                x.RetentionInactive
                                ).Contains(model.searchValue))
               .Where(x => x.Creator!.IsActive == true)
               .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
               .Skip(model.skip).Take(model.pageSize)
               .Select(x => new {
                   x.SubSubjectClassificationId,
                   x.SubSubjectClassificationCode,
                   x.SubSubjectClassificationName,
                   x.SubjectClassification.SubjectClassificationCode,
                   x.SubjectClassification.SubjectClassificationName,
                   x.SubjectClassification.Classification.ClassificationName,
                   x.SubjectClassification.Classification.TypeClassification.TypeClassificationName,
                   x.SecurityClassification.SecurityClassificationName,
                   x.RetentionActive,
                   x.RetentionInactive

               })
               .ToListAsync();
            return result;
        }

        public async Task<int> Insert(TrxSubSubjectClassification model)
        {
            int result = 0;

            if (model != null)
            {
                var dataCreator = await _context.TrxSubjectClassifications
                    .Include(x => x.Classification)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.SubjectClassificationId == model.SubjectClassificationId);

                model.IsActive = true;
                model.CreatorId = dataCreator!.Classification.CreatorId;
            

                foreach (var e in _context.ChangeTracker.Entries())
                {
                    e.State = EntityState.Detached;
                }

                model.IsActive = true;

                _context.Entry(model).State = EntityState.Added;
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
                    var dataCreator = await _context.TrxSubjectClassifications.Include(x => x.Classification).AsNoTracking().FirstOrDefaultAsync(x => x.SubjectClassificationId == model.SubjectClassificationId);

                    model.Creator = null;
                    model.SubjectClassification = null;
                    model.CreatorId = dataCreator!.Classification.CreatorId;
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }

        public async Task<IEnumerable<TrxSubSubjectClassification>> GetByArchiveUnit(List<string> listArchiveUnitCode)
        {
            return await _context.TrxSubSubjectClassifications
                .Include(c => c.Creator!.ArchiveUnit)
                .AsNoTracking()
                .Where($"{(listArchiveUnitCode.Count > 0 ? "@0.Contains(Creator.ArchiveUnit.ArchiveUnitCode)" : "1=1")} ", listArchiveUnitCode)
                .Where(x => x.IsActive == true)
                .ToListAsync();
        }
    }
}
