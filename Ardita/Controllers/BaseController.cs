using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Archive;
using Ardita.Models.ViewModels.SubSubjectClasscification;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.HPSF;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Ardita.Controllers;

public abstract class BaseController<T> : Controller
{
    #region Properties
    protected IHostingEnvironment _hostingEnvironment;

    //User Manage
    protected IEmployeeService _employeeService { get; set; }
    protected IPositionService _positionService { get; set; }
    protected IMenuService _menuService { get; set; }
    protected ISubMenuService _subMenuService { get; set; }
    protected IUserService _userService { get; set; }
    protected IPageService _pageService { get; set; }
    protected IRoleService _roleService { get; set; }
    protected IRolePageService _rolePageService { get; set; }
    protected IUserRoleService _userRoleService { get; set; }

    //Master Data
    protected IArchiveUnitService _archiveUnitService { get; set; }
    protected ICompanyService _companyService { get; set; }
    protected IArchiveCreatorService _archiveCreatorService { get; set; }
    protected IClassificationService _classificationService { get; set; }
    protected IClassificationTypeService _classificationTypeService { get; set; }
    protected IClassificationSubjectService _classificationSubjectService { get; set; }
    protected IClassificationSubSubjectService _classificationSubSubjectService { get; set; }
    protected IFloorService _floorService { get; set; }
    protected IGmdService _gmdService { get; set; }
    protected ILevelService _levelService { get; set; }
    protected IRackService _rackService { get; set; }
    protected IRoomService _roomService { get; set; }
    protected IRowService _rowService { get; set; }
    protected ISecurityClassificationService _securityClassificationService { get; set; }
    protected IArchiveService _archiveService { get; set; }
    protected IArchiveOwnerService _archiveOwnerService { get; set; }
    protected IArchiveTypeService _archiveTypeService { get; set; }
    protected ISubTypeStorageService _subTypeStorageService { get; set; }

    //Trx
    protected IFileArchiveDetailService _fileArchiveDetailService { get; set; }
    protected IMediaStorageService _mediaStorageService { get; set; }
    protected ITypeStorageService _typeStorageService { get; set; }
    protected IArchiveRetentionService _archiveRetentionService { get; set; }
    protected IArchiveExtendService _archiveExtendService { get; set; }
    protected IArchiveDestroyService _archiveDestroyService { get; set; }
    protected IArchiveMovementService _archiveMovementService { get; set; }
    protected IArchiveApprovalService _archiveApprovalService { get; set; }
    protected IArchiveReceivedService ArchiveReceivedService { get; set; } = null!;
    protected IMediaStorageInActiveService MediaStorageInActiveService { get; set; } = null!;
    protected IArchiveRentService _archiveRentService { get; set; } = null!;
  
    protected ISubTypeStorageService SubTypeStorageService { get; set; } = null!;
    #endregion

    #region Main Action
    public virtual async Task<ActionResult> Index()
    {
        await Task.Delay(0);
        return View();
    }

    public virtual async Task<JsonResult> GetData(DataTablePostModel model)
    {
        await Task.Delay(0);
        throw new NotImplementedException();
    }

    public virtual async Task<IActionResult> Add()
    {
        await Task.Delay(0);
        throw new NotImplementedException();
    }

    public virtual async Task<IActionResult> Update(Guid Id)
    {
        await Task.Delay(0);
        throw new NotImplementedException();
    }

    public virtual async Task<IActionResult> Remove(Guid Id)
    {
        await Task.Delay(0);
        throw new NotImplementedException();
    }

    public virtual async Task<IActionResult> Detail(Guid Id)
    {
        await Task.Delay(0);
        throw new NotImplementedException();
    }
    public virtual async Task<IActionResult> Preview(Guid Id)
    {
        await Task.Delay(0);
        throw new NotImplementedException();
    }
    public virtual async Task<IActionResult> Approval(Guid Id, int Level)
    {
        await Task.Delay(0);
        throw new NotImplementedException();
    }

