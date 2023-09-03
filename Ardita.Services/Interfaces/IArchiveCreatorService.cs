using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface IArchiveCreatorService
{
    Task<IEnumerable<MstCreator>> GetById(Guid id);
    Task<IEnumerable<MstCreator>> GetAll(string par = " 1=1 ");
    Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
    Task<int> Insert(MstCreator model);
    Task<bool> InsertBulk(List<MstCreator> mstCreators);
    Task<int> Delete(MstCreator model);
    Task<int> Update(MstCreator model);
}
