using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using NPOI.HSSF.Record.Chart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Interfaces
{
    public interface IFloorService
    {
        Task<TrxFloor> GetById(Guid id);
        Task<IEnumerable<TrxFloor>> GetAll(string par = " 1=1 ");
        Task<DataTableResponseModel<object>> GetListClassification(DataTablePostModel model);
        Task<int> Insert(TrxFloor model);
        Task<bool> InsertBulk(List<TrxFloor> floors);
        Task<int> Delete(TrxFloor model);
        Task<int> Update(TrxFloor model);
    }
}
