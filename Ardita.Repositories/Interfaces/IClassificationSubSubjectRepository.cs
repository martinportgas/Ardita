﻿using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IClassificationSubSubjectRepository
    {
        Task<IEnumerable<TrxSubSubjectClassification>> GetById(Guid id);
        Task<IEnumerable<TrxSubSubjectClassification>> GetAll();
        Task<IEnumerable<TrxSubSubjectClassification>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount();
        Task<int> Insert(TrxSubSubjectClassification model);
        Task<int> Delete(TrxSubSubjectClassification model);
        Task<int> Update(TrxSubSubjectClassification model);
    }
}