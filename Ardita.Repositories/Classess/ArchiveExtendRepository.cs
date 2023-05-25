using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Ardita.Repositories.Classess
{
    public class ArchiveExtendRepository : IArchiveExtendRepository
    {
        private readonly BksArditaDevContext _context;
        public ArchiveExtendRepository(BksArditaDevContext context)
        {
            _context = context;
        }
        public async Task<int> Delete(TrxArchiveExtend model)
        {
            int result = 0;

            if (model.ArchiveExtendId != Guid.Empty)
            {
                var data = _context.TrxArchiveExtends.AsNoTracking().Where(x => x.ArchiveExtendId == model.ArchiveExtendId).FirstOrDefault();
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
        public async Task<int> Submit(TrxArchiveExtend model)
        {
            int result = 0;

            if (model.ArchiveExtendId != Guid.Empty)
            {
                var data = await _context.TrxArchiveExtends.AsNoTracking().FirstAsync(x => x.ArchiveExtendId == model.ArchiveExtendId);
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
        public async Task<IEnumerable<TrxArchiveExtend>> GetAll()
        {
            var results = await _context.TrxArchiveExtends.Where(x => x.IsActive == true).ToListAsync();
            return results;
        }
        public async Task<int> GetCount()
        {
            var results = await _context.TrxArchiveExtends.Where(x => x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<TrxArchiveExtend> GetById(Guid id)
        {
            var result = await _context.TrxArchiveExtends.AsNoTracking().FirstAsync(x => x.ArchiveExtendId == id);
            return result;
        }
        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var result = await _context.TrxArchiveExtends
                    .Include(x => x.Status)
                    .Where(x => x.IsActive == true && ( x.ExtendCode + x.ExtendName + x.Note + x.Status.Name).Contains(model.searchValue))
                    .Where(" IsArchiveActive = @0 ", model.IsArchiveActive)
                    .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
                    .Skip(model.skip).Take(model.pageSize)
                    .Select(x => new {
                        x.ArchiveExtendId,
                        x.ExtendCode,
                        x.ExtendName,
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
            var result = await _context.TrxArchiveExtends
                    .Include(x => x.Status)
                    .Where(x => x.IsActive == true && (x.ExtendCode + x.ExtendName + x.Status.Name).Contains(model.searchValue))
                    .Where(" IsArchiveActive = @0 ", model.IsArchiveActive)
                    .CountAsync();

            return result;
        }

        public async Task<int> Insert(TrxArchiveExtend model)
        {
            int result = 0;

            var count = await _context.TrxArchiveExtends.CountAsync() + 1;

            if (model != null)
            {
                var archiveUnit = await _context.TrxArchiveUnits.FirstOrDefaultAsync(x => x.ArchiveUnitId == model.ArchiveUnitId);
                var company = await _context.MstCompanies.FirstOrDefaultAsync(x => x.CompanyId == archiveUnit!.CompanyId);

                model.IsActive = true;
                model.ExtendCode = $"EXT.{count.ToString("D3")}/{DateTime.Now.Month.ToString("D2")}/{DateTime.Now.Year}";
                model.DocumentCode = $"PR.{count.ToString("D3")}-{company!.CompanyCode}/{archiveUnit!.ArchiveUnitCode}/{DateTime.Now.Month.ToString("D2")}/{model.ArchiveYear}";
                _context.TrxArchiveExtends.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
        public async Task<bool> InsertBulk(List<TrxArchiveExtend> models)
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
        public async Task<int> Update(TrxArchiveExtend model)
        {
            int result = 0;

            if (model != null && model.ArchiveExtendId != Guid.Empty)
            {
                var data = await _context.TrxArchiveExtends.AsNoTracking().FirstAsync(x => x.ArchiveExtendId == model.ArchiveExtendId);
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
