﻿using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<MstEmployee> GetById(Guid id);
        Task<IEnumerable<MstEmployee>> GetAll();
        Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount(DataTableModel model);
        Task<int> Insert(MstEmployee model);
        Task<bool> InsertBulk(List<MstEmployee> employees);
        Task<int> Delete(MstEmployee model);
        Task<int> Update(MstEmployee model);
    }
}
