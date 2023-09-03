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
        Task<TrxArchiveExtend> GetById(Guid id);
        Task<IEnumerable<TrxArchiveExtendDetail>> GetDetailByMainId(Guid id);
        Task<IEnumerable<TrxArchiveExtend>> GetAll(string par = " 1=1 ");
        Task<IEnumerable<TrxArchiveExtendDetail>> GetDetailAll();
        Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
        Task<int> Insert(TrxArchiveExtend model);
        Task<int> InsertDetail(TrxArchiveExtendDetail model);
        Task<bool> InsertBulkDetail(List<TrxArchiveExtendDetail> models);
        Task<bool> InsertBulk(List<TrxArchiveExtend> models);
        Task<int> Delete(TrxArchiveExtend model);
        Task<int> DeleteDetailByMainId(Guid Id);
        Task<int> Update(TrxArchiveExtend model);
        Task<int> Submit(TrxArchiveExtend model);
    }
}
