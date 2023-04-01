﻿using Ardita.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<IEnumerable<MstCompany>> GetById(int id);
        Task<IEnumerable<MstCompany>> GetAll();
        Task<int> Insert(MstCompany model);
        Task<int> Delete(MstCompany model);
        Task<int> Update(MstCompany model);
    }
}
