using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Archive;
using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Ardita.Services.Classess;

public class ArchiveService : IArchiveService
{
    private readonly IArchiveUnitRepository _archiveUnitRepository;
    private readonly IArchiveRepository _archiveRepository;
    private readonly IConfiguration _configuration;


    public ArchiveService(
        IArchiveUnitRepository archiveUnitRepository,
        IArchiveRepository archiveRepository,
        IConfiguration configuration
        )
    {
        _archiveUnitRepository = archiveUnitRepository;
        _archiveRepository = archiveRepository;
        _configuration = configuration;
    }

    public async Task<int> Delete(TrxArchive model) => await _archiveRepository.Delete(model);

    public async Task<IEnumerable<TrxArchive>> GetAll(string par = " 1=1 ") => await _archiveRepository.GetAll(par);
    public async Task<int> GetCount(string par = " 1=1 ") => await _archiveRepository.GetCount(par);
    public async Task<IEnumerable<TrxArchive>> GetAllInActive(string par = " 1=1 ") => await _archiveRepository.GetAllInActive(par);

    public async Task<TrxArchive> GetById(Guid id)
    {
        return await _archiveRepository.GetById(id);
    }
    public async Task<IEnumerable<TrxArchive>> GetByParams(string param = "1=1")
    {
        return await _archiveRepository.GetByParams(param);
    }

