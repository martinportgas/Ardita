using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.Report.Controllers
{

    [CustomAuthorize]
    [Area("Report")]
    public class ReportArchiveReceivedInActiveController : Controller
    {
        private IReportService _reportService;
        public ReportArchiveReceivedInActiveController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GenerateReport()
        {
            var reportName = "RptReportArchiveReceivedInActive";
            var returnString = await _reportService.GenerateReportArchiveReceivedInActive(reportName);
            return File(returnString, "application/pdf");
        }
    }
}
