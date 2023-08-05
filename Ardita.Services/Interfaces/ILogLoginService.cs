using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface ILogLoginService
    {
        Task<LogLogin> GetById(Guid id);
        Task<IEnumerable<LogLogin>> GetAll();
        Task<DataTableResponseModel<object>> GetByFilterModel(DataTablePostModel model);
        Task<int> Insert(LogLogin model);
    }
}
