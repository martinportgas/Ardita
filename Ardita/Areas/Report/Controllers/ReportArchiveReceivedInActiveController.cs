using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.ReportModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.Report.Controllers
{

    [CustomAuthorize]
    [Area("Report")]
    public class ReportArchiveReceivedInActiveController : BaseController<ArchiveProcessingParams>
    {
        private IReportService _reportService;
        public ReportArchiveReceivedInActiveController(IReportService reportService,
            ICompanyService companyService,
            IArchiveUnitService archiveUnitService,
            IRoomService roomService,
            IRackService rackService,
            ILevelService levelService,
            IRowService rowService,
            IGmdService gmdService,
            ITypeStorageService typeStorageService,
            IArchiveCreatorService archiveCreatorService,
            IArchiveOwnerService archiveOwnerService,
            IClassificationService classificationService,
            IClassificationSubjectService classificationSubjectService)
        {
            _reportService = reportService;
            _companyService = companyService;
            _archiveUnitService = archiveUnitService;
            _roomService = roomService;
            _rackService = rackService;
            _levelService = levelService;
            _rowService = rowService;
            _gmdService = gmdService;
            _typeStorageService = typeStorageService;
            _archiveCreatorService = archiveCreatorService;
            _archiveOwnerService = archiveOwnerService;
            _classificationService = classificationService;
            _classificationSubjectService = classificationSubjectService;
        }
        public override async Task<ActionResult> Index()
        {
            ViewBag.Data = null;
            ViewBag.DataExcel = null;

            await AllViewBag();
            return View();
        }
        public async Task<IActionResult> GenerateReport(ArchiveActiveParams param)
        {
            var reportName = "RptArchiveActive";
            var returnString = await _reportService.GenerateReportArchiveActiveAsync(reportName, param);
            ViewBag.Data = String.Format("data:application/pdf;base64,{0}", Convert.ToBase64String(returnString.Item1));
            ViewBag.DataExcel = String.Format("data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{0}", Convert.ToBase64String(returnString.Item2));

            await AllViewBag();
            return View(GlobalConst.Index);
        }
        public async Task AllViewBag()
        {
            ViewBag.ListCompany = await BindCompanies();
            ViewBag.ListArchiveUnit = await BindAllArchiveUnits();
            ViewBag.ListRoom = await BindRooms();
            ViewBag.ListRack = await BindRacks();
            ViewBag.ListLevel = await BindLevels();
            ViewBag.ListRow = await BindRows();
            ViewBag.ListGMD = await BindGmds();
            ViewBag.ListTypeStorage = await BindTypeStorage();
            ViewBag.ListArchiveCreator = await BindArchiveCreators();
            ViewBag.ListArchiveOwner = await BindArchiveOwners();
            ViewBag.ListClassification = await BindClasscifications();
            ViewBag.ListSubjectClassification = await BindSubjectClasscifications();
        }
    }
}
