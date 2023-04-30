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
        Task<IEnumerable<TrxArchiveDestroy>> GetById(Guid id);
        Task<IEnumerable<TrxArchiveDestroy>> GetAll();
        Task<DataTableResponseModel<TrxArchiveDestroy>> GetListArchiveDestroy(DataTablePostModel model);
        Task<int> Insert(TrxArchiveDestroy model);
        Task<bool> InsertBulk(List<TrxArchiveDestroy> models);
        Task<int> Delete(TrxArchiveDestroy model);
        Task<int> Update(TrxArchiveDestroy model);
    }
}
