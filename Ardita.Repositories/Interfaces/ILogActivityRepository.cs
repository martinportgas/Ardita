using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface ILogActivityRepository
    {
        Task<LogActivity> GetById(Guid id);
        Task<IEnumerable<LogActivity>> GetAll();
        Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount(DataTableModel model);
        Task<int> Insert(LogActivity model);
    }
}
