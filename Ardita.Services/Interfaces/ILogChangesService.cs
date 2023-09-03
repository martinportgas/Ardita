using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface ILogChangesService
    {
        Task<LogChange> GetById(Guid id);
        Task<IEnumerable<LogChange>> GetAll(string par = " 1=1 ");
        Task<DataTableResponseModel<object>> GetByFilterModel(DataTablePostModel model);
        Task<int> Insert(LogChange model);
    }
}
