using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Positions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface IPositionService
    {
        Task<IEnumerable<MstPosition>> GetById(Guid id);
        Task<MstPosition> GetFirstById(Guid id);
        Task<IEnumerable<MstPosition>> GetAll();
        Task<PositionListViewModel> GetListPosition(DataTableModel tableModel);
        Task<int> Insert(MstPosition model);
        Task<int> Delete(MstPosition model);
        Task<int> Update(MstPosition model);
    }
}
