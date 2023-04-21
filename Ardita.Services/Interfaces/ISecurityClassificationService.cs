using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;

namespace Ardita.Services.Interfaces;

public interface ISecurityClassificationService
{
    Task<IEnumerable<MstSecurityClassification>> GetById(Guid id);
    Task<IEnumerable<MstSecurityClassification>> GetAll();
    Task<DataTableResponseModel<MstSecurityClassification>> GetList(DataTablePostModel model);
    Task<int> Insert(MstSecurityClassification model);
    Task<int> Delete(MstSecurityClassification model);
    Task<int> Update(MstSecurityClassification model);
}
