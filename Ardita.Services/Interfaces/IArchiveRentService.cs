﻿using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface IArchiveRentService
    {
        Task<IEnumerable<TrxArchiveRent>> GetById(Guid id);
        Task<IEnumerable<TrxArchiveRent>> GetAll();
        Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
        Task<DataTableResponseModel<object>> GetApprovalList(DataTablePostModel model);
        Task<int> Insert(TrxArchiveRent model);
        Task<int> Delete(TrxArchiveRent model);
        Task<int> Update(TrxArchiveRent model);
        Task<int> Approval(Guid id, string description, int status, Guid User);
    }
}
