using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.UserRoles;

namespace Ardita.Services.Interfaces
{
    public interface IUserRoleService
    {
        Task<IdxUserRole> GetById(Guid id);
        Task<IdxUserRole> GetByUserAndRoleId(Guid id, Guid role, Guid archiveUnit, Guid creator);
        Task<IEnumerable<IdxUserRole>> GetIdxUserRoleByUserId(Guid id);
        Task<int> GetCountIsPrimaryByUserId(Guid id);
        Task<IEnumerable<IdxUserRole>> GetAll();
        Task<UserRoleListViewModel> GetListUserRoles(Guid Id);
        Task<int> Insert(IdxUserRole model);
        Task<int> Delete(IdxUserRole model);
        Task<int> Update(IdxUserRole model);
        Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
    }
}
