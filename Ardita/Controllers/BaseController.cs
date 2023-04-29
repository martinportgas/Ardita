using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Models.ViewModels.SubSubjectClasscification;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ardita.Controllers;

public abstract class BaseController<T> : Controller
{
    #region Properties
    protected IArchiveUnitService _archiveUnitService { get; set; }
    protected ICompanyService _companyService { get; set; }
    protected IArchiveCreatorService _archiveCreatorService { get; set; }
    protected IClassificationService _classificationService { get; set; }
    protected IClassificationTypeService _classificationTypeService { get; set; }
    protected IClassificationSubjectService _classificationSubjectService { get; set; }
    protected IClassificationSubSubjectService _classificationSubSubjectService { get; set; }
    protected IPositionService _positionService { get; set; }
    protected IFloorService _floorService { get; set; }
    protected IGmdService _gmdService { get; set; }
    protected ILevelService _levelService { get; set; }
    protected IRackService _rackService { get; set; }
    protected IRoomService _roomService { get; set; }
    protected IRowService _rowService { get; set; }
    protected ISecurityClassificationService _securityClassificationService { get; set; }
    protected IArchiveService _archiveService { get; set; }
    protected IMediaStorageService _mediaStorage { get; set; }
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
    protected async Task<List<SelectListItem>> BindCompanies()
    {
        var data = await _companyService.GetAll();
        return data.Select(x => new SelectListItem
        {
            Value = x.CompanyId.ToString(),
            Text = x.CompanyName.ToString()
        }).ToList();
    }
    protected async Task<List<SelectListItem>> BindRacks()
    {
        var data = await _rackService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.RackId.ToString(),
            Text = x.RackName
        }).ToList();
    }
    protected async Task<List<SelectListItem>> BindRooms()
    {
        var data = await _roomService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.RoomId.ToString(),
            Text = x.RoomName
        }).ToList();
    }
    protected async Task<List<SelectListItem>> BindFloors()
    {
        var data = await _floorService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.FloorId.ToString(),
            Text = x.FloorName
        }).ToList();
    }
    protected async Task<List<SelectListItem>> BindArchiveUnits()
    {
        var data = await _archiveUnitService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.ArchiveUnitId.ToString(),
            Text = x.ArchiveUnitName
        }).ToList();
    }
    protected async Task<List<SelectListItem>> BindLevels()
    {
        var data = await _levelService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.LevelId.ToString(),
            Text = x.LevelName
        }).ToList();
    }
    protected async Task<List<SelectListItem>> BindGmds()
    {
        var data = await _gmdService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.GmdId.ToString(),
            Text = x.GmdName
        }).ToList();
    }
    protected async Task<List<SelectListItem>> BindSubSubjectClasscifications()
    {
        var data = await _classificationSubSubjectService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.SubSubjectClassificationId.ToString(),
            Text = x.SubSubjectClassificationName
        }).ToList();
    }
    protected async Task<List<SelectListItem>> BindSecurityClassifications()
    {
        var data = await _securityClassificationService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.SecurityClassificationId.ToString(),
            Text = x.SecurityClassificationName
        }).ToList();
    }
    protected async Task<List<SelectListItem>> BindArchiveCreators()
    {
        var data = await _archiveCreatorService.GetAll();

        return data.Select(x => new SelectListItem
        {
            Value = x.CreatorId.ToString(),
            Text = x.CreatorName
        }).ToList();
    }
    public async Task<JsonResult> BindFloors(string Id)
    {
        List<TrxFloor> listFloors = new();
        Guid ArchiveUnitId = new Guid(Id);

        var data = await _floorService.GetAll();
        listFloors = data.Where(x => x.ArchiveUnitId == ArchiveUnitId).ToList();
        return Json(listFloors);

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
    #endregion
}
