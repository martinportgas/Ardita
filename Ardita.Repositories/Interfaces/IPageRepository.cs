using Ardita.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IPageRepository
    {
        Task<IEnumerable<MstPage>> GetById(Guid id);
        Task<IEnumerable<MstPage>> GetAll();
        Task<int> Insert(MstPage model);
        Task<int> Delete(MstPage model);
        Task<int> Update(MstPage model);
    }
}
