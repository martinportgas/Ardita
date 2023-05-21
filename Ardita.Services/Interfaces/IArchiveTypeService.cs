using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface IArchiveTypeService
{
    Task<IEnumerable<MstArchiveType>> GetById(Guid id);
    Task<IEnumerable<MstArchiveType>> GetAll();
    Task<DataTableResponseModel<MstArchiveType>> GetList(DataTablePostModel model);
    Task<int> Insert(MstArchiveType model);
    Task<bool> InsertBulk(List<MstArchiveType> MstArchiveTypes);
    Task<int> Delete(MstArchiveType model);
    Task<int> Update(MstArchiveType model);
}
