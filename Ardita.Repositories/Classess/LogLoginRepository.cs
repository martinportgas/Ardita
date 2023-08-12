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
    public class LogLoginRepository : ILogLoginRepository
    {
        private readonly BksArditaDevContext _context;

        public LogLoginRepository(BksArditaDevContext context)
        { 
            _context = context;
        }

        public async Task<IEnumerable<LogLogin>> GetAll()
        {
            return await _context.LogLogins.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<object>> GetByFilterModel(DataTableModel model)
        {
            var result = await _context.LogLogins
             .Where(x => (x.Username + x.LoginDate + x.ComputerName + x.IpAddress + x.MacAddress).Contains(model.searchValue))
             .OrderBy($"{model.sortColumn} {model.sortColumnDirection}")
             .Skip(model.skip).Take(model.pageSize)
             .Select(x => new {
                 x.LogLoginId,
                 x.UserId,
                 x.Username,
                 LoginDate = x.LoginDate.ToString(),
                 x.ComputerName,
                 x.IpAddress,
                 x.MacAddress,
                 x.OsName,
                 x.BrowserName
             })
             .ToListAsync();
            return result;
        }

        public async Task<LogLogin> GetById(Guid id)
        {
            var data = await _context.LogLogins.AsNoTracking().FirstAsync(x => x.LogLoginId == id);
            return data;
        }

        public async Task<int> GetCount(DataTableModel model)
        {
            return await _context.LogLogins.AsNoTracking().Where(x => (x.Username + x.LoginDate + x.ComputerName + x.IpAddress + x.MacAddress)
            .Contains(model.searchValue)).CountAsync();
        }

        public async Task<int> Insert(LogLogin model)
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
