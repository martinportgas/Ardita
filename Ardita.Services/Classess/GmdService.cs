using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;

namespace Ardita.Services.Classess;

public class GmdService : IGmdService
{
    private readonly IGmdRepository _GmdRepository;

    public GmdService(IGmdRepository GmdRepository)
    {
        _GmdRepository = GmdRepository;
    }

    public async Task<int> Delete(MstGmd model) => await _GmdRepository.Delete(model);

    public async Task<IEnumerable<MstGmd>> GetAll(string par = " 1=1 ") => await _GmdRepository.GetAll(par);

    public async Task<IEnumerable<MstGmd>> GetById(Guid id) => await _GmdRepository.GetById(id);

    public async Task<IEnumerable<MstGmdDetail>> GetDetailByGmdId(Guid id) => await _GmdRepository.GetDetailByGmdId(id);

    public async Task<MstGmdDetail> GetDetailById(Guid Id) => await _GmdRepository.GetDetailById(Id);

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
                skip = model.start
            };

            var results = await _GmdRepository.GetByFilterModel(filterData);
            var dataCount = await _GmdRepository.GetCountByFilterModel(filterData);

            var responseModel = new DataTableResponseModel<object>
            {
                draw = model.draw,
                recordsTotal = dataCount,
                recordsFiltered = dataCount,
                data = results.ToList()
            };

            return responseModel;
        }
        catch (Exception)
        {
            return null;
        }

    }

    public async Task<int> Insert(MstGmd model, string[] listDetail)
    {

        List<MstGmdDetail> details = await GetDetail(listDetail);

        return await _GmdRepository.Insert(model, details);
    }

    public async Task<int> Update(MstGmd model, string[] listDetail)
    {
        List<MstGmdDetail> details = await GetDetail(listDetail);

        return await _GmdRepository.Update(model, details);
    }

    public async Task<bool> InsertBulk(List<MstGmd> mstGmds)
    {
        return await _GmdRepository.InsertBulk(mstGmds);
    }

    private static async Task<List<MstGmdDetail>> GetDetail(string[] listDetail)
    {
        await Task.Delay(0);
        List<MstGmdDetail> details = new();


        foreach (var item in listDetail)
        {
            string[] words = item!.Split('#');
            MstGmdDetail detailTwo = new()
            {
                Name = words[0],
                Unit = words[1]
            };

            details.Add(detailTwo);
        }

        return details;
    }

    public async Task<IEnumerable<MstGmdDetail>> GetAllDetail() => await _GmdRepository.GetAllDetail();
    
}
