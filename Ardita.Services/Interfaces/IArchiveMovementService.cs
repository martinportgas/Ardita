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
        Task<IEnumerable<TrxArchiveMovement>> GetById(Guid id);
        Task<IEnumerable<TrxArchiveMovement>> GetAll();
        Task<DataTableResponseModel<TrxArchiveMovement>> GetListArchiveMovement(DataTablePostModel model);
        Task<int> Insert(TrxArchiveMovement model);
        Task<bool> InsertBulk(List<TrxArchiveMovement> models);
        Task<int> Delete(TrxArchiveMovement model);
        Task<int> Update(TrxArchiveMovement model);
    }
}
