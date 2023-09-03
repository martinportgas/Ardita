using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services.Classess
{
    public class TemplateSettingService : ITemplateSettingService
    {
        private readonly ITemplateSettingRepository _templateSettingRepository;

        public TemplateSettingService(ITemplateSettingRepository templateSettingRepository) => _templateSettingRepository = templateSettingRepository;
        public async Task<int> Delete(MstTemplateSetting model)
        {
            return await _templateSettingRepository.Delete(model);
        }

        public async Task<IEnumerable<MstTemplateSetting>> GetAll(string par = " 1=1 ")
        {
            return await _templateSettingRepository.GetAll(par);
        }

        public async Task<MstTemplateSetting> GetById(Guid id)
        {
            return await _templateSettingRepository.GetById(id);
        }
        public async Task<DataTable> GetDataView(string viewName, Guid Id)
        {
            return await _templateSettingRepository.GetDataView(viewName, Id);
        }

        public async Task<DataTableResponseModel<object>> GetList(DataTablePostModel model)
        {
            try
            {
                var filterData = new DataTableModel
                {
                    sortColumn = model.columns[model.order[0].column].name,
                    sortColumnDirection = model.order[0].dir,
                    searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value,
                    pageSize = model.length,
                    skip = model.start,
                    IsArchiveActive = model.IsArchiveActive
                };

                var results = await _templateSettingRepository.GetByFilterModel(filterData);
                var dataCount = await _templateSettingRepository.GetCountByFilterModel(filterData);

                var responseModel = new DataTableResponseModel<object>
                {
                    draw = model.draw,
                    recordsTotal = dataCount,
                    recordsFiltered = dataCount,
                    data = results.ToList()
                };

                return responseModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> Insert(MstTemplateSetting model, IFormFile file, Tuple<string[], string[], string[], string[]> data)
        {
            var detail = await GetDetail(data);
            return await _templateSettingRepository.Insert(model, file, detail);
        }
        public async Task<IEnumerable<MstTemplateSettingDetail>> GetDetail(Tuple<string[], string[], string[], string[]> data)
        {
            await Task.Delay(0);
            List<MstTemplateSettingDetail> result = new();
            for (int i = 0; i < data.Item1.Length; i++)
            {
                MstTemplateSettingDetail item = new();
                item.TemplateSettingDetailId = Guid.NewGuid();
                item.VariableName = data.Item1[i];
                item.VariableType = data.Item2[i];
                item.VariableData = data.Item3[i];
                item.Other = data.Item4[i];
                result.Add(item);
            }
            return result;
        }

        public async Task<bool> InsertBulk(List<MstTemplateSetting> MstTemplateSettings)
        {
            return await _templateSettingRepository.InsertBulk(MstTemplateSettings);
        }

        public async Task<int> Update(MstTemplateSetting model, IFormFile file, Tuple<string[], string[], string[], string[]> data)
        {
            var detail = await GetDetail(data);
            return await _templateSettingRepository.Update(model, file, detail);
        }
        public List<string> GetListView()
        {
            return _templateSettingRepository.GetListView();
        }
        public List<string> GetListColumnByViewName(string viewName)
        {
            return _templateSettingRepository.GetListColumnByViewName(viewName);
        }
    }
}
