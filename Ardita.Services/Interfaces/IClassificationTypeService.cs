﻿using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface IClassificationTypeService
    {
        Task<MstTypeClassification> GetById(Guid id);
        Task<IEnumerable<MstTypeClassification>> GetAll(string par = " 1=1 ");
        Task<DataTableResponseModel<object>> GetListClassificationType(DataTablePostModel model);
        Task<int> Insert(MstTypeClassification model);
        Task<bool> InsertBulk(List<MstTypeClassification> models);
        Task<int> Delete(MstTypeClassification model);
        Task<int> Update(MstTypeClassification model);
    }
}
