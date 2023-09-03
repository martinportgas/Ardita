using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Ardita.Repositories.Classess
{
    public class PositionRepository : IPositionRepository
    {
        private readonly BksArditaDevContext _context;
        private readonly ILogChangesRepository _logChangesRepository;
        public PositionRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
        {
            _context = context;
            _logChangesRepository = logChangesRepository;
        }

        public async Task<int> Delete(MstPosition model)
        {
            int result = 0;

            if (model != null && model.PositionId != Guid.Empty)
            {
                var data = await _context.MstPositions.AsNoTracking().FirstAsync(x => x.PositionId == model.PositionId);
                if (data != null)
                {
                    model.IsActive = false;
                    _context.MstPositions.Update(model);
                    result = await _context.SaveChangesAsync();

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<MstPosition>(GlobalConst.Delete, new Guid(model.UpdateBy!), new List<MstPosition> { data }, new List<MstPosition> {  });
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return result;
        }

        public async Task<IEnumerable<MstPosition>> GetAll(string par = " 1=1 ")
        {
            var results = await _context.MstPositions.Where(x => x.IsActive == true).Where(par).AsNoTracking().ToListAsync();
            return results;
        }

        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {

            var result = await _context.MstPositions
               .Where(
                   x => (x.Code + x.Name).Contains(model.searchValue) &&
                   x.IsActive == true
                ).OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
               .Skip(model.skip).Take(model.pageSize)
               .ToListAsync();
            return result;
        }

        public async Task<MstPosition> GetById(Guid id)
        {
            var result = await _context.MstPositions.AsNoTracking().FirstAsync(x => x.PositionId == id);
            return result;
        }

        public async Task<int> GetCount(DataTableModel model)
        {
            var results = await _context.MstPositions
                .AsNoTracking()
                 .Where($"(Code+Name).Contains(@0) and IsActive = true", model.searchValue)
                .CountAsync();
            return results;
        }

        public async Task<int> Insert(MstPosition model)
        {
            int result = 0;

            if (model.PositionId == Guid.Empty)
            {
                var data = await _context.MstPositions.AsNoTracking().FirstOrDefaultAsync(x => x.Code.ToUpper() == model.Code.ToUpper());
                model.IsActive = true;
                if (data != null)
                {
                    model.PositionId = data.PositionId;
                    _context.MstPositions.Update(model);
                }
                else 
                {
                    _context.MstPositions.Add(model);
                }
                result = await _context.SaveChangesAsync();

                //Log
                if (result > 0)
                {
                    try
                    {
                        await _logChangesRepository.CreateLog<MstPosition>(GlobalConst.New, new Guid(model.CreatedBy!), new List<MstPosition> {  }, new List<MstPosition> { model });
                    }
                    catch (Exception ex) { }
                }
            }
            return result;
        }

        public async Task<int> Update(MstPosition model)
        {
            int result = 0;

            if (model != null && model.PositionId != Guid.Empty)
            {
                var data = await _context.MstPositions.AsNoTracking().FirstOrDefaultAsync(x => x.PositionId == model.PositionId);
                if (data != null)
                {
                    model.CreatedBy = data.CreatedBy;
                    model.CreatedDate = data.CreatedDate;
                    model.IsActive = true;
                    _context.Update(model);
                    result = await _context.SaveChangesAsync();

                    //Log
                    if (result > 0)
                    {
                        try
                        {
                            await _logChangesRepository.CreateLog<MstPosition>(GlobalConst.Update, new Guid(model.UpdateBy!), new List<MstPosition> { data }, new List<MstPosition> { model });
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            return result;
        }
    }
}
