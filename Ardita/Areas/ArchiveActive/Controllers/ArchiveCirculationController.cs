using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.ArchiveActive.Controllers;

[CustomAuthorize]
[Area(GlobalConst.ArchiveActive)]
public class ArchiveCirculationController : BaseController<TrxArchiveMovement>
{
    #region MEMBER AND CTR
    public ArchiveCirculationController(
        IArchiveDestroyService archiveDestroyService,
        IArchiveExtendService archiveExtendService,
        IArchiveMovementService archiveMovementService,
        IArchiveService archiveService,
        IEmployeeService employeeService,
        IArchiveRetentionService archiveRetentionService,
        IArchiveApprovalService archiveApprovalService,
        ITypeStorageService typeStorageService,
        IMediaStorageService mediaStorageService,
        IArchiveUnitService archiveUnitService,
        IClassificationService classificationService,
        IClassificationSubjectService classificationSubjectService,
        IFloorService floorService,
        IRoomService roomService,
        IRackService rackService,
        ILevelService levelService,
        IRowService rowService,
        IArchiveCreatorService archiveCreatorService,
        IClassificationSubSubjectService classificationSubSubjectService)
    {
        _archiveExtendService = archiveExtendService;
        _employeeService = employeeService;
        _archiveRetentionService = archiveRetentionService;
        _archiveApprovalService = archiveApprovalService;
        _archiveService = archiveService;
        _archiveDestroyService = archiveDestroyService;
        _archiveMovementService = archiveMovementService;
        _typeStorageService = typeStorageService;
        _mediaStorageService = mediaStorageService;
        _archiveUnitService = archiveUnitService;
        _classificationSubSubjectService = classificationSubSubjectService;
        _classificationSubjectService = classificationSubjectService;
        _levelService = levelService;
        _rowService = rowService;
        _archiveCreatorService = archiveCreatorService;
        _classificationService = classificationService;
        _rackService = rackService;
        _roomService = roomService;
        _floorService = floorService;
    }
    #endregion
    public override async Task<ActionResult> Index()
    {
        await AllViewBagIndex();
        return await base.Index();
    }
    public override async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            model.SessionUser = User;
            var result = await _archiveMovementService.GetList(model);

            return Json(result);

        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public async Task<IActionResult> Form()
    {
        await Task.Delay(0);
        return View(GlobalConst.Form);
    }
    public async Task AllViewBagIndex()
    {
        ViewBag.ListArchiveUnit = await BindAllArchiveUnits();
        ViewBag.ListFloor = await BindFloors();
        ViewBag.ListRoom = await BindRooms();
        ViewBag.ListRack = await BindRacks();
        ViewBag.ListLevel = await BindLevels();
        ViewBag.ListRow = await BindRows();
        ViewBag.ListArchiveCreator = await BindArchiveCreators();
        ViewBag.ListClassification = await BindClasscifications();
        ViewBag.ListSubjectClassification = await BindSubjectClasscifications();
    }
}
