using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Classess
{
    public class ArchiveExtendDetailRepository : IArchiveExtendDetailRepository
    {
        private readonly BksArditaDevContext _context;
        private readonly ILogChangesRepository _logChangesRepository;

        public ArchiveExtendDetailRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
        {
            _context = context;
            _logChangesRepository = logChangesRepository;
        }
        public async Task<int> Delete(TrxArchiveExtendDetail model)
        {
            int result = 0;

            if (model.ArchiveExtendDetailId != Guid.Empty)
            {
                var data = _context.TrxArchiveExtendDetails.AsNoTracking().Where(x => x.ArchiveExtendDetailId == model.ArchiveExtendDetailId).FirstOrDefault();
                if (data != null)
                {
                    data.IsActive = false;
                    _context.Update(data);
                    result = await _context.SaveChangesAsync();

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<TrxArchiveExtendDetail>(GlobalConst.Delete, (Guid)model!.CreatedBy!, new List<TrxArchiveExtendDetail> { data }, new List<TrxArchiveExtendDetail> { });
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return result;
        }
        public async Task<int> DeleteByMainId(Guid id)
        {
            int result = 0;
            if (id != null)
            {
                _context.Database.ExecuteSqlRaw($" delete from dbo.TRX_ARCHIVE_EXTEND_DETAIL where archive_extend_id='{id}'");
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
        public async Task<IEnumerable<TrxArchiveExtendDetail>> GetAll(string par = " 1=1 ")
        {
            var results = await _context.TrxArchiveExtendDetails
                .Include(x => x.ArchiveExtend)
                .Where(x => x.IsActive == true && x.ArchiveExtend.IsActive == true && x.ArchiveExtend.StatusId != 4)
                .Where(par)
                .AsNoTracking()
                .ToListAsync();
            return results;
        }
        public async Task<int> GetCount()
        {
            var results = await _context.TrxArchiveExtendDetails.Where(x => x.IsActive == true).CountAsync();
            return results;
        }

        public async Task<IEnumerable<TrxArchiveExtendDetail>> GetById(Guid id)
        {
            var result = await _context.TrxArchiveExtendDetails.AsNoTracking().Where(x => x.ArchiveExtendDetailId == id).ToListAsync();
            return result;
        }
        public async Task<IEnumerable<TrxArchiveExtendDetail>> GetByMainId(Guid id)
        {
            var result = await _context.TrxArchiveExtendDetails.Include(x => x.Archive).AsNoTracking().Where(x => x.ArchiveExtendId == id).ToListAsync();
            return result;
        }
        public async Task<IEnumerable<TrxArchiveExtendDetail>> GetByFilterModel(DataTableModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Insert(TrxArchiveExtendDetail model)
        {
            int result = 0;

            if (model != null)
            {
                model.IsActive = true;
                _context.TrxArchiveExtendDetails.Add(model);
                result = await _context.SaveChangesAsync();

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<TrxArchiveExtendDetail>(GlobalConst.New, (Guid)model!.CreatedBy!, new List<TrxArchiveExtendDetail> {  }, new List<TrxArchiveExtendDetail> { model });
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }
        public async Task<bool> InsertBulk(List<TrxArchiveExtendDetail> models)
        {
            bool result = false;
            if (models.Count() > 0)
            {
                await _context.AddRangeAsync(models);
                await _context.SaveChangesAsync();
                result = true;

                //Log
                if (result)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<TrxArchiveExtendDetail>(GlobalConst.New, (Guid)models.FirstOrDefault()!.CreatedBy!, new List<TrxArchiveExtendDetail> {  }, models);
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }
        public async Task<int> Update(TrxArchiveExtendDetail model)
        {
            int result = 0;

            if (model != null && model.ArchiveExtendDetailId != Guid.Empty)
            {
                var data = await _context.TrxArchiveExtendDetails.AsNoTracking().Where(x => x.ArchiveExtendDetailId == model.ArchiveExtendDetailId).FirstOrDefaultAsync();
                if (data != null)
                {
                    model.IsActive = true;
                    model.CreatedBy = data.CreatedBy;
                    model.CreatedDate = data.CreatedDate;
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<TrxArchiveExtendDetail>(GlobalConst.Update, (Guid)model!.CreatedBy!, new List<TrxArchiveExtendDetail> { data }, new List<TrxArchiveExtendDetail> { model });
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return result;
        }
    }
}
