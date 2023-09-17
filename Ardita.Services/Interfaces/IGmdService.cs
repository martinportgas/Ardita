using Ardita.Models;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface IGmdService
{
    Task<IEnumerable<MstGmd>> GetById(Guid id);
    Task<IEnumerable<MstGmd>> GetAll(string par = " 1=1 ");
    Task<IEnumerable<object>> GetGMDGroupByArchiveCount(GlobalSearchModel search, string par = " 1=1 ");
    Task<IEnumerable<MstGmdDetail>> GetAllDetail();
    Task<IEnumerable<MstGmdDetail>> GetDetailByGmdId(Guid Id);
    Task<MstGmdDetail> GetDetailById(Guid Id);
    Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
    Task<int> Insert(MstGmd model, string[] listDetail);
    Task<bool> InsertBulk(List<MstGmd> mstGmds);
    Task<int> Delete(MstGmd model);
    Task<int> Update(MstGmd model, string[] listDetail);
}
