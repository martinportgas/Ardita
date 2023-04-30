using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IMenuRepository
    {
        Task<MstMenu> GetById(Guid id);
        Task<IEnumerable<MstMenu>> GetAll();
        Task<IEnumerable<MstMenu>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount();
        Task<int> Insert(MstMenu model);
        Task<int> Delete(MstMenu model);
        Task<int> Update(MstMenu model);
    }
}
