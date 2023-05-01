using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace Ardita.Services.Classess;

public class ArchiveService : IArchiveService
{
    private readonly IArchiveUnitRepository _archiveUnitRepository;
    private readonly IArchiveRepository _archiveRepository;

    public ArchiveService(
        IArchiveUnitRepository archiveUnitRepository,
        IArchiveRepository archiveRepository
        )
    {
        _archiveUnitRepository = archiveUnitRepository;
        _archiveRepository = archiveRepository;
    }

    public async Task<int> Delete(TrxArchive model) => await _archiveRepository.Delete(model);

    public async Task<IEnumerable<TrxArchive>> GetAll() => await _archiveRepository.GetAll();

    public async Task<TrxArchive> GetById(Guid id)
    {
        return await _archiveRepository.GetById(id);
    }

    public async Task<DataTableResponseModel<TrxArchive>> GetList(DataTablePostModel model)
    {
        try
        {
            int dataCount = 0;
            if (model.PositionId != Guid.Empty)
            {
                dataCount = await _archiveRepository.GetCountForMonitoring(model.PositionId);
            }
            else
            {
                dataCount = await _archiveRepository.GetCount();
            }

            var filterData = new DataTableModel
            {
                sortColumn = model.columns[model.order[0].column].data,
                sortColumnDirection = model.order[0].dir,
                searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value,
                pageSize = model.length,
                skip = model.start,
                PositionId = model.PositionId
            };

            var results = await _archiveRepository.GetByFilterModel(filterData);

            var responseModel = new DataTableResponseModel<TrxArchive>
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

    public async Task<int> Insert(TrxArchive model, StringValues files)
    {
        List<FileModel> file = new();
        FileModel temp;
        string name;
        string type;
        string data;

        if (files[0] != string.Empty)
        {
            foreach (var item in files)
            {
                temp = new();
                JObject json = JObject.Parse(item);

                temp.FileName = (string)json[nameof(name)];
                temp.FileType = (string)json[nameof(type)];
                temp.Base64 = (string)json[nameof(data)];
                file.Add(temp);
            }
        }

        return await _archiveRepository.Insert(model, file);
    }

    public async Task<int> Update(TrxArchive model, StringValues files, string[] filesDeleted)
    {
        List<Guid> filesDeletedId = new();
        List<FileModel> file = new();
        FileModel temp;
        string name;
        string type;
        string data;

        if (files[0] != string.Empty)
        {
            foreach (var item in files)
            {
                temp = new();
                JObject json = JObject.Parse(item);

                temp.FileName = (string)json[nameof(name)];
                temp.FileType = (string)json[nameof(type)];
                temp.Base64 = (string)json[nameof(data)];
                file.Add(temp);
            }
        }

        if (filesDeleted.Any())
        {
            foreach (var item in filesDeleted)
            {
                filesDeletedId.Add(new Guid(item));
            }
        }

        return await _archiveRepository.Update(model, file, filesDeletedId);
    }
}
