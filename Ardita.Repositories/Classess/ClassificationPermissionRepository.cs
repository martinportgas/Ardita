using Ardita.Models.DbModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Classess
{
    public class ClassificationPermissionRepository : IClassificationPermissionRepository
    {
        private readonly BksArditaDevContext _context;
        public ClassificationPermissionRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(TrxPermissionClassification model)
        {
            int result = 0;

            if (model.PermissionClassificationId != Guid.Empty)
            {
                _context.TrxPermissionClassifications.Remove(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<int> DeleteByMainId(Guid id)
        {
            int result = 0;
            if (id != null)
            {
                _context.Database.ExecuteSqlRaw($" delete from dbo.TRX_PERMISSION_CLASSIFICATION where sub_subject_classification_id='{id}'");
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<IEnumerable<TrxPermissionClassification>> GetAll()
        {
            var results = await _context.TrxPermissionClassifications.ToListAsync();
            return results;
        }

        public async Task<TrxPermissionClassification> GetById(Guid id)
        {
            var result = await _context.TrxPermissionClassifications.AsNoTracking().FirstOrDefaultAsync(x => x.PermissionClassificationId == id);
            return result;
        }

        public async Task<IEnumerable<TrxPermissionClassification>> GetByMainId(Guid id)
        {
            var result = await _context.TrxPermissionClassifications.AsNoTracking().Where(x => x.SubSubjectClassificationId == id).OrderBy(x => x.CreatedDate).ToListAsync();
            return result;
        }

        public async Task<int> Insert(TrxPermissionClassification model)
        {
            int result = 0;

            if (model != null)
            {
                var data = await _context.TrxPermissionClassifications.AsNoTracking().Where(
                  x => x.SubSubjectClassificationId == model.SubSubjectClassificationId &&
                  x.PositionId == model.PositionId
                  ).ToListAsync();

                if (data.Count() == 0)
                {
                    _context.TrxPermissionClassifications.Add(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
        public async Task<bool> InsertBulk(List<TrxPermissionClassification> models)
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
        public async Task<int> Update(TrxPermissionClassification model)
        {
            int result = 0;

            if (model != null && model.PermissionClassificationId != Guid.Empty)
            {
                var data = await _context.TrxPermissionClassifications.AsNoTracking().Where(x => x.PermissionClassificationId == model.PermissionClassificationId).ToListAsync();
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
