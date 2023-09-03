using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories.Interfaces
{
    public interface IPositionRepository
    {
        Task<MstPosition> GetById(Guid id);
        Task<IEnumerable<MstPosition>> GetAll(string par = " 1=1 ");
        Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
        Task<int> GetCount(DataTableModel model);
        Task<int> Insert(MstPosition model);
        Task<int> Delete(MstPosition model);
        Task<int> Update(MstPosition model);
    }
}
