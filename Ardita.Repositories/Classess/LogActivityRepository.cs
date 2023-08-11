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
    public class LogActivityRepository : ILogActivityRepository
    {
        private readonly BksArditaDevContext _context;
        private readonly ILogChangesRepository _logChangesRepository;
        public LogActivityRepository(BksArditaDevContext context, ILogChangesRepository logChangesRepository)
        {
            _context = context;
            _logChangesRepository = logChangesRepository;
        }

        public async Task<IEnumerable<LogActivity>> GetAll()
        {
            return await _context.LogActivities.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var result = await _context.LogActivities

            .Where(x => (x.Username + x.ActivityDate + x.PageName).Contains(model.searchValue))
            .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
            .Skip(model.skip).Take(model.pageSize)
             .Select(x => new {
                 x.LogActivityId,
                 x.UserId,
                 x.Username,
                 ActivityDate = x.ActivityDate.ToString(),
                 x.PageId,
                 x.PageName
             }).ToListAsync();
            return result;
        }

        public async Task<LogActivity> GetById(Guid id)
        {
            return await _context.LogActivities.AsNoTracking().FirstAsync(x => x.LogActivityId == id);
        }

        public async Task<int> GetCount(DataTableModel model)
        {
            var result = await _context.LogActivities
           .Where(x => (x.Username + x.ActivityDate + x.PageName).Contains(model.searchValue)).CountAsync();
            return result;
        }

        public async Task<int> Insert(LogActivity model)
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
