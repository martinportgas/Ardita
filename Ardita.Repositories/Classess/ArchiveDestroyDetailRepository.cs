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
    public class ArchiveDestroyDetailRepository : IArchiveDestroyDetailRepository
    {
        private readonly BksArditaDevContext _context;
        public ArchiveDestroyDetailRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(TrxArchiveDestroyDetail model)
        {
            int result = 0;

            if (model.ArchiveDestroyDetailId != Guid.Empty)
            {
                var data = _context.TrxArchiveDestroyDetails.AsNoTracking().Where(x => x.ArchiveDestroyDetailId == model.ArchiveDestroyDetailId).FirstOrDefault();
                if (data != null)
                {
                    data.IsActive = false;
                    _context.Update(data);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
        public async Task<int> DeleteByMainId(Guid id)
        {
            int result = 0;
            if (id != null)
            {
                _context.Database.ExecuteSqlRaw($" delete from dbo.TRX_ARCHIVE_DESTROY_DETAIL where archive_destroy_id='{id}'");
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
        public async Task<IEnumerable<TrxArchiveDestroyDetail>> GetAll()
        {
            var results = await _context.TrxArchiveDestroyDetails.Where(x => x.IsActive == true).ToListAsync();
            return results;
        }
        public async Task<int> GetCount()
        {
            var results = await _context.TrxArchiveDestroyDetails.Where(x => x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<IEnumerable<TrxArchiveDestroyDetail>> GetById(Guid id)
        {
            var result = await _context.TrxArchiveDestroyDetails.AsNoTracking().Where(x => x.ArchiveDestroyDetailId == id).ToListAsync();
            return result;
        }
        public async Task<IEnumerable<TrxArchiveDestroyDetail>> GetByMainId(Guid id)
        {
            var result = await _context.TrxArchiveDestroyDetails.Include(x => x.Archive).AsNoTracking().Where(x => x.ArchiveDestroyId == id).ToListAsync();
            return result;
        }
        public async Task<IEnumerable<TrxArchiveDestroyDetail>> GetByFilterModel(DataTableModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Insert(TrxArchiveDestroyDetail model)
        {
            int result = 0;

            if (model != null)
            {
                model.IsActive = true;
                _context.TrxArchiveDestroyDetails.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
        public async Task<bool> InsertBulk(List<TrxArchiveDestroyDetail> models)
        {
            bool result = false;
            if (models.Count() > 0)
            {
                await _context.BulkInsertAsync(models);
                result = true;
            }
            return result;
        }
        public async Task<int> Update(TrxArchiveDestroyDetail model)
        {
            int result = 0;

            if (model != null && model.ArchiveDestroyDetailId != Guid.Empty)
            {
                var data = await _context.TrxArchiveDestroyDetails.AsNoTracking().Where(x => x.ArchiveDestroyDetailId == model.ArchiveDestroyDetailId).ToListAsync();
                if (data != null)
                {
                    model.IsActive = true;
                    model.CreatedBy = data.FirstOrDefault().CreatedBy;
                    model.CreatedDate = data.FirstOrDefault().CreatedDate;
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
