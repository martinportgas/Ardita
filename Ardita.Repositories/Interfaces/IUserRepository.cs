﻿using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<MstUser> GetById(Guid id);
        Task<IEnumerable<MstUser>> GetAll(string par = " 1=1 ");
        Task<GetLoginModel> GetLogin(string username, string password);
        Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount(DataTableModel model);
        Task<int> Insert(MstUser model);
        Task<bool> InsertBulk(List<MstUser> users);
        Task<int> Delete(MstUser model);
        Task<int> Update(MstUser model);
        Task<int> ChangePassword(MstUser model);
        Task<bool> FindPasswordByUsername(Guid Id, string password);
    }
}