    public virtual async Task<IActionResult> Save(T model)
    {
        await Task.Delay(0);
        throw new NotImplementedException();
    }

    public virtual async Task<IActionResult> Delete(T model)
    {
        await Task.Delay(0);
        throw new NotImplementedException();
    }
    public virtual async Task<IActionResult> Submit(T model)
    {
        await Task.Delay(0);
        throw new NotImplementedException();
    }
    public virtual async Task<IActionResult> SubmitApproval(T model)
    {
        await Task.Delay(0);
        throw new NotImplementedException();
    }
    #endregion

    #region "Global Action"
    public async Task<IActionResult> GetFileArchive(Guid Id, bool IsDownload = true)
    {
        var data = await _fileArchiveDetailService.GetById(Id);
        if (data != null)
        {
            var path = string.Concat(data.FilePath, data.FileNameEncrypt);
            var bytes = System.IO.File.ReadAllBytes(path);

            if(IsDownload)
                return File(bytes, data.FileType, data.FileName);
            else
                return File(bytes, data.FileType);
        }
        if (IsDownload)
            return File(new byte[] { }, "application/octet-stream", "NotFound.txt");
        else
            return File(new byte[] { }, "application/octet-stream");
    }

    #endregion

    #region Binding
    //selectlist
    #region SelectListItem
    public async Task<List<SelectListItem>> BindCompanies()
    {
        var data = await _companyService.GetAll();
        return data.Select(x => new SelectListItem
        {
            Value = x.CompanyId.ToString(),
            Text = x.CompanyName.ToString()
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindRoles()
    {
        var data = await _roleService.GetAll();
        return data.Select(x => new SelectListItem
        {
            Value = x.RoleId.ToString(),
            Text = x.Name.ToString()
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindRacks()
    {
        var data = await _rackService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.RackId.ToString(),
            Text = x.RackName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindRooms()
    {
        var data = await _roomService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.RoomId.ToString(),
            Text = x.RoomName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindFloors()
    {
        var data = await _floorService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.FloorId.ToString(),
            Text = x.FloorName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindArchiveUnits()
    {
        var data = await _archiveUnitService.GetByListArchiveUnit(AppUsers.CurrentUser(User).ListArchiveUnitCode);
        return data.Select(x => new SelectListItem
        {
            Value = x.ArchiveUnitId.ToString(),
            Text = x.ArchiveUnitName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindAllArchiveUnits()
    {
        var data = await _archiveUnitService.GetAll();
        data = data.OrderBy(x => x.ArchiveUnitName);
        return data.Where(x => x.CompanyId == AppUsers.CurrentUser(User).CompanyId).Select(x => new SelectListItem
        {
            Value = x.ArchiveUnitId.ToString(),
            Text = x.ArchiveUnitCode + " - " + x.ArchiveUnitName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindLevels()
    {
        var data = await _levelService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.LevelId.ToString(),
            Text = x.LevelName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindGmds()
    {
        var data = await _gmdService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.GmdId.ToString(),
            Text = x.GmdName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindSubSubjectClasscifications()
    {
        var data = await _classificationSubSubjectService.GetByArchiveUnit(AppUsers.CurrentUser(User).ListArchiveUnitCode);

        return data.Select(x => new SelectListItem
        {
            Value = x.SubSubjectClassificationId.ToString(),
            Text = x.SubSubjectClassificationName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindAllSubSubjectClasscifications()
    {
        var data = await _classificationSubSubjectService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.SubSubjectClassificationId.ToString(),
            Text = x.SubSubjectClassificationName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindSecurityClassifications()
    {
        var data = await _securityClassificationService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.SecurityClassificationId.ToString(),
            Text = x.SecurityClassificationName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindArchiveCreators()
    {
        var data = await _archiveCreatorService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.CreatorId.ToString(),
            Text = x.CreatorName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindClassificationTypes()
    {
        var data = await _classificationTypeService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.TypeClassificationId.ToString(),
            Text = x.TypeClassificationName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindClasscifications()
    {
        var data = await _classificationService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.ClassificationId.ToString(),
            Text = x.ClassificationName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindClasscificationSubjects()
    {
        var data = await _classificationSubjectService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.SubjectClassificationId.ToString(),
            Text = x.SubjectClassificationName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindPositions()
    {
        var data = await _positionService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.PositionId.ToString(),
            Text = x.Name
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindArchives()
    {
        var data = await _archiveService.GetAll(AppUsers.CurrentUser(User).ListArchiveUnitCode);

        return data.Select(x => new SelectListItem
        {
            Value = x.ArchiveId.ToString(),
            Text = x.TitleArchive.ToString()
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindArchivesInActive()
    {
        var data = await _archiveService.GetAllInActive(AppUsers.CurrentUser(User).ListArchiveUnitCode);

        return data
            .Select(x => new SelectListItem
        {
            Value = x.ArchiveId.ToString(),
            Text = x.TitleArchive.ToString()
        })
        .ToList();
    }
    public async Task<List<SelectListItem>> BindTypeStorage()
    {
        var data = await _typeStorageService.GetAll();
        return data.Select(x => new SelectListItem
        {
            Value = x.TypeStorageId.ToString(),
            Text = x.TypeStorageName.ToString()
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindRows()
    {
        var data = await _rowService.GetAll();
        return data.Select(x => new SelectListItem
        {
            Value = x.RowId.ToString(),
            Text = x.RowName.ToString()
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindTypeStorageByCompanyId(Guid Id)
    {
        var data = await _typeStorageService.GetAll();
        var result = data.Where(x => x.ArchiveUnit.CompanyId == Id).ToList();
        return result.Select(x => new SelectListItem
        {
            Value = x.TypeStorageId.ToString(),
            Text = x.TypeStorageName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindEmployeeIdBySubSubjectClassificationId(Guid Id)
    {
        var result = await _employeeService.GetListEmployeeBySubSubjectClassificationId(Id);
        return result.Select(x => new SelectListItem
        {
            Value = x.EmployeeId.ToString(),
            Text = x.Name
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindEmployee()
    {
        var result = await _employeeService.GetAll();
        return result.Select(x => new SelectListItem
        {
            Value = x.EmployeeId.ToString(),
            Text = x.Name
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindArchiveRetention()
    {
        var result = await _archiveRetentionService.GetAll();
        return result.Select(x => new SelectListItem
        {
            Value = x.ArchiveId.ToString(),
            Text = x.TitleArchive
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindArchiveOwners()
    {
        var data = await _archiveOwnerService.GetAll();
        return data.Select(x => new SelectListItem
        {
            Value = x.ArchiveOwnerId.ToString(),
            Text = x.ArchiveOwnerName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindArchiveTypes()
    {
        var data = await _archiveTypeService.GetAll();
        return data.Select(x => new SelectListItem
        {
            Value = x.ArchiveTypeId.ToString(),
            Text = x.ArchiveTypeName
        }).ToList();
    }
    #endregion
    //json
    #region Json Result
    public async Task<JsonResult> BindArchiveUnitsByParam(string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        var data = await _archiveUnitService.GetAll();
        var result = data.Where(x => x.ArchiveUnitName.ToLower().Contains(param.ToLower())).Select(x => new
        {
            id = x.ArchiveUnitId.ToString(),
            text = x.ArchiveUnitName
        }).OrderBy(x => x.text).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindFloorsByArchiveUnitId(string Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        List<TrxFloor> listFloors = new();
        Guid ArchiveUnitId = new(Id);

        var data = await _floorService.GetAll();
        listFloors = data.Where(x => x.ArchiveUnitId == ArchiveUnitId && x.FloorName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.FloorName).ToList();
        return Json(listFloors);

    }
    public async Task<JsonResult> BindRoomByFloorId(string Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        List<TrxRoom> list = new();
        Guid id = new(Id);

        var data = await _roomService.GetAll();
        list = data.Where(x => x.FloorId == id && x.RoomName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.RoomName).ToList();
        return Json(list);

    }
    public async Task<JsonResult> BindRoomActiveByFloorId(string Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        List<TrxRoom> list = new();
        Guid id = new(Id);

        var data = await _roomService.GetAll();
        list = data.Where(x => x.FloorId == id && x.ArchiveRoomType == GlobalConst.UnitPengolah && x.RoomName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.RoomName).ToList();
        return Json(list);
    }
    public async Task<JsonResult> BindRackByRoomId(string Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        List<TrxRack> list = new();
        Guid id = new(Id);

        var data = await _rackService.GetAll();
        list = data.Where(x => x.RoomId == id && x.RackName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.RackName).ToList();
        return Json(list);
    }
    public async Task<JsonResult> BindLevelByRackId(string Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        List<TrxLevel> list = new();
        Guid id = new(Id);

        var data = await _levelService.GetAll();
        list = data.Where(x => x.RackId == id && x.LevelName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.LevelName).ToList();
        return Json(list);
    }
    public async Task<JsonResult> BindRowByLevelId(string Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        List<TrxRow> list = new();
        Guid id = new(Id);

        var data = await _rowService.GetAll();
        list = data.Where(x => x.LevelId == id && x.RowName!.ToLower().Contains(param.ToLower())).ToList();
        return Json(list);
    }
    public async Task<JsonResult> BindRowArchiveByLevelId(string Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        List<TrxRow> list = new();
        Guid id = new(Id);

        var data = await _rowService.GetAll();
        list = data.Where(x => x.LevelId == id && x.TrxMediaStorages.FirstOrDefault() == null && x.RowName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.RowName).ToList();
        return Json(list);
    }
    public async Task<JsonResult> BindRowArchiveInActiveByLevelId(string Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        List<TrxRow> list = new();
        Guid id = new(Id);

        var data = await _rowService.GetAll();
        list = data.Where(x => x.LevelId == id && x.TrxMediaStorageInActives.FirstOrDefault() == null && x.RowName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.RowName).ToList();
        return Json(list);
    }
    public async Task<JsonResult> BindClassificationSubjectIdByClassificationId(Guid Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        var data = await _classificationSubjectService.GetAll();
        var result = data.Where(x => x.ClassificationId == Id && x.SubjectClassificationName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.SubjectClassificationName).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindClassificationIdByClassificationTypeId(Guid Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        var data = await _classificationService.GetAll();
        var result = data.Where(x => x.TypeClassificationId == Id && x.ClassificationName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.ClassificationName).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindSubSubjectClasscificationsById(string Id)
    {
        Guid SubSubjectClasscificationId = new(Id);

        var data = await _classificationSubSubjectService.GetById(SubSubjectClasscificationId);
        var subSubjectClassification = new SubSubjectClasscificationViewModel
        {
            SubSubjectClassificationId = data.SubSubjectClassificationId,
            SubSubjectClassificationCode = data.SubSubjectClassificationCode,
            CreatorId = data.CreatorId,
            CreatorName = data.Creator?.CreatorName,
            RetentionActive = data.RetentionActive,
            RetentionInactive = data.RetentionInactive,
            SubSubjectClassificationName = data.SubSubjectClassificationName
        };

        return Json(subSubjectClassification);

    }
    public async Task<JsonResult> BindArchiveById(string Id)
    {
        Guid ArchiveId = new(Id);

        var data = await _archiveService.GetById(ArchiveId);
        ArchiveViewModel result = new()
        {
            ArchiveId = data.ArchiveId,
            ArchiveCode = data.ArchiveCode!,
            TitleArchive = data.TitleArchive,
            TypeArchive = data.ArchiveType.ArchiveTypeName,
            TypeSender = data.TypeSender,
            Volume = data.Volume,
            ArchiveCreator = data.SubSubjectClassification.Creator!.CreatorName
        };
        return Json(result);
    }
    public async Task<JsonResult> BindArchivesBySubSubjectClassificationId(Guid Id, Guid mediaStorageId = new Guid(), string year = "") => Json(await _archiveService.GetAvailableArchiveBySubSubjectId(Id, mediaStorageId, year));
    public async Task<JsonResult> BindArchivesInActiveBySubSubjectClassificationId(Guid Id, Guid mediaStorageId = new Guid(), string year = "") => Json(await _archiveService.GetAvailableArchiveInActiveBySubSubjectId(Id, mediaStorageId, year));
    public async Task<JsonResult> BindTypeStorageByArchiveUnitId(Guid Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        var data = await _typeStorageService.GetAll();
        var result = data.Where(x => x.ArchiveUnitId == Id && x.TypeStorageName.ToLower().Contains(param.ToLower())).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindSubSubjectClassificationByArchiveUnitId(Guid Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        var data = await _classificationSubSubjectService.GetAll();
        var result = data.Where(x => x.Creator!.ArchiveUnitId == Id && x.SubSubjectClassificationName!.ToLower().Contains(param.ToLower())).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindTypeStorageByParam(string param = "")
    {
        string[] arrParam = param.Split(',');

        var keyword = string.IsNullOrEmpty(arrParam[0]) ? string.Empty : arrParam[0];
        Guid DetailId = Guid.Empty;
        Guid.TryParse(arrParam[1], out DetailId);

        var data = await _typeStorageService.GetAll();
        var result = data.Where(x => x.ArchiveUnitId == DetailId).Select(x => new
        {
            id = x.TypeStorageId.ToString(),
            text = x.TypeStorageName
        }).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindArchiveRetentionByParam(string param)
    {
        string[] arrParam = param.Split(',');

        var keyword = string.IsNullOrEmpty(arrParam[0]) ? string.Empty : arrParam[0];
        Guid DetailId = Guid.Empty;
        Guid.TryParse(arrParam[1], out DetailId);
        var type = arrParam[2];
        Guid SubSubjectClassificationId = Guid.Empty;
        Guid.TryParse(arrParam[3], out SubSubjectClassificationId);
        int year = DateTime.Now.Year;
        int.TryParse(arrParam[4], out year);

        var data = await _archiveRetentionService.GetAll();

        var dataExt = await _archiveExtendService.GetDetailAll();
        var dataDst = await _archiveDestroyService.GetDetailAll();
        var dataMove = await _archiveMovementService.GetDetailAll();

        var detailExt = type == GlobalConst.ArchiveExtend ? dataExt.Where(x => x.ArchiveExtendId != DetailId) : dataExt;
        var detailDst = type == GlobalConst.ArchiveDestroy ? dataDst.Where(x => x.ArchiveDestroyId != DetailId) : dataDst;
        var detailMove = type == GlobalConst.ArchiveMovement ? dataMove.Where(x => x.ArchiveMovementId != DetailId) : dataMove;

        var result =
            (from dataALl in data
             join dataDetailExt in detailExt on dataALl.ArchiveId equals dataDetailExt.ArchiveId into a
             from dataDetailExt in a.DefaultIfEmpty()
             join dataDetailDst in detailDst on dataALl.ArchiveId equals dataDetailDst.ArchiveId into b
             from dataDetailDst in b.DefaultIfEmpty()
             join dataDetailMove in detailMove on dataALl.ArchiveId equals dataDetailMove.ArchiveId into c
             from dataDetailMove in c.DefaultIfEmpty()
             where dataALl.SubSubjectClassificationId == SubSubjectClassificationId && dataALl.CreatedDateArchive.Year == year 
             && dataDetailExt == null && dataDetailDst == null && dataDetailMove == null && (dataALl.ArchiveCode + dataALl.TitleArchive).ToLower().Contains(keyword.ToLower())
             select new
             {
                 id = dataALl.ArchiveId,
                 text = dataALl.ArchiveCode + " - " + dataALl.TitleArchive,
                 order = dataALl.TitleArchive
             }
             )
             .OrderBy(x => x.order)
             .ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindArchiveRetentionInActiveByParam(string param)
    {
        string[] arrParam = param.Split(',');

        var keyword = string.IsNullOrEmpty(arrParam[0]) ? string.Empty : arrParam[0];
        Guid DetailId = Guid.Empty;
        Guid.TryParse(arrParam[1], out DetailId);
        var type = arrParam[2];
        Guid SubSubjectClassificationId = Guid.Empty;
        Guid.TryParse(arrParam[3], out SubSubjectClassificationId);
        int year = DateTime.Now.Year;
        int.TryParse(arrParam[4], out year);

        var data = await _archiveRetentionService.GetInActiveAll();

        var dataExt = await _archiveExtendService.GetDetailAll();
        var dataDst = await _archiveDestroyService.GetDetailAll();

        var detailExt = type == GlobalConst.ArchiveExtend ? dataExt.Where(x => x.ArchiveExtendId != DetailId) : dataExt;
        var detailDst = type == GlobalConst.ArchiveDestroy ? dataDst.Where(x => x.ArchiveDestroyId != DetailId) : dataDst;

        var result =
            (from dataALl in data
             join dataDetailExt in detailExt on dataALl.ArchiveId equals dataDetailExt.ArchiveId into a
             from dataDetailExt in a.DefaultIfEmpty()
             join dataDetailDst in detailDst on dataALl.ArchiveId equals dataDetailDst.ArchiveId into b
             from dataDetailDst in b.DefaultIfEmpty()
             where dataALl.SubSubjectClassificationId == SubSubjectClassificationId && dataALl.DateReceived.Year == year 
             && dataDetailExt == null && dataDetailDst == null && (dataALl.ArchiveCode + dataALl.TitleArchive).ToLower().Contains(keyword.ToLower())
             select new
             {
                 id = dataALl.ArchiveId,
                 text = dataALl.ArchiveCode + " - " + dataALl.TitleArchive,
                 order = dataALl.TitleArchive
             }
             )
             .OrderBy(x => x.order)
             .ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindEmployeeByParam(string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        var data = await _employeeService.GetAll();
        var result = data.Where(x => x.Name.ToLower().Contains(param.ToLower())).Select(x =>
            new {
                id = x.EmployeeId,
                text = x.Name
            }
            ).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindApprovalByParam(string param = "")
    {
        string[] arrParam = param.Split(',');

        var keyword = string.IsNullOrEmpty(arrParam[0]) ? string.Empty : arrParam[0];
        Guid archiveUnitId = Guid.Empty;
        Guid.TryParse(arrParam[1], out archiveUnitId);

        var data = await _employeeService.GetListApproval(archiveUnitId);
        var result = data.Where(x => x.Name.ToLower().Contains(keyword.ToLower())).Select(x =>
            new {
                id = x.EmployeeId,
                text = x.Name
            }
            ).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindTypeStorageById(Guid Id)
    {
        var data = await _typeStorageService.GetById(Id);
        return Json(data);
    }
    public async Task<JsonResult> BindSubTypeStorageByTypeStorageId(Guid Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        var data = await _subTypeStorageService.GetAllByTypeStorageId(Id);
        var result = data.Where(x => x.SubTypeStorageName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.SubTypeStorageName).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindSubTypeStorage(string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        var data = await _subTypeStorageService.GetAll();
        var result = data.Where(x => x.SubTypeStorageName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.SubTypeStorageName).ToList();
        return Json(result);
    }

    public async Task<JsonResult> BindArchiveActiveBySubjectId(Guid Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        var data = await _archiveService.GetArchiveActiveBySubjectId(Id);
        var result = data.Where(x => x.TitleArchive!.ToLower().Contains(param.ToLower())).OrderBy(x => x.TitleArchive).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindSubTypeStorageById(Guid Id)
    {
        var data = await _subTypeStorageService.GetById(Id);
        return Json(data.FirstOrDefault());
    }
    #endregion

    #endregion
}
