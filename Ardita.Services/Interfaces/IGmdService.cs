using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface IGmdService
{
    Task<IEnumerable<MstGmd>> GetById(Guid id);
    Task<IEnumerable<MstGmd>> GetAll();
    Task<DataTableResponseModel<MstGmd>> GetList(DataTablePostModel model);
    Task<int> Insert(MstGmd model, string[] listDetail);
    Task<bool> InsertBulk(List<MstGmd> mstGmds);
    Task<int> Delete(MstGmd model);
    Task<int> Update(MstGmd model, string[] listDetail);
}
