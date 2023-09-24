using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.ReportModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Ardita.Areas.Report.Controllers
{
    [CustomAuthorize]
    [Area("Report")]
    public class ReportTransferMediaArchiveInActiveController : BaseController<TransferMediaParams>
    {
        private IReportService _reportService;
        public ReportTransferMediaArchiveInActiveController(IReportService reportService,
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
            IClassificationSubjectService classificationSubjectService,
            IHostingEnvironment hostingEnvironment)
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
            _hostingEnvironment = hostingEnvironment;
        }
        public override async Task<ActionResult> Index()
        {
            ViewBag.Data = null;
            ViewBag.DataExcel = null;

            await AllViewBag();
            return View();
        }
        public async Task<IActionResult> GenerateReport(ReportGlobalParams param)
        {
            var reportName = "RptReportTransferMedia";
            var returnString = await _reportService.GenerateReportTransferMediaArchiveInActive(reportName, param, AppUsers.CurrentUser(User));
            var filePdf = $"{reportName}.pdf";
            var fileExcel = $"{reportName}.xlsx";
            System.IO.File.WriteAllBytes(Path.Combine(_hostingEnvironment.WebRootPath, GlobalConst.Report, filePdf), returnString.Item1);
            System.IO.File.WriteAllBytes(Path.Combine(_hostingEnvironment.WebRootPath, GlobalConst.Report, fileExcel), returnString.Item2);
            ViewBag.FilePdf = filePdf;
            ViewBag.FileExcel = fileExcel;

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
