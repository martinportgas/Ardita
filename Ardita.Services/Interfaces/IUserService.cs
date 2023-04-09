using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Users;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<MstUser>> GetById(Guid id);
        Task<IEnumerable<MstUser>> GetAll();
        Task<UserListViewModel> GetListUsers(DataTableModel tableModel);
        Task<List<Claim>> GetLogin(string username, string password);
        Task<List<UserMenuListViewModel>> GetUserMenu(Guid id);
        Task<int> Insert(MstUser model);
        Task<bool> InsertBulk(List<MstUser> users);
        Task<int> Delete(MstUser model);
        Task<int> Update(MstUser model);
    }
}
