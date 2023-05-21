using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface IArchiveOwnerService
{
    Task<IEnumerable<MstArchiveOwner>> GetById(Guid id);
    Task<IEnumerable<MstArchiveOwner>> GetAll();
    Task<DataTableResponseModel<MstArchiveOwner>> GetList(DataTablePostModel model);
    Task<int> Insert(MstArchiveOwner model);
    Task<bool> InsertBulk(List<MstArchiveOwner> MstArchiveOwners);
    Task<int> Delete(MstArchiveOwner model);
    Task<int> Update(MstArchiveOwner model);
}
