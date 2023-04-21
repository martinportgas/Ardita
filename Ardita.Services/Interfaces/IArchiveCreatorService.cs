using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface IArchiveCreatorService
{
    Task<IEnumerable<MstCreator>> GetById(Guid id);
    Task<IEnumerable<MstCreator>> GetAll();
    Task<DataTableResponseModel<MstCreator>> GetList(DataTablePostModel model);
    Task<int> Insert(MstCreator model);
    Task<int> Delete(MstCreator model);
    Task<int> Update(MstCreator model);
}
