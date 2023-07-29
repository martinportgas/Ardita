﻿using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IClassificationSubSubjectRepository
    {
        Task<TrxSubSubjectClassification> GetById(Guid id);
        Task<IEnumerable<TrxSubSubjectClassification>> GetAll();
        Task<IEnumerable<TrxSubSubjectClassification>> GetByArchiveUnit(List<string> listArchiveUnitCode);
        Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount(DataTableModel model);
        Task<int> Insert(TrxSubSubjectClassification model);
        Task<bool> InsertBulk(List<TrxSubSubjectClassification> models);
        Task<int> Delete(TrxSubSubjectClassification model);
        Task<int> Update(TrxSubSubjectClassification model);
    }
}
