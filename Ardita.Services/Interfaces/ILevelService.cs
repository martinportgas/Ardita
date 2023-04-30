using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface ILevelService
    {
        Task<TrxLevel> GetById(Guid id);
        Task<IEnumerable<TrxLevel>> GetAll();
        Task<DataTableResponseModel<TrxLevel>> GetListClassification(DataTablePostModel model);
        Task<int> Insert(TrxLevel model);
        Task<bool> InsertBulk(List<TrxLevel> levels);
        Task<int> Delete(TrxLevel model);
        Task<int> Update(TrxLevel model);
    }
}
