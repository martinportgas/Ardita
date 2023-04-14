using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface IClassificationService
    {
        Task<IEnumerable<TrxClassification>> GetById(Guid id);
        Task<IEnumerable<TrxClassification>> GetAll();
        Task<DataTableResponseModel<TrxClassification>> GetListClassification(DataTablePostModel model);
        Task<int> Insert(TrxClassification model);
        Task<int> Delete(TrxClassification model);
        Task<int> Update(TrxClassification model);
    }
}
