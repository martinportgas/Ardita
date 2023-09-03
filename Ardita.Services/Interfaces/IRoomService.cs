using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface IRoomService
    {
        Task<TrxRoom> GetById(Guid id);
        Task<IEnumerable<TrxRoom>> GetAll(string par = " 1=1 ");
        Task<DataTableResponseModel<TrxRoom>> GetListClassification(DataTablePostModel model);
        Task<int> Insert(TrxRoom model);
        Task<bool> InsertBulk(List<TrxRoom> rooms);
        Task<int> Delete(TrxRoom model);
        Task<int> Update(TrxRoom model);
    }
}
