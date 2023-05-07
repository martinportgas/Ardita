using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface IArchiveDestroyService
    {
        Task<TrxArchiveDestroy> GetById(Guid id);
        Task<IEnumerable<TrxArchiveDestroyDetail>> GetDetailByMainId(Guid id);
        Task<IEnumerable<TrxArchiveDestroy>> GetAll();
        Task<IEnumerable<TrxArchiveDestroyDetail>> GetDetailAll();
        Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
        Task<int> Insert(TrxArchiveDestroy model);
        Task<int> InsertDetail(TrxArchiveDestroyDetail model);
        Task<bool> InsertBulk(List<TrxArchiveDestroy> models);
        Task<bool> InsertBulkDetail(List<TrxArchiveDestroyDetail> models);
        Task<int> Delete(TrxArchiveDestroy model);
        Task<int> DeleteDetailByMainId(Guid Id);
        Task<int> Update(TrxArchiveDestroy model);
        Task<int> Submit(TrxArchiveDestroy model);
    }
}
