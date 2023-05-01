using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface IClassificationSubSubjectService
    {
        Task<TrxSubSubjectClassification> GetById(Guid id);
        Task<IEnumerable<TrxPermissionClassification>> GetDetailByMainId(Guid id);
        Task<IEnumerable<TrxSubSubjectClassification>> GetAll();
        Task<DataTableResponseModel<TrxSubSubjectClassification>> GetListClassificationSubSubject(DataTablePostModel model);
        Task<IEnumerable<TrxPermissionClassification>> GetListDetailPermissionClassifications(Guid id);
        Task<int> Insert(TrxSubSubjectClassification model);
        Task<bool> InsertBulk(List<TrxSubSubjectClassification> models);
        Task<int> InsertDetail(TrxPermissionClassification model);
        Task<int> Delete(TrxSubSubjectClassification model);
        Task<int> DeleteDetail(Guid id);
        Task<int> Update(TrxSubSubjectClassification model);
    }
}
