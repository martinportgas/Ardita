using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface IArchiveExtendService
    {
        Task<IEnumerable<TrxArchiveExtend>> GetById(Guid id);
        Task<IEnumerable<TrxArchiveExtendDetail>> GetDetailByMainId(Guid id);
        Task<IEnumerable<TrxArchiveExtend>> GetAll();
        Task<DataTableResponseModel<TrxArchiveExtend>> GetList(DataTablePostModel model);
        Task<int> Insert(TrxArchiveExtend model);
        Task<bool> InsertBulkDetail(List<TrxArchiveExtendDetail> models);
        Task<bool> InsertBulk(List<TrxArchiveExtend> models);
        Task<int> Delete(TrxArchiveExtend model);
        Task<int> DeleteDetailByMainId(Guid Id);
        Task<int> Update(TrxArchiveExtend model);
    }
}
