﻿using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.Archive;
using Ardita.Models.ViewModels.SubSubjectClasscification;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Ardita.Controllers;

public abstract class BaseController<T> : Controller
{
    #region Properties
    protected IHostingEnvironment _hostingEnvironment;

    //User Manage
    protected IEmployeeService _employeeService { get; set; }
    protected IPositionService _positionService { get; set; }

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

    //Trx
    protected IFileArchiveDetailService _fileArchiveDetailService { get; set; }

    protected IMediaStorageService _mediaStorageService { get; set; }
    protected ITypeStorageService _typeStorageService { get; set; }
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
    #endregion

    #region Binding
    //selectlist
    public async Task<List<SelectListItem>> BindCompanies()
    {
        var data = await _companyService.GetAll();
        return data.Select(x => new SelectListItem
        {
            Value = x.CompanyId.ToString(),
            Text = x.CompanyName.ToString()
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
        var data = await _archiveUnitService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.ArchiveUnitId.ToString(),
            Text = x.ArchiveUnitName
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
        var data = await _archiveService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.ArchiveId.ToString(),
            Text = x.TitleArchive.ToString()
        }).ToList();
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
    //json
    public async Task<JsonResult> BindFloorsByArchiveUnitId(string Id)
    {
        List<TrxFloor> listFloors = new();
        Guid ArchiveUnitId = new(Id);

        var data = await _floorService.GetAll();
        listFloors = data.Where(x => x.ArchiveUnitId == ArchiveUnitId).ToList();
        return Json(listFloors);

    }
    public async Task<JsonResult> BindRoomByFloorId(string Id)
    {
        List<TrxRoom> list = new();
        Guid id = new(Id);

        var data = await _roomService.GetAll();
        list = data.Where(x => x.FloorId == id).ToList();
        return Json(list);

    }
    public async Task<JsonResult> BindRackByRoomId(string Id)
    {
        List<TrxRack> list = new();
        Guid id = new(Id);

        var data = await _rackService.GetAll();
        list = data.Where(x => x.RoomId == id).ToList();
        return Json(list);
    }
    public async Task<JsonResult> BindLevelByRackId(string Id)
    {
        List<TrxLevel> list = new();
        Guid id = new(Id);

        var data = await _levelService.GetAll();
        list = data.Where(x => x.RackId == id).ToList();
        return Json(list);
    }
    public async Task<JsonResult> BindRowByLevelId(string Id)
    {
        List<TrxRow> list = new();
        Guid id = new(Id);

        var data = await _rowService.GetAll();
        list = data.Where(x => x.LevelId == id).ToList();
        return Json(list);
    }
    public async Task<JsonResult> BindClassificationSubjectIdByClassificationId(Guid Id)
    {
        var data = await _classificationSubjectService.GetAll();
        var result = data.Where(x => x.ClassificationId == Id).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindClassificationIdByClassificationTypeId(Guid Id)
    {
        var data = await _classificationService.GetAll();
        var result = data.Where(x => x.TypeClassificationId == Id).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindSubSubjectClasscificationsById(string Id)
    {
        Guid SubSubjectClasscificationId = new(Id);

        var data = await _classificationSubSubjectService.GetById(SubSubjectClasscificationId);
        var subSubjectClassification = from d in data
                                       where d.SubSubjectClassificationId == SubSubjectClasscificationId
                                       select new SubSubjectClasscificationViewModel
                                       {
                                           SubSubjectClassificationId = d.SubSubjectClassificationId,
                                           SubSubjectClassificationCode = d.SubSubjectClassificationCode,
                                           CreatorId = d.CreatorId,
                                           CreatorName = d.Creator?.CreatorName,
                                           RetentionActive = d.RetentionActive,
                                           RetentionInactive = d.RetentionInactive,
                                           SubSubjectClassificationName = d.SubSubjectClassificationName
                                       };



        return Json(subSubjectClassification.FirstOrDefault());

    }
    public async Task<JsonResult> BindArchiveById(string Id)
    {
        Guid ArchiveId = new(Id);

        var data = await _archiveService.GetById(ArchiveId);
        ArchiveViewModel result = new() 
        {
            ArchiveId = data.ArchiveId,
            TitleArchive = data.TitleArchive,
            TypeArchive = data.TypeArchive,
            TypeSender = data.TypeSender,
            Volume = data.Volume,
            ArchiveCreator = data.SubSubjectClassification.Creator.CreatorName
        };
        return Json(result);
    }
    public async Task<JsonResult> BindArchivesBySubSubjectClassificationId(Guid Id)
    {
        var data = await _archiveService.GetAll();

        var result = data.Where(x => x.SubSubjectClassificationId == Id).ToList();
        return Json(result);
    }
    public async Task<JsonResult> BindTypeStorageByArchiveUnitId(Guid Id)
    {
        var data = await _typeStorageService.GetAll();
        var result = data.Where(x => x.ArchiveUnitId == Id).ToList();
        return Json(result);
    }
    #endregion
}