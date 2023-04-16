using Ardita.Models.DbModels;
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
        Task<IEnumerable<MstTypeClassification>> GetById(Guid id);
        Task<IEnumerable<MstTypeClassification>> GetAll();
        Task<DataTableResponseModel<MstTypeClassification>> GetListClassificationType(DataTablePostModel model);
        Task<int> Insert(MstTypeClassification model);
        Task<int> Delete(MstTypeClassification model);
        Task<int> Update(MstTypeClassification model);
    }
}
