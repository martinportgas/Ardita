using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ardita.Repositories.Classess
{
    public class ArchiveOwnerRepository : IArchiveOwnerRepository
    {
        private readonly BksArditaDevContext _context;
        public ArchiveOwnerRepository(BksArditaDevContext context) => _context = context;
        public async Task<int> Delete(MstArchiveOwner model)
        {
            int result = 0;

            if (model.ArchiveOwnerId != Guid.Empty)
            {
                var data = await _context.MstArchiveOwners.AsNoTracking().FirstAsync(x => x.ArchiveOwnerId == model.ArchiveOwnerId);
                if (data != null)
                {
                    data.IsActive = false;
                    data.UpdatedDate = model.UpdatedDate;
                    data.UpdatedBy = model.UpdatedBy;
                    _context.MstArchiveOwners.Update(data);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }

        public async Task<IEnumerable<MstArchiveOwner>> GetAll() => await _context.MstArchiveOwners.Where(x => x.IsActive == true).AsNoTracking().ToListAsync();

        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var result = await _context.MstArchiveOwners
                .Where(x => (x.ArchiveOwnerCode + x.ArchiveOwnerName).Contains(model.searchValue) && x.IsActive == true)
                .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                .Skip(model.skip).Take(model.pageSize)
                .Select(x => new {
                    x.ArchiveOwnerId,
                    x.ArchiveOwnerCode,
                    x.ArchiveOwnerName
                })
                .ToListAsync();

            return result;
        }
        public async Task<int> GetCountByFilterModel(DataTableModel model)
        {
            var result = await _context.MstArchiveOwners
                .Where(x => (x.ArchiveOwnerCode + x.ArchiveOwnerName).Contains(model.searchValue) && x.IsActive == true)
                .CountAsync();

            return result;
        }

        public async Task<IEnumerable<MstArchiveOwner>> GetById(Guid id) => await _context.MstArchiveOwners.Where(x => x.ArchiveOwnerId == id).ToListAsync();

        public async Task<int> GetCount() => await _context.MstArchiveOwners.CountAsync(x => x.IsActive == true);

        public async Task<int> Insert(MstArchiveOwner model)
        {
            int result = 0;

            if (model != null)
            {
                model.IsActive = true;
                _context.MstArchiveOwners.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<bool> InsertBulk(List<MstArchiveOwner> MstArchiveOwners)
        {
            bool result = false;
            if (MstArchiveOwners.Count() > 0)
            {
                await _context.AddRangeAsync(MstArchiveOwners);
                await _context.SaveChangesAsync();
                result = true;
            }
            return result;
        }

        public async Task<int> Update(MstArchiveOwner model)
        {
            int result = 0;

            if (model != null && model.ArchiveOwnerId != Guid.Empty)
            {
                var data = await _context.MstArchiveOwners.AsNoTracking().FirstAsync(x => x.ArchiveOwnerId == model.ArchiveOwnerId);
                if (data != null)
                {
                    model.IsActive = data.IsActive;
                    model.CreatedBy = data.CreatedBy;
                    model.CreatedDate = data.CreatedDate;
                    _context.MstArchiveOwners.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
