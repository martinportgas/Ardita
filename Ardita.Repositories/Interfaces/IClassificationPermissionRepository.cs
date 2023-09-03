using Ardita.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IClassificationPermissionRepository
    {
        Task<TrxPermissionClassification> GetById(Guid id);
        Task<IEnumerable<TrxPermissionClassification>> GetByMainId(Guid id);
        Task<IEnumerable<TrxPermissionClassification>> GetAll(string par = " 1=1 ");
        Task<int> Insert(TrxPermissionClassification model);
        Task<bool> InsertBulk(List<TrxPermissionClassification> models);
        Task<int> Delete(TrxPermissionClassification model);
        Task<int> DeleteByMainId(Guid id);
        Task<int> Update(TrxPermissionClassification model);
    }
}
