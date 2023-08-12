using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
               ChangeDate = x.ChangeDate.ToString(),
               x.ChangeType,
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
        public async Task<int> CreateLog<T>(string type, Guid userId, List<T> oldValue, List<T> newValue)
        {
            int result = 0;
            var users = await _context.MstUsers.FirstOrDefaultAsync(x => x.UserId == userId);

            var objLogDB = new LogChange();
            objLogDB.UserId = users.UserId;
            objLogDB.Username = users.Username;
            objLogDB.ChangeType = type;
            objLogDB.TableReference = typeof(T).Name;
            objLogDB.ChangeDate = DateTime.Now;
            if (newValue.Count > 0)
            {
                DataTable dtNew = newValue.ToDataTable();
                var listRemove = new List<DataColumn>();
                foreach (DataColumn c in dtNew.Columns)
                {
                    if (c.DataType.ToString().Contains("Models"))
                    {
                        listRemove.Add(c);
                    }
                }
                if(listRemove.Count > 0)
                {
                    foreach(DataColumn dc in listRemove)
                    {
                        dtNew.Columns.Remove(dc);
                    }
                }
                objLogDB.NewValue = JsonConvert.SerializeObject(dtNew);
            }
                
            if (oldValue.Count > 0)
            {
                DataTable dtOld = oldValue.ToDataTable();
                var listRemove = new List<DataColumn>();
                foreach (DataColumn c in dtOld.Columns)
                {
                    if (c.DataType.ToString().Contains("Models"))
                    {
                        listRemove.Add(c);
                    }  
                }
                if (listRemove.Count > 0)
                {
                    foreach (DataColumn dc in listRemove)
                    {
                        dtOld.Columns.Remove(dc);
                    }
                }
                objLogDB.OldValue = JsonConvert.SerializeObject(dtOld);
            }

            result = await Insert(objLogDB);
            return result;
        }
    }
}
