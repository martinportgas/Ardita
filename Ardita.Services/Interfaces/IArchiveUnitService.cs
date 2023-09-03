using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface IArchiveUnitService
{
    Task<TrxArchiveUnit> GetById(Guid id);
    Task<IEnumerable<TrxArchiveUnit>> GetAll(string par = " 1=1 ");
    Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
    Task<int> Insert(TrxArchiveUnit model);
    Task<bool> InsertBulk(List<TrxArchiveUnit> trxArchiveUnits);
    Task<int> Delete(TrxArchiveUnit model);
    Task<int> Update(TrxArchiveUnit model);
    Task<IEnumerable<TrxArchiveUnit>> GetByListArchiveUnit(List<string> listArchiveUnitCode);
}
