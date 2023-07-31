using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.ArchiveInActive.Controllers
{
    [CustomAuthorize]
    [Area(GlobalConst.ArchiveInActive)]
    public class ArchiveRetentionController : BaseController<TrxArchive>
    {
        #region MEMBER AND CTR
        public ArchiveRetentionController(
            IArchiveRetentionService archiveRetentionService,
            IArchiveService archiveService,
            IFileArchiveDetailService fileArchiveDetailService,
            IArchiveCreatorService archiveCreatorService,
            IArchiveUnitService archiveUnitService,
            IFloorService floorService,
            IRoomService roomService,
            IRackService rackService,
            ILevelService levelService,
            IRowService rowService,
            IClassificationService classificationService,
            IClassificationSubjectService classificationSubjectService)
        {
            _archiveRetentionService = archiveRetentionService;
            _fileArchiveDetailService = fileArchiveDetailService;
            _archiveService = archiveService;
            _archiveCreatorService = archiveCreatorService;
            _archiveUnitService = archiveUnitService;
            _archiveCreatorService = archiveCreatorService;
            _floorService = floorService;
            _roomService = roomService;
            _rackService = rackService;
            _levelService = levelService;
            _rowService = rowService;
            _classificationService = classificationService;
            _classificationSubjectService = classificationSubjectService;
        }
        #endregion

        #region MAIN ACTION
        public override async Task<ActionResult> Index()
        {
            await AllViewBagIndex();
            return await base.Index();
        }
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                model.IsArchiveActive = false;
                model.SessionUser = User;
                var result = await _archiveRetentionService.GetList(model);

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
            ViewBag.title = GlobalConst.TitleArchiveRetention;
            ViewBag.backController = GlobalConst.ArchiveRetention;
            ViewBag.backArea = GlobalConst.ArchiveInActive;
            return PartialView(GlobalConst._ArchiveMonitoringDetail, model);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFile(Guid Id)
        {
            var model = await _fileArchiveDetailService.GetById(Id);
            string path = model.FilePath;

            if (System.IO.File.Exists(path))
            {
                return File(System.IO.File.OpenRead(path), "application/octet-stream", Path.GetFileName(path));
            }
            return NotFound();
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
        #endregion
    }
}
