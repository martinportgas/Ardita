﻿using Ardita.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface IFileArchiveDetailService
    {
        Task<IEnumerable<TrxFileArchiveDetail>> GetAll();
        Task<TrxFileArchiveDetail> GetById(Guid id);
    }
}
