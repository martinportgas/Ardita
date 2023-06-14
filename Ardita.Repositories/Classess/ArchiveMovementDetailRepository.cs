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
    public class ArchiveMovementDetailRepository : IArchiveMovementDetailRepository
    {
        private readonly BksArditaDevContext _context;
        public ArchiveMovementDetailRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(TrxArchiveMovementDetail model)
        {
            int result = 0;

            if (model.ArchiveMovementDetailId != Guid.Empty)
            {
                var data = _context.TrxArchiveMovementDetails.AsNoTracking().Where(x => x.ArchiveMovementDetailId == model.ArchiveMovementDetailId).FirstOrDefault();
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
                _context.Database.ExecuteSqlRaw($" delete from dbo.TRX_ARCHIVE_MOVEMENT_DETAIL where archive_movement_id='{id}'");
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
        public async Task<IEnumerable<TrxArchiveMovementDetail>> GetAll()
        {
            var results = await _context.TrxArchiveMovementDetails
                .Include(x => x.ArchiveMovement)
                .Where(x => x.IsActive == true && x.ArchiveMovement.IsActive == true && x.ArchiveMovement.StatusId != 4)
                .AsNoTracking()
                .ToListAsync();
            return results;
        }
        public async Task<int> GetCount()
        {
            var results = await _context.TrxArchiveMovementDetails.Where(x => x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<IEnumerable<TrxArchiveMovementDetail>> GetById(Guid id)
        {
            var result = await _context.TrxArchiveMovementDetails.AsNoTracking().Where(x => x.ArchiveMovementDetailId == id).ToListAsync();
            return result;
        }
        public async Task<IEnumerable<TrxArchiveMovementDetail>> GetByMainId(Guid id)
        {
            var result = await _context.TrxArchiveMovementDetails.Include(x => x.Archive.Creator).Include(x => x.Archive.Gmd).Include(x => x.Archive.SecurityClassification).AsNoTracking().Where(x => x.ArchiveMovementId == id).ToListAsync();
            return result;
        }
        public async Task<IEnumerable<TrxArchiveMovementDetail>> GetByFilterModel(DataTableModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Insert(TrxArchiveMovementDetail model)
        {
            int result = 0;

            if (model != null)
            {
                model.IsActive = true;
                _context.TrxArchiveMovementDetails.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
        public async Task<bool> InsertBulk(List<TrxArchiveMovementDetail> models)
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
        public async Task<int> Update(TrxArchiveMovementDetail model)
        {
            int result = 0;

            if (model != null && model.ArchiveMovementDetailId != Guid.Empty)
            {
                var data = await _context.TrxArchiveMovementDetails.AsNoTracking().Where(x => x.ArchiveMovementDetailId == model.ArchiveMovementDetailId).ToListAsync();
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
