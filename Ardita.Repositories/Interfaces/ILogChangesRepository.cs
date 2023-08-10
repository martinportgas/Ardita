using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface ILogChangesRepository
    {
        Task<LogChange> GetById(Guid id);
        Task<IEnumerable<LogChange>> GetAll();
        Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount(DataTableModel model);
        Task<int> Insert(LogChange model);
        Task<int> CreateLog<T>(string type, Guid userId, List<T> oldValue, List<T> newValue);
    }
}
