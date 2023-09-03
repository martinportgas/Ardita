using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Data;

namespace Ardita.Services.Interfaces;

public interface ITemplateSettingService
{
    Task<MstTemplateSetting> GetById(Guid id);
    Task<IEnumerable<MstTemplateSetting>> GetAll(string par = " 1=1 ");
    Task<DataTableResponseModel<object>> GetList(DataTablePostModel model);
    Task<int> Insert(MstTemplateSetting model, IFormFile file, Tuple<string[], string[], string[], string[]> data);
    Task<bool> InsertBulk(List<MstTemplateSetting> MstTemplateSettings);
    Task<int> Delete(MstTemplateSetting model);
    Task<int> Update(MstTemplateSetting model, IFormFile file, Tuple<string[], string[], string[], string[]> data);
    List<string> GetListView();
    List<string> GetListColumnByViewName(string viewName);
    Task<DataTable> GetDataView(string viewName, Guid Id);
}
