using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Data;

namespace Ardita.Repositories.Interfaces;

public interface ITemplateSettingRepository
{
    Task<MstTemplateSetting> GetById(Guid id);
    Task<IEnumerable<MstTemplateSetting>> GetAll();
    Task<IEnumerable<object>> GetByFilterModel(DataTableModel model);
    Task<int> GetCountByFilterModel(DataTableModel model);
    Task<int> Insert(MstTemplateSetting model, IFormFile file, IEnumerable<MstTemplateSettingDetail> detail);
    Task<bool> InsertBulk(List<MstTemplateSetting> MstTemplateSettings);
    Task<int> Delete(MstTemplateSetting model);
    Task<int> Update(MstTemplateSetting model, IFormFile file, IEnumerable<MstTemplateSettingDetail> detail);
    List<string> GetListView();
    List<string> GetListColumnByViewName(string viewName);
    Task<DataTable> GetDataView(string viewName, Guid Id);
}