    public async Task<DataTableResponseModel<object>> GetList(DataTablePostModel model)
    {
        try
        {
            List<string> listArchiveUnitCode = model.SessionUser != null ? AppUsers.CurrentUser(model.SessionUser).ListArchiveUnitCode : new List<string>();
            DateTime startDate = GlobalConst.MinDate;
            DateTime endDate = GlobalConst.MaxDate;
            bool validStart = DateTime.TryParse(model.columns[0].search.value, out startDate);
            bool validEnd = DateTime.TryParse(model.columns[1].search.value, out endDate);

            var filterData = new DataTableModel
            {
                sortColumn = model.columns[model.order[0].column].name,
                sortColumnDirection = model.order[0].dir,
                searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value,
                pageSize = model.length,
                skip = model.start,
                PositionId = model.PositionId,
                listArchiveUnitCode = listArchiveUnitCode,
                whereClause = model.whereClause,
                advanceSearch = new SearchModel
                {
                    StartDate = validStart ? startDate : GlobalConst.MinDate,
                    EndDate = validEnd ? endDate : GlobalConst.MaxDate,
                    Search = model.columns[2].search.value == null ? "1=1" : model.columns[2].search.value
                },
                IsArchiveActive = model.IsArchiveActive,
                SessionUser = model.SessionUser
            };

            int dataCount = await _archiveRepository.GetCountByFilterData(filterData);
            var results = await _archiveRepository.GetByFilterModel(filterData);

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
    public async Task<IEnumerable<ArchiveExportModel>> GetExportList(DataTablePostModel model)
    {
        try
        {
            List<string> listArchiveUnitCode = model.SessionUser != null ? AppUsers.CurrentUser(model.SessionUser).ListArchiveUnitCode : new List<string>();
            DateTime startDate = GlobalConst.MinDate;
            DateTime endDate = GlobalConst.MaxDate;
            bool validStart = DateTime.TryParse(model.columns[0].search.value, out startDate);
            bool validEnd = DateTime.TryParse(model.columns[1].search.value, out endDate);

            var filterData = new DataTableModel
            {
                sortColumn = model.columns[model.order[0].column].name,
                sortColumnDirection = model.order[0].dir,
                searchValue = string.IsNullOrEmpty(model.search.value) ? string.Empty : model.search.value,
                pageSize = model.length,
                skip = model.start,
                PositionId = model.PositionId,
                listArchiveUnitCode = listArchiveUnitCode,
                whereClause = model.whereClause,
                advanceSearch = new SearchModel
                {
                    StartDate = validStart ? startDate : GlobalConst.MinDate,
                    EndDate = validEnd ? endDate : GlobalConst.MaxDate,
                    Search = model.columns[2].search.value == null ? "1=1" : model.columns[2].search.value
                },
                IsArchiveActive = model.IsArchiveActive,
                SessionUser = model.SessionUser
            };

            var results = await _archiveRepository.GetExportByFilterModel(filterData);

            return results;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    public async Task<int> Insert(TrxArchive model, StringValues files)
    {
        int result;
        List<FileModel> file = new();
        FileModel temp;
        string name;
        string type;
        string data;
        var basePath = _configuration[GlobalConst.BASE_PATH_ARCHIVE];
        var basePathTemp = _configuration[GlobalConst.BASE_PATH_TEMP_ARCHIVE];

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

        string path = $"{basePathTemp}{model.CreatedDate:yyyy}\\{model.CreatedDate:MM}\\{model.CreatedDate:dd}";
        if (model.StatusId == (int)GlobalConst.STATUS.Submit)
            path = $"{basePath}{await _archiveRepository.GetPathArchive(model.SubSubjectClassificationId, model.CreatedDateArchive)}";

        result = await _archiveRepository.Insert(model, file, path);

        return result;
    }

    public async Task<bool> InsertBulk(List<TrxArchive> trxArchives)
    {
        return await _archiveRepository.InsertBulk(trxArchives);
    }
    public async Task<int> Submit(TrxArchive model)
    {
        return await _archiveRepository.Submit(model);
    }
    public async Task<int> Update(TrxArchive model, StringValues files, string[] filesDeleted)
    {
        int result;
        List<Guid> filesDeletedId = new();
        List<FileModel> file = new();
        FileModel temp;
        string name;
        string type;
        string data;
        var basePath = _configuration[GlobalConst.BASE_PATH_ARCHIVE];
        var basePathTemp = _configuration[GlobalConst.BASE_PATH_TEMP_ARCHIVE];


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

        string path = $"{basePathTemp}{model.CreatedDate:yyyy}\\{model.CreatedDate:MM}\\{model.CreatedDate:dd}";
        if (model.StatusId == (int)GlobalConst.STATUS.Submit)
            path = $"{basePath}{await _archiveRepository.GetPathArchive(model.SubSubjectClassificationId, model.CreatedDateArchive)}";

        result = await _archiveRepository.Update(model, file, filesDeletedId, path);

        //if (model.StatusId == (int)GlobalConst.STATUS.Submit)
        //{
        //    //TrxArchive archive = await _archiveRepository.GetById(model.ArchiveId);
        //    string dest = $"{basePath}\\{await _archiveRepository.GetPathArchive(model.SubSubjectClassificationId, model.CreatedDateArchive)}\\{model.ArchiveCode}\\";
        //    string src = $"{basePathTemp}\\{model.CreatedDate:yyyy}\\{model.CreatedDate:MM}\\{model.CreatedDate:dd}\\{model.ArchiveCode}\\";

        //    DirectoryMove(src, dest, false);
        //}

        return result;
    }

    private static void DirectoryMove(string sourceDirName, string destDirName, bool copySubDirs)
    {
        DirectoryInfo dir = new(sourceDirName);
        DirectoryInfo[] dirs = dir.GetDirectories();

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }

        FileInfo[] files = dir.GetFiles();

        foreach (FileInfo file in files)
        {
            string temppath = Path.Combine(destDirName, file.Name);

            file.CopyTo(temppath, false);
        }

        if (copySubDirs)
        {

            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);

                DirectoryMove(subdir.FullName, temppath, copySubDirs);
            }
        }

        Directory.Delete(sourceDirName, true);
    }

    public async Task<IEnumerable<object>> GetAvailableArchiveBySubSubjectId(Guid subSubjectId, Guid mediaStorageId = new Guid(), string year = "", Guid gmdDetailId = new Guid()) => await _archiveRepository.GetAvailableArchiveBySubSubjectId(subSubjectId, mediaStorageId, year, gmdDetailId);
    public async Task<IEnumerable<TrxArchive>> GetAvailableArchiveInActiveBySubSubjectId(Guid subSubjectId, Guid mediaStorageId = new Guid(), string year = "") => await _archiveRepository.GetAvailableArchiveInActiveBySubSubjectId(subSubjectId, mediaStorageId, year);

    public async Task<IEnumerable<TrxArchive>> GetArchiveActiveBySubjectId(Guid subSubjectId)
    {
        return await _archiveRepository.GetArchiveActiveBySubjectId(subSubjectId);
    }
}
