using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface IClassificationSubjectService
    {
        Task<IEnumerable<TrxSubjectClassification>> GetById(Guid id);
        Task<IEnumerable<TrxSubjectClassification>> GetByClassificationId(Guid id);
        Task<IEnumerable<TrxSubjectClassification>> GetAll();
        Task<DataTableResponseModel<TrxSubjectClassification>> GetListClassificationSubject(DataTablePostModel model);
        Task<int> Insert(TrxSubjectClassification model);
        Task<bool> InsertBulk(List<TrxSubjectClassification> models);
        Task<int> Delete(TrxSubjectClassification model);
        Task<int> Update(TrxSubjectClassification model);
    }
}
