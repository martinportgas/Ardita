using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ardita.Repositories.Classess
{
    public class ArchiveDestroyRepository : IArchiveDestroyRepository
    {
        private readonly BksArditaDevContext _context;
        public ArchiveDestroyRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(TrxArchiveDestroy model)
        {
            int result = 0;

            if (model.ArchiveDestroyId != Guid.Empty)
            {
                var data = _context.TrxArchiveDestroys.AsNoTracking().Where(x => x.ArchiveDestroyId == model.ArchiveDestroyId).FirstOrDefault();
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
        public async Task<int> Submit(TrxArchiveDestroy model)
        {
            int result = 0;

            if (model.ArchiveDestroyId != Guid.Empty)
            {
                var data = await _context.TrxArchiveDestroys.AsNoTracking().FirstAsync(x => x.ArchiveDestroyId == model.ArchiveDestroyId);
                if (data != null)
                {
                    data.Note = model.Note;
                    data.ApproveLevel = model.ApproveLevel;
                    data.IsActive = model.IsActive;
                    data.StatusId = model.StatusId;
                    data.UpdatedBy = model.UpdatedBy;
                    data.UpdatedDate = model.UpdatedDate;
                    _context.Update(data);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
        public async Task<IEnumerable<TrxArchiveDestroy>> GetAll()
        {
            var results = await _context.TrxArchiveDestroys.Where(x => x.IsActive == true).ToListAsync();
            return results;
        }
        public async Task<int> GetCount()
        {
            var results = await _context.TrxArchiveDestroys.Where(x => x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<TrxArchiveDestroy> GetById(Guid id)
        {
            var result = await _context.TrxArchiveDestroys.Include(x => x.ArchiveUnit.Company).AsNoTracking().FirstAsync(x => x.ArchiveDestroyId == id);
            return result;
        }
        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var result = await _context.TrxArchiveDestroys
                .Include(x => x.Status)
                .Where(x => x.IsActive == true && ( x.DestroyCode + x.DestroyName + x.Note + x.Status.Name).Contains(model.searchValue))
                .Where(" IsArchiveActive = @0 ", model.IsArchiveActive)
                .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                .Skip(model.skip).Take(model.pageSize)
                .Select(x => new {
                    x.ArchiveDestroyId,
                    x.DestroyCode,
                    x.DestroyName,
                    x.StatusId,
                    x.Note,
                    Color = x.Status.Color,
                    Status = x.Status.Name
                })
                .ToListAsync();

            return result;
        }
        public async Task<int> GetCountByFilterModel(DataTableModel model)
        {
            var result = await _context.TrxArchiveDestroys
                .Include(x => x.Status)
                .Where(x => x.IsActive == true && (x.DestroyCode + x.DestroyName + x.Status.Name).Contains(model.searchValue))
                .Where(" IsArchiveActive = @0 ", model.IsArchiveActive)
                .CountAsync();

            return result;
        }
        public async Task<int> Insert(TrxArchiveDestroy model)
        {
            int result = 0;

            var count = await _context.TrxArchiveDestroys.CountAsync() + 1;

            if (model != null)
            {
                var archiveUnit = await _context.TrxArchiveUnits.FirstOrDefaultAsync(x => x.ArchiveUnitId == model.ArchiveUnitId);
                var company = await _context.MstCompanies.FirstOrDefaultAsync(x => x.CompanyId == archiveUnit!.CompanyId);

                model.IsActive = true;
                model.DestroyCode = $"DST.{count.ToString("D3")}/{DateTime.Now.Month.ToString("D2")}/{DateTime.Now.Year}";
                model.DocumentCode = $"PH.{count.ToString("D3")}-{company!.CompanyCode}/{archiveUnit!.ArchiveUnitCode}/{DateTime.Now.Month.ToString("D2")}/{model.ArchiveYear}";
                _context.TrxArchiveDestroys.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
        public async Task<bool> InsertBulk(List<TrxArchiveDestroy> models)
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
        public async Task<int> Update(TrxArchiveDestroy model)
        {
            int result = 0;

            if (model != null && model.ArchiveDestroyId != Guid.Empty)
            {
                var data = await _context.TrxArchiveDestroys.AsNoTracking().FirstAsync(x => x.ArchiveDestroyId == model.ArchiveDestroyId);
                if (data != null)
                {
                    model.ArchiveUnit = null;
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();
                }
            }
            return result;
        }
    }
}
