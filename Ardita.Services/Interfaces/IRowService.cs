using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface IRowService
    {
        Task<IEnumerable<TrxRow>> GetById(Guid id);
        Task<IEnumerable<TrxRow>> GetAll();
        Task<DataTableResponseModel<TrxRow>> GetListClassification(DataTablePostModel model);
        Task<int> Insert(TrxRow model);
        Task<bool> InsertBulk(List<TrxRow> rows);
        Task<int> Delete(TrxRow model);
        Task<int> Update(TrxRow model);
    }
}
