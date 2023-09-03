using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface IArchiveMovementService
    {
        Task<TrxArchiveMovement> GetById(Guid id);
        Task<IEnumerable<TrxArchiveMovementDetail>> GetDetailByMainId(Guid id);
        Task<IEnumerable<TrxArchiveMovement>> GetAll(string par = " 1=1 ");
        Task<IEnumerable<TrxArchiveMovementDetail>> GetDetailAll();
        Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
        Task<int> Insert(TrxArchiveMovement model);
        Task<int> InsertDetail(TrxArchiveMovementDetail model);
        Task<bool> InsertBulk(List<TrxArchiveMovement> models);
        Task<bool> InsertBulkDetail(List<TrxArchiveMovementDetail> models);
        Task<int> Delete(TrxArchiveMovement model);
        Task<int> DeleteDetailByMainId(Guid Id);
        Task<int> Update(TrxArchiveMovement model);
        Task<int> Submit(TrxArchiveMovement model);
    }
}
