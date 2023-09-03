using Ardita.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IPageDetailRepository
    {
        Task<IEnumerable<MstPageDetail>> GetById(Guid id);
        Task<IEnumerable<MstPageDetail>> GetByMainId(Guid id);
        Task<IEnumerable<MstPageDetail>> GetAll(string par = " 1=1 ");
        Task<int> Insert(MstPageDetail model);
        Task<int> Delete(MstPageDetail model);
        Task<int> DeleteByMainId(Guid id);
        Task<int> Update(MstPageDetail model);
    }
}
