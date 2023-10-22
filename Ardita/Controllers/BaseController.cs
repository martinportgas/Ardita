using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Archive;
using Ardita.Models.ViewModels.SubSubjectClasscification;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NPOI.HPSF;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Ardita.Controllers;

public abstract class BaseController<T> : Controller
{
    #region Properties
    protected IHostingEnvironment _hostingEnvironment;
    protected ISessionService _sessionService;

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
    protected IMediaStorageInActiveService _MediaStorageInActiveService { get; set; } = null!;
    protected IArchiveRentService _archiveRentService { get; set; } = null!;
    protected IArchiveOutIndicatorService _archiveOutIndicatorService { get; set; } = null!;

    protected ISubTypeStorageService SubTypeStorageService { get; set; } = null!;
    protected IGeneralSettingsService GeneralSettingsService { get; set; } = null!;

    //Log

    protected ILogLoginService _logLoginService { get; set; } = null;
    protected ILogChangesService _logChangesService { get; set; } = null;
    protected ILogActivityService _logActivityService { get; set; } = null;

    //Configuration
    protected ITemplateSettingService _templateSettingService { get; set; } = null;

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
    public async Task<List<SelectListItem>> BindViews()
    {
        await Task.Delay(0);
        var data = _templateSettingService.GetListView();
        List<SelectListItem> result = new();
        if(data.Count > 0)
        {
            foreach (var item in data) 
            {
                var row = new SelectListItem
                {
                    Value = item,
                    Text = item
                };
                result.Add(row);
            }
        }
        return result;
    }
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
        string par = "1=1";
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            par += $" && Room.Floor.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
        var data = await _rackService.GetAll(par);

