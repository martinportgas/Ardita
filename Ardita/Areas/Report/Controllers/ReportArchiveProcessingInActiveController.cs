using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.Report.Controllers
{
    [CustomAuthorize]
    [Area("Report")]
    public class ReportArchiveProcessingInActiveController : Controller
    {
        private IReportService _reportService;
        public ReportArchiveProcessingInActiveController(IReportService reportService)
        {
            _reportService = reportService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GenerateReport()
        {
            var reportName = "RptReportArchiveProcessingInActive";
            var returnString = await _reportService.GenerateReportArchiveProcessingInActive(reportName);
            return File(returnString, "application/pdf");
        }
    }
}
