using Ardita.Controllers;
using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.ArchiveActive.Controllers
{
    [CustomAuthorize]
    [Area(GlobalConst.ArchiveActive)]
    public class ArchiveMonitoringController : BaseController<TrxArchive>
    {
        public ArchiveMonitoringController(
        IArchiveCreatorService archiveCreatorService,
        IRowService rowService,
        IArchiveService archiveService,
        IFileArchiveDetailService fileArchiveDetailService,
        IArchiveUnitService archiveUnitService,
        IFloorService floorService,
        IRoomService roomService,
        IRackService rackService,
        ILevelService levelService,
        IClassificationService classificationService,
        IClassificationSubjectService classificationSubjectService)
        {
            _archiveCreatorService = archiveCreatorService;
            _rowService = rowService;
            _fileArchiveDetailService = fileArchiveDetailService;
            _archiveService = archiveService;
            _archiveUnitService = archiveUnitService;
            _floorService = floorService;
            _roomService = roomService;
            _rackService = rackService;
            _levelService = levelService;
            _classificationService = classificationService;
            _classificationSubjectService = classificationSubjectService;
        }
        public override async Task<ActionResult> Index()
        {
            await AllViewBagIndex();
            return View();
        }
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                model.SessionUser = User;
                model.whereClause = GlobalConst.WhereClauseArchiveMonitoring;
                model.IsArchiveActive = true;
                if (AppUsers.CurrentUser(User).RoleCode == GlobalConst.ROLE.USV.ToString())
                    model.PositionId = AppUsers.CurrentUser(User).PositionId;
                var result = await _archiveService.GetList(model);

                return Json(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var model = await _archiveService.GetById(Id);
            ViewBag.title = GlobalConst.TitleArchiveMonitoring;
            ViewBag.backController = GlobalConst.ArchiveMonitoring;
            ViewBag.backArea = GlobalConst.ArchiveActive;
            return PartialView(GlobalConst._ArchiveMonitoringDetail, model);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFile(Guid Id)
        {
            var data = await _fileArchiveDetailService.GetById(Id);
            if (data != null)
            {
                var path = string.Concat(data.FilePath, data.FileNameEncrypt);
                var bytes = System.IO.File.ReadAllBytes(path);

                return File(bytes, data.FileType, data.FileName);
            }
            return File(new byte[] { }, "application/octet-stream", "NotFound.txt");
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
}