        return data.Select(x => new SelectListItem
        {
            Value = x.RackId.ToString(),
            Text = x.RackName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindRooms()
    {
        string par = "1=1";
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            par += $" && Floor.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
        var data = await _roomService.GetAll(par);
        //if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
        //    data = data.Where(x => x.Floor.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();

        return data.Select(x => new SelectListItem
        {
            Value = x.RoomId.ToString(),
            Text = x.RoomName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindFloors()
    {
        string par = "1=1";
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            par += $" && ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
        var data = await _floorService.GetAll(par);
        //if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
        //    data = data.Where(x => x.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();

        return data.Select(x => new SelectListItem
        {
            Value = x.FloorId.ToString(),
            Text = x.FloorName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindArchiveUnits()
    {
        string par = "1=1";
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            par += $" && ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
        var data = await _archiveUnitService.GetAll(par);
        return data.Select(x => new SelectListItem
        {
            Value = x.ArchiveUnitId.ToString(),
            Text = x.ArchiveUnitName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindAllArchiveUnits()
    {
        string par = $" CompanyId == \"{AppUsers.CurrentUser(User).CompanyId}\" ";
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            par += $" && ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
        var data = await _archiveUnitService.GetAll(par);
        //if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
        //    data = data.Where(x => x.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        data = data.OrderBy(x => x.ArchiveUnitName);
        return data.Select(x => new SelectListItem
        {
            Value = x.ArchiveUnitId.ToString(),
            Text = x.ArchiveUnitCode + " - " + x.ArchiveUnitName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindLevels()
    {
        string par = "1=1";
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            par += $" && Rack.Room.Floor.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
        var data = await _levelService.GetAll(par);
        //if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
        //    data = data.Where(x => x.Rack.Room.Floor.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();

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
    public async Task<List<SelectListItem>> BindGmdDetail()
    {
        var data = await _gmdService.GetAllDetail();

        return data.Select(x => new SelectListItem
        {
            Value = x.GmdDetailId.ToString(),
            Text = x.Name
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindSubjectClasscifications()
    {
        string par = "1=1";
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            par += $" && Classification.Creator.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
        if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
            par += $" && Classification.CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
        var data = await _classificationSubjectService.GetAll(par);
        //if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
        //    data = data.Where(x => x.Classification.Creator.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        //if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
        //    data = data.Where(x => x.Classification.CreatorId == AppUsers.CurrentUser(User).CreatorId).ToList();

        return data.Select(x => new SelectListItem
        {
            Value = x.SubjectClassificationId.ToString(),
            Text = x.SubjectClassificationCode + " - " + x.SubjectClassificationName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindBorrower()
    {
        var data = await _archiveRentService.GetBorrower();

        return data.Select(x => new SelectListItem
        {
            Value = x.BorrowerId.ToString(),
            Text = x.BorrowerName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindSubSubjectClasscifications()
    {
        string par = "1=1";
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            par += $" && Creator.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
        if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
            par += $" && CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
        var data = await _classificationSubSubjectService.GetAll(par);
        //if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
        //    data = data.Where(x => x.Creator.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        //if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
        //    data = data.Where(x => x.CreatorId == AppUsers.CurrentUser(User).CreatorId).ToList();

        return data.Select(x => new SelectListItem
        {
            Value = x.SubSubjectClassificationId.ToString(),
            Text = x.SubSubjectClassificationName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindMySubSubjectClasscifications()
    {
        string par = "1=1";
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            par += $" && Creator.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
        if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
            par += $" && CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
        var data = await _classificationSubSubjectService.GetAll(par);
        //if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
        //    data = data.Where(x => x.Creator.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        //if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
        //    data = data.Where(x => x.CreatorId == AppUsers.CurrentUser(User).CreatorId).ToList();

        return data.Select(x => new SelectListItem
        {
            Value = x.SubSubjectClassificationId.ToString(),
            Text = x.SubSubjectClassificationName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindAllSubjectClasscifications()
    {
        string par = "1=1";
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            par += $" && Classification.Creator.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
        if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
            par += $" && Classification.CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
        var data = await _classificationSubjectService.GetAll(par);
        //if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
        //    data = data.Where(x => x.Classification.Creator.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        //if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
        //    data = data.Where(x => x.Classification.CreatorId == AppUsers.CurrentUser(User).CreatorId).ToList();

        return data.Select(x => new SelectListItem
        {
            Value = x.SubjectClassificationId.ToString(),
            Text = x.SubjectClassificationCode + " - " + x.SubjectClassificationName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindAllSubSubjectClasscifications()
    {
        string par = "1=1";
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            par += $" && Creator.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
        if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
            par += $" && CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
        var data = await _classificationSubSubjectService.GetAll(par);
        //if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
        //    data = data.Where(x => x.Creator.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        //if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
        //    data = data.Where(x => x.CreatorId == AppUsers.CurrentUser(User).CreatorId).ToList();

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
        string par = "1=1";
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            par += $" && ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
        if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
            par += $" && CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
        var data = await _archiveCreatorService.GetAll(par);
        //if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
        //    data = data.Where(x => x.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        //if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
        //    data = data.Where(x => x.CreatorId == AppUsers.CurrentUser(User).CreatorId).ToList();

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
        string par = "1=1";
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            par += $" && Creator.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
        if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
            par += $" && CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
        var data = await _classificationService.GetAll(par);
        //if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
        //    data = data.Where(x => x.Creator.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        //if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
        //    data = data.Where(x => x.CreatorId == AppUsers.CurrentUser(User).CreatorId).ToList();

        return data.Select(x => new SelectListItem
        {
            Value = x.ClassificationId.ToString(),
            Text = x.ClassificationName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindClasscificationSubjects()
    {
        string par = "1=1";
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            par += $" && Classification.Creator.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
        if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
            par += $" && Classification.CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
        var data = await _classificationSubjectService.GetAll(par);

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
        string par = "1=1";
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            par += $" && Creator.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
        if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
            par += $" && CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
        var data = await _archiveService.GetAll(par);
        //if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
        //    data = data.Where(x => x.Creator.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        //if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
        //    data = data.Where(x => x.CreatorId == AppUsers.CurrentUser(User).CreatorId).ToList();

        return data.Select(x => new SelectListItem
        {
            Value = x.ArchiveId.ToString(),
            Text = x.TitleArchive.ToString()
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindArchivesInActive()
    {
        string par = "1=1";
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            par += $" && Creator.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
        if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
            par += $" && CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
        var data = await _archiveService.GetAllInActive();
        //if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
        //    data = data.Where(x => x.Creator.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        //if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
        //    data = data.Where(x => x.CreatorId == AppUsers.CurrentUser(User).CreatorId).ToList();

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
        string par = "1=1";
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            par += $" && ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
        var data = await _typeStorageService.GetAll(par);
        //if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
        //    data = data.Where(x => x.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        return data.Select(x => new SelectListItem
        {
            Value = x.TypeStorageId.ToString(),
            Text = x.TypeStorageName.ToString()
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindRows()
    {
        string par = "1=1";
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            par += $" && Level.Rack.Room.Floor.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
        var data = await _rowService.GetAll(par);
        //if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
        //    data = data.Where(x => x.Level.Rack.Room.Floor.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        return data.Select(x => new SelectListItem
        {
            Value = x.RowId.ToString(),
            Text = x.RowName.ToString()
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindRowsWithDetails()
    {
        string spr = " - ";
        string par = "1=1";
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            par += $" && Level.Rack.Room.Floor.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
        var data = await _rowService.GetAll(par);
        //if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
        //    data = data.Where(x => x.Level.Rack.Room.Floor.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        return data.Select(x => new SelectListItem
        {
            Value = x.RowId.ToString(),
            Text = x.Level.Rack.Room.Floor.ArchiveUnit.ArchiveUnitName + spr + x.Level.Rack.Room.Floor.FloorName + spr + x.Level.Rack.Room.RoomName + spr + x.Level.Rack.RackName + spr + x.Level.LevelName + spr + x.RowName
        }).ToList();
    }
    public async Task<List<SelectListItem>> BindTypeStorageByCompanyId(Guid Id)
    {
        string par = $" ArchiveUnit.CompanyId == \"{Id}\" ";
        var result = await _typeStorageService.GetAll(par);
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
            Text = x.Nik + " - " + x.Name
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

    public async Task<JsonResult> BindColumnByTableName(string id, string param = "")
    {
        await Task.Delay(0);
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        var data = _templateSettingService.GetListColumnByViewName(id).Where(x => x.ToLower().Contains(param.ToLower()));
        List<object> result = new();
        if (data.Count() > 0)
        {
            foreach (var item in data)
            {
                var row = new
                {
                    id = item,
                    text = item
                };
                result.Add(row);
            }
        }
        return Json(result);
    }
    public async Task<JsonResult> BindArchiveOwnerByType(string type, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;

        var data = await _archiveOwnerService.GetAll();
        var result = data
                    .Where(x => x.ArchiveOwnerType.ToLower() == type.ToLower())
                    .Where(x => x.ArchiveOwnerName.ToLower().Contains(param))
                    .Select(x => new
                    {
                        id = x.ArchiveOwnerId.ToString(),
                        text = x.ArchiveOwnerName
                    }).OrderBy(x => x.text).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindGmdDetailByGmdId(string Id, string param = "")
    {

        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        List<MstGmdDetail> list = new();
        Guid id = new(Id);

        var data = await _gmdService.GetDetailByGmdId(new Guid(Id));
        list = data.Where(x => x.Name!.ToLower().Contains(param.ToLower())).OrderBy(x => x.Name).ToList();
        return Json(list);
    }
    public async Task<JsonResult> BindGmdDetailById(string Id)
    {
        Guid id = new(Id);
        var data = await _gmdService.GetDetailById(new Guid(Id));
        return Json(data);
    }
    public async Task<JsonResult> BindArchiveUnitsByParam(string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        var data = await _archiveUnitService.GetAll();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        var result = data.Where(x => x.ArchiveUnitName.ToLower().Contains(param.ToLower())).Select(x => new
        {
            id = x.ArchiveUnitId.ToString(),
            text = x.ArchiveUnitName
        }).OrderBy(x => x.text).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindArchiveUnitsByCompanyIdAndParam(Guid Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        var data = await _archiveUnitService.GetAll();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        if (Id != Guid.Empty)
            data = data.Where(x => x.CompanyId == Id).ToList();
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
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        listFloors = data.Where(x => x.ArchiveUnitId == ArchiveUnitId && x.FloorName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.FloorName).ToList();
        return Json(listFloors);

    }
    public async Task<JsonResult> BindParamFloorsByArchiveUnitId(Guid Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;

        var data = await _floorService.GetAll();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        if (Id != Guid.Empty)
            data = data.Where(x => x.ArchiveUnitId == Id).ToList();
        var result = data.Where(x => x.FloorName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.FloorName).Select(
            x => new
            {
                id = x.FloorId.ToString(),
                text = x.FloorName
            }
            ).ToList();
        return Json(result);

    }
    public async Task<JsonResult> BindRoomByFloorId(Guid Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;

        var data = await _roomService.GetAll();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.Floor.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        if (Id != Guid.Empty)
            data = data.Where(x => x.FloorId == Id).ToList();
        var result = data.Where(x => x.RoomName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.RoomName).OrderBy(x => x.RoomName).Select(
            x => new
            {
                id = x.RoomId.ToString(),
                text = x.RoomName
            }
            ).ToList();
        return Json(result);

    }
    public async Task<JsonResult> BindRoomActiveByFloorId(string Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        List<TrxRoom> list = new();
        Guid id = new(Id);

        var data = await _roomService.GetAll();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.Floor.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        list = data.Where(x => x.FloorId == id && x.ArchiveRoomType == GlobalConst.UnitPengolah && x.RoomName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.RoomName).ToList();
        return Json(list);
    }
    public async Task<JsonResult> BindParamRoomActiveByFloorId(Guid Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;

        var data = await _roomService.GetAll();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.Floor.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        if (Id != Guid.Empty)
            data = data.Where(x => x.FloorId == Id).ToList();
        var result = data.Where(x => x.ArchiveRoomType == GlobalConst.UnitPengolah && x.RoomName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.RoomName).OrderBy(x => x.RoomName).Select(
            x => new
            {
                id = x.RoomId.ToString(),
                text = x.RoomName
            }
            ).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindParamRoomInActiveByFloorId(Guid Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;

        var data = await _roomService.GetAll();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.Floor.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        if (Id != Guid.Empty)
            data = data.Where(x => x.FloorId == Id).ToList();
        var result = data.Where(x => x.ArchiveRoomType == GlobalConst.UnitKearsipan && x.RoomName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.RoomName).OrderBy(x => x.RoomName).Select(
            x => new
            {
                id = x.RoomId.ToString(),
                text = x.RoomName
            }
            ).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindRoomActiveByArchiveUnitId(Guid Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;

        var data = await _roomService.GetAll();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.Floor.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        if (Id != Guid.Empty)
            data = data.Where(x => x.Floor.ArchiveUnitId == Id).ToList();
        var result = data.Where(x => x.ArchiveRoomType == GlobalConst.UnitPengolah && x.RoomName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.RoomName).Select(
            x => new
            {
                id = x.RoomId.ToString(),
                text = x.RoomName
            }
            ).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindRoomInActiveByFloorId(string Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        List<TrxRoom> list = new();
        Guid id = new(Id);

        var data = await _roomService.GetAll();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.Floor.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        list = data.Where(x => x.FloorId == id && x.ArchiveRoomType == GlobalConst.UnitKearsipan && x.RoomName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.RoomName).ToList();
        return Json(list);
    }
    public async Task<JsonResult> BindRackByRoomId(string Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        List<TrxRack> list = new();
        Guid id = new(Id);

        var data = await _rackService.GetAll();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.Room.Floor.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        list = data.Where(x => x.RoomId == id && x.RackName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.RackName).ToList();
        return Json(list);
    }
    public async Task<JsonResult> BindParamRackByRoomId(Guid Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;

        var data = await _rackService.GetAll();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.Room.Floor.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        if (Id != Guid.Empty)
            data = data.Where(x => x.RoomId == Id).ToList();
        var result = data.Where(x => x.RackName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.RackName).Select(
            x => new
            {
                id = x.RackId.ToString(),
                text = x.RackName
            }
            ).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindLevelByRackId(string Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        List<TrxLevel> list = new();
        Guid id = new(Id);

        var data = await _levelService.GetAll();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.Rack.Room.Floor.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        list = data.Where(x => x.RackId == id && x.LevelName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.LevelName).ToList();
        return Json(list);
    }
    public async Task<JsonResult> BindParamLevelByRackId(Guid Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;

        var data = await _levelService.GetAll();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.Rack.Room.Floor.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        if (Id != Guid.Empty)
            data = data.Where(x => x.RackId == Id).ToList();
        var result = data.Where(x => x.LevelName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.LevelName).Select(
            x => new
            {
                id = x.LevelId.ToString(),
                text = x.LevelName
            }
            ).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindRowByLevelId(string Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        List<TrxRow> list = new();
        Guid id = new(Id);

        var data = await _rowService.GetAll();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.Level.Rack.Room.Floor.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        list = data.Where(x => x.LevelId == id && x.RowName!.ToLower().Contains(param.ToLower())).ToList();
        return Json(list);
    }
    public async Task<JsonResult> BindParamRowByLevelId(Guid Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;

        var data = await _rowService.GetAll();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.Level.Rack.Room.Floor.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        if (Id != Guid.Empty)
            data = data.Where(x => x.LevelId == Id).ToList();
        var result = data.Where(x => x.RowName!.ToLower().Contains(param.ToLower())).Select(
            x => new
            {
                id = x.RowId.ToString(),
                text = x.RowName
            }
            ).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindParamCreatorByArchiveUnitId(Guid Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;

        var data = await _archiveCreatorService.GetAll();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
            data = data.Where(x => x.CreatorId == AppUsers.CurrentUser(User).CreatorId).ToList();
        if (Id != Guid.Empty)
            data = data.Where(x => x.ArchiveUnitId == Id).ToList();
        var result = data.Where(x => x.CreatorName!.ToLower().Contains(param.ToLower())).Select(
            x => new
            {
                id = x.CreatorId.ToString(),
                text = x.CreatorName
            }
            ).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindRowArchiveByLevelId(string Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        List<TrxRow> list = new();
        Guid id = new(Id);

        var data = await _rowService.GetAll();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.Level.Rack.Room.Floor.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        list = data.Where(x => x.LevelId == id && x.TrxMediaStorages.FirstOrDefault() == null && x.RowName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.RowName).ToList();
        return Json(list);
    }
    public async Task<JsonResult> BindRowArchiveInActiveByLevelId(string Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        List<TrxRow> list = new();
        Guid id = new(Id);

        var data = await _rowService.GetAll();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.Level.Rack.Room.Floor.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        list = data.Where(x => x.LevelId == id && x.TrxMediaStorageInActives.FirstOrDefault() == null && x.RowName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.RowName).ToList();
        return Json(list);
    }
    public async Task<JsonResult> BindRowAvailableArchiveInActiveByLevelId(string Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        List<TrxRow> list = new();
        Guid id = new(Id);

        var data = await _rowService.GetAvailableRow();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.Level.Rack.Room.Floor.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
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
    public async Task<JsonResult> BindParamClassificationSubjectIdByClassificationId(Guid Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        var data = await _classificationSubjectService.GetAll();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.Classification.Creator.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
            data = data.Where(x => x.Classification.CreatorId == AppUsers.CurrentUser(User).CreatorId).ToList();
        if (Id != Guid.Empty)
            data = data.Where(x => x.ClassificationId == Id).ToList();
        var result = data.Where(x => x.SubjectClassificationName!.ToLower().Contains(param.ToLower())).OrderBy(x => x.SubjectClassificationName).Select(
            x => new
            {
                id = x.SubjectClassificationId.ToString(),
                text = x.SubjectClassificationCode + " - " + x.SubjectClassificationName
            }
            ).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindClassificationIdByClassificationTypeId(Guid Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        var data = await _classificationService.GetAll();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.Creator.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
            data = data.Where(x => x.CreatorId == AppUsers.CurrentUser(User).CreatorId).ToList();
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
    public async Task<object> BindArchivesBySubSubjectClassificationId(Guid Id, Guid mediaStorageId = new Guid(), string year = "", Guid gmdDetailId = new Guid()) 
    {
        var data = await _archiveService.GetAvailableArchiveBySubSubjectId(Id, mediaStorageId, year, gmdDetailId);
        var result = Json(data);
        return result;
    } 
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
        string par = "1=1";
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            par += $" && Creator.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
        if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
            par += $" && CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
        var data = await _classificationSubSubjectService.GetAll(par);
        var result = data.Where(x => x.Creator!.ArchiveUnitId == Id && x.SubSubjectClassificationName!.ToLower().Contains(param.ToLower())).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindSubSubjectClassificationBySubjectId(Guid Id, Guid SubjectId, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        string par = "1=1";
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            par += $" && Creator.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
        if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
            par += $" && CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
        var data = await _classificationSubSubjectService.GetAll(par);
        //if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
        //    data = data.Where(x => x.Creator.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        //if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
        //    data = data.Where(x => x.CreatorId == AppUsers.CurrentUser(User).CreatorId).ToList();
        var result = data.Where(x => x.Creator!.ArchiveUnitId == Id && x.SubjectClassificationId == SubjectId && x.SubSubjectClassificationName!.ToLower().Contains(param.ToLower()))
            .Select(x => new { id = x.SubSubjectClassificationId, text = x.SubSubjectClassificationName }).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindSubjectClassificationByArchiveUnitId(Guid Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        var data = await _classificationSubjectService.GetAll();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.Classification.Creator.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
            data = data.Where(x => x.Classification.CreatorId == AppUsers.CurrentUser(User).CreatorId).ToList();
        var result = data.Where(x => x.Classification.Creator!.ArchiveUnitId == Id && x.SubjectClassificationName!.ToLower().Contains(param.ToLower())).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindSubSubjectClassificationByClassificationId(Guid Id, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        string par = "1=1";
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            par += $" && Creator.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
        if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
            par += $" && CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
        var data = await _classificationSubSubjectService.GetAll(par);
        //if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
        //    data = data.Where(x => x.Creator.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        //if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
        //    data = data.Where(x => x.CreatorId == AppUsers.CurrentUser(User).CreatorId).ToList();
        var result = data.Where(x => x.SubjectClassificationId == Id).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindTypeStorageByParam(string param = "")
    {
        string[] arrParam = param.Split(',');

        var keyword = string.IsNullOrEmpty(arrParam[0]) ? string.Empty : arrParam[0];
        Guid DetailId = Guid.Empty;
        Guid.TryParse(arrParam[1], out DetailId);

        var data = await _typeStorageService.GetAll();
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
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
        Guid GmdDetailId = Guid.Empty;

        var data = await _archiveRetentionService.GetAll();

        var dataExt = await _archiveExtendService.GetDetailAll();
        var dataDst = await _archiveDestroyService.GetDetailAll();
        var dataMove = await _archiveMovementService.GetDetailAll();

        var detailExt = type == GlobalConst.ArchiveExtend ? dataExt.Where(x => x.ArchiveExtendId != DetailId).ToList() : dataExt;
        var detailDst = type == GlobalConst.ArchiveDestroy ? dataDst.Where(x => x.ArchiveDestroyId != DetailId).ToList() : dataDst;
        var detailMove = type == GlobalConst.ArchiveMovement ? dataMove.Where(x => x.ArchiveMovementId != DetailId).ToList() : dataMove;


        if (type == GlobalConst.ArchiveMovement)
        {
            Guid.TryParse(arrParam[5], out GmdDetailId);
            data = data.Where(x => x.GmdDetailId == GmdDetailId).ToList();
        }

        var result =
            (from dataALl in data
             join dataDetailExt in detailExt on dataALl.ArchiveId equals dataDetailExt.ArchiveId into a
             from dataDetailExt in a.DefaultIfEmpty()
             join dataDetailDst in detailDst on dataALl.ArchiveId equals dataDetailDst.ArchiveId into b
             from dataDetailDst in b.DefaultIfEmpty()
             join dataDetailMove in detailMove on dataALl.ArchiveId equals dataDetailMove.ArchiveId into c
             from dataDetailMove in c.DefaultIfEmpty()
             where dataALl.SubSubjectClassificationId == SubSubjectClassificationId && dataALl.RetentionDateArchive.ToString().Contains(year.ToString())
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

        var detailExt = type == GlobalConst.ArchiveExtend ? dataExt.Where(x => x.ArchiveExtendId != DetailId).ToList() : dataExt;
        var detailDst = type == GlobalConst.ArchiveDestroy ? dataDst.Where(x => x.ArchiveDestroyId != DetailId).ToList() : dataDst;

        var result =
            (from dataALl in data
             join dataDetailExt in detailExt on dataALl.ArchiveId equals dataDetailExt.ArchiveId into a
             from dataDetailExt in a.DefaultIfEmpty()
             join dataDetailDst in detailDst on dataALl.ArchiveId equals dataDetailDst.ArchiveId into b
             from dataDetailDst in b.DefaultIfEmpty()
             where dataALl.SubSubjectClassificationId == SubSubjectClassificationId && dataALl.RetentionDateArchive.ToString().Contains(year.ToString())
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

    public async Task<JsonResult> BindArchiveActiveBySubjectId(Guid Id, Guid formId, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        var data = await _archiveService.GetArchiveActiveBySubjectId(Id, formId);
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.Creator.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
            data = data.Where(x => x.CreatorId == AppUsers.CurrentUser(User).CreatorId).ToList();
        var result = data.Where(x => x.TitleArchive!.ToLower().Contains(param.ToLower())).OrderBy(x => x.TitleArchive).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindArchiveActiveBySubjectandGmdDetailId(Guid Id, Guid gmdDetailId, Guid formId, string param = "")
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;
        var data = await _archiveService.GetArchiveActiveBySubjectId(Id, formId);
        if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            data = data.Where(x => x.Creator.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId).ToList();
        if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
            data = data.Where(x => x.CreatorId == AppUsers.CurrentUser(User).CreatorId).ToList();
        var result = data.Where(x => x.GmdDetailId == gmdDetailId && x.TitleArchive!.ToLower().Contains(param.ToLower())).OrderBy(x => x.TitleArchive).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindSubTypeStorageById(Guid Id)
    {
        var data = await _subTypeStorageService.GetById(Id);
        return Json(data.FirstOrDefault());
    }

    public async Task<JsonResult> BindGMDDetailByTypeStorageId(string param, Guid Id)
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;

        var data = await _typeStorageService.GetAllByTypeStorageId(Id);
        var result = data
            .Where(x => x.GmdDetail.Name.ToLower().Contains(param))
            .Select(x => new {
           id = x.GmdDetail.GmdDetailId.ToString(),
           text = x.GmdDetail.Name
        });
        return Json(result);
    }
    public async Task<JsonResult> BindGMDDetailVolumeByTypeStorageId(Guid TypeStorageId, Guid GMDDetailId)
    {
        var data = await _typeStorageService.GetAllByTypeStorageId(TypeStorageId);
        var result = data
            .Where(x  => x.GmdDetailId == GMDDetailId)
            .Select(x => new {
                volume = x.Size,
                unit = x.GmdDetail.Unit
            });
        return Json(result);
    }
    public async Task<JsonResult> BindSubTypeStorageIdByGMDDetailID(string param, Guid Id, Guid GMDDetailId)
    {
        param = string.IsNullOrEmpty(param) ? string.Empty : param;

        var data = await _subTypeStorageService.GetAllByTypeStorageandGMDDetailId(Id, GMDDetailId);
        var result = data.Where(x => x.SubTypeStorageName.ToLower().Contains(param.ToLower()))
            .Select(x => new {
                id = x.SubTypeStorageId.ToString(),
                text = $"Kode : {x.SubTypeStorageCode}, Nama : {x.SubTypeStorageName}, Jumlah : {x.Volume}"
            });
        return Json(result);
    }
    public async Task<JsonResult> BindGMDDetailVolumeBySubTypeStorageId(Guid SubTypeStorageId, Guid GMDDetailId)
    {
        var data = await _subTypeStorageService.GetAllDetailBySubTypeStorageId(SubTypeStorageId);
        var result = data
            .Where(x => x.GmdDetailId == GMDDetailId)
            .Select(x => new {
                code = x.SubTypeStorage.SubTypeStorageCode,
                name = x.SubTypeStorage.SubTypeStorageName,
                volume = x.Size,
                unit = x.GmdDetail.Unit
            }).FirstOrDefault();
        return Json(result);
    }
    public async Task<JsonResult> BindViewDetailArchive(Guid Id)
    {
        var x = await _archiveService.GetById(Id);
        var result = new {
                title = x.TitleArchive,
                security = x.SecurityClassification.SecurityClassificationName,
                classification = x.SubSubjectClassification.SubjectClassification.Classification.ClassificationName,
                subjectClassification = x.SubSubjectClassification.SubjectClassification.SubjectClassificationName,
                docNo = x.DocumentNo,
                archiveUnit = x.Creator.ArchiveUnit.ArchiveUnitName,
                dateArchive = x.CreatedDateArchive.ToString("dd MMMM yyyy"),
                owner = x.ArchiveOwner.ArchiveOwnerName,
                creator = x.Creator.CreatorName,
                file = x.TrxFileArchiveDetails
            };
        return Json(result);
    }
    #endregion

    #endregion
    public async Task<IActionResult> BindFileArchive(Guid Id, bool IsDownload = true)
    {
        var data = await _fileArchiveDetailService.GetById(Id);
        if (data != null)
        {
            var path = string.Concat(data.FilePath, data.FileNameEncrypt);
            if (System.IO.File.Exists(path))
            {
                var bytes = System.IO.File.ReadAllBytes(path);
                if (IsDownload)
                    return File(bytes, data.FileType, data.FileName);
                else
                    return File(bytes, data.FileType);
            }
        }
        return File(new byte[] { }, "application/octet-stream", "FileNotFound.txt");
    }
    [HttpGet]
    public async Task<FileResult> BindLogoCompany()
    {
        BksArditaDevContext context = new BksArditaDevContext();
        var data = await context.MstGeneralSettings
            .Include(x => x.IdxGeneralSettingsFormatFiles)
            .Where(x => x.IsActive == true)
            .AsNoTracking()
            .OrderBy(x => x.GeneralSettingsId)
            .LastOrDefaultAsync();

        if (data != null)
            return File(Convert.FromBase64String(data.CompanyLogoContent), "application/octet-stream", data.CompanyLogoFileName);
        else
        {
            var bytes = System.IO.File.ReadAllBytes("~/img/bks.png");
            return File(bytes, "application/octet-stream", "bks.png");
        }

    }
    [HttpGet]
    public async Task<string> BindTitleApplication()
    {
        BksArditaDevContext context = new BksArditaDevContext();
        var data = await context.MstGeneralSettings
            .Include(x => x.IdxGeneralSettingsFormatFiles)
            .Where(x => x.IsActive == true)
            .AsNoTracking()
            .OrderBy(x => x.GeneralSettingsId)
            .LastOrDefaultAsync();

        if (data != null)
            return data.AplicationTitle;
        else
        {
            return "Ardita";
        }

    }
    [HttpGet]
    public async Task<string> BindFooter()
    {
        BksArditaDevContext context = new BksArditaDevContext();
        var data = await context.MstGeneralSettings
            .Include(x => x.IdxGeneralSettingsFormatFiles)
            .Where(x => x.IsActive == true)
            .AsNoTracking()
            .OrderBy(x => x.GeneralSettingsId)
            .LastOrDefaultAsync();

        if (data != null)
            return data.Footer;
        else
        {
            return "Ardita";
        }

    }
}
