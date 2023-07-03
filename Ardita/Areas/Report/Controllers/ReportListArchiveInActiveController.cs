using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.Report.Controllers
{
    [CustomAuthorize]
    [Area("Report")]
    public class ReportListArchiveInActiveController : Controller
    {
        private IReportService _reportService;
        public ReportListArchiveInActiveController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GenerateReport()
        {
            var reportName = "RptListArchiveInActive";
            var returnString = await _reportService.GenerateReportListArchiveInActive(reportName);
            return File(returnString, "application/pdf");
        }
    }
}
