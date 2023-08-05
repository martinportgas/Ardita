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
    public class LogChangesRepository : ILogChangesRepository
    {
        private readonly BksArditaDevContext _context;
        public LogChangesRepository(BksArditaDevContext context) 
        { 
            _context = context;
        }

        public async Task<IEnumerable<LogChange>> GetAll()
        {
            return await _context.LogChanges.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var result = await _context.LogChanges
          
            .Where(x => (x.Username + x.TableReference + x.ChangeDate).Contains(model.searchValue))
           .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
           .Skip(model.skip).Take(model.pageSize)
           .Select(x => new {
               x.LogChangeId,
               x.UserId,
               x.Username,
               x.TableReference,
               x.ChangeDate,
               x.OldValue,
               x.NewValue
           }).ToListAsync();
            return result;
        }

        public async Task<LogChange> GetById(Guid id)
        {
            return await _context.LogChanges.AsNoTracking().FirstAsync(x => x.LogChangeId == id);
        }

        public async Task<int> GetCount(DataTableModel model)
        {
            var result = await _context.LogChanges
            .Where(x => (x.Username + x.TableReference + x.ChangeDate).Contains(model.searchValue))
            .CountAsync();
          
            return result;
        }

        public async Task<int> Insert(LogChange model)
        {
            int result = 0;
            if (model != null)
            {
                _context.Add(model);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
    }
}
